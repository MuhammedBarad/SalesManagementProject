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
    public partial class Frm_SaleQty : DevExpress.XtraEditors.XtraForm
    {
        public Frm_SaleQty()
        {
            InitializeComponent();
        }

        private void Frm_SaleQty_Load(object sender, EventArgs e)
        {
            txtQty.Text = Properties.Settings.Default.Item_Qty.ToString();
            txtSalePrice.Text = Properties.Settings.Default.Item_SalePrice.ToString();
            txtDiscount.Text = Properties.Settings.Default.Item_Discount.ToString();

            txtQty.Focus();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtQty.Text == "" || txtSalePrice.Text == "" || txtDiscount.Text == "")
            {
                MessageBox.Show("Lütfen tüm alanları doldurun", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Properties.Settings.Default.Item_Qty = Convert.ToDecimal(txtQty.Text);//yeni girdiğimiz ürün miktarı (Item_Qty) değişkene aktar.
            Properties.Settings.Default.Item_SalePrice = Convert.ToDecimal(txtSalePrice.Text);//yeni girdiğimiz ürün fiyatı (Item_SalePrice) değişkene aktar. 
            Properties.Settings.Default.Item_Discount = Convert.ToDecimal(txtDiscount.Text);//yeni girdiğimiz ürün indirimi (Item_Discount) değişkene aktar.
            Properties.Settings.Default.Save();
            Close();
        }

        private void Frm_SaleQty_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (txtQty.Text == "" || txtSalePrice.Text == "" || txtDiscount.Text == "")
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Properties.Settings.Default.Item_Qty = Convert.ToDecimal(txtQty.Text);
                Properties.Settings.Default.Item_SalePrice = Convert.ToDecimal(txtSalePrice.Text);
                Properties.Settings.Default.Item_Discount = Convert.ToDecimal(txtDiscount.Text);
                Properties.Settings.Default.Save();
                Close();
            }
            
        }
        //bu form kapatılınca yeni değişikler faturaya yansıt.
        private void Frm_SaleQty_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Frm_Sales.GetForm.dgvSale.CurrentRow.Cells[2].Value = Properties.Settings.Default.Item_Qty;
                Frm_Sales.GetForm.dgvSale.CurrentRow.Cells[3].Value = Properties.Settings.Default.Item_SalePrice;
                Frm_Sales.GetForm.dgvSale.CurrentRow.Cells[4].Value = Properties.Settings.Default.Item_Discount;

            }
            catch (Exception)
            { }
        }
    }
}