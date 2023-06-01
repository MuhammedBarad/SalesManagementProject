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
 
    public partial class Frm_Customers : Person
    {           
            DataBase DB = new DataBase();
            DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur. 
                     
            //müşteri numarasını otomatik olarak numaralandırmak için.
            public void AutoNumber()
            {
                table.Clear();
                table = DB.ReadData("select max(Cust_ID) from Customers");//en son eklenen müşteri numarasını getirir.
                if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk müşteri bir olarak numaralndır.
                {
                    txtID.Text = "1";
                }
                else//tablo boş değilse yeni müşteri No en son eklenen müşteri numarasınıa bir artarak numaralandır.
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
            int row;//müşteriler arasında geçiş yapabilmek için global variable.
            private void Show()//müşteriler arasında geçiş yapabilmek için.
            {
                table.Clear();
                table = DB.ReadData("select * from Customers");
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
                catch (Exception) { }
                    

                }
                //istenen müşterinin bilgilerini getirip gösterdikten sonra delete,update ve deleteall butonları kullanmaya aç.
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
            public Frm_Customers()
            {
                InitializeComponent();
            }
            
            private void Frm_Customers_Load(object sender, EventArgs e)
            {
                try
                {
                    AutoNumber();
                }
                catch (Exception) { }  
            }
            //yeni müşteri eklemek için.
            private void btnAdd_Click(object sender, EventArgs e)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Lütfen Müşterinin adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (txtPhone.Text == "")
                {
                    MessageBox.Show("Lütfen Müşterinin telefon numarasını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DB.ExecuteData("insert into Customers values (" + txtID.Text + ",'" + txtName.Text + "','" + txtAddress.Text + "','" + txtPhone.Text + "','" + txtNotes.Text + "')", "Yeni Müşteri başarıyla eklendi.");
                    AutoNumber();
                }

            }
            //yeni müşteri ekleme işlemini aktifleştirmek.
            private void btnNew_Click(object sender, EventArgs e)
            {
                AutoNumber();
            }
            //ilk müşterinin bilgilerini göstermek için.
            private void btnFirst_Click(object sender, EventArgs e)
            {
                row = 0;
                Show();
            }
            //yeni değişklikleri kaydetmek için.
            private void btnSave_Click(object sender, EventArgs e)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Lütfen Müşterinin adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (txtPhone.Text == "")
                {
                    MessageBox.Show("Lütfen Müşterinin telefon numarasını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DB.ExecuteData("update Customers set Cust_Name='" + txtName.Text + "',Cust_Address='" + txtAddress.Text + "',Cust_Phone='" + txtPhone.Text + "',Notes='" + txtNotes.Text + "' where Cust_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");
            }
            //bir müşteri silmek için.
            private void btnDelete_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show("" + txtID.Text + " Nolu Müşeriyi silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DB.ExecuteData("delete from Customers where Cust_ID=" + txtID.Text + "", "Müşteri başarıyla Silindi");
                    AutoNumber();
                }
            }
            //tüm müşterileri silmek için.
            private void btnDeleteAll_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show("Tüm Müşeriler silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DB.ExecuteData("delete from Customers ", "Tüm Müşteriler başarıyla Silindi");
                    AutoNumber();
                }
            }
            //son müşterinin bilgilerini göstermek için.
            private void btnLast_Click(object sender, EventArgs e)
            {
                table.Clear();
                table = DB.ReadData("select count(*) from Customers");//müşteri tablosunun satır sayısını getirir. 
                row = Convert.ToInt32(table.Rows[0][0]) - 1;//son satır numarasını getirir.
                Show();
            }
            //bir sonraki müşterinin bilgilerini göstermek için.
            private void btnNext_Click(object sender, EventArgs e)
            {
                table.Clear();
                table = DB.ReadData("select count(*) from Customers");
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
            //bir önceki müşterinin bilgilerini göstermek için.
            private void btnPrev_Click(object sender, EventArgs e)
            {
                table.Clear();
                table = DB.ReadData("select count(*) from Customers");
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
            //bir müşteri aramak ve bilgilerini göstermek için.
            private void btnSearch_Click(object sender, EventArgs e)
            {
                DataTable tablesearch = new DataTable();
                tablesearch.Clear();
                tablesearch = DB.ReadData("select * from Customers where Cust_ID=" + txtSearch.Text + "");
                if(tablesearch.Rows.Count<=0)
                {
                    MessageBox.Show("böyle bir müşteri yoktur", "",MessageBoxButtons.OK,MessageBoxIcon.Warning);
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
            //satış ekrenından müşterilerin bilgilerini değişikler yapabilemk için.
            private void Frm_Customers_FormClosing(object sender, FormClosingEventArgs e)
            {
                try
                {
                    Frm_Sales.GetForm.FillCustomers();
                }
                catch (Exception)
                {  }
            }
        }
}
