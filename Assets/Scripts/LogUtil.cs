using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogUtil
{
    public static event Action<string> LogEvent;

    public static void DebugLog(string info)
    {
        if(!string.IsNullOrEmpty(info))
        {
            LogEvent?.Invoke(info);
        }
    }
}
