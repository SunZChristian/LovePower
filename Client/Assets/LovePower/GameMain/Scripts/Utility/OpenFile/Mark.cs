using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using UnityEngine;

namespace LovePower
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal class OpenFileName
	{
		public int structSize = 0;       //结构的内存大小
		public IntPtr dlgOwner = IntPtr.Zero;       //设置对话框的句柄
		public IntPtr instance = IntPtr.Zero;       //根据flags标志的设置，确定instance是谁的句柄，不设置则忽略
		public string filter = null;         //调取文件的过滤方式
		public string customFilter = null;  //一个静态缓冲区 用来保存用户选择的筛选器模式
		public int maxCustFilter = 0;     //缓冲区的大小
		public int filterIndex = 0;                 //指向的缓冲区包含定义过滤器的字符串对
		public string file = null;                  //存储调取文件路径
		public int maxFile = 0;                     //存储调取文件路径的最大长度 至少256
		public string fileTitle = null;             //调取的文件名带拓展名
		public int maxFileTitle = 0;                //调取文件名最大长度
		public string initialDir = null;            //最初目录
		public string title = null;                 //打开窗口的名字
		public int flags = 0;                       //初始化对话框的一组位标志  参数类型和作用查阅官方API
		public short fileOffset = 0;                //文件名前的长度
		public short fileExtension = 0;             //拓展名前的长度
		public string defExt = null;                //默认的拓展名
		public IntPtr custData = IntPtr.Zero;       //传递给lpfnHook成员标识的钩子子程的应用程序定义的数据
		public IntPtr hook = IntPtr.Zero;           //指向钩子的指针。除非Flags成员包含OFN_ENABLEHOOK标志，否则该成员将被忽略。
		public string templateName = null;          //模块中由hInstance成员标识的对话框模板资源的名称
		public IntPtr reservedPtr = IntPtr.Zero;
		public int reservedInt = 0;
		public int flagsEx = 0;                     //可用于初始化对话框的一组位标志


	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal class OpenDialogDir
	{
		public IntPtr hwndOwner = IntPtr.Zero;
		public IntPtr pidlRoot = IntPtr.Zero;
		public String pszDisplayName = null;
		public String lpszTitle = null;
		public UInt32 ulFlags = 0;
		public IntPtr lpfn = IntPtr.Zero;
		public IntPtr lParam = IntPtr.Zero;
		public int iImage = 0;
	}

	/// <summary>
	/// 打开Windows文件夹/文件
	/// </summary>
	public static class OpenDialogHelper
	{
		/// <summary>
		/// 打开文件
		/// </summary>
		/// <param name="callBack">获得文件回调</param>
		/// <param name="fileter">文件类型</param>
		public static void SelectFile(Action<string> callBack, string fileter)
		{
			try
			{
				OpenFileName openFileName = new OpenFileName();
				openFileName.structSize = Marshal.SizeOf(openFileName);
				openFileName.filter = fileter;
				openFileName.file = new string(new char[1024]);
				openFileName.maxFile = openFileName.file.Length;
				openFileName.fileTitle = new string(new char[64]);
				openFileName.maxFileTitle = openFileName.fileTitle.Length;
				openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
				openFileName.title = "选择文件";
				//openFileName.defExt = "FBX";
				//openFileName.flags = 0x00001000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
				openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

				if (LocalDialog.GetFile(openFileName))
				{
					string filePath = openFileName.file;
					if (File.Exists(filePath))
					{
						callBack?.Invoke(filePath);
						return;
					}
				}
			}
			catch (Exception)
			{

			}
		}

		/// <summary>
		/// 选择文件夹
		/// </summary>
		/// <param name="callBack"></param>
		/// <param name="title"></param>
		public static void SelectFolder(Action<string> callBack, string title = "请选择文件夹")
		{
			try
			{
				OpenDialogDir ofn2 = new OpenDialogDir();
				ofn2.pszDisplayName = new string(new char[2048]);
				ofn2.lpszTitle = title; // 标题  
				ofn2.ulFlags = 0x00000040; // 新的样式,带编辑框  
				IntPtr pidlPtr = LocalDialog.GetFolder(ofn2);

				char[] charArray = new char[2048];

				for (int i = 0; i < 2048; i++)
				{
					charArray[i] = '\0';
				}
				LocalDialog.GetPathFormIDList(pidlPtr, charArray);
				string res = new string(charArray);
				res = res.Substring(0, res.IndexOf('\0'));
				if (Directory.Exists(res))
				{
					callBack?.Invoke(res);
				}

			}
			catch (Exception)
			{

			}



		}

		/// <summary>
		/// 文件另存为
		/// </summary>
		public static void SaveFile(Action<string> callBack, string fileter)
		{
			try
			{
				OpenFileName openFileName = new OpenFileName();
				openFileName.structSize = Marshal.SizeOf(openFileName);
				openFileName.filter = fileter;
				openFileName.file = new string(new char[1024]);
				openFileName.maxFile = openFileName.file.Length;
				openFileName.fileTitle = new string(new char[64]);
				openFileName.maxFileTitle = openFileName.fileTitle.Length;
				openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
				openFileName.title = "另存为";
				//openFileName.defExt = "FBX";
				//openFileName.flags = 0x00001000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
				openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

				if (LocalDialog.SaveFile(openFileName))
				{
					string filePath = openFileName.file;
					if (File.Exists(filePath))
					{
						callBack?.Invoke(filePath);
						return;
					}
				}
			}
			catch (Exception)
			{

			}
		}

		/// <summary>
		/// 打开目录
		/// </summary>
		/// <param name="path">将要打开的文件目录</param>
		public static void OpenFolder(string path)
		{
			System.Diagnostics.Process.Start("explorer.exe", path);
		}


	}
	internal class LocalDialog
	{
		//链接指定系统函数       打开文件对话框
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

		//链接指定系统函数        另存为对话框
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);


		[DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		private static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

		[DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		private static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

		/// <summary>
		/// 获取文件
		/// </summary>
		/// <param name="ofn"></param>
		/// <returns></returns>
		public static bool GetFile([In, Out] OpenFileName ofn)
		{
			return GetOpenFileName(ofn);
		}

		/// <summary>
		/// 另存为
		/// </summary>
		/// <param name="ofn"></param>
		/// <returns></returns>
		public static bool SaveFile([In, Out] OpenFileName ofn)
		{
			return GetSaveFileName(ofn);
		}


		/// <summary>
		///  获取文件夹路径
		/// </summary>
		public static IntPtr GetFolder([In, Out] OpenDialogDir ofn)
		{
			return SHBrowseForFolder(ofn);
		}
		/// <summary>
		/// 获取文件夹路径
		/// </summary>
		public static bool GetPathFormIDList([In] IntPtr pidl, [In, Out] char[] fileName)
		{
			return SHGetPathFromIDList(pidl, fileName);
		}
	}
}