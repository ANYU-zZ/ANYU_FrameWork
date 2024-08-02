/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：DefaultCompany
 *  项目：ANYU_FrameWork
 *  文件：ResourceManager.cs
 *  作者：ANYU
 *  日期：2024/8/1 14:32:44
 *  功能：Nothing
*************************************************************************/

using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ANYU_FrameWork
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        string ReadConfigURL;
        string DataURL;

        protected override async void Awake()
        {
            SetConfigURL();

            AddConfigTypeKey();

            await UniTask.Delay(100);
            AddConfigData();
        }

        /// <summary>
        /// 设置Config相关文件及文件夹路径
        /// </summary>
        public void SetConfigURL()
        {
#if UNITY_EDITOR
            ReadConfigURL = Application.streamingAssetsPath + "/" + "DatasInfo/ConfigUrl.json";
            DataURL = Application.streamingAssetsPath + "/";
#else
        ReadConfigURL = System.AppDomain.CurrentDomain.BaseDirectory +  "/Data/Config/ConfigURL.json";
        //ConfigURL = System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Config";
        DataURL = System.AppDomain.CurrentDomain.BaseDirectory + "/ResourcesInfo";
#endif
        }

        /// <summary>
        /// 通过读取配置文件类型反射unity类的键值对
        /// </summary>
        public void AddConfigTypeKey()
        {
            //文本信息存放地址
            StartCoroutine(StreamingAssetPathConfigReader.TextReader("DatasInfo/ConfigUrl.json", (jsonStr) =>
            {
                Configs.strDatasDict = JsonConvert.DeserializeObject<Dictionary<string, ConfigUrl>>(jsonStr);

                string debug = "";

                foreach (var keyValuePair in Configs.strDatasDict)
                {
                    string key = keyValuePair.Key;
                    ConfigUrl configUrl = keyValuePair.Value;

                    Debug.Log($"获取TypeKey:ConfigType--{key},DataType--{configUrl.Type}\n");
                    debug += $"获取TypeKey:ConfigType--{key},DataType--{configUrl.Type}\n";

                    Type type = TypeExtend.GetUType(configUrl.Type);
                    ConfigDataType dataType = (ConfigDataType)Enum.Parse(typeof(ConfigDataType), configUrl.DataType);
                    Configs.ConfigTypeKeys.Add(key, new ConfigData(type, dataType));

                    switch (dataType)
                    {
                        case ConfigDataType.File:
                            Configs.FileConfigs.Add(type, new Dictionary<string, object>());
                            break;
                        case ConfigDataType.Class:
                            Configs.ClassConfigs.Add(type, new Dictionary<string, object>());
                            break;
                    }
                }

                Debug.Log(debug);
            }));
        }

        /// <summary>
        /// 获取全部配置文件名称及路径
        /// </summary>
        /// <returns></returns>
        async void AddConfigData()
        {
            //文本信息存放地址
            foreach (var keyValuePair in Configs.strDatasDict)
            {
                string configType = keyValuePair.Key;
                Type type = Configs.ConfigTypeKeys[configType].baseClass;
                ConfigDataType dataType = Configs.ConfigTypeKeys[configType].dataType;

                ConfigUrl configUrl = keyValuePair.Value;
                string csvData = await StreamingAssetPathConfigReader.TextReader(configUrl.Url);

                Dictionary<string, object> csvParsedData = new Dictionary<string, object>();
                switch (configUrl.ReadMe)
                {
                    case "csv":
                        csvParsedData = GetConfigData(csvData);
                        break;
                    case "json":
                        csvParsedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(csvData);
                        break;
                    case "xml":
                        csvParsedData = null;
                        break;
                }

                switch (dataType)
                {
                    case ConfigDataType.File:
                        foreach (var data in csvParsedData)
                        {
                            string fillName = data.Key;

                            // 检查 data.Value 的类型并进行相应的转换
                            object url = string.Empty;
                            if (data.Value is string)
                            {
                                url = (string)data.Value;
                            }
                            else if (data.Value is JObject)
                            {
                                //url = ((JObject)data.Value)["Url"].ToString();
                                url = (JObject)data.Value;
                            }

                            Debug.Log($"成功读取config：type-{type},name--{fillName},url--{url}");
                            Configs.FileConfigs[type].Add(fillName, url);
                        }
                        break;
                    case ConfigDataType.Class:
                        foreach (var data in csvParsedData)
                        {
                            string className = data.Key;
                            object collection = data.Value;

                            object instance = Helper.GetTypeInstance(type, className, collection);
                            Configs.ClassConfigs[type].Add(className, instance);

                            Debug.Log($"成功实例化对象：type-{type},name--{className},\ninfo--{Helper.PrintProperties(instance, className, collection)}");
                        }
                        break;
                }
                //}));
            }
        }

        /// <summary>
        /// 添加指定文本内容信息
        /// </summary>
        /// <param name="csvData">文本内容</param>
        /// <param name="str_Datas">要存放的的字典</param>
        private Dictionary<string, object> GetConfigData(string csvData, int startIndex = 1)
        {
            Dictionary<string, object> parsedData = new Dictionary<string, object>();

            string[] rows = csvData.Split('\n');
            for (int i = startIndex; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');
                if (columns.Length >= 2)
                {
                    string key = columns[0];
                    object value = columns.ToList();
                    if (!parsedData.ContainsKey(key))
                    {
                        parsedData.Add(key, value);
                    }
                    else
                    {
                        parsedData[key] = value;
                    }
                }
            }

            return parsedData;
        }

        /// <summary>
        /// 加载数据成功
        /// </summary>
        public void OnLoadSuccess()
        {

        }

        /// <summary>
        /// 加载数据失败
        /// </summary>
        public void OnLoadFail()
        {

        }

        /// <summary>
        /// 获取指定类型的文件
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetFileData<T>(string configName)
        {
            Type type = typeof(T);
            string fileUrl = DataURL + Configs.FileConfigs[type][configName];

            switch (type.FullName)
            {
                case "UnityEngine.AudioClip":
                    var clipTask = ConfigDataBase.Instance.LoadMusicAsync(fileUrl);
                    AudioClip clip = await clipTask;
                    return (T)(object)clip;
                case "Video":
                    Video videoURL = new Video(fileUrl);
                    return (T)(object)videoURL;
                case "UnityEngine.Sprite":
                    var spriteTask = ConfigDataBase.Instance.LoadImageAsync(fileUrl);
                    Sprite sprite = await spriteTask;
                    return (T)(object)sprite;
            }
            return default(T);
        }

        public async Task<Dictionary<string, T>> GetAllFileData<T>()
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            Type type = typeof(T);

            foreach (var config in Configs.FileConfigs[type])
            {
                string configName = config.Key;
                object configValue = config.Value;

                string fileUrl = DataURL + configValue;
                // 移除file://前缀以便使用File.Exists检查文件
                if (fileUrl.StartsWith("file://"))
                {
                    fileUrl = fileUrl.Substring("file://".Length);
                }
                if (!File.Exists(fileUrl))
                {
                    return null;
                }

                switch (type.Name)
                {
                    case "AudioClip":
                        var clipTask = ConfigDataBase.Instance.LoadMusicAsync(fileUrl);
                        AudioClip clip = await clipTask;
                        result.Add(configName, (T)(object)clip);
                        break;
                    case "Video":
                        Video videoURL = new Video(fileUrl);
                        result.Add(configName, (T)(object)videoURL);
                        break;
                    case "Sprite":
                        var spriteTask = ConfigDataBase.Instance.LoadImageAsync(fileUrl);
                        Sprite sprite = await spriteTask;
                        result.Add(configName, (T)(object)sprite);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <returns></returns>
        public T GetClassData<T>(string className) where T : new()
        {
            Type type = typeof(T);
            T instance = new T();
            instance = (T)Configs.ClassConfigs[type][className];
            return instance;
        }

        /// <summary>
        /// 获取指定类型的装箱实例
        /// </summary>
        /// <returns></returns>
        public object GetClassData(Type type, string className)
        {
            object instance = new object();
            instance = Configs.ClassConfigs[type][className];
            return instance;
        }

        /// <summary>
        /// 获取指定xml类型的类
        /// </summary>
        /// <returns></returns>
        public Type GetKeyType(string key)
        {
            return Configs.ConfigTypeKeys[key].baseClass;
        }

        /// <summary>
        /// 获取指定类型的全部实例字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, T> GetAllClassData<T>()
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            Type type = typeof(T);
            foreach (var i in Configs.ClassConfigs[type])
            {
                result.Add(i.Key, (T)i.Value);
            }
            return result;
        }
    }
}
