using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using W;
using W.RPC;

namespace W.RPC.ClientDemo
{
    public partial class frmMain : Form
    {
        private W.RPC.IClient _client = new Client();

        public frmMain()
        {
            InitializeComponent();
            _client.Connected += (client, address) =>
            {
                this.InvokeEx(o =>
                {
                    btnConnect.Enabled = true;
                    btnSend.Enabled = true;
                    btnTestTimeout.Enabled = true;
                    btnTestTimeout2.Enabled = true;
                    btnSimplestRPC.Enabled = true;
                    btnConnect.Text = "&Disconnect";
                });
            };
            _client.Disconnected += (client, exception) =>
            {
                this.InvokeEx(o =>
                {
                    btnConnect.Enabled = true;
                    btnSend.Enabled = false;
                    btnTestTimeout.Enabled = false;
                    btnTestTimeout2.Enabled = false;
                    btnSimplestRPC.Enabled = false;
                    btnConnect.Text = "&Connect";
                });
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _client.Disconnect();
            base.OnClosing(e);
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            if (_client.IsConnected)
                _client.Disconnect();
            else
                await _client.ConnectAsync("127.0.0.1", 5150); //Disconnected will be raised if this fails to connect
        }
        private async void btnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            if (_client.IsConnected)
            {
                _client.MakeRPCCall("W.RPC.API.Demo.Sample.WriteLine", async () =>
                {
                    await Task.Run(() => //ensure that this method exits without blocking the ManualResetEvent
                    {
                        this.InvokeEx(o =>
                        {
                            MessageBox.Show("RPC Call Succeeded");
                            btnSend.Enabled = true;
                        });
                    });
                }, txtMessage.Text);
            }
        }

        private async void btnTestTimeout_Click(object sender, EventArgs e)
        {
            btnTestTimeout.Enabled = false;
            if (_client.IsConnected)
            {
                //this call returns a boolean result
                var mre = _client.MakeRPCCall("W.RPC.API.Demo.Sample.CauseTimeout", async () =>
                {
                    //ensure that this method exits without blocking the UI
                    await Task.Run(() =>
                    {
                        this.InvokeEx(o =>
                        {
                            MessageBox.Show("RPC Call Succeeded");
                            btnTestTimeout.Enabled = true;
                        });
                    });
                });

                await Task.Run(() =>
                {
                    if (!mre.WaitOne(3000))
                    {
                        this.InvokeEx(o =>
                        {
                            MessageBox.Show("The RPC call timed out");
                            btnTestTimeout.Enabled = true;
                        });
                    }
                });
            }
        }

        private async void btnTestTimeoutWithResult_Click(object sender, EventArgs e)
        {
            btnTestTimeout2.Enabled = false;
            if (_client.IsConnected)
            {
                //this call returns a boolean result
                var mre = _client.MakeRPCCall<bool>("W.RPC.API.Demo.Sample.CauseTimeout", async (result) =>
                {
                    //ensure that this method exits without blocking the UI
                    await Task.Run(() =>
                    {
                        if (!result)
                            return;
                        this.InvokeEx(o =>
                        {
                            MessageBox.Show("RPC Call Succeeded");
                            btnTestTimeout2.Enabled = true;
                        });
                    });
                });

                await Task.Run(() =>
                {
                    if (!mre.WaitOne(3000))
                    {
                        this.InvokeEx(o =>
                        {
                            MessageBox.Show("The RPC call timed out");
                            btnTestTimeout2.Enabled = true;
                        });
                    }
                });
            }
        }

        private void btnSimplestRPC_Click(object sender, EventArgs e)
        {
            if (_client.IsConnected)
            {
                _client.MakeRPCCall("W.RPC.API.Demo.Sample.WriteLine", null, txtMessage.Text);
            }
        }

    }
}
