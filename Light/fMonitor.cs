using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Light
{
    public partial class fMonitor : Form
    {
        public SerialPort Port { get; set; }

        public string recvTXData;
        public string recvRXData;

        public int Len { get; set; }

        public fMonitor()
        {
            InitializeComponent();
        }

        public void ReadTXData()
        {
            rtxtTX.Text += recvTXData;
        }

        private void fMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnClearTX_Click(object sender, EventArgs e)
        {
            rtxtTX.Clear();
            recvTXData = "";
        }

        private void btnClearRX_Click(object sender, EventArgs e)
        {
            rtxtRX.Clear();
            recvTXData = "";
        }
    }
}