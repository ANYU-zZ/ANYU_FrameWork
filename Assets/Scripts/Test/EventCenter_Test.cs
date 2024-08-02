/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：Test01.cs
 *  作者：ANYU
 *  日期：2024/7/25 14:14:50
 *  功能：测试事件中心功能
*************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANYU_FrameWork
{
    public class EventCenter_Test : UIPanelBase
    {
        private void OnEnable()
        {
            EventCenter.AddListener(EventsType.Test01, Test);
            EventCenter.AddListener<string>(EventsType.Test02, Test_HanCan);
        }

        private void Start()
        {
            transform.Find("btn_01").GetComponent<Button>().onClick.AddListener(() =>
            {
                EventCenter.TriggerEvent(EventsType.Test01);
            });

            transform.Find("btn_02").GetComponent<Button>().onClick.AddListener(() =>
            {
                EventCenter.TriggerEvent(EventsType.Test02, "A");
            });
        }

        public void Test()
        {
            Debug.Log("当前传入的消息内容没有参数:" + gameObject.name);
        }

        public void Test_HanCan(string str)
        {
            Debug.Log("当前传入的消息内容含有一个参数，内容为:" + str);
            List<object> list = new List<object>();
            foreach (var i in ResourceManager.Instance.GetAllClassData<DataInfo>())
            {
                list.Add(i.Value);
            }
            UIManager.Instance.ShowUI("Datas_Panel", list.ToArray());
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener(EventsType.Test01, Test);
            EventCenter.RemoveListener<string>(EventsType.Test02, Test_HanCan);
        }
    }
}
