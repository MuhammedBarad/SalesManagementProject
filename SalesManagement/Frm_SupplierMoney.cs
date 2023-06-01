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
    public partial class Frm_SupplierMoney : DevExpress.XtraEditors.XtraForm
    {
        public Frm_SupplierMoney()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();
        //comboboxtan bir satıcı seçebilmek için 
        private void FillSuppliers()
        {
            cbxSuppliers.DataSource = db.ReadData("select * from Suppliers");
            cbxSuppliers.DisplayMember = "Sup_Name";//gösterilecek müşteri adları.
            cbxSuppliers.ValueMember = "Sup_ID";//her gösterilecek bir müşterinin bir numarası var.
        }
        private void Frm_SupplierMoney_Load(object sender, EventArgs e)
        {
            try
            {
                FillSuppliers();
            }
            catch(Exception)
            {  }
            dtpDate.Text = DateTime.Now.ToShortDateString();

            table.Clear();
            table = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödenecek Tarihi' from Supplier_Money SM,Suppliers S where SM.Sup_ID=S.Sup_ID");
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for(int i=0; i<dgvSearch.Rows.Count; i++)//satıcılara borç toplamını hesaplayack.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();

        }
        //belirli bir satıcı veya tüm satıcılara ödenmesi gereken para görebilmek için
        private void btnSearch_Click(object sender, EventArgs e)
        {
            table.Clear();
            if (rbtnAllSuppliers.Checked==true)//tüm satıcılara borçlar.
            {
                table = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödeme Tarihi' from Supplier_Money SM,Suppliers S where SM.Sup_ID=S.Sup_ID");
            }
            else if(rbtnOneSupplier.Checked==true)//belirli satıcıya borç.
            {
                table = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödeme Tarihi' from Supplier_Money SM,Suppliers S where SM.Sup_ID=S.Sup_ID and S.Sup_ID=" + cbxSuppliers.SelectedValue+" ");
            }
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();
        }
        //bir satıcıya borç ödemek için.
        private void btnPay_Click(object sender, EventArgs e)
        {
            if(dgvSearch.Rows.Count>=1)
            {
                String d = dtpDate.Value.ToString("dd/MM/yyyy");
                if(rbtnAllPay.Checked==true)//bir satıcıya borcç tam olarak ödemek için.
                {
                    if (MessageBox.Show("Ödeme Yapmaktan emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if(rbtnAllSuppliers.Checked==true)//ödeme yaparken belirli bir satıcı seçilmeli.
                        {
                            MessageBox.Show("Lütfen belirli bir satıcı seçin", "Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        db.ExecuteData("delete from Supplier_Money where Order_ID="+dgvSearch.CurrentRow.Cells[0].Value+ " and Price="+dgvSearch.CurrentRow.Cells[2].Value+" ", "Ödeme başarıyla yapıldı");//ödeme yaptıktan sonra borç sil
                        db.ExecuteData("insert into Supplier_Report values("+dgvSearch.CurrentRow.Cells[0].Value+","+dgvSearch.CurrentRow.Cells[2].Value+",'"+d+"',"+cbxSuppliers.SelectedValue+") ", "");//ödeme yaptıktan sonra ödendiği para (ödenen) olarak kaydet.
                        Frm_SupplierMoney_Load(null, null);
                    }

                }
                else if(rbtnPartPay.Checked==true)//bir satıcaya borcçtan bir kısmı ödemek için.
                {
                    if (MessageBox.Show("Ödeme Yapmaktan emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (rbtnAllSuppliers.Checked == true)//ödeme yaparken belirli bir satıcı seçilmeli.
                        {
                            MessageBox.Show("Lütfen belirli bir satıcı seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        decimal newMoney = Convert.ToDecimal(dgvSearch.CurrentRow.Cells[2].Value) -Convert.ToDecimal(nudPrice.Value);//satıcıya kalan borç=eski borç-ödenen para
                        db.ExecuteData("update Supplier_Money set Price="+newMoney+ " where Order_ID="+dgvSearch.CurrentRow.Cells[0].Value+" and Price="+dgvSearch.CurrentRow.Cells[2].Value+" ", "Ödeme başırıyla yapıldı");//ödeme yaptıktan sonra ödenen para borçtan çıkar.
                        db.ExecuteData("insert into Supplier_Report values(" + dgvSearch.CurrentRow.Cells[0].Value + "," + nudPrice.Value + ",'" + d + "'," + cbxSuppliers.SelectedValue + ") ", "");//ödeme yaptıktan sonra satıcıya ödendiği para (ödenen) olarak kaydet.
                        Frm_SupplierMoney_Load(null, null);
                    }
                }
            }
        }
        private void PrintOneSupplier()
        {
            int id = Convert.ToInt32(cbxSuppliers.SelectedValue);
            DataTable tablereport = new DataTable();
            tablereport.Clear();
            tablereport = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Tutar',Order_Date as'Fatura Tarihi',Reminder_Date as'Ödeme Tarihi' from Supplier_Money SM,Suppliers S where SM.Sup_ID=S.Sup_ID and S.Sup_ID="+id+" ");
            try
            {
                Frm_Printing frm = new Frm_Printing();
                frm.crystalReportViewer1.RefreshReport();
                Rpt_SupplierMoney rpt = new Rpt_SupplierMoney();
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
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if(rbtnOneSupplier.Checked==true)
            {
                if(dgvSearch.Rows.Count>=1)
                {
                    PrintOneSupplier();
                }
            }
            else
            {
                MessageBox.Show("Lütfen belirli bir satıcı seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}