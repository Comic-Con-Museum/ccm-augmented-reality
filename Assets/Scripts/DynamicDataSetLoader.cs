using UnityEngine;
using System.Collections; 
using Vuforia;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
 
 
public class DynamicDataSetLoader : MonoBehaviour {
    public string dataSetDir = "";
    public string inactivePrefab = "Inactive";

    private string activeHandlerName;

    void Start() {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSets);
        activeHandlerName = NativeAppInterface.CurrentExperience + "TrackableEventHandler";
        SetInactiveOverlayObject();
    }
         
    void LoadDataSets() {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        objectTracker.Stop();

        string[] files;
        if (string.IsNullOrEmpty(dataSetDir)) {
            Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    		Debug.Log("Streaming Assets Path: " + Application.streamingAssetsPath);
            files = Directory.GetFiles(Application.persistentDataPath + "/ImageTargetDataSets", "*.xml");
            // files = Directory.GetFiles(Application.streamingAssetsPath + "/Vuforia", "*.xml");
        } else {
            files = Directory.GetFiles(dataSetDir, "*.xml");    
        }

        foreach (string dataSetFile in files) {
            string dataSetName = dataSetFile.Split('/').Last().Split('.')[0];
            Debug.Log("Found dataset: " + dataSetName);
            
            DataSet dataSet = objectTracker.CreateDataSet();

            if (dataSet.Load(dataSetFile, VuforiaUnity.StorageType.STORAGE_ABSOLUTE)) {
                if (!objectTracker.ActivateDataSet(dataSet)) {
                    // Note: ImageTracker cannot have more than 1000 total targets activated
                    Debug.Log("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
                }

                SetActiveOverlayObject(dataSetName);

                IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
                foreach (TrackableBehaviour tb in tbs) {
                    if (tb.name == "New Game Object") {
                        Debug.Log("Found TrackableBehavior: " + tb.name + ", in " + dataSetName);

                        string[] trackableInfo = tb.TrackableName.Split(new string[] {"_-_"}, 2, StringSplitOptions.None);
                        var contentId = trackableInfo[0];
                        var modelName = trackableInfo[1];
                        
                        tb.gameObject.name = "ImageTarget:" + dataSetName + ":" + modelName + ":" + contentId;

                        UnityEngine.Object prefab;
                        if (string.Equals(dataSetName, NativeAppInterface.CurrentExperience, StringComparison.OrdinalIgnoreCase)) {
                            Debug.Log("^In current experience");

                            prefab = Resources.Load("Prefabs/" + modelName, typeof(GameObject));
                            tb.gameObject.AddComponent(Type.GetType(activeHandlerName));
                        } 
                        else {
                            Debug.Log("^Not in current experience");

                            prefab = Resources.Load("Prefabs/" + inactivePrefab, typeof(GameObject));
                            tb.gameObject.AddComponent<InactiveExperienceTrackableEventHandler>();
                        }
                        
                        tb.gameObject.AddComponent<TurnOffBehaviour>();
    
                        if (prefab == null) {
                            Debug.Log("<color=yellow>Warning: No augmentation object available for: " + tb.TrackableName + "</color>"); 
                        } 
                        else {
                            GameObject augmentation = Instantiate(prefab) as GameObject;
                            augmentation.transform.parent = tb.gameObject.transform;
                            augmentation.transform.localPosition = new Vector3(0f, 0f, 0f);
                            augmentation.gameObject.SetActive(true);    
                        }
                    }
                }
            } 
            else {
                Debug.LogError("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
            }
        }

        if (!objectTracker.Start()) {
            Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
        }

        NativeAppInterface.NotifyVuforiaLoaded((int)VuforiaUnity.InitError.INIT_SUCCESS);
    }

    private void SetActiveOverlayObject(string dataSetName) {
        if (string.Equals(dataSetName, NativeAppInterface.CurrentExperience, StringComparison.OrdinalIgnoreCase)) {
            var overlayPrefab = Resources.Load("Prefabs/" + NativeAppInterface.CurrentExperience + "Overlay", typeof(GameObject));
            if (overlayPrefab == null) {
                Debug.Log("Warning: No overlay pefab available for active experience");
            }
            else {
                GameObject overlayObj = Instantiate(overlayPrefab) as GameObject;
                overlayObj.SetActive(false);
                Type.GetType(activeHandlerName).GetMethod("SetOverlayObject").Invoke(null, new object[] { overlayObj });
            }
        }
    }

    private void SetInactiveOverlayObject() {
        var overlayPrefab = Resources.Load("Prefabs/" + inactivePrefab + "Overlay", typeof(GameObject));
        if (overlayPrefab == null) {
            Debug.Log("Warning: No overlay pefab available for inactive experience");
        }
        else {
            GameObject overlayObj = Instantiate(overlayPrefab) as GameObject;
            overlayObj.SetActive(false);
            InactiveExperienceTrackableEventHandler.SetOverlayObject(overlayObj);
        }
    }
}
