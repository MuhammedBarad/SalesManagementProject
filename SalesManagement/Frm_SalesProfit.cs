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
    public partial class Frm_SalesProfit : DevExpress.XtraEditors.XtraForm
    {
        public Frm_SalesProfit()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();
        private void Frm_SalesProfit_Load(object sender, EventArgs e)
        {
            try
            {
                FillUsers();
            }
            catch (Exception) { }
            dtpFrom.Text = DateTime.Now.ToShortDateString();
            dtpTo.Text = DateTime.Now.ToShortDateString();
        }
        //comboboxtan belirli bir kullancı seçebilmek için.
        private void FillUsers()
        {
            cbxUsers.DataSource = db.ReadData("select * from Users");
            cbxUsers.DisplayMember = "User_Name";//gösterilecek kullancı adları.
            cbxUsers.ValueMember = "User_ID";//her bir gösterilecek kullancı bir numarası var.
        }
        //fatura arabilmek için.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");
            table.Clear();
            if (checkOrderID.Checked == false)//fatura no ile aramak istemezse
            {
                if(rbtnAllUsers.Checked==true)//tüm kullancı seçerse.
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',SP.Qty as'Adet',Buy_Price as'Satın alma Fiyatı',Price as'Fiyat',Discount as'indirim',Total as'Toplam',(Price-SP.BuPrice)*(SP.Qty)as'Kazanç',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kasiyer'FROM Sales_profit SP,Customers  C,Products P where SP.Cust_ID=C.Cust_ID and SP.Pro_ID=P.Pro_ID  and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");
                }
                else if(rbtnOneUser.Checked==true)//belirli bir kullancı seçerse.
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',SP.Qty as'Adet',Buy_Price as'Satın alma Fiyatı',Price as'Fiyat',Discount as'indirim',Total as'Toplam',(Price-SP.BuPrice)*(SP.Qty)as'Kazanç',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kasiyer'FROM Sales_profit SP,Customers  C,Products P where SP.Cust_ID=C.Cust_ID and SP.Pro_ID=P.Pro_ID and User_Name='" + cbxUsers.Text+"' and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");
                }
            }
            else if (checkOrderID.Checked == true)//fatura no ile aramak isterse.
            {
                if(rbtnAllUsers.Checked==true)//tüm kullancı seçerse.
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',SP.Qty as'Adet',Buy_Price as'Satın alma Fiyatı',Price as'Fiyat',Discount as'indirim',Total as'Toplam',(Price-SP.BuPrice)*(SP.Qty)as'Kazanç',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kasiyer'FROM Sales_profit SP,Customers  C,Products P where SP.Cust_ID=C.Cust_ID and SP.Pro_ID=P.Pro_ID  and Order_ID=" + txtOrderID.Text + " and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");
                }
                else if(rbtnOneUser.Checked==true)//belirli bir kullancı seçerse.
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',SP.Qty as'Adet',Buy_Price as'Satın alma Fiyatı',Price as'Fiyat',Discount as'indirim',Total as'Toplam',(Price-SP.BuPrice)*(SP.Qty)as'Kazanç',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kasiyer'FROM Sales_profit SP,Customers  C,Products P where SP.Cust_ID=C.Cust_ID and SP.Pro_ID=P.Pro_ID  and Order_ID=" + txtOrderID.Text + " and User_Name='"+cbxUsers.Text+"' and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");
                }
            }
            dgvSearch.DataSource = table;
            decimal Sum = 0 , sumprofit=0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//arama sonuçları olarak faturaların ve kazanç toplamını yapacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[8].Value);//satış toplamı
                sumprofit += Convert.ToDecimal(dgvSearch.Rows[i].Cells[9].Value);//satış kazanç toplamı
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();
            txtsTotalprofit.Text = Math.Round(sumprofit, 2).ToString();
        }
        //belirli bir fatura silemek için.
        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                if (dgvSearch.CurrentRow != null)
                {
                    if (MessageBox.Show("seçtiğiniz faturayı silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        db.ExecuteData("delete from Sales_profit where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "" + dgvSearch.CurrentRow.Cells[0].Value + " Nolu Fatura silindi");
                        btnSearch_Click(null, null);
                    }
                }
            }
        }
        //belli tarihler arasında faturalar yazdırmak için.
        private void PrintAll()
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");

            DataTable tablereport = new DataTable();
            tablereport.Clear();
            tablereport = db.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',SP.Qty as'Adet',Buy_Price as'Satın alma Fiyatı',Price as'Fiyat',Discount as'indirim',Total as'Toplam',(Price-SP.BuPrice)*(SP.Qty)as'Kazanç',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kullancı Adı'FROM Sales_profit SP,Customers  C,Products P where SP.Cust_ID=C.Cust_ID and SP.Pro_ID=P.Pro_ID  and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc ");
            try
            {
                Frm_Printing frm = new Frm_Printing();
                frm.crystalReportViewer1.RefreshReport();
                RptSalesProfit rpt = new RptSalesProfit();
                rpt.SetDatabaseLogon("", "", "DESKTOP-IPR2HMP", "Sales_System");

                rpt.SetDataSource(tablereport);
                rpt.SetParameterValue("From", date1);
                rpt.SetParameterValue("To", date2);
                frm.crystalReportViewer1.ReportSource = rpt;
                //bilgisyara bağlı bir yazıcı var ise
                //System.Drawing.Printing.PrintDocument PrintDocument = new System.Drawing.Printing.PrintDocument();
                //rpt.PrintOptions.PrinterName = PrintDocument.PrinterSettings.PrinterName;
                //rpt.PrintToPrinter(1, true, 0, 0);

                frm.ShowDialog();

            }
            catch (Exception)
            { }

        }
        
        private void btnPrintAllOrders_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                PrintAll();
            }
        }
    }
}