using UnityEngine;
using Vuforia;

/// <summary>
/// A TrackableEvent handler for items that are part of the Eisners
/// experience.
/// 
/// Brings up information overlay and adds item to collection on tap.
/// Rotates augmentation object on swipe.
/// </summary>
public class EisnersTrackableEventHandler : BaseExperienceTrackableEventHandler {

    public float rotationSpeed = 50.0f;

    public override void OnTap() {
        Debug.Log("Eisner item " + gameObject.name + " tapped.");
    }
}
