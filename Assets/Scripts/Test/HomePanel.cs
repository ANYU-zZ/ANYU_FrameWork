/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：HomePanel.cs
 *  作者：ANYU
 *  日期：2024/7/29 11:39:58
 *  功能：Nothing
*************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANYU_FrameWork
{
    public class HomePanel : UIPanelBase
    {
        void Start()
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.Instance.ShowUI("EventCenter_Panel");
            });
        }
    }
}
