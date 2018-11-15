using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenExitBtn : MonoBehaviour {

    public void ExitBtn() {
        Application.Quit();
        Debug.Log("exit btn pressed");
    }
}
