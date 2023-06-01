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
    public partial class Frm_BuyReport : DevExpress.XtraEditors.XtraForm
    {
        public Frm_BuyReport()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();
        private void Frm_BuyReport_Load(object sender, EventArgs e)
        {
            try
            {
                FillSupliers();
            }
            catch (Exception) { }
            
            dtpFrom.Text = DateTime.Now.ToShortDateString();
            dtpTo.Text = DateTime.Now.ToShortDateString();
        }
        //comboboxtan satıcı seçebilmek için.
        private void FillSupliers()
        {
            cbxSuppliers.DataSource = db.ReadData("select * from Suppliers");
            cbxSuppliers.DisplayMember = "Sup_Name";//gösterilecek satıcı adları.
            cbxSuppliers.ValueMember = "Sup_ID";//her bir gösterilecek satıcı bir numarası var.
        }
        //satın alma işlemeri arabilmek için.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");
            table.Clear();
            if(rbtnAllSuppliers.Checked==true)//tüm satıcılar seçerse.
            {
                if(checkOrderID.Checked==false)//fatura no ile aramak istemezse
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',BD.Qty as'Miktar',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kullancı Adı'FROM Buy_Details BD,Suppliers  S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");
                }
                else//fatura no ile aramak isterse.
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',BD.Qty as'Miktar',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kullancı Adı'FROM Buy_Details BD,Suppliers  S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and Order_ID=" + txtOrderID.Text+" and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");
                }

            }
            else if(rbtnOneSupplier.Checked==true)//sadece bir satıcıyı aramak isterse.
            {
                if(checkOrderID.Checked== false)//fatura no ile aramak istemezse
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',BD.Qty as'Miktar',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kullancı Adı'FROM Buy_Details BD,Suppliers  S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and BD.Sup_ID=" + cbxSuppliers.SelectedValue + " and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");

                }
                else//fatura no ile aramak isterse.
                {
                    table = db.ReadData("SELECT Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',BD.Qty as'Miktar',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kullancı Adı'FROM Buy_Details BD,Suppliers  S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and BD.Sup_ID=" + cbxSuppliers.SelectedValue + " and Order_ID="+txtOrderID.Text+" and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc");

                }

            }

            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//arama sonuçları olarak faturaların toplamını yapacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[7].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();

        }
        //belirli bir fatura yazdırmak için.
        private void Print()
        {
            if(dgvSearch.CurrentRow!=null)
            {
                int id = Convert.ToInt32(dgvSearch.CurrentRow.Cells[0].Value);
                DataTable tablereport = new DataTable();
                tablereport.Clear();
                tablereport = db.ReadData("SELECT [Order_ID] as'Fatura No',[Sup_Name] as'Satıcı Adı',[Pro_Name] as'Ürün Adı',[Date] as'Fatura Tarihi',BD.[Qty] as'Miktar',[User_Name] as'Kullancı Adı',[Price] as'Fiyat',[Discount] as'İndirim',[Total] as'Toplam',[Total_Order] as'Fatura Tutarı',[Paied] as'Ödenen Tutar',[Remainder] as'Kalan Tutar'FROM Buy_Details BD,Suppliers S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and Order_ID=" + id + " ");
                try
                {
                    Frm_Printing frm = new Frm_Printing();
                    frm.crystalReportViewer1.RefreshReport();
                    RptOrderBuy rpt = new RptOrderBuy();
                    rpt.SetDatabaseLogon("", "", "DESKTOP-IPR2HMP", "Sales_System");
                    rpt.SetDataSource(tablereport);
                    rpt.SetParameterValue("ID", id);
                    frm.crystalReportViewer1.ReportSource = rpt;
                    //bilgisyara bağlı bir yazıcı var ise
                    //System.Drawing.Printing.PrintDocument PrintDocument = new System.Drawing.Printing.PrintDocument();
                    //rpt.PrintOptions.PrinterName = PrintDocument.PrinterSettings.PrinterName;
                    //rpt.PrintToPrinter(1, true, 0, 0);

                    frm.ShowDialog();

                }
                catch (Exception)
                {   }
            }
            


        }
        //belirli fatura yazdır.
        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            try
            {
                Print();
            }
            catch (Exception) { }
               
        }
        //belirli bir fatura silemek için.
        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if(dgvSearch.Rows.Count>=1)
            {
                if(dgvSearch.CurrentRow!=null)
                {
                    if (MessageBox.Show("seçtiğiniz faturayı silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        db.ExecuteData("delete from Buy where Order_ID="+dgvSearch.CurrentRow.Cells[0].Value+"", ""+ dgvSearch.CurrentRow.Cells[0].Value + " Nolu Fatura silindi");
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
                tablereport = db.ReadData("SELECT [Order_ID] as'Fatura No',[Sup_Name] as'Satıcı Adı',[Pro_Name] as'Ürün Adı',[Date] as'Fatura Tarihi',BD.[Qty] as'Miktar',[User_Name] as'Kullancı Adı',[Price] as'Fiyat',[Discount] as'İndirim',[Total] as'Toplam',[Total_Order] as'Fatura Tutarı',[Paied] as'Ödenen Tutar',[Remainder] as'Kalan Tutar'FROM Buy_Details BD,Suppliers S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' order by Order_ID asc ");
                try
                {
                    Frm_Printing frm = new Frm_Printing();
                    frm.crystalReportViewer1.RefreshReport();
                    RptBuyReport rpt = new RptBuyReport();
                    rpt.SetDatabaseLogon("", "", "DESKTOP-IPR2HMP", "Sales_System");

                    rpt.SetDataSource(tablereport);
                    rpt.SetParameterValue("Form", date1);
                    rpt.SetParameterValue("To", date2);
                    frm.crystalReportViewer1.ReportSource = rpt;
                    //bilgisyara bağlı bir yazıcı var ise
                    //System.Drawing.Printing.PrintDocument PrintDocument = new System.Drawing.Printing.PrintDocument();
                    //rpt.PrintOptions.PrinterName = PrintDocument.PrinterSettings.PrinterName;
                    //rpt.PrintToPrinter(1, true, 0, 0);

                    frm.ShowDialog();

                }
                catch (Exception)
                {  }
            
        }
        //belli tarihler arasında faturalar yazdır.
        private void btnPrintAllOrders_Click(object sender, EventArgs e)
        {
            if(dgvSearch.Rows.Count>=1)
            {
                PrintAll();
            }
          
        }
    }
}