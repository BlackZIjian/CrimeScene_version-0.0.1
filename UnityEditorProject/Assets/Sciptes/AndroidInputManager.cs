using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AndroidInputManager : MonoBehaviour {

    // Use this for initialization
    void Start() {
        aaa();
    }

    // Update is called once per frame
    void Update() {

    }
    void aaa()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("RegisterSensor", 0);
        
    }

}

