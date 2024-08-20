using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Light
{
    public partial class fMain : Form
    {
        private const byte LF = 0x0A;
        private const byte CR = 0x0D;

        private TextBox[] txtCh1, txtCh2, txtCh3;
        private CheckBox[] chBox1, chBox2, chBox3;

        private bool IsRun;

        private string[,] WriteData;

        private fMonitor Monitor;

        public SerialPort sPort;
        public List<SerialPort> PortList;
        public PortInfo pInfo;

        public Thread PageTrigerThread;

        public int ChannelCount;

        public string pageNow = "00";

        public int MaxPage;

        public fMain()
        {
            InitializeComponent();

            #region ComboBox Init
            cbxBaudRate.SelectedIndex = 9;
            cbxDataBits.SelectedIndex = 3;
            cbxStopBits.SelectedIndex = 0;
            cbxParity.SelectedIndex = 0;
            cbxFlowControl.SelectedIndex = 2;
            #endregion

            #region Component 일괄처리 변수
            txtCh1 = new TextBox[] { txt1_1, txt1_2, txt1_3, txt1_4, txt1_5, txt1_6, txt1_7, txt1_8,
                txt1_9, txt1_10, txt1_11, txt1_12 };
            txtCh2 = new TextBox[] {txt2_1, txt2_2, txt2_3, txt2_4, txt2_5, txt2_6, txt2_7, txt2_8,
                txt2_9, txt2_10, txt2_11, txt2_12};
            txtCh3 = new TextBox[] { txt3_1, txt3_2, txt3_3, txt3_4, txt3_5, txt3_6, txt3_7, txt3_8,
                txt3_9, txt3_10, txt3_11, txt3_12 };

            chBox1 = new CheckBox[] { cbx1_1, cbx1_2, cbx1_3, cbx1_4, cbx1_5, cbx1_6, cbx1_7, cbx1_8,
                cbx1_9, cbx1_10, cbx1_11, cbx1_12 };
            chBox2 = new CheckBox[] { cbx2_1, cbx2_2, cbx2_3, cbx2_4, cbx2_5, cbx2_6, cbx2_7, cbx2_8,
                cbx2_9, cbx2_10, cbx2_11, cbx2_12 };
            chBox3 = new CheckBox[] { cbx3_1, cbx3_2, cbx3_3, cbx3_4, cbx3_5, cbx3_6, cbx3_7, cbx3_8,
                cbx3_9, cbx3_10, cbx3_11, cbx3_12 };
            #endregion

            sPort = new SerialPort();

            Monitor = new fMonitor();

            InitializeChannelCount();

            MaxPage = int.Parse(txtMaxPage.Text);

            WriteData = new string[MaxPage, ChannelCount];

            IsRun = true;

            InitPort();

            sPort.DataReceived += new SerialDataReceivedEventHandler(sPort_DataReceived);

            sPort.ErrorReceived += new SerialErrorReceivedEventHandler(sPort_ErrorReceived);
        }

        #region PortComboBox Init
        public void InitPort()
        {
            string[] GetPort = SerialPort.GetPortNames();

            // SerialPort 객체 리스트
            PortList = new List<SerialPort>();

            for (int i = 0; i < GetPort.Length; i++)
            {
                cbxPort.Items.Add(GetPort[i]);
                PortList.Add(new SerialPort(GetPort[i], 19200, Parity.None, 8, StopBits.One)); // 각 PortName 으로 객체 생성

                PortList[i].DataReceived += new SerialDataReceivedEventHandler(PortList_DataReceived);
                
                try
                {
                    if (!PortList[i].IsOpen)
                        PortList[i].Open();
                }
                catch
                {
                    continue;   // PortList[i].Open() 실패시
                }
                WriteFirst();
            }
        }
        public void WriteFirst()
        {
            string sbData;
            byte[] SendData;

            sbData = $":00B03001000,000,000,000,000,000,000,000,000,000,000,000";

            SendData = new byte[sbData.Length + 2];

            Encoding.ASCII.GetBytes(sbData.ToString()).CopyTo(SendData, 0);
            SendData[sbData.Length] = CR;
            SendData[sbData.Length + 1] = LF;

            for (int i = 0; i < PortList.Count; i++)
            {
                if (PortList[i].IsOpen)
                    PortList[i].Write(SendData, 0, SendData.Length);
            }
        }
        void PortList_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            for (int i = 0; i < PortList.Count; i++)
            {
                string rData;
                byte[] recvData;

                try
                {
                    recvData = new byte[PortList[i].BytesToRead];
                    PortList[i].Read(recvData, 0, recvData.Length);
                }
                catch
                {
                    continue;
                }
                rData = Encoding.ASCII.GetString(recvData);

                // \u0006 == ACK 
                if (rData == "\u0006")
                {
                    cbxPort.SelectedItem = PortList[i].PortName;
                    break;
                }
            }
        }
        #endregion

        #region Set & Connect
        public void SetPort()
        {
            try
            {
                for(int i=0; i < PortList.Count; i++)
                {
                    if (PortList[i].IsOpen)
                        PortList[i].Close();
                }

                pInfo = new PortInfo();

                int BaudRate = Convert.ToInt32(cbxBaudRate.SelectedItem);
                int DataBits = Convert.ToInt32(cbxDataBits.SelectedItem);

                pInfo.Port = cbxPort.SelectedItem.ToString();
                pInfo.BaudRate = BaudRate;
                pInfo.DataBits = DataBits;
                pInfo.StopBits = cbxStopBits.SelectedItem.ToString();
                pInfo.Parity = cbxParity.SelectedItem.ToString();
                pInfo.HandShake = cbxFlowControl.SelectedItem.ToString();

                if(pInfo != null)
                {
                    sPort.PortName = pInfo.Port;
                    sPort.BaudRate = pInfo.BaudRate;
                    sPort.DataBits = pInfo.DataBits;

                    sPort.ReadTimeout = 500;
                    sPort.WriteTimeout = 500;

                    switch (pInfo.StopBits)
                    {
                        #region StopBits
                        case "None":
                            sPort.StopBits = StopBits.None;
                            break;
                        case "1":
                            sPort.StopBits = StopBits.One;
                            break;
                        case "1.5":
                            sPort.StopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            sPort.StopBits = StopBits.Two;
                            break;
                            #endregion
                    }
                    switch (pInfo.Parity)
                    {
                        #region Parity
                        case "None":
                            sPort.Parity = Parity.None;
                            break;
                        case "Odd":
                            sPort.Parity = Parity.Odd;
                            break;
                        case "Even":
                            sPort.Parity = Parity.Even;
                            break;
                        case "Mark":
                            sPort.Parity = Parity.Mark;
                            break;
                        case "Space":
                            sPort.Parity = Parity.Space;
                            break;
                            #endregion
                    }
                    switch (pInfo.HandShake)
                    {
                        #region HandShake
                        case "Hardware":
                            sPort.Handshake = Handshake.RequestToSend;
                            break;
                        case "Software":
                            sPort.Handshake = Handshake.XOnXOff;
                            break;
                        case "None":
                            sPort.Handshake = Handshake.None;
                            break;
                        case "Custom":
                            sPort.Handshake = Handshake.RequestToSendXOnXOff;
                            break;
                            #endregion
                    }
                }
            }
            catch (Exception e)
            {
                if (sPort.IsOpen)
                    MessageBox.Show("Disconnect First");
                else
                    MessageBox.Show(e.Message);
            }
        }
        public void Connect()
        {
            try
            {
                sPort.Open();
                Monitor.Port = sPort;
            }
            catch
            {
                MessageBox.Show("Port Open Fail");
            }
        }
        #endregion

        #region Read/Write/Confirm/Save/PageTriger
        public void MakeData()
        {
            MaxPage = int.Parse(txtMaxPage.Text);

            for (int i = 0; i < MaxPage; i++)
            {
                for (int j = 0; j < ChannelCount; j++)
                {
                    WriteData[0, j] = txtCh1[j].Text;
                    WriteData[1, j] = txtCh2[j].Text;
                    WriteData[2, j] = txtCh3[j].Text;
                }
            }
            Monitor.Len = MaxPage;
        }
        public void InitializeChannelCount()
        {
            ChannelCount = 0;

            for (int i = 0; i < chBox1.Length; i++)
            {
                if (chBox1[i].Checked == true)
                    ChannelCount++;
            }
        }
        public void WriteAll()
        {
            StringBuilder sbData;
            byte[] SendData;
            
            MakeData();

            for (int i = 0; i < MaxPage; i++)
            {
                sbData = new StringBuilder();

                // :00B03001
                sbData.Append($":{txtID.Text}B{txtMaxPage.Text}{i:D02}{txtReplay.Text}");
                for (int j = 0; j < ChannelCount; j++)
                {
                    sbData.Append(WriteData[i, j].ToString());
                    sbData.Append(",");
                }
                sbData.Remove(sbData.Length - 1, 1);

                SendData = new byte[sbData.Length + 2];

                Encoding.ASCII.GetBytes(sbData.ToString()).CopyTo(SendData, 0);
                SendData[sbData.Length] = CR;
                SendData[sbData.Length + 1] = LF;

                sPort.Write(SendData, 0, SendData.Length);

                Monitor.recvTXData += sbData.ToString() + "\n";
            }
            Monitor.ReadTXData();
            Monitor.recvTXData = "";
        }
        public void ConfirmData()
        {
            string sbData;
            byte[] SendData;

            if (sPort.IsOpen)
            {
                sbData = $":00R{CR}{LF}";

                SendData = new byte[sbData.Length];

                Encoding.ASCII.GetBytes(sbData.ToString()).CopyTo(SendData, 0);
                sPort.Write(SendData, 0, SendData.Length);

                Monitor.rtxtTX.Text += sbData.Substring(0, 4) + "\n";
            }
        }
        public void FlashSave()
        {
            string sbData;
            byte[] SendData;
           
            sbData = $":00S{CR}{LF}";
            
            SendData = new byte[sbData.Length];
            
            Encoding.ASCII.GetBytes(sbData.ToString()).CopyTo(SendData, 0);
            sPort.Write(SendData, 0, SendData.Length);
            
            Monitor.rtxtTX.Text += sbData.Substring(0, 4) + "\n";
        }
        public void PageTriger()
        {
            string sbData;
            byte[] SendData;

            while (IsRun)
            {
                if (cbxReset.Checked)
                    sbData = $":00TPS{CR}{LF}";
                else
                    sbData = $":00TPP{CR}{LF}";

                SendData = new byte[sbData.Length];

                Thread.Sleep(150);
                if (sPort.IsOpen)
                {
                    Encoding.ASCII.GetBytes(sbData.ToString()).CopyTo(SendData, 0);
                    sPort.Write(SendData, 0, SendData.Length);

                    Monitor.rtxtTX.Text += sbData.Substring(0, 6) + "\n";
                }
            }
        }
        #endregion

        #region Port_EventHandler
        void sPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender == sPort)
            {
                string rData;
                byte[] recvData;
                
                recvData = new byte[sPort.BytesToRead];
                
                sPort.Read(recvData, 0, recvData.Length);
                rData = Encoding.ASCII.GetString(recvData);
                
                txtStatus.BackColor = Color.Green;
                txtStatus.Text = "ACK";

                // rData = ACK 신호
                Monitor.rtxtRX.Text += rData;
            }
        }
        void sPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            txtStatus.BackColor = Color.Pink;
            txtStatus.Text = "FAIL";
        }
        #endregion

        #region Button CONNECT/SEND/DISCONNECT
        private void btnConnect_Click(object sender, EventArgs e)
        {
            object s = sender;
            SetPort();
            Connect();
            if (sPort.IsOpen)
            {
                btnDisconnect.Visible = true;
                txtConnected.BackColor = Color.Green;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            txtStatus.BackColor = Color.Yellow;
            txtStatus.Text = "IDLE";

            if (sPort.IsOpen)
            {
                WriteAll();
                if (sPort.BytesToRead == 0)
                {
                    txtStatus.BackColor = Color.IndianRed;
                    txtStatus.Text = "FAIL";
                }
            }
            else
            {
                txtStatus.BackColor = Color.IndianRed;
                txtStatus.Text = "FAIL";
            }
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            sPort.Close();
            if (!sPort.IsOpen)
            {
                btnDisconnect.Visible = false;
                txtConnected.BackColor = Color.Black;
            }
        }
        #endregion

        #region Control Event
        private void btnIdUp_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(txtID.Text);
            if (id < 63)
                id++;
            else
                id = 0;
            txtID.Text = id.ToString("D02");
        }
        private void btnIdDown_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(txtID.Text);
            if (id > 0)
                id--;
            else
                id = 63;
            txtID.Text = id.ToString("D02");
        }
        private void btnPageUp_Click(object sender, EventArgs e)
        {
            int mPage;
            mPage = int.Parse(txtMaxPage.Text);
            if (mPage < 3)
                mPage++;
            else
                mPage = 1;
            txtMaxPage.Text = mPage.ToString("D02");
        }
        private void btnPageDown_Click(object sender, EventArgs e)
        {
            int mPage;
            mPage = int.Parse(txtMaxPage.Text);
            if (mPage > 1)
                mPage--;
            else
                mPage = 3;
            txtMaxPage.Text = mPage.ToString("D02");
        }
        private void btnReplayUp_Click(object sender, EventArgs e)
        {
            int replay;
            replay = int.Parse(txtReplay.Text);
            if (replay < 9)
                replay++;
            else
                replay = 1;
            txtReplay.Text = replay.ToString();
        }
        private void btnReplayDown_Click(object sender, EventArgs e)
        {
            int replay;
            replay = int.Parse(txtReplay.Text);
            if (replay > 1)
                replay--;
            else
                replay = 9;
            txtReplay.Text = replay.ToString();
        }
        private void btnMonitor_Click(object sender, EventArgs e)
        {
            Monitor.Show();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (sPort.IsOpen)
            {
                FlashSave();
                sPort.WriteLine("");
            }
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (sPort.IsOpen)
            {
                ConfirmData();
                sPort.WriteLine("");
            }
        }
        private void btnTPageUp_Click(object sender, EventArgs e)
        {
            int mPage;
            mPage = int.Parse(txtTPage.Text);
            if (mPage < 3)
                mPage++;
            else
                mPage = 1;
            txtTPage.Text = mPage.ToString("D02");
        }
        private void btnTPageDown_Click(object sender, EventArgs e)
        {
            int mPage;
            mPage = int.Parse(txtTPage.Text);
            if (mPage > 1)
                mPage--;
            else
                mPage = 3;
            txtTPage.Text = mPage.ToString("D02");
        }
        private void btnTri_Click(object sender, EventArgs e)
        {
            if (sPort.IsOpen)
            {
                PageTrigerThread = new Thread(PageTriger);
                IsRun = true;
                PageTrigerThread.Start();

                sPort.WriteLine("");

                btnStop.Visible = true;
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            IsRun = false;
            btnStop.Visible = false;
        }
        private void rbtnAllCheck_CheckedChanged(object sender, EventArgs e)
        {
            switch (pageNow)
            {
                case "00":
                    for (int i = 0; i < chBox1.Length; i++)
                    {
                        chBox1[i].Checked = true;
                    }
                    break;
                case "01":
                    for (int i = 0; i < chBox1.Length; i++)
                    {
                        chBox2[i].Checked = true;
                    }
                    break;
                case "02":
                    for (int i = 0; i < chBox1.Length; i++)
                    {
                        chBox3[i].Checked = true;
                    }
                    break;
            }
            InitializeChannelCount();
        }
        private void rbtnAllClear_CheckedChanged(object sender, EventArgs e)
        {
            switch (pageNow)
            {
                case "00":
                    for (int i = 0; i < chBox1.Length; i++)
                    {
                        chBox1[i].Checked = false;
                    }
                    break;
                case "01":
                    for (int i = 0; i < chBox1.Length; i++)
                    {
                        chBox2[i].Checked = false;
                    }
                    break;
                case "02":
                    for (int i = 0; i < chBox1.Length; i++)
                    {
                        chBox3[i].Checked = false;
                    }
                    break;
            }
            InitializeChannelCount();
        }
        private void Page_CheckedChanged(object sender, EventArgs e)
        {
            if (rbn00.Checked)
            {
                pageNow = "00";
                panelChannel1.Visible = true;
                panelChannel2.Visible = false;
                panelChannel3.Visible = false;
            }
            else if (rbn01.Checked)
            {
                pageNow = "01";
                panelChannel1.Visible = false;
                panelChannel2.Visible = true;
                panelChannel3.Visible = false;
            }
            else if (rbn02.Checked)
            {
                pageNow = "02";
                panelChannel1.Visible = false;
                panelChannel2.Visible = false;
                panelChannel3.Visible = true;
            }
        }
        private void cbx_CheckedChanged(object sender, EventArgs e)
        {
            InitializeChannelCount();
        }
        private void btnScrollSet_Click(object sender, EventArgs e)
        {
            switch (pageNow)
            {
                case "00":
                    for (int i = 0; i < txtCh1.Length; i++)
                    {
                        if (chBox1[i].Checked)
                            txtCh1[i].Text = string.Format("{0:D3}", int.Parse(txtSet.Text));
                    }
                    txtSet.Text = string.Format("{0:D3}", int.Parse(txtSet.Text));
                    break;
                case "01":
                    for (int i = 0; i < txtCh2.Length; i++)
                    {
                        if (chBox2[i].Checked)
                            txtCh2[i].Text = string.Format("{0:D3}", int.Parse(txtSet.Text));
                    }
                    txtSet.Text = string.Format("{0:D3}", int.Parse(txtSet.Text));
                    break;
                case "02":
                    for (int i = 0; i < txtCh3.Length; i++)
                    {
                        if (chBox3[i].Checked)
                            txtCh3[i].Text = string.Format("{0:D3}", int.Parse(txtSet.Text));
                    }
                    txtSet.Text = string.Format("{0:D3}", int.Parse(txtSet.Text));
                    break;
            }
        }
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            switch (pageNow)
            {
                case "00":
                    for (int i = 0; i < txtCh1.Length; i++)
                    {
                        if (chBox1[i].Checked)
                            txtCh1[i].Text = hScrollBar.Value.ToString("D03");
                    }
                    break;
                case "01":
                    for (int i = 0; i < txtCh2.Length; i++)
                    {
                        if (chBox2[i].Checked)
                            txtCh2[i].Text = hScrollBar.Value.ToString("D03");
                    }
                    break;
                case "02":
                    for (int i = 0; i < txtCh3.Length; i++)
                    {
                        if (chBox3[i].Checked)
                            txtCh3[i].Text = hScrollBar.Value.ToString("D03");
                    }
                    break;
            }
            txtSet.Text = hScrollBar.Value.ToString("D03");
        }
        #endregion
    }
}