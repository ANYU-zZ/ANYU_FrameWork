/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：TestDatasInfo.cs
 *  作者：ANYU
 *  日期：2024/7/26 11:1:12
 *  功能：Nothing
*************************************************************************/

using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ANYU_FrameWork
{
    public class TestDatasInfo : UIPanelBase
    {
        public Transform tran_Content;

        private void Start()
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(async () =>
            {
                Debug.Log("一次");
                UIManager.Instance.ShowUI("Video_Panel");
            });
        }

        public override void OnShow(object[] objects = null)
        {
            ListInitateData(objects);
        }

        public void ListInitateData(object[] objects = null) // 实例化字典数据内容
        {
            List<DataInfo> list = new List<DataInfo>();

            if (objects != null)
            {
                foreach (DataInfo item in objects)
                {
                    list.Add(item);
                }
            }

            this.CreatePre(tran_Content, list, (par, data, index) =>
            {
                par.name = data.Name;

                this.CreatePre(par, new List<string> { data.ID, data.Name, data.Age, data.Sex, data.ETC }, (a, b, c) =>
                {
                    a.name = "Text_" + b;
                    a.GetComponentInChildren<TextMeshProUGUI>().text = b;
                }, startIndex: 0);
            }, startIndex: 1);
        }
    }
}

