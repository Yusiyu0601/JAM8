using System.Net;
using JAM8.Utilities;
using JAM8.Work.常用工具;
using JAM8.Work.建模方法;
using JAM8.Work.模块测试;
using JAM8.Work.测试;
using JAM8.Work.研究方法;

namespace JAM8.Work
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();

        string taskName;//选中的任务

        Dictionary<string, string> Task_常用工具 = new()
        {
            {"ToolBox_Grid","该工具箱功能:Grid各种操作，例如展示、切片等" },
            {"ToolBox_WellLog","该工具箱功能:测井曲线各种操作，例如展示、提取深度段等" },
            {"ToolBox_Excel","该工具箱功能:Excel文件各种操作" },
            {"ToolBox_Variogram","该工具箱功能:地质统计学的变差函数各种操作，例如自动拟合、自动提取" },
            {"ToolBox_Image","该工具箱功能:图像处理的各种操作，例如图像二值化等" },
            {"ToolBox_MachineLearning","该工具箱功能:机器学习的各种操作，例如降维聚类等" },
            {"ToolBox_Plot","该工具箱功能:绘图操作，例如散点图等" },
            {"ToolBox_MyDataBox","该工具箱功能:管理数据箱" }
        };

        readonly Dictionary<string, string> Task_建模方法 = new()
        {
            {"Modeling_GRFS","功能:高斯随机场建模" },
            {"Modeling_Kriging","功能:克里金方法" },
            {"Modeling_MPS","功能:多点地质统计建模" },
            {"Modeling_IDW","功能:IDW建模" }
        };

        readonly Dictionary<string, string> Task_研究方法 = new()
        {
            {"Research_NonStationary","功能:非平稳方面的研究方法" },
            {"Research_Mould","功能:样式方面的研究" },
            {"Research_模型评价问题","功能:评价随机模型质量，比如与cd匹配性、噪点数据情况" },
        };

        readonly Dictionary<string, string> Task_测试 = new()
        {
            {"test_in_work","该工具箱功能:本项目的测试，不用调用JAM8" },
            {"Test_Numerics","功能:测试相关功能" },
            {"Test_ML","功能:测试Machine Learning相关功能" },
            {"Test_Geometry","功能:测试Geometry相关功能" },
            {"Test_Utilities","功能:测试Utilities相关功能" },
            {"Test_Image","功能:测试Image模块函数" },
            {"Test_ExportUsed","功能:测试导入外部库相关功能" },
            {"Test_somethings","功能:一般测试" },
            {"Test_TorchSharp","功能:TorchSharp测试" }
        };

        public Form1()
        {
            InitializeComponent();
        }

        #region 登录验证

        public bool CheckDateTime()
        {
            string out_datetime = "2024-12-03 9:45:00";
            DateTime out_dt = Convert.ToDateTime(out_datetime);
            string now_datetime = GetDateTime();
            if (now_datetime == string.Empty)
                return false;
            now_datetime = Convert.ToDateTime(now_datetime).ToString("yyyy-MM-dd HH:mm:ss");
            DateTime now_dt = Convert.ToDateTime(now_datetime);
            if (now_dt >= out_dt)
                return false;//已经过期
            else
                return true;//在有效期内
        }
        public string GetDateTime()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { if (h == "Date") { datetime = headerCollection[h]; } }
                return datetime;
            }
            catch (Exception)
            {
                return datetime;
            }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }
        public void NoteMe()
        {
            //收件人列表
            List<string> emailList = new();
            //smtp
            string smtp = null;
            //用户名
            string userName;
            //密码
            string password;
            //来源邮件
            string fromEmail = null;
            //来源显示
            string fromName;
            //端口(一般为25)
            int smtpPort = 25;

            //获取收件人列表
            string[] receivers = "573315294@qq.com".Split(';');
            for (int i = 0; i < receivers.Length; i++)
            {
                emailList.Add(receivers[i]);
            }

            //获取smtp
            smtp = "smtp.163.com";
            //获取用户名
            userName = "feiyuno2";
            //获取密码
            password = "ACZXXJSEOFKMJJPG";
            //来源邮件
            fromEmail = userName + "@163.com";
            //来源显示
            fromName = "me";

            EmailSender oMailHelper = new(emailList, smtp, userName, password, fromEmail, fromName, smtpPort);

            try
            {
                string Name = HardwareInfoHelper.GetComputerName();

                if (Name == "DESKTOP-OHQ94NO" || Name == "DESKTOP-NTHMMPS")
                    return;
                oMailHelper.Send("建模工具箱启动", HardwareInfoHelper.GetComputerName());
            }
            catch
            {
                return;
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();//开启控制台

            Console.WriteLine("免责声明：本软件免费使用，不得出售与商业应用，同时在使用产生损害，软件作者不承担任何责任。" +
                "\n使用遇到问题可与作者联系(邮箱:573315294@qq.com)");

            Init();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FreeConsole();//关闭控制台
        }

        //执行选中任务
        private void button_run_Click(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToLocalTime().ToString();
            Console.WriteLine();
            Console.WriteLine($@"*   *   *   *   *   执行 Task:{taskName}   {time}  *   *   *   *   *");
            Console.WriteLine();

            Run();

            time = DateTime.Now.ToLocalTime().ToString();
            Console.WriteLine();
            Console.WriteLine($@"*   *   *   *   *   Task 结束:{taskName}   {time}  *   *   *   *   * ");
            Console.WriteLine();
        }

        //调用外部程序
        private void button2_Click(object sender, EventArgs e)
        {

        }

        #region 不用修改

        private void listBox_常用工具_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_常用工具.SelectedItem == null)
                return;
            if (listBox_常用工具.SelectedItem.ToString() != string.Empty)
            {
                taskName = listBox_常用工具.SelectedItem.ToString();
                textBox1.Text = Task_常用工具[taskName];
            }
        }
        private void listBox_建模方法_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_建模方法.SelectedItem == null)
                return;
            if (listBox_建模方法.SelectedItem.ToString() != string.Empty)
            {
                taskName = listBox_建模方法.SelectedItem.ToString();
                textBox1.Text = Task_建模方法[taskName];
            }
        }
        private void listBox_研究方法_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_研究方法.SelectedItem == null)
                return;
            if (listBox_研究方法.SelectedItem.ToString() != string.Empty)
            {
                taskName = listBox_研究方法.SelectedItem.ToString();
                textBox1.Text = Task_研究方法[taskName];
            }
        }
        private void listBox_测试_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_测试.SelectedItem == null)
                return;
            if (listBox_测试.SelectedItem.ToString() != string.Empty)
            {
                taskName = listBox_测试.SelectedItem.ToString();
                textBox1.Text = Task_测试[taskName];
            }
        }

        /// <summary>
        /// 初始化工具箱列表
        /// </summary>
        void Init()
        {
            foreach (var item in Task_常用工具)
            {
                listBox_常用工具.Items.Add(item.Key);
            }
            foreach (var item in Task_建模方法)
            {
                listBox_建模方法.Items.Add(item.Key);
            }
            foreach (var item in Task_研究方法)
            {
                listBox_研究方法.Items.Add(item.Key);
            }
            foreach (var item in Task_测试)
            {
                listBox_测试.Items.Add(item.Key);
            }
        }

        #endregion

        //运行
        void Run()
        {
            switch (taskName)
            {
                #region ToolBox

                case "ToolBox_Grid":
                    ToolBox_Grid.Run();
                    break;
                case "ToolBox_WellLog":
                    ToolBox_WellLog.Run();
                    break;
                case "ToolBox_Excel":
                    ToolBox_Excel.Run();
                    break;
                case "ToolBox_Variogram":
                    ToolBox_Variogram.Run();
                    break;
                case "ToolBox_Image":
                    ToolBox_Image.Run();
                    break;
                case "ToolBox_MachineLearning":
                    ToolBox_MachineLearning.Run();
                    break;
                case "ToolBox_Plot":
                    ToolBox_Plot.Run();
                    break;
                case "ToolBox_MyDataBox":
                    ToolBox_MyDataBox.Run();
                    break;

                #endregion

                #region Modeling

                case "Modeling_GRFS":
                    Modeling_GRFS.Run();
                    break;
                case "Modeling_Kriging":
                    Modeling_Kriging.Run();
                    break;
                case "Modeling_MPS":
                    Modeling_MPS.Run();
                    break;
                case "Modeling_IDW":
                    Modeling_IDW.Run();
                    break;

                #endregion

                #region Research

                case "Research_NonStationary":
                    ResearchNonStationary.run();
                    break;
                case "Research_模型评价问题":
                    Research_模型评价问题.Run();
                    break;
                case "Research_Mould":
                    Research_Mould.Run();
                    break;

                #endregion

                #region Test

                case "test_in_work":
                    test_in_work.Run();
                    break;
                case "Test_Numerics":
                    Test_Numerics.Run();
                    break;
                case "Test_ML":
                    Test_ML.Run();
                    break;
                case "Test_Geometry":
                    Test_Geometry.Run();
                    break;
                case "Test_Utilities":
                    Test_Utilities.Run();
                    break;
                case "Test_somethings":
                    Test_somethings.Run();
                    break;
                case "Test_Image":
                    Test_Image.Run();
                    break;
                case "Test_TorchSharp":
                    Test_TorchSharp.Run();
                    break;

                #endregion

                default:
                    break;
            }
        }

        //切换Tab，更新选中项
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                case "常用工具":
                    listBox_常用工具_SelectedIndexChanged(sender, e);
                    break;
                case "建模方法":
                    listBox_建模方法_SelectedIndexChanged(sender, e);
                    break;
                case "研究方法":
                    listBox_研究方法_SelectedIndexChanged(sender, e);
                    break;
                case "模块测试":
                    listBox_测试_SelectedIndexChanged(sender, e);
                    break;
                default:
                    break;
            }
        }
    }
}