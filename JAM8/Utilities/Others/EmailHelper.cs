using System.Net;
using System.Net.Mail;
using System.Net.Sockets;

namespace JAM8.Utilities
{
    /// <summary>
    /// 邮件发送类
    /// </summary>
    public class EmailSender
    {
        private MailMessage mail;
        private SmtpClient smtp;

        public EmailSender(List<string> emailList, string strSmpt, string userName, string password,
            string fromEmail, string fromName, int smtpPort)
        {
            smtp = new SmtpClient();
            smtp.Host = strSmpt;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(userName, password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Port = smtpPort;
            mail = new MailMessage(new MailAddress(fromEmail, fromName), new MailAddress(emailList[0]));
            if (emailList.Count > 1)
            {
                for (int i = 1; i < emailList.Count; i++)
                {
                    mail.CC.Add(new MailAddress(emailList[i]));
                }
            }
            mail.IsBodyHtml = true;
        }

        public void Send(string subject, string body)
        {
            mail.Priority = MailPriority.Normal;
            mail.Subject = subject;
            mail.Body = body;
            smtp.Send(mail);
        }

        public void SendAttach(string path)
        {
            mail.Subject = Dns.GetHostName() + " " + path;
            mail.Attachments.Add(new Attachment(path));
        }
    }

    /// <summary>
    /// 邮件接收类
    /// </summary>
    public class EmailReceiver
    {
        private TcpClient Server;
        private string SendString;
        private byte[] BufferString;
        private NetworkStream networkstream;
        private StreamReader streamreader;

        private List<string> Connect(string POP3Server, string UserName, string Password)
        {
            Server = new TcpClient(POP3Server, 110);
            networkstream = Server.GetStream();
            streamreader = new StreamReader(networkstream);

            List<string> result = new List<string>();
            try
            {
                SendString = "USER " + UserName + "\r\n";
                BufferString = System.Text.Encoding.ASCII.GetBytes(SendString.ToCharArray());
                networkstream.Write(BufferString, 0, BufferString.Length);
                result.Add(streamreader.ReadLine());

                SendString = "PASS " + Password + "\r\n";
                BufferString = System.Text.Encoding.ASCII.GetBytes(SendString.ToCharArray());
                networkstream.Write(BufferString, 0, BufferString.Length);
                result.Add(streamreader.ReadLine());

                SendString = "STAT" + "\r\n";
                BufferString = System.Text.Encoding.ASCII.GetBytes(SendString.ToCharArray());
                networkstream.Write(BufferString, 0, BufferString.Length);
                result.Add(streamreader.ReadLine());

                SendString = "LIST" + "\r\n";
                BufferString = System.Text.Encoding.ASCII.GetBytes(SendString.ToCharArray());
                networkstream.Write(BufferString, 0, BufferString.Length);
                result.Add(streamreader.ReadLine());
            }
            catch (InvalidOperationException err)
            {
                throw new Exception(err.ToString());
            }
            return result;
        }

        private void DisConnect(object sender, System.EventArgs e)
        {
            List<string> result = new List<string>();
            try
            {
                SendString = "QUIT\r\n";
                BufferString = System.Text.Encoding.ASCII.GetBytes(SendString.ToCharArray());
                networkstream.Write(BufferString, 0, BufferString.Length);
                result.Add(streamreader.ReadLine());
                networkstream.Close();
                streamreader.Close();
            }
            catch { throw new Exception("The Connection has not exist!"); }

        }

        private List<string> Receive(int MailIndex)
        {
            List<string> result = new List<string>();
            string TempString;
            TempString = " ";
            SendString = "RETR " + MailIndex + "\r\n";
            BufferString = System.Text.Encoding.ASCII.GetBytes(SendString.ToCharArray());
            networkstream.Write(BufferString, 0, BufferString.Length);
            TempString = streamreader.ReadLine();
            if (TempString[0] != '-')
            {
                while (TempString != ".")
                {
                    TempString = streamreader.ReadLine();
                }
                TempString = streamreader.ReadLine();
                while (TempString != ".")
                {
                    result.Add(TempString + "\r\n");
                    TempString = streamreader.ReadLine();
                }
            }
            else
            {
                result.Add("\r\n Error!\r\n");
            }
            return result;
        }
    }
}

//以下是 EmailSender 示例代码：

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace SendMail
//{
//    public partial class MainForm : Form
//    {
//        public MainForm()
//        {
//            InitializeComponent();

//            comMailType.SelectedIndex = 0;
//        }

//        /// <summary>
//        /// 点击发送
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void btnSend_Click(object sender, EventArgs e)
//        {
//            //收件人列表
//            List<string> emailList = new List<string>();
//            //smtp
//            string smtp = null;
//            //用户名
//            string userName;
//            //密码
//            string password;
//            //来源邮件
//            string fromEmail = null;
//            //来源显示
//            string fromName;
//            //端口(一般为25)
//            int smtpPort = 25;

//            //获取收件人列表
//            string[] receivers = txtReveivers.Text.Split(';');
//            for (int i = 0; i < receivers.Length; i++)
//            {
//                emailList.Add(receivers[i]);
//            }

//            //获取smtp
//            if (comMailType.SelectedIndex == 0)
//            {
//                smtp = "smtp.163.com";
//            }

//            //获取用户名
//            userName = txtUserName.Text;

//            //获取密码
//            password = txtPwd.Text;

//            //来源邮件
//            if (comMailType.SelectedIndex == 0)
//            {
//                fromEmail = userName + "@163.com";
//            }

//            //来源显示
//            fromName = "猛男";

//            MailHelper oMailHelper = new MailHelper(emailList, smtp, userName, password, fromEmail, fromName, smtpPort);

//            try
//            {
//                oMailHelper.Send(txtSubject.Text, txtBody.Text);
//                MessageBox.Show("发送完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//    }
//}

/*        EmailReceiver 使用方法
 
     Connect("pop3.sina.com", "yusiyu1987@sina.com", "51379888sina")

 */
