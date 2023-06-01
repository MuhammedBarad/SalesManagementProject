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
    public partial class Frm_EmployeeSalary : DevExpress.XtraEditors.XtraForm
    {
        public Frm_EmployeeSalary()
        {
            InitializeComponent();
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        //çalışan maaşını ödeme işlemi numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(ID) from EmployeeSalary");//en son eklenen maaş ödeme işlemi numarasını getirir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk maaş ödeme işlemi bir olarak numaralndır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni maaş ödeme işlemi No en son eklenen çalışan numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            txtNot.Clear();
            txtSalary.Clear();
            dtpPaymentDate.Text = DateTime.Now.ToShortDateString();//maaş ödeme işlemi ne zaman yapıldı.
            dtpSalaryDate.Text = DateTime.Now.ToShortDateString();//maaş ödeme işlemi ne zaman yapılmalı.
            try
            {
                cbxEmployee.SelectedIndex = 0;
            }
            catch(Exception)
            {}
        }
        //bir çalışan seçmek için çalışan adlarını comboboxa doldur
        private void FillEmployees()
        {
            cbxEmployee.DataSource = DB.ReadData("select * from Employees");//veritabanından çalışanlar getir.
            cbxEmployee.DisplayMember = "Emp_Name";//combobox içinde çalışan adları gösterilecek.
            cbxEmployee.ValueMember = "Emp_ID";//her gösterilecek çalışan bir numarası vardır.
        }
        private void Frm_EmployeeSalary_Load(object sender, EventArgs e)
        {
            try
            {
                FillEmployees();
                AutoNumber();
            }
            catch (Exception) { }
        }
        //comboboxtan bir çalışan seçtikten sonra o çalışanın bilgilerini gösterecek.
        private void cbxEmployee_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                table.Clear();
                table = DB.ReadData("select Salary,Date from Employees where Emp_ID="+cbxEmployee.SelectedValue+"");
                txtSalary.Text = table.Rows[0][0].ToString();
                //tarih formatları farklı olduğu için tarih belirli format olarak gösterilecektir.
                this.Text = table.Rows[0][1].ToString();
                DateTime dt = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
                dtpSalaryDate.Value = dt;
            }
            catch (Exception)
            { }
        }
        //seçilen çalışanın maaşını öde. 
        private void btnSalaryPay_Click(object sender, EventArgs e)
        {
            //maaş ne zaman ödenmeli
            String salarydate = dtpSalaryDate.Value.ToString("dd/MM/yyyy");
            //maaş ne zaman ödendi
            String paymentdate = dtpPaymentDate.Value.ToString("dd/MM/yyyy");
            if(cbxEmployee.Items.Count <= 0)//hiç bir çalışan eklenmediyse
            {
                MessageBox.Show("Lütfen Çalışanları giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DB.ExecuteData("insert into EmployeeSalary values ("+txtID.Text+","+cbxEmployee.SelectedValue+",'"+txtSalary.Text+"','"+salarydate+"','"+paymentdate+"','"+txtNot.Text+"')", "Ödeme başarıyla yapıldı");
            AutoNumber();
        }
    }
}