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
    public partial class Frm_ShowProducts : DevExpress.XtraEditors.XtraForm
    {
        DataBase db = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        public Frm_ShowProducts()
        {
            InitializeComponent();
        }
        //tüm ürün bilgilerini gösterilecek.
        private void ShowAllProducts()
        {
            table.Clear();
            table = db.ReadData("SELECT[Pro_ID] as'Ürün No',[Pro_Name] as'Ürün Adı',[Qty] as'Adet',[Buy_Price] as'Satın alma Fiyatı',[Sale_Price] as'Satış Fiyatı',[BarCode] as'Barkod',[MinQty] as'Talep Limiti',[MaxDiscount] as'maksimum indirim'FROM[dbo].[Products] ");
            if (table.Rows.Count <= 0)
            {
                MessageBox.Show("gösterilecek bir Ürün Yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dgvSearch.DataSource = table;
        }
        private void Frm_ShowProducts_Load(object sender, EventArgs e)
        {
            try
            {
                ShowAllProducts();
            }
            catch (Exception) { }
            
        }

        private void btnSearchName_Click(object sender, EventArgs e)
        {
            if(txtName.Text!="")
            {
                table.Clear();
                table = db.ReadData("SELECT[Pro_ID] as'Ürün No',[Pro_Name] as'Ürün Adı',[Qty] as'Adet',[Buy_Price] as'Satın alma Fiyatı',[Sale_Price] as'Satış Fiyatı',[BarCode] as'Barkod',[MinQty] as'Talep Limiti',[MaxDiscount] as'maksimum indirim'FROM[dbo].[Products] where [Pro_Name] like '%"+txtName.Text+"%' ");
                if(table.Rows.Count<=0)
                {
                    MessageBox.Show("Böyle bir Ürün Yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dgvSearch.DataSource = table;
            }
        }

        private void btnSearchBarkod_Click(object sender, EventArgs e)
        {
            if (txtBarkod.Text != "")
            {
                table.Clear();
                table = db.ReadData("SELECT[Pro_ID] as'Ürün No',[Pro_Name] as'Ürün Adı',[Qty] as'Adet',[Buy_Price] as'Satın alma Fiyatı',[Sale_Price] as'Satış Fiyatı',[BarCode] as'Barkod',[MinQty] as'Talep Limiti',[MaxDiscount] as'maksimum indirim'FROM[dbo].[Products] where [BarCode]='" + txtBarkod.Text + "' ");
                if (table.Rows.Count <= 0)
                {
                    MessageBox.Show("Böyle bir Ürün Yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dgvSearch.DataSource = table;
            }
        }
    }
}