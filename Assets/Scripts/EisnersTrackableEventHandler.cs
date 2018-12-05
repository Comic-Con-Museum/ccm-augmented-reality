using UnityEngine;
using Vuforia;
using System.Linq;

/// <summary>
/// A TrackableEvent handler for items that are part of the Eisners
/// experience.
/// 
/// Brings up information overlay and adds item to collection on tap.
/// Rotates augmentation object on swipe.
/// </summary>
public class EisnersTrackableEventHandler : BaseExperienceTrackableEventHandler {

    public float rotationSpeed = 5.0f;

    public override void OnTap() {
        Debug.Log("Eisner item " + gameObject.name + " tapped.");

        var contentId = gameObject.name.Split(':').Last();
        NativeAppInterface.RegisterNewCollection(contentId);
        NativeAppInterface.ViewContentItemInApp(contentId);
    }

    public override void OnSwipe(Vector2 deltaPosition) {
        Debug.Log("Eisner item " + gameObject.name + " swiped. DeltaPosition: " + deltaPosition);

        foreach (Transform tf in transform) {
            if (tf.CompareTag("ARModel")) {
                tf.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * Vector3.Dot(Vector3.left, deltaPosition));
            }
        }
    }
}
