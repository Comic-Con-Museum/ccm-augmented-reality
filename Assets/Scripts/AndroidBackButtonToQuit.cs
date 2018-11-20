using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButtonToQuit : MonoBehaviour {

    void FixedUpdate() {
        if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)) {
                Application.Quit();
            }
        }
    }
    
}
