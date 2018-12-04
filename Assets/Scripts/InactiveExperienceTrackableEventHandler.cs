using UnityEngine;
using Vuforia;

/// <summary>
/// A custom TrackableEvent handler for items that are not part of the current
/// experience.
/// 
/// Rotates the augmentation object.
/// </summary>
public class InactiveExperienceTrackableEventHandler : BaseExperienceTrackableEventHandler {

    public float rotationSpeed = 50.0f;
    protected bool m_IsRotating = false;

    void Update() {
        if (m_IsRotating) {
            foreach (Transform tf in transform) {
				if (tf.CompareTag("ARModel")) {
                    tf.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                }
            }
        }
    }

    protected override void OnTrackingFoundImpl() {
        m_IsRotating = true;
    }

    protected override void OnTrackingLostImpl() {
        m_IsRotating = false;
    }

    public override void OnTap() {
        Debug.Log(gameObject.name + " tapped.");
    }
}
