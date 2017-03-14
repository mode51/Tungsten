using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using W.Domains;
using W.RPC.Interfaces;

namespace W.RPC.Host.Demo
{
    public partial class frmMain : Form
    {
        private readonly W.Domains.DomainLoader _domain;

        public frmMain()
        {
            InitializeComponent();
            _domain = new DomainLoader("RPC", true);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Server?.Stop();
            Server?.Dispose();
            Server = null;
            _domain.Dispose();

            base.OnClosing(e);
        }

        private IServer Server
        {
            get
            {
                return _domain.GetData<IServer>("RPCServer");
            }
            set
            {
                _domain.SetData("RPCServer", value);
            }
        } 

        private void btnStart_Click(object sender, EventArgs e)
        {
            _domain.Load();

            //_domain.ExecuteStaticMethod("W.RPC.Server", "StartInstance", IPAddress.Parse("127.0.0.1"), 5150);
            Server = _domain.Create<IServer>("W.RPC.Server");
            Server?.Start(IPAddress.Parse("127.0.0.1"), 5150);

            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //_domain.ExecuteStaticMethod("W.RPC.Server", "StopInstance");
            Server?.Stop();
            _domain.Unload();

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}
