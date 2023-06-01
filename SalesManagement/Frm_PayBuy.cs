using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SalesManagement
{
    public partial class Frm_PayBuy : DevExpress.XtraEditors.XtraForm
    {
        public Frm_PayBuy()
        {
            InitializeComponent();
        }

        private void Frm_PayBuy_Load(object sender, EventArgs e)
        {
            try
            {
                txtTotal.Text = Properties.Settings.Default.TotalOrder.ToString();
            }
            catch(Exception)
            { }
            txtpaied.Text = "0.0";
            txtRemainder.Text = "0.0";
            txtpaied.Focus();
        }
        //ödemek istediğimiz miktar girdikten sonra kalan tutar güncelle.
        private void txtpaied_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal Remainder = Convert.ToDecimal(txtTotal.Text) - Convert.ToDecimal(txtpaied.Text);
                txtRemainder.Text = Remainder.ToString();
            }
            catch(Exception)
            {  }
        }
        //ödeme yap.
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if(txtpaied.Text=="")
            {
                MessageBox.Show("Lütfen ödenecek tutarı giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Properties.Settings.Default.Paied = Convert.ToDecimal(txtpaied.Text);
            Properties.Settings.Default.Remainder = Convert.ToDecimal(txtRemainder.Text);
            Properties.Settings.Default.CheckButton = true;
            Properties.Settings.Default.Save();
            Close();
        }

        private void Frm_PayBuy_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)//ödeme yap.
            {
                if (txtpaied.Text == "")
                {
                    MessageBox.Show("Lütfen ödenecek tutarı giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Properties.Settings.Default.Paied = Convert.ToDecimal(txtpaied.Text);
                Properties.Settings.Default.Remainder = Convert.ToDecimal(txtRemainder.Text);
                Properties.Settings.Default.CheckButton = true;
                Properties.Settings.Default.Save();
                Close();
            }
            else if(e.KeyCode==Keys.F12)//geri dön (fatura ödemekten vazgeç)
            {
                Properties.Settings.Default.CheckButton = false;//fatura ödeme işlemine devam etmesin diye.
                Properties.Settings.Default.Save();
                Close();
            }
        }
        //geri dön (fatura ödemekten vazgeç)
        private void btnBack_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckButton = false;//f12 fatura ödemekten vazgeç.
            Properties.Settings.Default.Save();
            Close();
        }
    }
}