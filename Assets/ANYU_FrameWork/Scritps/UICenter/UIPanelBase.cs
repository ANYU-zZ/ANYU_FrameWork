/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：UIPanelBase.cs
 *  作者：ANYU
 *  日期：2024/7/25 15:1:54
 *  功能：Nothing
*************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ANYU_FrameWork
{
    public class UIPanelBase : MonoBehaviour
    {
        public bool IsDontHide;
        private bool DisableControl;
        public LayerType LayerType;

        /// <summary>
        /// UI打开时调用
        /// </summary>
        /// <returns></returns>
        public virtual void OnShow(object[] objects = null)
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 重新打开UI
        /// </summary>
        /// <returns></returns>
        public virtual void ReShow(object[] objects = null)
        {

        }

        /// <summary>
        /// UI隐藏时调用
        /// </summary>
        /// <returns></returns>
        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 刷新当前UI布局
        /// </summary>
        public virtual void Refresh(object[] objects = null)
        {

        }
    }
}
