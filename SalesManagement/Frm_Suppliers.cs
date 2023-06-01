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
    public partial class Frm_Suppliers : Person
    {
        public Frm_Suppliers()
        {
            InitializeComponent();
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        //satıcı numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Sup_ID) from Suppliers");//en son eklenen satıcı numarasını getrir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk satıcı bir olarak numaralndır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni satıcı No en son eklenen müşteri numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtNotes.Clear();
            btnAdd.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            btnSave.Enabled = false;
        }
        int row;//satıcılar arasında geçiş yapabilmek için global variable
        private void Show()//satıcılar arasında geçiş yapabilmek için.
        {
            table.Clear();
            table = DB.ReadData("select * from Suppliers");
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
                    txtNotes.Text = table.Rows[row][4].ToString();
                }
                catch(Exception)
                { }
            }
            //istenen satıcı bilgilerini getirdikten sonra delete,update ve deleteall butonları kullanmaya aç.
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
        private void Frm_Suppliers_Load(object sender, EventArgs e)
        {
            try
            {
                AutoNumber();
            }
            catch (Exception) { }
        }
        //ilk satıcının bilgilerini göstermek için.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            row = 0;
            Show();
        }
        //son satıcının bilgilerini göstermek için.
        private void btnLast_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Suppliers");//satıcı tablosunun satır sayısını getirir. 
            row = Convert.ToInt32(table.Rows[0][0]) - 1;//son satır numarasını getirir.
            Show();
        }
        //bir sonraki satıcının bilgilerini göstermek için.
        private void btnNext_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Suppliers");
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
        //bir önceki satıcının bilgilerini göstermek için.
        private void btnPrev_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Suppliers");
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
        //yeni satıcı eklemek için.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen Satıcının adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtPhone.Text == "")
            {
                MessageBox.Show("Lütfen Satıcının telefon numarasını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DB.ExecuteData("insert into Suppliers values (" + txtID.Text + ",'" + txtName.Text + "','" + txtAddress.Text + "','" + txtPhone.Text + "','" + txtNotes.Text + "')", "Yeni Satıcı başarıyla eklendi.");
                AutoNumber();
            }
        }
        //yeni satıcı ekleme işlemini aktifleştirmek.
        private void btnNew_Click(object sender, EventArgs e)
        {
            AutoNumber();
        }
        //yeni değişklikleri kaydetmek için.
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen Satıcının adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtPhone.Text == "")
            {
                MessageBox.Show("Lütfen Satıcının telefon numarasını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DB.ExecuteData("update Suppliers set Sup_Name='" + txtName.Text + "',Sup_Address='" + txtAddress.Text + "',Sup_Phone='" + txtPhone.Text + "',Notes='" + txtNotes.Text + "' where Sup_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");
                AutoNumber();
            }
        }
        //bir satıcı silmek için.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("" + txtID.Text + " Nolu Satıcıyı silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Suppliers where Sup_ID=" + txtID.Text + "", "Satıcı başarıyla Silindi");
                AutoNumber();
            }
        }
        //tüm satıcılar silmek için.
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm Satıcılar silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Suppliers ", "Tüm Satıcılar başarıyla Silindi");
                AutoNumber();
            }
        }
        //bir satıcı aramak ve bilgilerini göstermek için.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable tablesearch = new DataTable();
            tablesearch.Clear();
            tablesearch = DB.ReadData("select * from Suppliers where Sup_ID=" + txtSearch.Text + "");
            if (tablesearch.Rows.Count <= 0)
            {
                MessageBox.Show("böyle bir satıcı yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                txtID.Text = tablesearch.Rows[0][0].ToString();
                txtName.Text = tablesearch.Rows[0][1].ToString();
                txtAddress.Text = tablesearch.Rows[0][2].ToString();
                txtPhone.Text = tablesearch.Rows[0][3].ToString();
                txtNotes.Text = tablesearch.Rows[0][4].ToString();
            }
            catch (Exception)
            {}
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
            btnSave.Enabled = true;
        }
        //satın alma sayfasında satıcıların bilgilerini değişikler yapabilemk için.
        //satıcı sayfası kapatılınca yeni değişikler satın alma sayfasında satıcı comboboxuna yansıtmak için.
        private void Frm_Suppliers_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Frm_Buy.GetForm.FillSuppliers();
            }catch(Exception)
            { }
            
        }
    }
}