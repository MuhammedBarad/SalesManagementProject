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
    public partial class Frm_deservedReport : DevExpress.XtraEditors.XtraForm
    {
        public Frm_deservedReport()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        private void Frm_deservedReport_Load(object sender, EventArgs e)
        {
            dtpFrom.Text = DateTime.Now.ToShortDateString();
            dtpTo.Text = DateTime.Now.ToShortDateString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");//bu tarihten itibaren aramaya başla.
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");//bu tarihe kadar aramaya devamet.
            table.Clear();
            table = db.ReadData("select d.Des_ID as'Ödeme NO',d.Price as'Ödeme Miktarı',d.Date as'Ödeme Tarihi',d.Notes as'Not',dt.Name as'Masraf Türü' from deserved d , deserved_Type dt where d.Type_ID=dt.Des_ID and Convert(date,Date,105) between '"+date1+"' and '"+date2+"'");
           
            if (table.Rows.Count>=1)//gelen verileri içeren tablo boş değilse.
            {
                dgvSearch.DataSource = table;//gelen verileri datagridviewe aktar.
                decimal sum = 0;
                for(int i=0; i<table.Rows.Count; i++)//ödenen masraflar toplamını yapan for loop.
                {
                    sum += Convert.ToDecimal(table.Rows[i][1]);
                }
                txtTotal.Text = Math.Round(sum, 2).ToString();
            }
            else
            {
                txtTotal.Text = "0";
            }
        }
        //belli tarihler arasında ödenen masraflar silmek için.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");//bu tarihten itibaren aramaya başla.
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");//bu tarihe kadar aramaya devamet.
            if (MessageBox.Show("Ödemeler silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                db.ExecuteData("delete from deserved where Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' ", "Ödemeler başarıyla Silindi");      
            }
        }
    }
}