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
    public partial class Frm_SuppliersReport : DevExpress.XtraEditors.XtraForm
    {
        public Frm_SuppliersReport()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();
        //comboboxtan bir satıcı seçebilmek için 
        private void FillSupliers()
        {
            cbxSuppliers.DataSource = db.ReadData("select * from Suppliers");
            cbxSuppliers.DisplayMember = "Sup_Name";//gösterilecek satıcı adları.
            cbxSuppliers.ValueMember = "Sup_ID";//her gösterilecek bir satıcının bir numarası var.
        }
        private void Frm_SuppliersReport_Load(object sender, EventArgs e)
        {
            try
            {
                FillSupliers();

            }
            catch(Exception)
            {  }
            dtpDate.Text = DateTime.Now.ToShortDateString();
            table.Clear();
            table = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Ödenen Tutar',Date as'Ödeme Tarihi'from Supplier_Report SR,Suppliers S where SR.Sup_ID=S.Sup_ID");
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//ödenen para toplamını hesaplayacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();


        }
        //belirli bir satıcı veya tüm satıcılara ödendiği para görebilmek için
        private void btnSearch_Click(object sender, EventArgs e)
        {
            table.Clear();
            if(rbtnAllSuppliers.Checked==true)//tüm satıcılara ödendiği para.
            {
                table = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Ödenen Tutar',Date as'Ödeme Tarihi'from Supplier_Report SR,Suppliers S where SR.Sup_ID=S.Sup_ID");
            }
            else if(rbtnOneSupplier.Checked==true)//belirli saatıcıya ödendiği para.
            {
                table = db.ReadData("select Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',Price as'Ödenen Tutar',Date as'Ödeme Tarihi'from Supplier_Report SR,Suppliers S where SR.Sup_ID=S.Sup_ID and S.Sup_ID="+cbxSuppliers.SelectedValue+"");
            }
            dgvSearch.DataSource = table;
            decimal Sum = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//ödenen para toplamını hesaplayacak.
            {
                Sum += Convert.ToDecimal(dgvSearch.Rows[i].Cells[2].Value);
            }
            txtTotal.Text = Math.Round(Sum, 2).ToString();

        }
        //satıcıya ödendiği para sil.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvSearch.Rows.Count>=1)
            {
                if (MessageBox.Show("Ödemeleri silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (rbtnOneSupplier.Checked == true)
                    {
                        db.ExecuteData("delete from Supplier_Report where Sup_ID="+cbxSuppliers.SelectedValue+" ", "");
                        Frm_SuppliersReport_Load(null,null);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen bir Satıcı seçin","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                }
                
            }
        }
    }
}