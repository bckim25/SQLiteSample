using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SQLiteSample
{
    public partial class InsertForm : Form
    {
        public string Scn_name;
        public string location;
        public string season;
        public string weather;
        public string visdist;
        public string timeofday;
        public string cloudlow;
        public string cloudhigh;
        public string cloudquantity;

        public InsertForm()
        {
            InitializeComponent();
            CenterToParent();
        }

        public void ChangeBtnTxt(string txt)
        {
            button1.Text = txt;
        }

        public void setData(string Scn_name, string location, string season, string weather, string visdist, string timeofday, string cloudlow, string cloudhigh, string cloudquantity)
        {
            textBox1.Text = Scn_name;
            textBox2.Text = location;
            textBox3.Text = season;
            textBox4.Text = weather;
            textBox5.Text = visdist;
            textBox6.Text = timeofday;
            textBox7.Text = cloudlow;
            textBox8.Text = cloudhigh;
            textBox9.Text = cloudquantity;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Scn_name = textBox1.Text;
            location = textBox2.Text;
            season = textBox3.Text;
            weather = textBox4.Text;
            visdist = textBox5.Text;
            timeofday = textBox6.Text;
            cloudlow = textBox7.Text;
            cloudhigh = textBox8.Text;
            cloudquantity = textBox9.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
