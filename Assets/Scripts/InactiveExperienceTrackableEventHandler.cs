using UnityEngine;
using Vuforia;
using UnityEngine.UI;

/// <summary>
/// A custom TrackableEvent handler for items that are not part of the current
/// experience.
/// 
/// Rotates the augmentation object.
/// </summary>
public class InactiveExperienceTrackableEventHandler : BaseExperienceTrackableEventHandler {

    public float rotationSpeed = 50.0f;

    protected bool m_IsRotating = false;

    private static GameObject overlayObject = null;
    private string experienceName;
    
    void Update() {
        if (m_IsRotating) {
            foreach (Transform tf in transform) {
				if (tf.CompareTag("ARModel")) {
                    tf.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                }
            }
        }
    }

    protected override void StartImpl() {
        experienceName = gameObject.name.Split(':')[1];
        Debug.Log("Setting inactive experience name to " + experienceName);
    }

    public static void SetOverlayObject(GameObject obj) {
        Debug.Log("Setting inactive overlay object to " + obj);
        overlayObject = obj;
    }

    protected override void OnTrackingFoundImpl() {
        m_IsRotating = true;
    }

    protected override void OnTrackingLostImpl() {
        m_IsRotating = false;
        overlayObject.SetActive(false);
    }

    public override void OnTap() {
        Debug.Log(gameObject.name + " tapped.");
        ShowOverlay();
    }

    private void ShowOverlay() {
        var textComponent = overlayObject.GetComponentInChildren<Text>();
        textComponent.text = "Switch to the " + experienceName + " experience to find out more!";
        overlayObject.SetActive(true);
    }
}
