using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    private string logText = "";
    private const int kMaxLogSize = 16382;
    private string url = "";

    private bool UIEnabled;

    // Start is called before the first frame update
    void Start()
    {
        UIEnabled = true;
        LogUtil.LogEvent += HandleLog;
        DataEmitter.Instance.Init();

        DataEmitter.Instance.SendData("Start");
    }

    private void OnDestroy()
    {
        LogUtil.LogEvent -= HandleLog;
    }

    private void HandleLog(string str)
    {
        logText += str + "\n";

        while (logText.Length > kMaxLogSize)
        {
            var index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }

        scrollViewVector.y = int.MaxValue;
    }

    private void OnGUI()
    {
        Rect logArea, controlArea;
        if (Screen.width < Screen.height)
        {
            // Portrait mode
            controlArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
            logArea = new Rect(0.0f, Screen.height * 0.5f, Screen.width, Screen.height * 0.5f);
        }
        else
        {
            // Landscape mode
            controlArea = new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height);
            logArea = new Rect(Screen.width * 0.5f, 0.0f, Screen.width * 0.5f, Screen.height);
        }

        GUILayout.BeginArea(logArea);
        GUIDisplayLog();
        GUILayout.EndArea();

        GUILayout.BeginArea(controlArea);
        GUIDisplayControls();
        GUILayout.EndArea();
    }


    private void GUIDisplayLog()
    {
        scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
        GUILayout.Label(logText);
        GUILayout.EndScrollView();
    }

    private void GUIDisplayControls()
    {
        if (UIEnabled)
        {
            controlsScrollViewVector = GUILayout.BeginScrollView(controlsScrollViewVector);
            GUILayout.BeginVertical();

            url = GUILayout.TextField(url);

            if (GUILayout.Button("Send Test Data"))
            {
                SendTestData();
            }

            if(GUILayout.Button("Use Specified Url"))
            {
                SendTestDataWithUrl();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }

    private void SendTestData()
    {
        DataEmitter.Instance.SendData("Test Data");
    }

    private void SendTestDataWithUrl()
    {
        if (!string.IsNullOrEmpty(url))
        {
            DataEmitter.Instance.InitServerUrl(url);
            DataEmitter.Instance.SendData("Test Data 2222222222");
        }
    }
}
