using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using W.Domains.Plugin.Interface.Demo;
using W.Logging;
using W.Domains;

namespace W.Domains.Demo
{
    public partial class frmMain : Form
    {
        private W.Domains.DomainLoader _domain;// = new CustomDomain("Plugins", true, true);

        public frmMain()
        {
            InitializeComponent();
            _domain = new DomainLoader("Plugins", true);
            _domain.Load();
            AddMessage("Domain Loaded");
        }

        ~frmMain()
        {
            _domain.Dispose();
        }

        private void AddMessage(string format, params object[] args)
        {
            var msg = string.Format(format, args);
            lstMessages.Items.Insert(0, msg);
        }
        private void btnReloadDomain_Click(object sender, EventArgs e)
        {
            _domain.Unload();
            _domain.Load();
            AddMessage("Domain Reloaded");
        }

        private void btnExecute1_Click(object sender, EventArgs e)
        {
            var plugin = _domain.Create<IDomainPlugin>("W.Domains.Plugin.Demo.Plugin_Example");
            plugin.LogInformation("This is a test {0}", "log message.");
        }

        private void btnExecute2_Click(object sender, EventArgs e)
        {
            var plugin = _domain.Create<IDomainPlugin>("W.Domains.Plugin.Demo.Plugin_Example");
            AddMessage(plugin.GetMessage());
        }

        private void btnExecute3_Click(object sender, EventArgs e)
        {
            var result = _domain.ExecuteStaticMethod<int>("W.Domains.Plugin.Demo.Plugin_Example", "Multiply", 4, 5);
            AddMessage("Multiply = {0}", result);
        }

        private void btnExecute4_Click(object sender, EventArgs e)
        {
            var plugin = _domain.Create<IDomainPlugin>("W.Domains.Plugin.Demo.Plugin_Example");
            var result = plugin.GetSomeValue("some value");
            AddMessage("Success={0}, Result={1}, Exception={2}", result.Success, result.Result, result.Exception.Message);
        }

        private void btnExecute5_Click(object sender, EventArgs e)
        {
            var result = _domain.ExecuteStaticMethod<IDomainResult>("W.Domains.Plugin.Demo.Plugin_Example", "MultiplyEx", 4, 5);
            AddMessage("Success={0}, Result={1}, Exception={2}", result.Success, result.Result, result.Exception.Message);
        }

        private void btnDoCallback_Click(object sender, EventArgs e)
        {
            _domain.DoCallback(() =>
            {
                Console.WriteLine("AppDomain({0}): Success={1}, Result={2}, Exception={3}", AppDomain.CurrentDomain.FriendlyName, true, null, "No Exception");
            });
            Console.WriteLine("AppDomain({0}): Success={1}, Result={2}, Exception={3}", AppDomain.CurrentDomain.FriendlyName, true, null, "No Exception");
        }

        private void btnDoCallback2_Click(object sender, EventArgs e)
        {

            //_domain.DoCallback(() =>
            //{
            //    Console.WriteLine("AppDomain({0}): Success={1}, Result={2}, Exception={3}", AppDomain.CurrentDomain.FriendlyName, true, null, "No Exception");
            //});
            //Console.WriteLine("AppDomain({0}): Success={1}, Result={2}, Exception={3}", AppDomain.CurrentDomain.FriendlyName, true, null, "No Exception");
        }
    }
}
