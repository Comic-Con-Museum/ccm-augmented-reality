using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour {

	// public GUIText message = null;
	private Transform pickedObject = null;
	private Vector3 lastPlanePoint;
	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
		Plane targetPlane = new Plane(transform.up, transform.position);
		// Debug.Log("Target Plane=" + targetPlane);

		// Unity Player testing

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast(ray, out hit, 1000)) { //True when Ray intersects colider. 
			//   If true, hit contains additional info about where collider was hit
				Debug.Log("Object hit. " + hit.collider.gameObject.name);
				Debug.DrawLine(ray.origin, hit.point);
				pickedObject = hit.transform;
				// GameObject hitTarget = hit.collider.gameObject;
				// foreach (Transform child in hitTarget.transform) {
				// 	if (child.CompareTag("ARModel")) {
				// 		pickedObject = child;
				// 	}
				// }
				// lastPlanePoint = planePoint;
			} else {
				Debug.Log("Object not hit. " + hit);
				pickedObject = null;
			}
			Debug.Log("Picked object=" + pickedObject);
		}

		foreach (Touch touch in Input.touches) {
			//Gets the ray at position where the screen is touched
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			Debug.Log("Ray=" + ray);

			//Gets the position of ray along plane
			float dist = 0.0f;

			//Intersects ray with the plane. Sets dist to distance along the ray where intersects
			targetPlane.Raycast(ray, out dist);
			
			//Returns point dist along the ray.
			Vector3 planePoint = ray.GetPoint(dist);
			// Debug.Log("Point=" + planePoint);
			//True if finger touch began. If ray intersects collider, set pickedObject to transform 
				//  of collider object
			
			// Debug.Log("Touch phase=" + touch.phase);
			if (touch.phase == TouchPhase.Began) {
				//Struct used to get info back from a raycast
				RaycastHit hit = new RaycastHit();
			
				if (Physics.Raycast(ray, out hit, 1000)) { //True when Ray intersects colider. 
				//   If true, hit contains additional info about where collider was hit
					Debug.Log("Object hit. " + hit.transform.name);
					// Debug.DrawLine(ray.origin, hit.point);
					if (hit.transform.CompareTag("ARModel"))
					{
						pickedObject = hit.transform;
					}
					// pickedObject = hit.transform;
					// GameObject hitTarget = hit.collider.gameObject;
					// foreach (Transform child in hitTarget.transform) {
					// 	if (child.CompareTag("ARModel")) {
					// 		pickedObject = child;
					// 	}
					// }
					lastPlanePoint = planePoint;
				} else {
					Debug.Log("Object not hit. " + hit);
					pickedObject = null;
				}
				Debug.Log("Picked object=" + pickedObject);
			} 

			//Move Object when finger moves after object selected.
			else if (touch.phase == TouchPhase.Moved) {
				if (pickedObject != null) {
					// pickedObject.position += planePoint - lastPlanePoint;
					Debug.Log("Delta=" + touch.deltaPosition);
					pickedObject.position += new Vector3(touch.deltaPosition.x/20.0f, touch.deltaPosition.y/20.0f, 0.0f);
					// pickedObject.position.Set(touch.position.x, touch.position.y, 0.0f);
					// Debug.Log("Plane point=" + planePoint);
					// Debug.Log("Last plane point=" + lastPlanePoint);
					// Debug.Log("Diff=" + (planePoint - lastPlanePoint));
					lastPlanePoint = planePoint;
				}
			//Set pickedObject to null after touch ends.
			} else if (touch.phase == TouchPhase.Ended) {
				pickedObject.position.Set(0.0f, 0.0f, 0.0f);
				pickedObject = null;
			}
		}
	}
}
