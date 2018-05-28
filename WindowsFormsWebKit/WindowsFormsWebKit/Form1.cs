using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsWebKit.Model;
using WindowsFormsWebKit.Service;

namespace WindowsFormsWebKit
{
    public partial class Form1 : Form
    {
        delegate void delNextClick();
        List<AliAccountDto> accounts = new List<AliAccountDto>();
        AliAuthService aliAuthService;
        int currIndex = -1;
        public Form1()
        {
            InitializeComponent();
            aliAuthService = new AliAuthService();
            accounts = aliAuthService.GetAccountData();
        }

        private void btn_go_Click(object sender, EventArgs e)
        {
            currIndex++;
            if (accounts.Count - 1 >= currIndex)
            {
                var account = accounts[currIndex];
                this.txt_url.Text = aliAuthService.GetSignUrl(account.UserId);
                this.webBrowser1.Url = new Uri(this.txt_url.Text);
            }
            else {
                this.webBrowser1.Url = new Uri(this.txt_url.Text);
            }
            
        }

        private void btn_next_Click(object sender, EventArgs e)
        {

            this.webBrowser1.Document.InvokeScript("btn_next");
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                if (btn_submit.InvokeRequired)
                {
                    btn_submit.Invoke(new delNextClick(() =>
                    {
                        btn_submit.PerformClick();
                    }));
                }
            });
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (currIndex > accounts.Count - 1) {
                    return;
                }
                var account = accounts[currIndex];
                HtmlElement ele = webBrowser1.Document.CreateElement("script");
                ele.SetAttribute("type", "text/javascript");
                ele.SetAttribute("text", "function formSubmit(){ document.getElementById('u22').firstChild.click();} function btn_next(){document.getElementById('J_FormNext').click(); };");
                webBrowser1.Document.Body.AppendChild(ele);  
                var document=webBrowser1.Document;
                var txtAccount = document.GetElementById("user_principal_name");
                txtAccount.SetAttribute("value", account.Account);
                var txtPwd = document.GetElementById("password_ims");
                txtPwd.SetAttribute("value",account.Pwd);
                var htmlBtn_Next = webBrowser1.Document.GetElementById("J_FormNext");
                var dic_submint = document.GetElementById("u22").FirstChild;
                dic_submint.Click += dic_submint_Click;
                htmlBtn_Next.Click += htmlBtn_Next_Click;
                Task.Run(() => {
                    Thread.Sleep(1000);
                    if (btn_next.InvokeRequired) {
                        btn_next.Invoke(new delNextClick(() => {
                            btn_next.PerformClick();
                        }));
                    } 
                });
            }
            catch (Exception ex) {
            }
            
        }

        void dic_submint_Click(object sender, HtmlElementEventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show("on submit click");
        }

        void htmlBtn_Next_Click(object sender, HtmlElementEventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show("on next click");
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Document.InvokeScript("formSubmit");
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                if (webBrowser1.InvokeRequired)
                {
                    webBrowser1.Invoke(new delNextClick(() =>
                    {
                        webBrowser1.Document.Cookie.Remove(0, webBrowser1.Document.Cookie.Length);
                        webBrowser1.Document.ExecCommand("ClearAuthenticationCache", false, null);
                    }));
                }
            });
        }
    }
}
