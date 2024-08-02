/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：EventCenter.cs
 *  作者：ANYU
 *  日期：2024/7/25 10:27:13
 *  功能：消息中心控制
*************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ANYU_FrameWork
{
    public class EventCenter
    {
        private static Dictionary<EventsType, Delegate> handlerDic = new Dictionary<EventsType, Delegate>();

        /// <summary>
        /// 添加事件的监听（无参数）
        /// <param name="eventsType">事件的枚举名称</param>
        /// <param name="callBack">事件处理函数</param>
        public static void AddListener(EventsType eventsType, CallBack callBack)
        {
            if (!handlerDic.ContainsKey(eventsType))
            {
                handlerDic.Add(eventsType, callBack);
            }
            else
            {
                handlerDic[eventsType] = (CallBack)Delegate.Combine(handlerDic[eventsType], callBack);
            }
        }

        /// <summary>
        /// 添加事件的监听（一个参数）
        /// <param name="eventsType">事件的枚举名称</param>
        /// <param name="callBack">事件处理函数</param>
        public static void AddListener<T>(EventsType eventsType, CallBack<T> callBack)
        {
            if (!handlerDic.ContainsKey(eventsType))
            {
                handlerDic.Add(eventsType, callBack);
            }
            else
            {
                handlerDic[eventsType] = (CallBack<T>)Delegate.Combine(handlerDic[eventsType], callBack);
            }
        }

        /// <summary>
        /// 移除事件的监听者(无参数)
        /// </summary>
        /// <param name="eventsType">事件名</param>
        /// <param name="callBack">事件处理函数</param>
        public static void RemoveListener(EventsType eventsType, CallBack callBack)
        {
            if (handlerDic.ContainsKey(eventsType))
            {
                handlerDic[eventsType] = (CallBack)Delegate.Remove(handlerDic[eventsType], callBack);

                // 如果该事件类型没有更多的回调，移除它
                if (handlerDic[eventsType] == null)
                {
                    handlerDic.Remove(eventsType);
                }
            }
        }

        /// <summary>
        /// 移除事件的监听者(无参数)
        /// </summary>
        /// <param name="eventsType">事件名</param>
        /// <param name="callBack">事件处理函数</param>
        public static void RemoveListener<T>(EventsType eventsType, CallBack<T> callBack)
        {
            if (handlerDic.ContainsKey(eventsType))
            {
                handlerDic[eventsType] = (CallBack<T>)Delegate.Remove(handlerDic[eventsType], callBack);

                // 如果该事件类型没有更多的回调，移除它
                if (handlerDic[eventsType] == null)
                {
                    handlerDic.Remove(eventsType);
                }
            }
        }

        /// <summary>
        /// 触发事件（无参数）
        /// </summary>
        /// <param name="eventsType">事件名</param>
        /// <param name="sender">触发源</param>
        public static void TriggerEvent(EventsType eventsType)
        {
            if (handlerDic.TryGetValue(eventsType, out Delegate d))
            {
                // 调试信息，查看找到的委托类型
                Debug.Log($"Triggering event: {eventsType}, Delegate Type: {d.GetType()}");

                if (d is CallBack callBack)
                {
                    callBack();
                }
                else
                {
                    throw new Exception($"广播事件错误：事件{eventsType}对应委托具有不同的类型");
                }
            }
            else
            {
                Debug.LogWarning($"No event found for {eventsType}");
            }
        }

        /// <summary>
        /// 触发事件（一个参数）
        /// </summary>
        /// <param name="eventsType">事件名</param>
        /// <param name="arg">事件参数</param>
        public static void TriggerEvent<T>(EventsType eventsType, T arg)
        {
            if (handlerDic.TryGetValue(eventsType, out Delegate d))
            {
                // 调试信息，查看找到的委托类型
                Debug.Log($"Triggering event: {eventsType}, Delegate Type: {d.GetType()}");

                if (d is CallBack<T> callBack)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception($"广播事件错误：事件{eventsType}对应委托具有不同的类型");
                }

            }
            else
            {
                Debug.LogWarning($"No event found for {eventsType}");
            }
        }
        /// <summary>
        /// 清空所有事件
        /// </summary>
        public static void Clear()
        {
            handlerDic.Clear();
        }
    }
}
