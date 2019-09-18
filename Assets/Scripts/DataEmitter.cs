using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataEmitter : MonoBehaviour
{
    private static DataEmitter instance;
    public static DataEmitter Instance
    {
        get
        {
            if(null == instance)
            {
                string mgrName = typeof(DataEmitter).ToString();
                GameObject obj = GameObject.Find(mgrName);
                if(null == obj)
                {
                    obj = new GameObject(mgrName);
                }
                instance = obj.GetComponent<DataEmitter>();
                if(null == instance)
                {
                    instance = obj.AddComponent<DataEmitter>();
                }
            }

            return instance;
        }
    }

    private string dataCache;
    private string serverUrl;

    public void Init()
    {
        serverUrl = "";
    }

    public void InitServerUrl(string url)
    {
        serverUrl = url;
    }

    public void SendData(string data)
    {
        dataCache = data;
        StartCoroutine(DoUploadData());
    }

    private IEnumerator DoUploadData()
    {
        using (var inputStream = new MemoryStream())
        {
            var data = Encoding.UTF8.GetBytes(dataCache);
            var gZip = new System.IO.Compression.GZipStream(inputStream, System.IO.Compression.CompressionMode.Compress);
            gZip.Write(data, 0, data.Length);
            gZip.Close();

            var toUploadData = Convert.ToBase64String(inputStream.ToArray());
            if(null != toUploadData && toUploadData.Length > 0)
            {
                using (var request = UnityEngine.Networking.UnityWebRequest.Put(serverUrl, toUploadData))
                {
                    yield return request.SendWebRequest();

                    if(request.isHttpError || request.isNetworkError)
                    {
                        LogUtil.DebugLog("send data error: " + request.error);
                    }
                    else
                    {
                        LogUtil.DebugLog("send data success: " + dataCache);
                        dataCache = "";
                    }
                }
            }

            gZip.Dispose();
        }
    }
}
