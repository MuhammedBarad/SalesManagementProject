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
    public partial class Frm_EmployeeSalaryRrport : DevExpress.XtraEditors.XtraForm
    {
        public Frm_EmployeeSalaryRrport()
        {
            InitializeComponent();
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        //bir çalışan seçmek için çalışan adlarını comboboxa doldur
        private void FillEmployees()
        {
            cbxEmployees.DataSource = DB.ReadData("select * from Employees");//veritabanından çalışanlar getir.
            cbxEmployees.DisplayMember = "Emp_Name";//combobox içinde çalışan adları gösterilecek.
            cbxEmployees.ValueMember = "Emp_ID";//her gösterilecek çalışan bir numarası vardır.
        }
        private void Frm_EmployeeSalaryRrport_Load(object sender, EventArgs e)
        {
            try
            {
                FillEmployees();
            }
            catch (Exception) { }         
        }
        //belli tarihlar arasında ödenen maaşlar göstermek için.
        private void btnSearch_Click(object sender, EventArgs e)
        {     
            String date1 = dtpFrom.Value.ToString("yyyy-MM-dd");//bu tarihten itibaren aramaya başla.
            String date2 = dtpTo.Value.ToString("yyyy-MM-dd");//bu tarihe kadar aramaya devamet.
            table.Clear();
            if (rbtnAllEmployees.Checked==true)//bu tarihte tüm çalışan maaşlarını göster.
            {
                table=DB.ReadData("SELECT [ID] as 'Işlem No',E.[Emp_Name]as 'Çalışan Adı',ES.[Salary]as  'Maaş',[SalaryDate]as  'Maaş Tarihi',[PaymentDate]as'Maaş ödendiğ Tarih',[Notes] as'Not'FROM EmployeeSalary ES,Employees E where E.Emp_ID=ES.Emp_ID and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' ");
            }
            else if(rbtnOneEmployee.Checked==true)//bu tarihte belirli bir çalışan maaşlarını göster.
            {
                table=DB.ReadData("SELECT [ID] as 'Işlem No',E.[Emp_Name]as 'Çalışan Adı',ES.[Salary]as  'Maaş',[SalaryDate]as  'Maaş Tarihi',[PaymentDate]as'Maaş ödendiğ Tarih',[Notes] as'Not'FROM EmployeeSalary ES,Employees E where E.Emp_ID=ES.Emp_ID and ES.Emp_ID="+cbxEmployees.SelectedValue+" and Convert(date,Date,105) between '" + date1 + "' and '" + date2 + "' ");
            }
            if(table.Rows.Count>=1)//gelen verileri içeren tablo boş değilse.
            {
                dgvSearch.DataSource = table;//gelen verileri datagridviewe aktar.
                decimal sum = 0;
                for (int i = 0; i < table.Rows.Count; i++)//ödenen maaşların toplamını yapan for loop.
                {
                    sum += Convert.ToDecimal(table.Rows[i][2]);
                }
                txtTotal.Text = Math.Round(sum, 2).ToString();
            }
            else//gelen verileri içeren tablo boş ise.
            {
                txtTotal.Text = "0";
            }
        }
        //belli tarihlar arasında ödenen maaşlar silmek için.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            String date1;
            String date2;
            date1 = dtpFrom.Value.ToString("yyyy-MM-dd");//bu tarihten itibaren aramya başla.
            date2 = dtpTo.Value.ToString("yyyy-MM-dd");//bu tarihe kadar aramaya devamet.
            if (MessageBox.Show("Ödemeler silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from EmployeeSalary where Convert(date,SalaryDate,105) between '" + date1 + "' and '" + date2 + "' ", "maaşlar başarıyla Silindi");
            }
            btnSearch_Click(null, null);//değişikler görebilmek için yeniden arama yap.
        }
    }
    
}