using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesManagement
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
          
        }
        DataBase db = new DataBase();
        DataTable tbl = new DataTable();
        private void Login_Load(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtUserName.Focus();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        bool check = true;
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (check)
            {
                this.pictureBox4.SendToBack();
                this.txtPassword.UseSystemPasswordChar = false;
                check = false;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (check == false)
            {
                this.pictureBox3.SendToBack();
                this.txtPassword.UseSystemPasswordChar = true;
                check = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbl.Clear();
            if (rbtnManager.Checked == true)
                tbl = db.ReadData("select * from Users where User_Name='" + txtUserName.Text + "' and Password='" + txtPassword.Text + "' and Type='Manager'");
            else if (rbtnEmp.Checked == true)
                tbl = db.ReadData("select * from Users where User_Name='" + txtUserName.Text + "' and Password='" + txtPassword.Text + "' and Type='Employee'");
            
            if (tbl.Rows.Count >= 1)
            {
                Properties.Settings.Default.USERNAME = txtUserName.Text;
                Properties.Settings.Default.Save();
                this.Hide();
                Form1 frm = new Form1();
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Kullancı adı veya şifre Hatalı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(null, null);
            }
        }

        private void rbtnEmp_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
