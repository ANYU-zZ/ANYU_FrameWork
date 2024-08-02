/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：Video.cs
 *  作者：ANYU
 *  日期：2024/8/2 11:22:50
 *  功能：Nothing
*************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANYU_FrameWork
{
    /// <summary>
    /// 音频类
    /// </summary>
    public class Video
    {
        //public string name;
        public string url { get; set; }
        //public int frame;
        //public int width;
        //public int height;
        public Video(string url)
        {
            this.url = url;
        }
    }
}
