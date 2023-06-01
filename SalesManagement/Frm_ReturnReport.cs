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
    public partial class Frm_ReturnReport : DevExpress.XtraEditors.XtraForm
    {
        public Frm_ReturnReport()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();
        private void Frm_ReturnReport_Load(object sender, EventArgs e)
        {
            dtpFrom.Text = DateTime.Now.ToShortDateString();
            dtpTo.Text = DateTime.Now.ToShortDateString();
        }
        //iade edilen faturalar aramak için.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");
            table.Clear();
            if(rbtnAllReturns.Checked==true)//tüm iade edilen faturalar göstermek için.
            {
                table = db.ReadData("SELECT [Order_ID] as'Fatura No' ,[Sup_Name] as'Satıcı Adı',[Cust_Name] as'Müşteri Adı',[Pro_Name] as'Ürün Adı',[Date] as'Fartura Tarihi',[Qty] as'Adet',[Price] as'Fiyat',[Total] as'Toplam',[Total_Order] as'Fatura Tutarı',[Paied] as 'Ödenen Miktar',[Remainder] as'kalan Miktar',[User_Name] as'Kasiyer'FROM [dbo].[Returns_Details] where Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "'");
            }
            else if(rbtnBuyReturns.Checked==true)//satıcıya iade edilen faturalar göstemek için.
            {
                table = db.ReadData("SELECT [Order_ID] as'Fatura No' ,[Sup_Name] as'Satıcı Adı',[Cust_Name] as'Müşteri Adı',[Pro_Name] as'Ürün Adı',[Date] as'Fartura Tarihi',[Qty] as'Adet',[Price] as'Fiyat',[Total] as'Toplam',[Total_Order] as'Fatura Tutarı',[Paied] as 'Ödenen Miktar',[Remainder] as'kalan Miktar',[User_Name] as'Kasiyer'FROM [dbo].[Returns_Details] where [Cust_Name] =' ' and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "'");
            }
            else if(rbtSalesReturns.Checked==true)//müşteriden iade edilen faturalar göstemek için.
            {
                table = db.ReadData("SELECT [Order_ID] as'Fatura No' ,[Sup_Name] as'Satıcı Adı',[Cust_Name] as'Müşteri Adı',[Pro_Name] as'Ürün Adı',[Date] as'Fartura Tarihi',[Qty] as'Adet',[Price] as'Fiyat',[Total] as'Toplam',[Total_Order] as'Fatura Tutarı',[Paied] as 'Ödenen Miktar',[Remainder] as'kalan Miktar',[User_Name] as'Kasiyer'FROM [dbo].[Returns_Details] where [Sup_Name] =' ' and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "'");
            }
            dgvSearch.DataSource = table;

            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//arama sonuçları olarak iade edilen faturaların toplamını yapacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[7].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();
        }
        //iade edilen belirli bir fatura silmek için.
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                if (dgvSearch.CurrentRow != null)
                {
                    if (MessageBox.Show("seçtiğiniz faturayı silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        db.ExecuteData("delete from Returns where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "" + dgvSearch.CurrentRow.Cells[0].Value + " Nolu Fatura silindi");
                        btnSearch_Click(null, null);
                    }
                }
            }
        }
    }
}