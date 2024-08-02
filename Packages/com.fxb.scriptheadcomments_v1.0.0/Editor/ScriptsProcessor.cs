using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace ScriptHeadComments.Editor
{
    public class ScriptsProcessor : UnityEditor.AssetModificationProcessor
    {
        private const double NewFileThresholdInSeconds = 1.0; // 1秒的时间阈值

        /// <summary>
        /// 当在Unity中新建脚本时会调用
        /// 当从外部导入新脚本时会调用
        /// </summary>
        /// <param name="path"></param>
        private static void OnWillCreateAsset(string path)
        {
            ScriptHeadComments scriptHead = Resources.Load<ScriptHeadComments>("ScriptHeadComments");
            if (scriptHead == null) return;

            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                //添加判断脚本是否新建的
                if (!CheckScriptNewBuild(path)) return;

                try
                {
                    string scriptTemplate = File.ReadAllText(SourceScriptTemplatePath);

                    scriptTemplate = scriptTemplate.Replace("#USERNAME#", Environment.UserName);
                    scriptTemplate = scriptTemplate.Replace("#COMPANYNAME#", PlayerSettings.companyName);
                    scriptTemplate = scriptTemplate.Replace("#PROJECTNAME#", PlayerSettings.productName);
                    scriptTemplate = scriptTemplate.Replace("#FILEEXTENSION#", Path.GetFileName(path));
                    scriptTemplate = scriptTemplate.Replace("#AUTHORNAME#", scriptHead.authorName);
                    scriptTemplate = scriptTemplate.Replace("#CREATETIME#", string.Concat(DateTime.Now.ToString("d"), " ", DateTime.Now.Hour, ":", DateTime.Now.Minute, ":", DateTime.Now.Second));
                    scriptTemplate = scriptTemplate.Replace("#ASSEMBLYNAME#", scriptHead.assembleName);
                    scriptTemplate = Regex.Replace(scriptTemplate, @"\s*#ROOTNAMESPACEBEGIN#", string.Empty);
                    scriptTemplate = scriptTemplate.Replace("#SCRIPTNAME#", Path.GetFileNameWithoutExtension(path));
                    scriptTemplate = scriptTemplate.Replace("#NOTRIM#", "");
                    scriptTemplate = Regex.Replace(scriptTemplate, @"\s*#ROOTNAMESPACEEND#", string.Empty);

                    File.WriteAllText(path, scriptTemplate);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// 检查脚本是否新建的
        /// </summary>
        /// <param name="path"></param>
        private static bool CheckScriptNewBuild(string path)
        {
            string fullPath = Path.GetFullPath(path);
            DateTime lastWriteTime = File.GetLastWriteTime(fullPath);

            // 如果文件在创建后1秒内被修改，则认为是新建脚本，否则是导入的脚本
            if (lastWriteTime.AddSeconds(NewFileThresholdInSeconds) >= DateTime.Now)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 源脚本模板
        /// </summary>
        static string SourceScriptTemplatePath
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();

                var pInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(assembly);

                if (pInfo == null)
                    return null;

                var customTemplatePath = Path.GetFullPath(pInfo.assetPath);

                customTemplatePath = Path.Combine(customTemplatePath, "Resources/81-C# Script-NewBehaviourScript.cs.txt");

                customTemplatePath = customTemplatePath.Replace('\\', '/');

                return customTemplatePath;
            }
        }
    }
}
