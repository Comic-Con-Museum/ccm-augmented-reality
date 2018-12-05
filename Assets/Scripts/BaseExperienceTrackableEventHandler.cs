using UnityEngine;
using Vuforia;

/// <summary>
/// Base class for TrackableEvent handlers used by augmentation objects in any
/// experience.
/// 
/// Extends Vuforia's DefaultTrackableEventHandler.
/// Inherit from this class to implement handlers for new experiences.
/// </summary>
public abstract class BaseExperienceTrackableEventHandler : DefaultTrackableEventHandler {

    protected override void OnTrackingFound() {
        base.OnTrackingFound();
        OnTrackingFoundImpl();
    }

    protected override void OnTrackingLost() {
        base.OnTrackingLost();
        OnTrackingLostImpl();
    }

    protected virtual void OnTrackingFoundImpl() {}
    protected virtual void OnTrackingLostImpl() {}
    public virtual void OnTap() {}
    public virtual void OnSwipe(Vector2 deltaPosition) {}
}
