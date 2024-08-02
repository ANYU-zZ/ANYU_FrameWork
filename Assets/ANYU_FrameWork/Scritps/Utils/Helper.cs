/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：Helper.cs
 *  作者：ANYU
 *  日期：2024/7/30 15:9:8
 *  功能：Nothing
*************************************************************************/

using System.Reflection;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace ANYU_FrameWork
{
    public static class Helper
    {
        // <summary>
        /// 返回一个空的AudioClip实例
        /// </summary>
        /// <returns></returns>
        public static AudioClip EmptyClip()
        {
            AudioClip audioClip = AudioClip.Create("EmptyAudioClip", 1, 1, 44100, false);
            // 填充数据（这里创建一个空的音频片段，所以不需要数据）
            float[] data = new float[1]; // 这里的长度应与创建AudioClip时指定的长度一致
            audioClip.SetData(data, 0);
            return audioClip;
        }

        /// <summary>
        /// 获取当前按下的键盘按键
        /// </summary>
        /// <returns></returns>
        public static KeyCode GetNowGetKeyDown()
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }
            return KeyCode.None;
        }

        /// <summary>
        /// 通过配置文件单行信息实例化一个指定类并赋值
        /// </summary>
        /// <returns></returns>
        public static object GetTypeInstance(Type _type, string key, object data)
        {
            Type type = TypeExtend.GetUType(_type.Name);
            if (type == null)
            {
                Debug.LogError($"Type '{_type.Name}' not found.");
                return null;
            }

            var instance = Activator.CreateInstance(type);

            List<string> data_Temp = (List<string>)data;

            if (key != null && data_Temp != null)
            {
                PropertyInfo[] properties = type.GetProperties();
                for (int i = 0; i < properties.Length && i < data_Temp.Count; i++)
                {
                    PropertyInfo property = properties[i];
                    Type propertyType = property.PropertyType;
                    object value = null;

                    try
                    {
                        if (propertyType == typeof(int))
                        {
                            value = int.Parse(data_Temp[i]);
                        }
                        else if (propertyType == typeof(float))
                        {
                            value = float.Parse(data_Temp[i]);
                        }
                        else if (propertyType == typeof(double))
                        {
                            value = double.Parse(data_Temp[i]);
                        }
                        else if (propertyType == typeof(bool))
                        {
                            value = bool.Parse(data_Temp[i]);
                        }
                        else if (propertyType == typeof(string))
                        {
                            value = data_Temp[i];
                        }
                        else
                        {
                            value = Convert.ChangeType(data_Temp[i], propertyType);
                        }

                        property.SetValue(instance, value);
                    }
                    catch (FormatException ex)
                    {
                        Debug.LogError($"Error setting property '{property.Name}' with value '{data_Temp[i]}': {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Unexpected error setting property '{property.Name}' with value '{data_Temp[i]}': {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.LogError($"Data for is not a List<string>");
            }

            return instance;
        }

        /// <summary>
        /// 输出Class类配置文件单行的实例化结果
        /// </summary>
        /// <returns></returns>
        public static string PrintProperties(object obj, string key, object data)
        {
            string result = "\n";
            // 获取对象的类型  
            Type type = obj.GetType();
            // 遍历每个公共属性  
            foreach (PropertyInfo property in type.GetProperties())
            {
                var attribute = data;
                if (attribute != null)
                {
                    // 获取属性名称和属性值  
                    string propertyName = property.Name;
                    object propertyValue = property.GetValue(obj);
                    Type propertyType = propertyValue.GetType();

                    if (propertyType.IsClass && propertyType.Name != "String")
                    {
                        foreach (var item in (List<string>)data)
                        {
                            if (item.Equals(propertyValue))
                            {
                                result += $"{propertyName}: {key}\n";
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 输出属性名和对应值  
                        result += $"{propertyName}: {propertyValue}\n";
                    }
                }
            }
            return result;
        }
    }
}
