using UnityEngine;
using System.Collections; 
using Vuforia;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 
 
public class DynamicDataSetLoader : MonoBehaviour
{
    public GameObject augmentationObject = null;
    public string dataSetDir = "";

    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSets);
    }
         
    void LoadDataSets()
    {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        objectTracker.Stop();

        string[] files = Directory.GetFiles(dataSetDir, "*.xml");

        foreach (string dataSetFile in files)
        {
            string dataSetName = dataSetFile.Split('/').Last();
            Debug.Log("Found dataset: " + dataSetName);
            
            DataSet dataSet = objectTracker.CreateDataSet();

            if (dataSet.Load(dataSetFile, VuforiaUnity.StorageType.STORAGE_ABSOLUTE)) 
            {
                if (!objectTracker.ActivateDataSet(dataSet)) 
                {
                    // Note: ImageTracker cannot have more than 1000 total targets activated
                    Debug.Log("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
                }

                int counter = 0;
    
                IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
                foreach (TrackableBehaviour tb in tbs) {
                    Debug.Log("Found TrackableBehavior: " + tb.name + ", in " + dataSetName);
                    if (tb.name == "New Game Object") {
                        // change generic name to include trackable name
                        tb.gameObject.name = "ImageTarget:" + dataSetName + (++counter) + ":" + tb.TrackableName;
    
                        // add additional script components for trackable
                        tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
                        tb.gameObject.AddComponent<TurnOffBehaviour>();
    
                        if (augmentationObject != null) {
                            GameObject augmentation = (GameObject)GameObject.Instantiate(augmentationObject);
                            augmentation.transform.parent = tb.gameObject.transform;
                            augmentation.transform.localPosition = new Vector3(0f, 0f, 0f);
                            augmentation.transform.localRotation = Quaternion.identity;
                            augmentation.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
                            augmentation.gameObject.SetActive(true);
                        } else {
                            Debug.Log("<color=yellow>Warning: No augmentation object specified for: " + tb.TrackableName + "</color>");
                        }
                    }
                }
            } 
            else {
                Debug.LogError("<color=yellow>Failed to load dataset: '" + dataSetFile + "'</color>");
            }
        }

        if (!objectTracker.Start()) 
        {
            Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
        }
    }
}
