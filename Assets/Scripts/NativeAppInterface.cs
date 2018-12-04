using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeAppInterface : MonoBehaviour {

	public static string CurrentExperience { get; private set; }

#if UNITY_ANDROID && !UNITY_EDITOR
	private static string collectionEventEndpoint = "newCollectionEvent";
	private static string currentExperienceEndpoint = "getExperience";
	private static string contentItemDataEndpoint = "getContentModel";
	private static string loadedEndpoint = "newLoadingCompletedEvent";
	private static string viewNativeContentEndpoint = "newViewContentEvent";
	
	private static AndroidJavaObject currentActivity;
#endif

	void Awake () {
        Screen.fullScreen = false;
	}

	void Start () {
		
	#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Current Android activity initialized");

		Debug.Log("Streaming assets: " + Application.streamingAssetsPath);

		CurrentExperience = currentActivity.Call<string>(currentExperienceEndpoint);
		Debug.Log("Current experience: " + CurrentExperience);
		
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

	public static void RegisterNewCollection(string contentId) {
		Debug.Log("Registering collection for contentId: " + contentId);
	
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
				break;
		}
	#endif
	
	}

	public static void NotifyVuforiaLoaded(int initCode) {
		Debug.Log("Sending Init code " + initCode + " to native app.");
	
	#if UNITY_ANDROID && !UNITY_EDITOR
		currentActivity.Call(loadedEndpoint, initCode);
		// Debug.Log("Got result: " + result);
	#endif

	}

	public static void GetContentItemInfo(string contentId) {
		Debug.Log("Retreiving info for contentId: " + contentId);

	#if UNITY_ANDROID && !UNITY_EDITOR
		var info = currentActivity.Call<AndroidJavaObject>(contentItemDataEndpoint, contentId);

	#endif

	}
}