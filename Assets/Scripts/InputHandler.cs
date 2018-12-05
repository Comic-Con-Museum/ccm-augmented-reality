using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	private Transform pickedObject = null;
	private Vector2 initialTouchPosition;

	void Start () {
	}

	void Update () {

	#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast(ray, out hit)) { 
				Debug.Log("Object hit by Raycast: " + hit.collider.gameObject.name);
	
				if (hit.transform.CompareTag("ARModel")) {
					pickedObject = hit.transform;
					pickedObject.parent.GetComponent<BaseExperienceTrackableEventHandler>().OnTap();
				}
			} 
			else {
				Debug.Log("Object not hit. " + hit);
				pickedObject = null;
			}
			Debug.Log("Picked object=" + pickedObject);
		}
	#endif
		
	#if UNITY_ANDROID && !UNITY_EDITOR
		foreach (Touch touch in Input.touches) {
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			Debug.Log("Ray=" + ray);
			
			if (touch.phase == TouchPhase.Began) {
				RaycastHit hit = new RaycastHit();
			
				if (Physics.Raycast(ray, out hit)) {
					Debug.Log("Object hit by Raycast: " + hit.transform.name);
					if (hit.transform.CompareTag("ARModel")) {
						pickedObject = hit.transform;
						initialTouchPosition = touch.position;
					}
				} else {
					Debug.Log("Object not hit. " + hit);
					pickedObject = null;
				}
				Debug.Log("Picked object=" + pickedObject);
			} 

			else if (touch.phase == TouchPhase.Moved) {
				if (pickedObject != null) {
					pickedObject.parent.GetComponent<BaseExperienceTrackableEventHandler>().OnSwipe(touch.deltaPosition);
				}
			} 
			
			else if (touch.phase == TouchPhase.Ended) {
				float touchDistance = Vector2.Distance(initialTouchPosition, touch.position);
				Debug.Log("Touch ended. Initial pos=" + initialTouchPosition + ", Current pos=" + touch.position + ", Distance=" + touchDistance);
				
				if (pickedObject != null && touchDistance < 1.0f) {
					pickedObject.parent.GetComponent<BaseExperienceTrackableEventHandler>().OnTap();
				}
				pickedObject = null;
				initialTouchPosition = Vector2.zero;
			}
		}
	#endif
	
	}
}
