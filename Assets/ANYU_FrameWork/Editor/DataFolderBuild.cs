using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class DataFolderBuild
{
    [MenuItem("Build/Build Project")]
    public static void BuildProject()
    {
        // 设置构建路径  
        string buildMainPath = "Builds";
        string buildPath = "Builds/DpFrame.exe";
        // 构建项目  
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, BuildTarget.StandaloneWindows, BuildOptions.None);

        // 拷贝自定义文件夹  
        CopyCustomFolder(buildMainPath);
    }
    private static void CopyCustomFolder(string buildPath)
    {
        string sourceFolder = "Assets/Data"; // 你自定义文件夹的路径  
        string destinationFolder = Path.Combine(buildPath, "Data"); // 构建输出中的目标路径  
        if (Directory.Exists(sourceFolder))
        {
            // 确保目标目录存在  
           // Directory.CreateDirectory(destinationFolder);

            // 拷贝文件  
            foreach (string file in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
            {
                string destFile = file.Replace(sourceFolder, destinationFolder);
                string destDir = Path.GetDirectoryName(destFile);

                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                if(!file.Contains(".meta"))
                File.Copy(file, destFile, true);
            }

            Debug.Log("Custom folder copied to build!");
        }
        else
        {
            Debug.LogWarning("Source folder does not exist: " + sourceFolder);
        }
    }
}
