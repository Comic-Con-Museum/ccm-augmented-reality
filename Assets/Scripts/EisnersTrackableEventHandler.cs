using UnityEngine;
using Vuforia;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// A TrackableEvent handler for items that are part of the Eisners
/// experience.
/// 
/// Brings up information overlay and adds item to collection on tap.
/// Rotates augmentation object on swipe.
/// </summary>
public class EisnersTrackableEventHandler : BaseExperienceTrackableEventHandler {

    public float rotationSpeed = 5.0f;

    private string contentId;
    private AndroidJavaObject contentInfo = null;
    private static GameObject overlayObject = null;

    protected override void StartImpl() {
        contentId = gameObject.name.Split(':').Last();
    }

    public static void SetOverlayObject(GameObject obj) {
        Debug.Log("Setting overlay object to " + obj);
        overlayObject = obj;
    }

    public override void OnTap() {
        Debug.Log("Eisner item " + gameObject.name + " tapped.");
        NativeAppInterface.RegisterNewCollection(contentId);
        ShowOverlay();
    }

    public override void OnSwipe(Vector2 deltaPosition) {
        Debug.Log("Eisner item " + gameObject.name + " swiped. DeltaPosition: " + deltaPosition);

        foreach (Transform tf in transform) {
            if (tf.CompareTag("ARModel")) {
                tf.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * Vector3.Dot(Vector3.left, deltaPosition));
            }
        }
    }

    protected override void OnTrackingFoundImpl() {
        if (contentInfo == null) {
            contentInfo = NativeAppInterface.GetContentItemInfo(contentId);
        }
    }

    protected override void OnTrackingLostImpl() {
        var button = overlayObject.GetComponentInChildren<Button>();
        button.onClick.RemoveListener(OnOverlayButtonClick);
        overlayObject.SetActive(false);
    }

    private void ShowOverlay() {
        var textComponents = overlayObject.GetComponentsInChildren<Text>();
        
        if (contentInfo != null) {
            var contentItem = contentInfo.Get<AndroidJavaObject>("contentItem");
            textComponents[0].text = contentItem.Get<string>("title");
            textComponents[1].text = contentItem.Get<string>("description");
        }
        else {
            textComponents[0].text = "Eisner Title";
            textComponents[1].text = "Description for this Eisner-winning work.";
        }

        var button = overlayObject.GetComponentInChildren<Button>();
        button.onClick.AddListener(OnOverlayButtonClick);

        overlayObject.SetActive(true);
    }

    public void OnOverlayButtonClick() {
        var button = overlayObject.GetComponentInChildren<Button>();
        button.onClick.RemoveListener(OnOverlayButtonClick);

        overlayObject.SetActive(false);
        NativeAppInterface.ViewContentItemInApp(contentId);
    }
}
