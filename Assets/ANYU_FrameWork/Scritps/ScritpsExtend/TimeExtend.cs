/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：TimeExtend.cs
 *  作者：ANYU
 *  日期：2024/8/1 17:29:45
 *  功能：Nothing
*************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANYU_FrameWork
{
    public static class TimeExtend
    {
        public static void SetDelay(this MonoBehaviour t, float time, UnityAction action)
        {
            new GameObject().AddComponent<theTime>().set(time, action);
        }
    }

    public class theTime : MonoBehaviour
    {
        public UnityAction action;
        public float time;
        public void set(float time, UnityAction action)
        {
            this.time = time;
            this.action = action;
        }
        float i = 0;
        void Update()
        {
            i += UnityEngine.Time.deltaTime;
            if (i > time)
            {
                action.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
