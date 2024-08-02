/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：MonoSingleton.cs
 *  作者：ANYU
 *  日期：2024/7/23 15:1:54
 *  功能：Nothing
*************************************************************************/
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    private static readonly object locker = new object();

    private static bool bAppQuitting;

    public static T Instance
    {
        get
        {
            if (bAppQuitting)
            {
                instance = null;
                return instance;
            }

            lock (locker)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError("不应该存在多个单例！");
                        return instance;
                    }

                    if (instance == null)
                    {
                        var singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton)" + typeof(T);
                        singleton.hideFlags = HideFlags.None;
                        DontDestroyOnLoad(singleton);
                    }
                    else
                        DontDestroyOnLoad(instance.gameObject);
                }
                instance.hideFlags = HideFlags.None;
                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        bAppQuitting = false;
    }

    protected virtual void OnDestroy()
    {
        bAppQuitting = true;
    }
}