﻿using System.Collections;
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

		CurrentExperience = currentActivity.Call<string>(currentExperienceEndpoint);
		Debug.Log("Current experience: " + CurrentExperience);
	#endif
	
	}
	
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
	#endif

	}

	public static AndroidJavaObject GetContentItemInfo(string contentId) {
		Debug.Log("Retreiving info for contentId: " + contentId);
		AndroidJavaObject info = null;

	#if UNITY_ANDROID && !UNITY_EDITOR
		info = currentActivity.Call<AndroidJavaObject>(contentItemDataEndpoint, contentId);
	#endif

		return info;
	}

	public static void ViewContentItemInApp(string contentId) {
		Debug.Log("Viewing info for contentId " + contentId + " in app.");
	
	#if UNITY_ANDROID && !UNITY_EDITOR
		var result = currentActivity.Call<int>(viewNativeContentEndpoint, contentId);
		Debug.Log("Got result: " + result);
	#endif
	
	}
}