using System.Drawing;
using System.Windows.Forms;
using Computator.NET.Data;
using Computator.NET.DataTypes.Localization;
using Computator.NET.Functions;
using Computator.NET.Properties;

namespace Computator.NET.UI.Controls.AutocompleteMenu
{
    internal class WebBrowserForm : Form
    {
        private static readonly WebBrowserForm _instance = new WebBrowserForm();


        private readonly WebBrowser webBrowser;


        public WebBrowserForm()
        {
            FormClosing += Form_FormClosing;
            Text = Strings.WebBrowserForm_WebBrowserForm_Functions___Constants_Details;
            webBrowser = new WebBrowser
            {
                MinimumSize = new Size(300, 195),
                ScrollBarsEnabled = true,
                Dock = DockStyle.Fill
            };

            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;

            Icon = Resources.computator_net_icon;

            TopMost = true;
            ShowInTaskbar = false;


            Controls.Add(webBrowser);
        }

        public string HTMLCode
        {
            set
            {
                webBrowser.Size = webBrowser.MinimumSize;
                webBrowser.DocumentText = value;
            }
        }

        public static void Show(FunctionInfo functionInfo)
        {
            _instance.SetFunctionInfo(functionInfo);
            _instance.Show();
        }

        public void SetFunctionInfo(FunctionInfo functionInfo)
        {
            HTMLCode = @"<b>" + functionInfo.Title + @"</b>" + @"<hr>" + functionInfo.Description + @" <br /><br /><i>" +
                       Strings.BrBrISourceBrAHref + @"<br /><a href=""" +
                       functionInfo.Url.Replace("http://en.wikipedia", "http://en.m.wikipedia") + @""">" +
                       functionInfo.Url + @"</a></i>";
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // this cancels the close event.
            Hide();
        }

        private void WebBrowser_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            var webBrowser = sender as WebBrowser;

            var r = webBrowser.Document.Body.ScrollRectangle;

            int height;
            var overlapp = 18;

            if (r.Size.Height + overlapp < webBrowser.Size.Height)
                height = r.Size.Height + overlapp;
            else
                height = webBrowser.Size.Height;

            webBrowser.Size = new Size(r.Width + overlapp, height);
            //  webBrowser.Document.Body.Style = "zoom:80%;";
        }
    }
}