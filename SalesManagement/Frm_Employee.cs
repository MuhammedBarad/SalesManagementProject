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
    public partial class Frm_Employee : Person
    {
        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        //çalışan numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Emp_ID) from Employees");//en son eklenen çalışan numarasını getirir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk çalışan bir olarak numaralndır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni çalışan No en son eklenen çalışan numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtSalary.Clear();
            dtpDateSalary.Text = DateTime.Now.ToShortDateString();
            btnAdd.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            btnSave.Enabled = false;
        }
        int row;//çalışanlar arasında geçiş yapabilmek için kullanılan bir global variable
        private void Show()//çalışanlar arasında geçiş yapabilmek için.
        {
            table.Clear();
            table = DB.ReadData("select * from Employees");
            if (table.Rows.Count <= 0)//tablo boş ise uyarı göster.
            {
                MessageBox.Show("Bu ekranda gösterilecek bir bilgi yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else//tablo boş değilse satır numarasına göre (row) verileri textboxlara yükle.
            {
                try
                { 
                txtID.Text = table.Rows[row][0].ToString();
                txtName.Text = table.Rows[row][1].ToString();
                txtAddress.Text = table.Rows[row][2].ToString();
                txtPhone.Text = table.Rows[row][3].ToString();
                txtSalary.Text = table.Rows[row][4].ToString();
                //tarih formatları farklı olduğu için tarih belirli bir format olarak gösterilecektir.
                this.Text = table.Rows[row][5].ToString();
                DateTime dt = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
                dtpDateSalary.Value = dt;
                }
                catch(Exception)
                {}

            }
            //istenen çalışan bilgilerini getirdikten sonra delete,update ve deleteall butonları kullanmaya aç.
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
            btnSave.Enabled = true;
        }
        //override
        //protected void Insert()
        //{
        //    btnAdd_Click(null, null);
        //}
        public Frm_Employee()
        {
            InitializeComponent();
        }

        private void Frm_Employee_Load(object sender, EventArgs e)
        {
            try
            {
                AutoNumber();
            }
            catch (Exception) { }          
        }
        //yeni çalışan eklemek için.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen Çalışanın adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtPhone.Text == "")
            {
                MessageBox.Show("Lütfen Çalışanın telefon numarasını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtSalary.Text == "")
            {
                MessageBox.Show("Lütfen Çalışanın maaşını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //tarih formatları farklı olduğu için tarih belirli format olarak veritabanına kaydedilecektir.
                String d = dtpDateSalary.Value.ToString("dd/MM/yyyy");
                DB.ExecuteData("insert into Employees values (" + txtID.Text + ",'" + txtName.Text + "','" + txtAddress.Text + "','" + txtPhone.Text + "',"+txtSalary.Text+",'"+d+"')", "Yeni Çalışan başarıyla eklendi.");
                AutoNumber();
            }
        }
        //yeni çalışan ekleme işlemini aktifleştirmek.
        private void btnNew_Click(object sender, EventArgs e)
        {
            AutoNumber();
        }
        //yeni değişklikleri kaydetmek için.
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen Çalışanın adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtPhone.Text == "")
            {
                MessageBox.Show("Lütfen Çalışanın telefon numarasını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtSalary.Text == "")
            {
                MessageBox.Show("Lütfen Çalışanın maaşını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            String d = dtpDateSalary.Value.ToString("dd/MM/yyyy");
            DB.ExecuteData("update Employees set Emp_Name='" + txtName.Text + "',Emp_Address='" + txtAddress.Text + "',Emp_Phone='" + txtPhone.Text + "',Salary=" + txtSalary.Text + ", Date='"+d+"' where Emp_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");

        }
        //bir çalışan silmek için
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("" + txtID.Text + " Nolu Çalışanı silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Employees where Emp_ID=" + txtID.Text + "", "Çalışan başarıyla Silindi");
                AutoNumber();
            }
        }
        //ilk çalışan bilgilerini göstermek için.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            row = 0;
            Show();
        }
        //bir önceki çalışan bilgilerini göstermek için.
        private void btnPrev_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Employees");
            if (row == 0)//ilk satıra gelince.
            {
                row = Convert.ToInt32(table.Rows[0][0]) - 1;//son satıra geri dönecek
                Show();
            }
            else
            {
                row--;
                Show();
            }
        }
        //bir sonraki çalışan bilgilerini göstermek için.
        private void btnNext_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Employees");
            if (row == Convert.ToInt32(table.Rows[0][0]) - 1)//son satıra gelince.
            {
                row = 0;//ilk satıra  geri dönecek
                Show();
            }
            else
            {
                row++;
                Show();
            }
        }
        //son çalışan bilgilerini göstermek için.
        private void btnLast_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Employees");
            row = Convert.ToInt32(table.Rows[0][0]) - 1;
            Show();
        }
        //tüm çalışanlar silmek için.
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Tüm Çalışanlar silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Employees ", "Tüm Çalışanlar başarıyla Silindi");
                AutoNumber();
            }
        }
        //bir çalışan aramak ve bilgilerini göstermek için.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable tablesearch = new DataTable();
            tablesearch.Clear();
            tablesearch = DB.ReadData("select * from Employees where Emp_ID=" + txtSearch.Text + "");
            if(tablesearch.Rows.Count<=0)
            {
                MessageBox.Show("böyle bir çalışan yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                txtID.Text = tablesearch.Rows[0][0].ToString();
                txtName.Text = tablesearch.Rows[0][1].ToString();
                txtAddress.Text = tablesearch.Rows[0][2].ToString();
                txtPhone.Text = tablesearch.Rows[0][3].ToString();
                txtSalary.Text= tablesearch.Rows[0][4].ToString();
            }
            catch (Exception)
            { }
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
            btnSave.Enabled = true;
        }
    }
}