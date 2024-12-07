using System.Runtime.InteropServices;
using System.Text;

namespace JAM8.Utilities
{
    public static class FileHelper
    {
        /// <summary>
        /// 通过Shell命令打开文件对话框，选择文件
        /// </summary>
        /// <returns></returns>
        public static string OpenFileByShellCommand()
        {
            string command = "Add-Type -AssemblyName System.Windows.Forms\n" +
            "$OpenFileDialog = New-Object System.Windows.Forms.OpenFileDialog\n" +
            "$OpenFileDialog.InitialDirectory = $initialDirectory\n" +
            "$OpenFileDialog.ShowDialog() | Out-Null\n" +
            "return $OpenFileDialog.FileName";
            string fileName = PowerShellHelper.ExecuteCommand(command);
            fileName = fileName.TrimEnd('\n');
            fileName = fileName.TrimEnd('\r');
            if (fileName == "")
                return null;
            else
                return fileName;
        }

        /// <summary>
        /// 通过Shell命令保存文件对话框，选择文件
        /// </summary>
        /// <returns></returns>
        public static string SaveFileByShellCommand()
        {
            string command = "Add-Type -AssemblyName System.Windows.Forms\n" +
            "$saveFileDialog = New-Object System.Windows.Forms.SaveFileDialog\n" +
            "$saveFileDialog.InitialDirectory = $initialDirectory\n" +
            "$saveFileDialog.ShowDialog() | Out-Null\n" +
            "return $saveFileDialog.FileName";
            string fileName = PowerShellHelper.ExecuteCommand(command);
            fileName = fileName.TrimEnd('\n');
            fileName = fileName.TrimEnd('\r');
            if (fileName == "")
                return null;
            else
                return fileName;
        }

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns>bool 是否存在文件</returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 根据传来的文件全路径，获取文件名称部分默认包括扩展名
        /// </summary>
        /// <param name="fileFullPath">文件全路径</param>
        /// <returns>string 文件名称</returns>
        public static string GetFileName(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                var f = new FileInfo(fileFullPath);
                return f.Name;
            }
            return null;
        }

        /// <summary>
        /// 根据传来的文件全路径，获取文件名称部分
        /// </summary>
        /// <param name="fileFullPath">文件全路径</param>
        /// <param name="includeExtension">是否包括文件扩展名</param>
        /// <returns>string 文件名称</returns>
        public static string GetFileName(string fileFullPath, bool includeExtension)
        {
            if (File.Exists(fileFullPath))
            {
                var f = new FileInfo(fileFullPath);
                if (includeExtension)
                {
                    return f.Name;
                }
                return f.Name.Replace(f.Extension, "");
            }
            return null;
        }

        /// <summary>
        /// 根据传来的文件全路径，获取新的文件名称全路径,一般用作临时保存用
        /// </summary>
        /// <param name="fileFullPath">文件全路径</param>
        /// <returns>string 新的文件全路径名称</returns>
        public static string GetNewFileFullName(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                var f = new FileInfo(fileFullPath);
                string tempFileName = fileFullPath.Replace(f.Extension, "");
                for (int i = 0; i < 1000; i++)
                {
                    fileFullPath = tempFileName + i.ToString() + f.Extension;
                    if (File.Exists(fileFullPath) == false)
                    {
                        break;
                    }
                }
            }
            return fileFullPath;
        }

        /// <summary>
        /// 根据传来的文件全路径，获取文件扩展名包括“.”，如“.doc”
        /// </summary>
        /// <param name="fileFullPath">文件全路径</param>
        /// <returns>string 文件扩展名</returns>
        public static string GetFileExtension(string fileFullPath)
        {
            return Path.GetExtension(fileFullPath);
        }

        /// <summary>
        /// 根据传来的文件全路径，获取其上一级目录
        /// 网址：http://bbs.csdn.net/topics/350254818
        /// 例如：
        /// d:/111/222/fileName.txt
        /// 则返回
        /// d:/111/222/
        /// 喻思羽 2015.5
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static string GetParentDirectory(string fileFullPath)
        {
            DirectoryInfo i = new DirectoryInfo(fileFullPath);
            //上级目录
            string path = i.Parent.FullName;
            return path;
        }

        #region CheckFileCanBeUse 检查文件能否被使用

        [DllImport("kernel32.dll")]
        static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);

        const int OF_READWRITE = 2;
        const int OF_SHARE_DENY_NONE = 0x40;
        static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 检查文件能否被使用
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool CheckFileCanBeUse(string FileName)
        {
            if (!File.Exists(FileName))
            {
                //MessageBox.Show("文件都不存在!");
                return false;
            }
            IntPtr vHandle = _lopen(FileName, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                //MessageBox.Show("文件被占用！");
                return false;
            }
            CloseHandle(vHandle);
            //MessageBox.Show("没有被占用！");
            return true;
        }

        #endregion

        #region SortByName

        public static void SortByName(ref List<string> fileName)
        {
            string[] temp = fileName.ToArray();
            SortByName(ref temp);
            fileName = temp.ToList();
        }
        /// <summary>
        /// 对文件名进行排序（与Windows里面文件名排序一样）
        /// </summary>
        /// <param name="fileNames"></param>
        public static void SortByName(ref string[] fileNames)
        {
            Array.Sort(fileNames, new FileNameSort());
        }
        internal class FileNameSort : System.Collections.IComparer
        {
            //调用windos 的 DLL
            [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
            private static extern int StrCmpLogicalW(string param1, string param2);

            //前后文件名进行比较。
            public int Compare(object name1, object name2)
            {
                if (null == name1 && null == name2)
                {
                    return 0;
                }
                if (null == name1)
                {
                    return -1;
                }
                if (null == name2)
                {
                    return 1;
                }
                return StrCmpLogicalW(name1.ToString(), name2.ToString());
            }
        }

        #endregion
    }
}
