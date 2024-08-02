/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：ConfigDataBase.cs
 *  作者：ANYU
 *  日期：2024/8/1 17:11:42
 *  功能：Nothing
*************************************************************************/

using System.Collections;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ANYU_FrameWork
{
    public class ConfigDataBase : MonoSingleton<ConfigDataBase>
    {
        /// <summary>
        /// 读取XML
        /// </summary>
        /// <returns></returns>
        public static XmlDocument ReadXML(string url)
        {
            XmlDocument dom = new XmlDocument();
            dom.Load(url);
            return dom;
        }

        /// <summary>
        /// 异步加载音频文件
        /// </summary>
        /// <returns></returns>
        public Task<AudioClip> LoadMusicAsync(string url)
        {
            TaskCompletionSource<AudioClip> tcs = new TaskCompletionSource<AudioClip>();
            StartCoroutine(LoadMusicCoroutine(url, tcs));
            return tcs.Task;
        }
        IEnumerator LoadMusicCoroutine(string url, TaskCompletionSource<AudioClip> tcs)
        {
            UnityWebRequest www = new UnityWebRequest();
            if (url.Contains(".wav") || url.Contains(".WAV"))
            {
                www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            }
            else if (url.Contains(".mp3") || url.Contains(".MP3"))
            {
                www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            }
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                Debug.Log("成功加载Wav文件");
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                tcs.SetResult(clip);
            }
            else
            {
                Debug.LogError(www.error);
            }
        }

        /// <summary>
        /// 异步加载图片文件
        /// </summary>
        /// <returns></returns>
        public Task<Sprite> LoadImageAsync(string url)
        {
            TaskCompletionSource<Sprite> tcs = new TaskCompletionSource<Sprite>();
            StartCoroutine(LoadImageCoroutine(url, tcs));
            return tcs.Task;
        }
        IEnumerator LoadImageCoroutine(string url, TaskCompletionSource<Sprite> tcs)
        {
            string path = "file:///" + url.Replace("\\", "/");
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                // 发送请求  
                yield return www.SendWebRequest();

                // 检查错误  
                if (www.error != null)
                {
                    Debug.LogError(www.error);
                    tcs.SetResult(null); // 可能根据需要处理错误  
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    if (texture != null)
                    {
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        tcs.SetResult(sprite);
                        Debug.Log("成功加载图片资源:" + url);
                    }
                    else
                    {
                        Debug.LogError("Texture is null.");
                        tcs.SetResult(null); // 处理 null 情况  
                    }
                }
            }
        }
    }
}
