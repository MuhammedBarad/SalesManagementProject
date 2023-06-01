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
    public partial class Frm_CustomerMoney : DevExpress.XtraEditors.XtraForm
    {
        public Frm_CustomerMoney()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();
        //comboboxtan bir müşteri seçebilmek için 
        private void FillCustomers()
        {
            cbxCustomers.DataSource = db.ReadData("select * from Customers");
            cbxCustomers.DisplayMember = "Cust_Name";//gösterilecek müşteri adları.
            cbxCustomers.ValueMember = "Cust_ID";//her gösterilecek bir müşterinin bir numarası var.
        }

        private void Frm_CustomerMoney_Load(object sender, EventArgs e)
        {
            try
            {
                FillCustomers();
            }
            catch (Exception)
            { }
            dtpDate.Text = DateTime.Now.ToShortDateString();

            table.Clear();
            table = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödenecek Tarihi' from Customer_Money CM,Customers C where CM.Cust_ID=C.Cust_ID");
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//müşteriler borç toplamını hesaplayack.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();
        }
        //belirli bir müşteri veya tüm müşteriler ödemeleri gereken para görebilmek için
        private void btnSearch_Click(object sender, EventArgs e)
        {
            table.Clear();
            if (rbtnAllCustomers.Checked == true)//tüm müşteriler borçları.
            {
                table = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödenecek Tarihi' from Customer_Money CM,Customers C where CM.Cust_ID=C.Cust_ID");
            }
            else if (rbtnOneCustomer.Checked == true)//belirli müşteri borcu.
            {
                table = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödenecek Tarihi' from Customer_Money CM,Customers C where CM.Cust_ID=C.Cust_ID and C.Cust_ID=" + cbxCustomers.SelectedValue + " ");
            }
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();
        }
        //bir müşteri borcunu ödemek için.
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                String d = dtpDate.Value.ToString("dd/MM/yyyy");
                if (rbtnAllPay.Checked == true)//bir müşterinin borcu tam olarak ödemek için.
                {                              
                    if (MessageBox.Show("Ödeme Yapmaktan emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (rbtnAllCustomers.Checked == true)//ödeme yaparken belirli bir müşteri seçilmeli.
                        {
                            nudPrice.Enabled = false;
                            MessageBox.Show("Lütfen belirli bir Müşteri seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        db.ExecuteData("delete from Customer_Money where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Price=" + dgvSearch.CurrentRow.Cells[2].Value + " ", "Ödeme başarıyla yapıldı");//ödeme yaptıktan sonra müşeri borcu sil
                        db.ExecuteData("insert into Customer_Report values(" + dgvSearch.CurrentRow.Cells[0].Value + "," + dgvSearch.CurrentRow.Cells[2].Value + ",'" + d + "'," + cbxCustomers.SelectedValue + ") ", "");//ödeme yaptıktan sonra müşterinin ödediği para (ödenen) olarak kaydet.
                        Frm_CustomerMoney_Load(null, null);
                    }
                }
                else if (rbtnPartPay.Checked == true)//bir müşterinin borcundan bir kısmı ödemek için.
                {
                    if (MessageBox.Show("Ödeme Yapmaktan emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (rbtnAllCustomers.Checked == true)//ödeme yaparken belirli bir müşteri seçilmeli.
                        {
                            MessageBox.Show("Lütfen belirli bir Müşteri seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        decimal newMoney = Convert.ToDecimal(dgvSearch.CurrentRow.Cells[2].Value) - Convert.ToDecimal(nudPrice.Value);//müşterinin kalan borcu=eski borç-ödenen para
                        db.ExecuteData("update Customer_Money set Price=" + newMoney + " where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Price=" + dgvSearch.CurrentRow.Cells[2].Value + " ", "Ödeme başırıyla yapıldı");//ödeme yaptıktan sonra ödenen para müşeri borcundan çıkar.
                        db.ExecuteData("insert into Customer_Report values(" + dgvSearch.CurrentRow.Cells[0].Value + "," + nudPrice.Value + ",'" + d + "'," + cbxCustomers.SelectedValue + ") ", "");//ödeme yaptıktan sonra müşterinin ödediği para (ödenen) olarak kaydet.
                        Frm_CustomerMoney_Load(null, null);
                    }
                }
            }
        }
        private void PrintOneCustomer()
        {
            String Name = Convert.ToString(cbxCustomers.Text);
            DataTable tablereport = new DataTable();
            tablereport.Clear();
            tablereport = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödenecek Tarihi' from Customer_Money CM,Customers C where CM.Cust_ID=C.Cust_ID and C.Cust_Name='"+Name+"'");
            try
            {
                Frm_Printing frm = new Frm_Printing();
                frm.crystalReportViewer1.RefreshReport();
                RptCustomerMoney rpt = new RptCustomerMoney();
                rpt.SetDatabaseLogon("", "", "DESKTOP-IPR2HMP", "Sales_System");

                rpt.SetDataSource(tablereport);
                rpt.SetParameterValue("Name", Name);
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
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (rbtnOneCustomer.Checked == true)
            {
                if (dgvSearch.Rows.Count >= 1)
                {
                    PrintOneCustomer();
                }
            }
            else
            {
                MessageBox.Show("Lütfen belirli bir Müşteri seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}