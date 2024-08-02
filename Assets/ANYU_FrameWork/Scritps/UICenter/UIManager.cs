/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：UIManager.cs
 *  作者：ANYU
 *  日期：2024/7/25 15:48:15
 *  功能：Nothing
*************************************************************************/

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ANYU_FrameWork
{
    public class UIManager : MonoSingleton<UIManager>
    {
        /// <summary>
        /// 所有已配置PanelBaseExtend的面板
        /// </summary>
        protected Dictionary<string, PanelBaseExtend> AllDefinesDatas = new Dictionary<string, PanelBaseExtend>();

        /// <summary>
        /// 所有添加UIPanelBase的面板
        /// </summary>
        protected Dictionary<string, UIPanelBase> AllPanels = new Dictionary<string, UIPanelBase>();

        /// <summary>
        /// 所有已经展示的面板
        /// </summary>
        protected Dictionary<string, UIPanelBase> ShownPanels = new Dictionary<string, UIPanelBase>();

        /// <summary>
        /// 静态面板
        /// </summary>
        protected Transform Static_Panel;

        /// <summary>
        /// 动态面板
        /// </summary>
        protected Transform Dynamic_Panel;

        public async void Start()
        {
            Init();
            await UniTask.Delay(200);
            GetDefineInfo();
        }

        void Update()
        {
            ResopeneKeyDown();
        }

        void Init()
        {
            GameObject UI = GameObject.Find("UIRoot");
            if (UI == null)
            {
                UI = Instantiate(Resources.Load("UIRoot") as GameObject);
            }

            UI.name = UI.name.Replace("(Clone)", "");
            Static_Panel = UI.transform.Find("Static_Panel");
            Dynamic_Panel = UI.transform.Find("Dynamic_Panel");
        }

        /// <summary>
        /// 常用UI通用事件调用
        /// </summary>
        /// <returns></returns>
        public void DoUINormalEvent(UIEvent uIEvent, object[] userDatas = null)
        {
            switch (uIEvent)
            {
                case UIEvent.WindowTip:
                    ShowUI("WindowTip", userDatas);
                    break;
            }
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <returns></returns>
        public void ShowUI(string panel, object[] userDatas = null)
        {
            if (AllPanels.ContainsKey(panel))
            {
                OpenSet(panel, AllPanels[panel], userDatas);
            }
            else
            {
                CreateUI(panel, userDatas);
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <returns></returns>
        public void HideUI(string panelName)
        {
            if (ShownPanels.ContainsKey(panelName))
            {
                UIPanelBase panel = ShownPanels[panelName];
                panel.OnHide();
                panel.gameObject.SetActive(false);
                ShownPanels.Remove(panelName);
            }
            else if (AllPanels.ContainsKey(panelName))
            {
                Debug.Log($"面板-{panelName}-已隐藏");
            }
            else
            {
                Debug.LogError($"面板-{panelName}-未加载");
            }
        }

        /// <summary>
        /// 刷新UI
        /// </summary>
        /// <returns></returns>
        public void RefreshUI(UIPanelBase panel, object[] userDatas = null)
        {
            panel.Refresh(userDatas);
        }

        /// <summary>
        /// 触发指定UI面板的返回事件
        /// </summary>
        /// <returns></returns>
        public void ReturnUI(UIPanelBase panel)
        {
            if (panel.GetComponent<PanelReturn>())
            {
                PanelReturn returnPanel = panel.GetComponent<PanelReturn>();
                if (returnPanel.returnPanelName != "")
                {
                    ShowUI(returnPanel.returnPanelName, null);
                }
                if (returnPanel.sceneName != "")
                {
                    SceneManager.LoadScene(returnPanel.sceneName);
                }
            }
        }

        /// <summary>
        /// 实时检测当前案件并对可触发UI进行操作
        /// </summary>
        /// <returns></returns>
        void ResopeneKeyDown()
        {
            if (Helper.GetNowGetKeyDown() != KeyCode.None)
            {
                foreach (var i in AllDefinesDatas)
                {
                    //if (i.Value.shortcotKey == Helper.GetNowGetKeyDown())
                    //{
                    //    ReverseShowUI(i.Key);
                    //}
                }
            }
        }

        /// <summary>
        /// 反向操作现有UI，若已经打开则关闭，反之则打开
        /// </summary>
        /// <returns></returns>
        public void ReverseShowUI(string panelName, object[] userDatas = null)
        {
            if (ShownPanels.ContainsKey(panelName))
            {
                HideUI(panelName);
            }
            else if (AllPanels.ContainsKey(panelName))
            {
                ShowUI(panelName, userDatas);
            }
            else
            {
                Debug.Log($"面板-{panelName}-未加载");
            }
        }

        /// <summary>
        /// 获取加载到资源配置器里的PanelBaseExtend对象数据
        /// </summary>
        /// <returns></returns>
        void GetDefineInfo()
        {
            AllDefinesDatas = ResourceManager.Instance.GetAllClassData<PanelBaseExtend>();
            Debug.Log(AllDefinesDatas.Count);
        }

        /// <summary>
        /// 加载UI
        /// </summary>
        /// <returns></returns>
        UIPanelBase CreateUI(string panelName, object[] userDatas = null)
        {
            UIPanelBase panel;
            PanelBaseExtend baseExtend = AllDefinesDatas[panelName];
            if (baseExtend == null)
            {
                Debug.LogError("该UI未配置");
                return null;
            }

            //Resource下UI预制体路径
            string goPath = "UIPanel/" + baseExtend.panelName;

            GameObject panelGo = Resources.Load(goPath) as GameObject;

            if (panelGo == null)
            {
                Debug.LogError("未找到UI的配置资源预制体");
                return null;
            }

            GameObject inGo = Instantiate(panelGo, transform);

            if (!inGo.GetComponent<UIPanelBase>())
            {
                inGo.AddComponent<UIPanelBase>();
            }

            panel = inGo.GetComponent<UIPanelBase>();

            if (panel == null)
            {
                Debug.LogError("该UI资源未配置PanelBase");
                Destroy(inGo);
                return null;
            }

            AllPanels.Add(panelName, panel);

            OpenSet(panelName, panel, userDatas);

            inGo.name = inGo.name.Replace("Clone", "");
            inGo.transform.localPosition = panelGo.transform.position;
            inGo.transform.localScale = panelGo.transform.localScale;

            inGo.GetComponent<RectTransform>().sizeDelta = panelGo.GetComponent<RectTransform>().sizeDelta;
            return panel;
        }

        /// <summary>
        /// 开启UI时对Panelbase进行的数据操作
        /// </summary>
        /// <returns></returns>
        void OpenSet(string panelName, UIPanelBase panel, object[] userDatas = null)
        {
            switch (panel.LayerType)
            {
                case LayerType.Home:
                    panel.transform.parent = Static_Panel;
                    break;
                case LayerType.Tip:
                    panel.transform.parent = Dynamic_Panel;
                    break;
                case LayerType.CZ:
                    panel.transform.parent = Static_Panel;
                    break;
            }

            panel.OnShow(userDatas);

            if (!ShownPanels.ContainsKey(panelName))
            {
                ShownPanels.Add(panelName, panel);
            }

            if (AllDefinesDatas[panelName].isHideOther)
            {
                foreach (var i in ShownPanels)
                {
                    if (i.Value.LayerType == LayerType.Tip)
                    {
                        HideUI(i.Key);
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏所有可以隐藏的UI
        /// </summary>
        /// <returns></returns>
        void HideOtherUI()
        {
            foreach (var i in ShownPanels)
            {
                HideUI(i.Key);
            }
        }
    }
}
