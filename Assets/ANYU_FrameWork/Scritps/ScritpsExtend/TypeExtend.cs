/*************************************************************************
 *  Copyright © 2023-2030 ANYU. All rights reserved.
 *------------------------------------------------------------------------
 *  公司：ANYU
 *  项目：Tools_TestProject
 *  文件：TypeExtend.cs
 *  作者：ANYU
 *  日期：2024/7/29 14:52:46
 *  功能：Nothing
*************************************************************************/

using System;
using System.Reflection;

namespace ANYU_TEST
{
    public class TypeExtend
    {
        public static Type GetUType(string name)
        {
            foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (System.Type type in assembly.GetTypes())
                {
                    if (type.Name == name)
                        return type;
                }
            }

            return null;
        }
    }
}
