using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LovePower
{
    public class FilePickerComponent : GameFrameworkComponent
    {
        public void OpenFile()
        {
            AndroidJavaObject ao = new AndroidJavaObject("com.zrqz.filebrower.FileBrower");
            AndroidJavaObject m_file = ao.CallStatic<AndroidJavaObject>("GetInstance");
            if (m_file != null)
            {
                m_file.Call("UnityFunc", "GameFramework/Custom/AndroidFilePicker", "GetPath");
                m_file.Call("fileBrower");
            }
        }
        public void GetPath(string str)
        {
            Log.Info("android 路径：" + str);
        }
    }
}
