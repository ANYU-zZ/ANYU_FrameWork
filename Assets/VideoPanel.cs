/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：VideoPanel.cs
 *  作者：ANYU
 *  日期：2024/8/2 10:53:25
 *  功能：Nothing
*************************************************************************/

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ANYU_FrameWork
{
    public class VideoPanel : UIPanelBase
    {
        public Transform raw_Parent;
        private int currentVideoIndex = 0;
        void Start()
        {
            transform.Find("btn_A").GetComponent<Button>().onClick.AddListener(() =>
            {

            });
        }

        public override async void OnShow(object[] objects = null)
        {
            base.OnShow(objects);
            await UniTask.Delay(1000);

            VideoControl(objects);
        }

        public async void VideoControl(object[] objects = null)
        {
            List<Video> list = new List<Video>();

            Dictionary<string, Video> result = await ResourceManager.Instance.GetAllFileData<Video>();
            foreach (var i in result)
            {
                list.Add(i.Value);
            }

            this.CreatePre(raw_Parent, list, (pre, data, index) =>
            {
                // 创建RenderTexture
                RenderTexture renderTexture = new RenderTexture(1920, 1080, 0);
                pre.GetComponent<VideoPlayer>().url = data.url;
                pre.GetComponent<VideoPlayer>().targetTexture = renderTexture;
                pre.GetComponent<RawImage>().texture = renderTexture;
                OnVideoLoopPointReached(pre.GetComponent<VideoPlayer>(), renderTexture, data.url);
            }, startIndex: 0);
        }

        /// <summary>
        /// 保留当前帧
        /// </summary>
        /// <param name="videoPlayer"></param>
        /// <param name="renderTexture"></param>
        /// <param name="url"></param>
        void OnVideoLoopPointReached(VideoPlayer videoPlayer, RenderTexture renderTexture, string url)
        {
            StartCoroutine(PrepareAndPlayNextVideo(videoPlayer, renderTexture, url));
        }

        IEnumerator PrepareAndPlayNextVideo(VideoPlayer videoPlayer, RenderTexture renderTexture, string url)
        {
            videoPlayer.Pause();
            RenderTexture currentTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = currentTexture;

            videoPlayer.url = url;
            videoPlayer.Prepare();

            // 等待视频准备完成
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            videoPlayer.Play();
        }
    }
}
