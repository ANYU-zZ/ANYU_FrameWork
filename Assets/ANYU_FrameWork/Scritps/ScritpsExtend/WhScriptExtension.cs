/*******************************************************************************
* 版权声明：北京润尼尔网络科技有限公司，保留所有版权
* 版本声明：v1.0.0
* 类 名 称：WhScriptExtension
* 创建日期：2022-03-25 11:36:53
* 作者名称：王豪
* CLR 版本：4.0.30319.42000
* 修改记录：
* 描述：
******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public static class WhScriptExtension
{
    /// <summary>
    /// 根据数据列表设置子物体的信息,父物体下第一个子物体作为模板
    /// </summary>
    /// <typeparam name="Q">MonoBehaviour</typeparam>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="q">MonoBehaviour脚本</param>
    /// <param name="par">父物体</param>
    /// <param name="data">数据列表</param>
    /// <param name="setPreAction">子物体配置方法,参数为子物体及对应的数据</param>
    /// <param name="setNewPreAction">新创建的子物体额外的配置方法,如绑定按钮事件</param>
    public static void CreatePre<Q, T>(this Q q, Transform par, List<T> data, Action<Transform, T, int> setPreAction = null, Action<Transform, T, int> setNewPreAction = null, int startIndex = 0) where Q : MonoBehaviour
    {
        if (par.childCount - startIndex > data.Count)
        {
            for (int i = startIndex; i < data.Count + startIndex; i++)
            {
                setPreAction(par.GetChild(i), data[i - startIndex], i - startIndex);
                par.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = data.Count + startIndex; i < par.childCount; i++)
            {
                par.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = startIndex; i < par.childCount; i++)
            {
                setPreAction(par.GetChild(i), data[i - startIndex], i - startIndex);
                par.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = par.childCount; i < data.Count + startIndex; i++)
            {
                Transform temp = GameObject.Instantiate(par.GetChild(startIndex), par);
                setNewPreAction?.Invoke(temp, data[i - startIndex], i - startIndex);
                setPreAction(temp, data[i - startIndex], i - startIndex);
            }
        }
    }

    /// <summary>
    /// 改变Toggle状态为ture并执行Toggle为true时的方法
    /// </summary>
    /// <param name="tog"></param>
    public static void RunTogTrueAction(this Toggle tog)
    {
        if (tog.isOn)
        {
            tog.onValueChanged.Invoke(true);
        }
        else
        {
            tog.isOn = true;
        }
    }

    /// <summary>
    /// TextMeshProUGUI扩展DOText
    /// </summary>
    /// <param name="t"></param>
    /// <param name="str">目标语句</param>
    /// <param name="time">消耗时间</param>
    /// <returns></returns>
    public static Tweener DOText(this TextMeshProUGUI t, string str, float time, Action updateAction = null)
    {
        if (t.text == str) time = 0;
        string temp = "";
        return DOTween.To(() => temp, x => temp = x, str, time).OnUpdate(() =>
        {
            t.text = temp;
            updateAction?.Invoke();
        });
    }

    ///// <summary>
    ///// 添加鼠标滚轮事件(返回值true为下滑)
    ///// </summary>
    ///// <typeparam name="Q"></typeparam>
    ///// <param name="q"></param>
    ///// <param name="scrollAction"></param>
    //public static void OnScorll<Q>(this Q q, Action<bool> scrollAction) where Q : MonoBehaviour
    //{
    //    ScrollActionTool tool = q.GetComponent<ScrollActionTool>();
    //    if (tool == null) tool = q.gameObject.AddComponent<ScrollActionTool>();
    //    tool.AddListen(scrollAction);
    //}
}
