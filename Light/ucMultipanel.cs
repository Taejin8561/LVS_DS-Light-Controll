using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Light
{
    public partial class ucMultipanel : UserControl
    {
        public TextBox[] txtCh1, txtCh2, txtCh3;
        public CheckBox[] chBox1, chBox2, chBox3;

        public string pageNow = "00";
        public int ChannelCount;

        public ucMultipanel()
        {
            InitializeComponent();
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
        private void Page_CheckedChanged(object sender, EventArgs e)
        {
            if (Page00.Checked)
            {
                pageNow = "00";
                panelChannel1.Visible = true;
                panelChannel2.Visible = false;
                panelChannel3.Visible = false;
            }
            else if (Page01.Checked)
            {
                pageNow = "01";
                panelChannel1.Visible = false;
                panelChannel2.Visible = true;
                panelChannel3.Visible = false;
            }
            else if (Page02.Checked)
            {
                pageNow = "02";
                panelChannel1.Visible = false;
                panelChannel2.Visible = false;
                panelChannel3.Visible = true;
            }
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
        private void cbx_CheckedChanged(object sender, EventArgs e)
        {
            InitializeChannelCount();
        }
    }
}
