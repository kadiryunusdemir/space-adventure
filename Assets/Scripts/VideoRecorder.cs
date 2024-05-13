using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class VideoRecording : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void StartVideoRecording();
    [DllImport("__Internal")]
    private static extern void StopVideoRecording(string str);

    public void startVideo()
    {
        StartVideoRecording();

    }
    public void stopVideo(string str)
    {
        StopVideoRecording(str);

    }
}
