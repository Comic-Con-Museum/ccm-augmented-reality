using UnityEngine;
using System.Collections; 
using Vuforia;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 
 
public class DynamicDataSetLoader : MonoBehaviour
{
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
    
                        var prefab = Resources.Load("Prefabs/" + tb.TrackableName, typeof(GameObject));
                        if (prefab == null)
                        {
                            Debug.Log("<color=yellow>Warning: No augmentation object available for: " + tb.TrackableName + "</color>"); 
                        }
                        else
                        {
                            GameObject augmentation = Instantiate(Resources.Load("Prefabs/" + tb.TrackableName, typeof(GameObject))) as GameObject;
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

        if (!objectTracker.Start()) 
        {
            Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
        }
    }
}
