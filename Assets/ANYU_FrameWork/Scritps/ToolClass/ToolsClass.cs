/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：ToolsClass.cs
 *  作者：ANYU
 *  日期：2024/8/1 14:37:56
 *  功能：Nothing
*************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ANYU_FrameWork
{
    /// <summary>
    /// 存储配置内容
    /// </summary>
    public static class Configs
    {
        public static Dictionary<string, ConfigUrl> strDatasDict { get; set; }

        /// <summary>
        /// 文件存储
        /// </summary>
        public static Dictionary<Type, Dictionary<string, object>> FileConfigs = new Dictionary<Type, Dictionary<string, object>>();

        /// <summary>
        /// 类存储
        /// </summary>
        public static Dictionary<Type, Dictionary<string, object>> ClassConfigs = new Dictionary<Type, Dictionary<string, object>>();

        /// <summary>
        /// 类与ID对应
        /// </summary>
        public static Dictionary<string, ConfigData> ConfigTypeKeys = new Dictionary<string, ConfigData>();
    }

    /// <summary>
    /// 配置内容解析
    /// </summary>
    public class ConfigUrl
    {
        public string Url;
        public string DataType;
        public string Type;
        public string ReadMe;
    }

    /// <summary>
    /// 数据的类型枚举
    /// </summary>
    public enum ConfigDataType
    {
        File, Class
    }

    /// <summary>
    /// 数据的实例
    /// </summary>
    public class ConfigData
    {
        public Type baseClass;
        public ConfigDataType dataType;
        public ConfigData(Type baseClass, ConfigDataType dataType)
        {
            this.baseClass = baseClass;
            this.dataType = dataType;
        }
    }

    /// <summary>
    /// 面板类型
    /// </summary>
    public enum LayerType
    {
        Home,
        Tip,
        CZ
    }

    /// <summary>
    /// 
    /// </summary>
    public enum UIEvent
    {
        WindowTip = 0
    }
}
