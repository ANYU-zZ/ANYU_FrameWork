/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：StreamingAssetPathConfigReader.cs
 *  作者：ANYU
 *  日期：2024/8/1 14:20:26
 *  功能：Nothing
*************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.IO;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Security.Policy;
using Unity.VisualScripting;

namespace ANYU_FrameWork
{
    public static class StreamingAssetPathConfigReader
    {
        public delegate void ReadFinishEvtHandler(EventArgs e);
        public static event ReadFinishEvtHandler OnReadFinishEvtHandler;
        /// <summary>
        /// 读取StreamingAsset中的配置文件
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="action"></param>
        /// <returns></returns>

        public static IEnumerator TextReader(string configName, UnityAction<string> action = null)
        {
            string path;
#if UNITY_WIN_STANDALONE || UNITY_IPHONE && !UNITY_EDITOR
        path ="file://"+ Application.streamingAssetsPath + configName;
#else
            path = Application.streamingAssetsPath + "/" + configName;
#endif
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.error != null)
                Debug.Log(unityWebRequest.error);
            else
            {
                string content = unityWebRequest.downloadHandler.text;
                if (action != null)
                    action(content);
                if (OnReadFinishEvtHandler != null)
                {
                    OnReadFinishEvtHandler.Invoke(EventArgs.Empty);
                }
            }
        }

        public static async UniTask<string> TextReader(string configName)
        {
            string path;
#if UNITY_WIN_STANDALONE || UNITY_IPHONE && !UNITY_EDITOR
        path ="file://"+ Application.streamingAssetsPath + configName;
#else
            path = Application.streamingAssetsPath + "/" + configName;
#endif
            // 检查文件是否存在
            if (!FileExists(path))
            {
                Debug.Log($"File not found: {path}");
                return "";
            }

            UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);

            await unityWebRequest.SendWebRequest();

            if (unityWebRequest.error != null)
            {
                Debug.Log(unityWebRequest.error);
                return "";
            }
            else
            {
                return unityWebRequest.downloadHandler.text;
            }
        }

        private static bool FileExists(string path)
        {
            // 移除file://前缀以便使用File.Exists检查文件
            if (path.StartsWith("file://"))
            {
                path = path.Substring("file://".Length);
            }

            return File.Exists(path);
        }

        /// <summary>
        /// 读取streamingAsset中的图片
        /// </summary>
        /// <param name="mediaName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerator TextureReader(string mediaName, UnityAction<Texture> action)
        {
            string path;
#if UNITY_WIN_STANDALONE || UNITY_IPHONE && !UNITY_EDITOR
        path ="file://"+ Application.streamingAssetsPath + configName;
#else
            path = Application.streamingAssetsPath + "/" + mediaName;
#endif

            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(path);
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.error != null)
                Debug.Log(unityWebRequest.error);
            else
            {
                byte[] bts = unityWebRequest.downloadHandler.data;
                if (action != null)
                {
                    action(DownloadHandlerTexture.GetContent(unityWebRequest));
                }
            }
        }


        public static async UniTask<Texture2D> TextureReader(string configName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, configName);
            Debug.Log("Path:" + path);

            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(path);

            await unityWebRequest.SendWebRequest();


            if (unityWebRequest.error != null)
            {
                Debug.Log(unityWebRequest.error);
                return Texture2D.whiteTexture;
            }
            else
            {
                return DownloadHandlerTexture.GetContent(unityWebRequest);
            }
        }

        /// <summary>
        /// 读取streamingAsset文件夹中的多媒体（音频）
        /// </summary>
        /// <param name="mediaName"></param>
        /// <param name="action"></param>
        /// <returns></returns>

        public static IEnumerator AudioClipReader(string mediaName, UnityAction<AudioClip> action = null)
        {
            string path;
#if UNITY_WIN_STANDALONE || UNITY_IPHONE && !UNITY_EDITOR
        path ="file://"+ Application.streamingAssetsPath + configName;
#else
            path = Application.streamingAssetsPath + "/" + mediaName;
#endif
            FileInfo fileInfo = new FileInfo(path);
            string fileExtension = fileInfo.Extension;
            AudioType audioType;

            switch (fileExtension)
            {
                case ".mp3":
                    audioType = AudioType.MPEG;
                    break;
                case ".ogg":
                    audioType = AudioType.OGGVORBIS;
                    break;
                case ".wav":
                    audioType = AudioType.WAV;
                    break;
                case ".aiff":
                    audioType = AudioType.AIFF;
                    break;
                default:
                    audioType = AudioType.MPEG;
                    break;
            }

            UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(path, audioType);
            yield return unityWebRequest.SendWebRequest();


            if (unityWebRequest.error != null)
                Debug.Log(unityWebRequest.error);
            else
            {
                if (action != null)
                    action(DownloadHandlerAudioClip.GetContent(unityWebRequest));
            }
        }
    }
}
