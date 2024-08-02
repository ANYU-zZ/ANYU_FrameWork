/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：Test.cs
 *  作者：ANYU
 *  日期：2024/8/1 17:28:29
 *  功能：Nothing
*************************************************************************/

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANYU_FrameWork
{
    public class Test : MonoBehaviour
    {
        async void Start()
        {
            await UniTask.Delay(500);
            UIManager.Instance.ShowUI("Home_Panel");
        }
    }
}
