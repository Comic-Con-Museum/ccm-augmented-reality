using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeAppInterface : MonoBehaviour {

#if UNITY_ANDROID && !UNITY_EDITOR
	private string collectionEventEndpoint = "newCollectionEvent";
	private string getDataEndpoint = "getData";
	private AndroidJavaObject currentActivity;
#endif
	void Start () {
		
	#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Current Android activity initialized");
		
		// var intent = currentActivity.Call<AndroidJavaObject>("getIntent");
		// Debug.Log("Got intent: " + intent);
		// var hasExtra = intent.Call<bool> ("foo");
		// var fooRet = currentActivity.Call<string> ("foo", "this is an integer");
		// Debug.Log("foo() called. Got: " + fooRet);

		// var unityObj = new AndroidJavaObject("com.comic_con.museum.ar.experience.ExperienceActivity.FooObject");
		// unityObj.Set<string>("str", "string2");
		// unityObj.Set<AndroidJavaObject>("foo", null);

		// var unityObj2 = new AndroidJavaObject("com.comic_con.museum.ar.experience.ExperienceActivity.FooObject");
		// unityObj2.Set<string>("str", "string1");
		// unityObj2.Set<AndroidJavaObject>("foo", unityObj);

		// int newCollectionEvent(string contentId);

		// var fooObj = currentActivity.Call<AndroidJavaObject>("foo", unityObj2);
		// while (fooObj != null)
		// {
		// 	Debug.Log("Got: " + fooObj.Get<string>("str"));
		// 	fooObj = fooObj.Get<AndroidJavaObject>("foo");			
		// }
		// Debug.Log(fooObj.Get("str") + " " + fooObj.Get("foo").Get("str"));
	#endif
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static void RegisterNewCollection(string contentId) {
	
	#if UNITY_ANDROID && !UNITY_EDITOR
		var result = currentActivity.Call<int>(collectionEventEndpoint, contentId);
		switch (result)
		{
			case 0:
				Debug.Log("Item collected; Progress updated");
				break;
			case 1:
				Debug.Log("Item collected; Progress not updated");
				break;
			case 2:
				Debug.Log("Item already collected");
				break;
			case 500:
				Debug.Log("No active experience; Collection unsuccessful");
				break;
			case 501:
				Debug.Log("Item not part of current experience");
				break;
			default:
				Debug.LogError("Error calling native Android method");
		}
	#endif
	
	}
}