namespace JAM8.Utilities
{
    /// <summary>
    /// 文件对话框辅助类
    /// </summary>
    public class FileDialogHelper
    {
        #region FileFilter(常见文件过滤器)

        private const string ConfigFilter = "配置文件(*.cfg)|*.cfg|All File(*.*)|*.*";
        private const string ExcelFilter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
        private const string CSVFilter = "CSV(*.csv)|*.csv|All File(*.*)|*.*";

        private const string ImageFilter = "Image Files(*.BMP;*.bmp;*.JPG;*.jpg;*.GIF;*.gif;*.png)|*.BMP;*.bmp;*.JPG;*.jpg;*.GIF;*.gif;*.png|All File(*.*)|*.*";
        private const string HtmlFilter = "HTML files (*.html;*.htm)|*.html;*.htm|All files (*.*)|*.*";
        private const string AccessFilter = "Access(*.mdb)|*.mdb|All File(*.*)|*.*";
        private const string ZipFillter = "Zip(*.zip)|*.zip|All files (*.*)|*.*";
        private const string TxtFilter = "(*.txt)|*.txt|All files (*.*)|*.*";

        public const string Csv = "CSV格式文件*.csv|*.csv";
        public const string SQLiteDB = "SQLite数据库文件*.db,*.db3|*.db;*.db3";
        public const string Txt = "Txt文本文件*.txt|*.txt";
        public const string Mdb = "Access数据库文件*.mdb|*.mdb";
        public const string Pdf = "PDF文件*.pdf|*.pdf";
        public const string PHP = "PHP程序文件*.php|*.php";
        public const string Py = "Python程序文件*.py|*.py";
        public const string Aspx = "Aspx程序文件*.aspx|*.aspx";
        public const string Html = "Html网页文件*.htm,*.html|*.htm;*.html";
        public const string Jpg = "Jpg图片文件*.jpg,*.jpeg|*.jpg;*.jpeg";
        public const string Png = "Png透明图片文件*.png|*.png";
        public const string Gif = "Gif动态图片文件*.gif|*.gif";
        public const string Bmp = "Bmp图片文件*.bmp|*.bmp";
        public const string Exe = "可执行程序文件*.exe|*.exe";

        #endregion

        #region Construcor

        ///<summary>
        ///Initializes a new instance of the <see cref="MyFileDialogHelper"></see>
        ///  class.
        ///</summary>
        private FileDialogHelper()
        {
        }

        #endregion

        #region Txt相关对话框

        /// <summary>
        /// 打开Txt对话框
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenText()
        {
            return Open("文本文件选择", TxtFilter);
        }

        /// <summary>
        /// 保存Excel对话框,并返回保存全路径
        /// </summary>
        /// <returns>返回值</returns>
        public static string SaveText()
        {
            return SaveText(string.Empty);
        }

        /// <summary>
        /// 保存Excel对话框,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string SaveText(string filename)
        {
            return Save("保存文本文件", TxtFilter, filename);
        }

        /// <summary>
        /// 保存Excel对话框,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="initialDirectory">目录</param>
        /// <returns>返回值</returns>
        public static string SaveText(string filename, string initialDirectory)
        {
            return Save("保存文本文件", TxtFilter, filename, initialDirectory);
        }

        #endregion

        #region Excel相关对话框

        /// <summary>
        /// 打开Excel对话框
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenExcel()
        {
            return Open("Excel选择", ExcelFilter);
        }

        /// <summary>
        /// 保存Excel对话框,并返回保存全路径
        /// </summary>
        /// <returns>返回值</returns>
        public static string SaveExcel()
        {
            return SaveExcel(string.Empty);
        }

        /// <summary>
        /// 保存Excel对话框,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string SaveExcel(string filename)
        {
            return Save("保存Excel", ExcelFilter, filename);
        }

        /// <summary>
        /// 保存Excel对话框,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="initialDirectory">目录</param>
        /// <returns>返回值</returns>
        public static string SaveExcel(string filename, string initialDirectory)
        {
            return Save("保存Excel", ExcelFilter, filename, initialDirectory);
        }

        #endregion

        #region CSV对话框

        /// <summary>
        /// 打开CSV对话框
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenCSV()
        {
            return Open("CSV选择", CSVFilter);
        }

        #endregion

        #region HTML相关对话框

        /// <summary>
        /// 打开Html对话框
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenHtml()
        {
            return Open("Html选择", HtmlFilter);
        }

        /// <summary>
        /// 保存Html对话框,并返回保存全路径
        /// </summary>
        /// <returns>返回值</returns>
        public static string SaveHtml()
        {
            return SaveHtml(string.Empty);
        }

        /// <summary>
        /// 保存Html对话框,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string SaveHtml(string filename)
        {
            return Save("保存Html", HtmlFilter, filename);
        }

        /// <summary>
        /// 保存Html对话框,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="initialDirectory">目录</param>
        /// <returns>返回值</returns>
        public static string SaveHtml(string filename, string initialDirectory)
        {
            return Save("保存Html", HtmlFilter, filename, initialDirectory);
        }

        #endregion

        #region 压缩文件相关

        /// <summary>
        /// Opens the Zip.
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenZip()
        {
            return Open("压缩文件选择", ZipFillter);
        }

        /// <summary>
        /// Opens the Zip.
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string OpenZip(string filename)
        {
            return Open("压缩文件选择", ZipFillter, filename);
        }

        /// <summary>
        /// Save the Zip
        /// </summary>
        /// <returns>返回值</returns>
        public static string SaveZip()
        {
            return SaveZip(string.Empty);
        }

        /// <summary>
        /// Save the Zip
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string SaveZip(string filename)
        {
            return Save("压缩文件保存", ZipFillter, filename);
        }

        /// <summary>
        /// Save the Zip
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="initialDirectory">目录</param>
        /// <returns>返回值</returns>
        public static string SaveZip(string filename, string initialDirectory)
        {
            return Save("压缩文件保存", ZipFillter, filename, initialDirectory);
        }

        #endregion

        #region 图片相关

        /// <summary>
        /// Opens the image.
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenImage()
        {
            return Open("图片选择", ImageFilter);
        }

        /// <summary>
        /// 保存图片对话框,并返回保存全路径
        /// </summary>
        /// <returns>返回值</returns>
        public static string SaveImage()
        {
            return SaveImage(string.Empty);
        }

        /// <summary>
        /// 保存图片对话框并设置默认文件名,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string SaveImage(string filename)
        {
            return Save("保存图片", ImageFilter, filename);
        }

        /// <summary>
        /// 保存图片对话框并设置默认文件名,并返回保存全路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="initialDirectory">目录</param>
        /// <returns>返回值</returns>
        public static string SaveImage(string filename, string initialDirectory)
        {
            return Save("保存图片", ImageFilter, filename, initialDirectory);
        }

        #endregion

        #region 数据库备份还原

        /// <summary>
        /// 保存数据库备份对话框
        /// </summary>
        /// <returns>数据库备份路径</returns>
        public static string SaveAccessDb()
        {
            return Save("数据库备份", AccessFilter);
        }

        /// <summary>
        /// 保存数据库备份对话框
        /// </summary>
        /// <returns>数据库备份路径</returns>
        public static string SaveBakDb()
        {
            return Save("数据库备份", "Access(*.bak)|*.bak");
        }

        /// <summary>
        /// 数据库还原对话框
        /// </summary>
        /// <param name="file">还原文件路径</param>
        /// <returns>数据库还原路径</returns>
        public static string OpenBakDb(string file)
        {
            return Open("数据库还原", "Access(*.bak)|*.bak", file);
        }

        /// <summary>
        /// 数据库还原对话框
        /// </summary>
        /// <returns>数据库还原路径</returns>
        public static string OpenAccessDb()
        {
            return Open("数据库还原", AccessFilter);
        }

        #endregion

        #region 配置文件

        /// <summary>
        /// 保存配置文件备份对话框
        /// </summary>
        /// <returns>配置文件备份路径</returns>
        public static string SaveConfig()
        {
            return Save("配置文件备份", ConfigFilter);
        }

        /// <summary>
        /// 配置文件还原对话框
        /// </summary>
        /// <returns>配置文件还原路径</returns>
        public static string OpenConfig()
        {
            return Open("配置文件还原", ConfigFilter);
        }

        #endregion

        #region 通用函数

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <returns>返回值</returns>
        public static string OpenDir()
        {
            return OpenDir(string.Empty);
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="selectedPath">路径</param>
        /// <returns>返回值</returns>
        public static string OpenDir(string selectedPath)
        {
            var dialog = new FolderBrowserDialog { Description = "请选择路径", SelectedPath = selectedPath };
            return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : string.Empty;
        }

        /// <summary>
        /// Opens the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string Open(string title, string filter, string filename)
        {
            var dialog = new OpenFileDialog { Filter = filter, Title = title, RestoreDirectory = true, FileName = filename };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Opens the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>返回值</returns>
        public static string Open(string title, string filter)
        {
            return Open(title, filter, string.Empty);
        }

        /// <summary>
        /// Saves the specified tile.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="filter">The filter.</param>
        /// <param name="filename">文件名</param>
        /// <returns>返回值</returns>
        public static string Save(string title, string filter, string filename)
        {
            return Save(title, filter, filename, "");
        }

        /// <summary>
        /// Saves the specified tile.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="filter">The filter.</param>
        /// <param name="filename">文件名</param>
        /// <param name="initialDirectory">目录</param>
        /// <returns>返回值</returns>
        public static string Save(string title, string filter, string filename, string initialDirectory)
        {
            var dialog = new SaveFileDialog { Filter = filter, Title = title, FileName = filename, RestoreDirectory = true };
            if (!string.IsNullOrEmpty(initialDirectory))
            {
                dialog.InitialDirectory = initialDirectory;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Saves the specified tile.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="filter">The filter.</param>
        /// <returns>返回值</returns>
        public static string Save(string title, string filter)
        {
            return Save(title, filter, string.Empty);
        }

        #endregion

        #region 获取颜色对话框的颜色

        /// <summary>
        /// 获取对话框颜色
        /// </summary>
        /// <returns>返回值</returns>
        public static Color PickColor()
        {
            Color result = SystemColors.Control;
            var form = new ColorDialog();
            if (DialogResult.OK == form.ShowDialog())
            {
                result = form.Color;
            }
            return result;
        }

        /// <summary>
        /// 获取对话框颜色
        /// </summary>
        /// <param name="color">颜色值</param>
        /// <returns>返回值</returns>
        public static Color PickColor(Color color)
        {
            Color result = SystemColors.Control;
            var form = new ColorDialog { Color = color };
            if (DialogResult.OK == form.ShowDialog())
            {
                result = form.Color;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 根据文件的扩展名初始化Filter
        /// </summary>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string InitFilterByExtension(string Extension)
        {
            switch (Extension)
            {
                case ".cfg":
                    return "配置文件(*.cfg)|*.cfg";
                case ".xls":
                    return "Excel文件(*.xls)|*.xls;";
                case ".xlsx":
                    return "Excel文件(*.xlsx)|*.xlsx";
                case ".doc":
                    return "Word文件(*.doc)|*.doc";
                case ".docx":
                    return "Word文件(*.docx)|*.docx";
                case ".ppt":
                    return "Word文件(*.ppt)|*.ppt";
                case ".pptx":
                    return "Word文件(*.pptx)|*.pptx";
                case ".caj":
                    return "CAJ文件(*.caj)|*.caj";
                case ".pdf":
                    return "CAJ文件(*.pdf)|*.pdf";
                case ".BMP":
                    return "Bmp图片文件(*.bmp)|*.bmp";
                case ".Jpg":
                    return "Jpg图片文件(*.jpg,*.jpeg)|*.jpg;*.jpeg";
                case ".Png":
                    return "Png透明图片文件(*.png)|*.png";
                case ".Gif":
                    return "Gif动态图片文件(*.gif)|*.gif";
                case ".Exe":
                    return "可执行程序文件(*.exe)|*.exe";
                case ".html":
                    return "Html网页文件(*.htm,*.html)|*.htm;*.html";
                case ".mdb":
                    return "Access数据库文件(*.mdb)|*.mdb";
                case ".accdb":
                    return "Access数据库文件(*.accdb)|*.accdb";
                case ".zip":
                    return "Zip(*.zip)|*.zip";
                case ".txt":
                    return "Txt文本文件(*.txt)|*.txt";
                case ".csv":
                    return "CSV格式文件(*.csv)|*.csv";
                case ".db":
                    return "SQLite数据库文件(*.db)|*.db";
                case ".db3":
                    return "SQLite数据库文件(*.db3)|*.db3";
                case ".php":
                    return "PHP程序文件(*.php)|*.php";
                case ".py":
                    return "Python程序文件(*.py)|*.py";
                case ".aspx":
                    return "Aspx程序文件(*.aspx)|*.aspx";

            }
            return "All File(*.*) | *.* ";
        }
    }
}
