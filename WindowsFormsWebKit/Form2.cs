using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LumiSoft.Net.POP3.Client;
using LumiSoft.Net.Mail;
using System.Text.RegularExpressions;
namespace WindowsFormsWebKit
{
    public partial class Form2 : Form
    {
        private static Regex regx = new Regex("已经为您开通云呼叫中心账户，登录账户：(.*?)，密码：(.*?)</td>", RegexOptions.IgnoreCase, new TimeSpan(0, 0, 10));
        delegate void dispText();
        public Form2()
        {
            InitializeComponent();
        }

        private void btn_receive_email_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                GetEmails();
            }); 
        }

        private void GetEmails()
        {
            using (POP3_Client c = new POP3_Client())
            {
                c.Connect("pop.exmail.qq.com", 995, true);
                c.Login("wangruimeng@gaodun.com", "1qaz!QAZ");
                if (c.Messages.Count > 0)
                {
                    for (var i = 960; i > -1; i--)
                    {
                        //var t = Mail_Message.ParseFromByte(c.Messages[i].MessageTopLinesToByte(50));
                        var header = Mail_Message.ParseFromByte(c.Messages[i].HeaderToByte());
                        var from = header.From;
                        if (from != null && from.ToArray().Any(a => a.Address == "system@notice.aliyun.com"))
                        {

                            var t = Mail_Message.ParseFromByte(c.Messages[i].MessageToByte());
                            var to = t.To;
                            var date = t.Date;
                            var subject = t.Subject;
                            var bodyText = t.BodyText;
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(t.BodyHtmlText))
                                {
                                    bodyText = t.BodyHtmlText;
                                }
                                var match = regx.Match(bodyText);
                                if (this.richText_content.InvokeRequired)
                                {
                                    this.richText_content.Invoke(new dispText(() => {
                                        this.richText_content.AppendText("\r\n" + string.Format("Account:{0};Pwd:{1};", match.Groups[1].Value, match.Groups[2].Value));
                                    }));
                                }
                                else {
                                    this.richText_content.AppendText("\r\n" + string.Format("Account:{1};Pwd:{2};", match.Groups[0].Value, match.Groups[1].Value));
                                }
                            }
                            catch (Exception ex)
                            {}
                        }

                    }

                }
            }
        }
    }
}
