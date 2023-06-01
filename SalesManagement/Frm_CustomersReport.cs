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
    public partial class Frm_CustomersReport : DevExpress.XtraEditors.XtraForm
    {
        public Frm_CustomersReport()
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

        private void Frm_CustomersReport_Load(object sender, EventArgs e)
        {
            try
            {
                FillCustomers();

            }
            catch (Exception)
            {  }
            dtpDate.Text = DateTime.Now.ToShortDateString();
            table.Clear();
            table = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Ödenen Tutar',Date as'Ödeme Tarihi'from Customer_Report CR,Customers C where CR.Cust_ID=C.Cust_ID");
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//ödenen para toplamını hesaplayacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();
        }
        //belirli bir müşteri veya tüm müşteriler ödedikleri para görebilmek için
        private void btnSearch_Click(object sender, EventArgs e)
        {
            table.Clear();
            if (rbtnAllCustomers.Checked == true)//tüm müşteriler ödedikleri para.
            {
                table = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Ödenen Tutar',Date as'Ödeme Tarihi'from Customer_Report CR,Customers C where CR.Cust_ID=C.Cust_ID");
            }
            else if (rbtnOneCustomer.Checked == true)//belirli müşteri ödediği para.
            {
                table = db.ReadData("select Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',Price as'Ödenen Tutar',Date as'Ödeme Tarihi'from Customer_Report CR,Customers C where CR.Cust_ID=C.Cust_ID and C.Cust_ID=" + cbxCustomers.SelectedValue + "");
            }
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//ödenen para toplamını hesaplayacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();

        }
        //müşteri ödediği para sil.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                if (MessageBox.Show("Ödemeleri silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (rbtnOneCustomer.Checked == true)
                    {
                        db.ExecuteData("delete from Customer_Report where Cust_ID=" + cbxCustomers.SelectedValue + " ", "");
                        Frm_CustomersReport_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen bir Müşteri seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

            }
        }
    }
}