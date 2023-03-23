using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEditor;
using UnityEngine;

public class CopyFilesToStreamingAssets : Editor
{
    [MenuItem("Tools/Copy Files To StreamingAssets")]
    public static void CopyFiles()
    {
        // Get the source directory based on the current platform
        string sourceDirectory = Application.dataPath+"/../"+"ABs/Package";

        // Get the destination directory
        string destinationDirectory = Application.streamingAssetsPath;

        // Delete all files in the destination directory
        if (Directory.Exists(destinationDirectory))
        {
            DirectoryInfo di = new DirectoryInfo(destinationDirectory);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
        else
        {
            Directory.CreateDirectory(destinationDirectory);
        }
        // 删除StreamingAssets下的所有文件
        string streamingAssetsPath = Application.streamingAssetsPath;
        DirectoryInfo dirInfo = new DirectoryInfo(streamingAssetsPath);
        foreach (FileInfo file in dirInfo.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in dirInfo.GetDirectories())
        {
            dir.Delete(true);
        }

        // 找到最大版本号的文件夹
        DirectoryInfo sourceFolder = new DirectoryInfo(sourceDirectory);
        DirectoryInfo[] dirs = sourceFolder.GetDirectories();
        string maxVersionFolderName = "0_0_0_0";
        foreach (DirectoryInfo dir in dirs)
        {
            string[] dirNameParts = dir.Name.Split('_');
            string[] maxVersionParts = maxVersionFolderName.Split('_');
            if (dirNameParts.Length == 4 && maxVersionParts.Length == 4)
            {
                int dirVersion = 0;
                int maxVersion = 0;
                for (int i = 0; i < 4; i++)
                {
                    int.TryParse(dirNameParts[i], out dirVersion);
                    int.TryParse(maxVersionParts[i], out maxVersion);
                    if (dirVersion > maxVersion)
                    {
                        maxVersionFolderName = dir.Name;
                        break;
                    }
                }
            }
        }

        Debug.Log("当前最大的版本号是----"+maxVersionFolderName);
#if UNITY_STANDALONE_WIN
        sourceDirectory = Application.dataPath+"/../"+"ABs/Package/"+maxVersionFolderName+"/Windows64";
#elif UNITY_IOS
        sourceDirectory = Application.dataPath+"/../"+"ABs/Package/"+maxVersionFolderName+"/IOS";
#elif UNITY_ANDROID
        sourceDirectory = Application.dataPath+"/../"+"ABs/Package/"+maxVersionFolderName+"/Android";
#endif
        Debug.Log("当前拷贝的路径是---"+sourceDirectory);
        // Copy files from source directory to destination directory
        foreach (string file in Directory.GetFiles(sourceDirectory))
        {
            string fileName = Path.GetFileName(file);
            string dest = Path.Combine(destinationDirectory, fileName);
            File.Copy(file, dest, true);
        }

        // Refresh asset database to reflect the changes
        AssetDatabase.Refresh();
    }
    
     
        // if (!Directory.Exists(destinationPath))
        // {
        //     Directory.CreateDirectory(destinationPath);
        // }
        // sourceFolderPath = sourceFolderPath + "/" + maxVersionFolderName + "/";
        // if (Directory.Exists(sourceFolderPath))
        // {
        //     foreach (string sourceFilePath in Directory.GetFiles(sourceFolderPath))
        //     {
        //         string fileName = Path.GetFileName(sourceFilePath);
        //         string destinationFilePath = Path.Combine(destinationPath, fileName);
        //         File.Copy(sourceFilePath, destinationFilePath, true);
        //     }
        // }
    
}
