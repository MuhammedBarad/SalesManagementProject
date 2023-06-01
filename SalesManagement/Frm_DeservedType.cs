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
    public partial class Frm_DeservedType : DevExpress.XtraEditors.XtraForm
    {
        public Frm_DeservedType()
        {
            InitializeComponent();
        }

        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içeren tablodur.
        //Masraftipi numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Des_ID) from deserved_Type");//en son eklenen masraftipi numarasını getrir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk masraftipi bir olarak numaralndır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni masraftipi No en son eklenen masraftipi numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            txtName.Clear();
            btnAdd.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            btnSave.Enabled = false;
        }
        int row;//masraftipi arasında geçiş yapabilmek için global variable.
        private void Show()//masraftipleri arasında geçiş yapabilmek için.
        {
            table.Clear();
            table = DB.ReadData("select * from deserved_Type");
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
                }
                catch (Exception) { }
               
            }
            //istenen masraftipi bilgilerini getirdikten sonra delete,update ve deleteall butonları kullanmaya aç.
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
            btnSave.Enabled = true;
        }

        private void Frm_DeservedType_Load(object sender, EventArgs e)
        {
            try
            {
                AutoNumber();
            }
            catch (Exception) { }
        }
        //ilk masraftipi bilgilerini göstermek için.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            row = 0;
            Show();
        }
        //son masraftipi bilgilerini göstermek için.
        private void btnLast_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from deserved_Type");
            row = Convert.ToInt32(table.Rows[0][0]) - 1;
            Show();
        }
        //bir önceki masraftipi bilgilerini göstermek için.
        private void btnPrev_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from deserved_Type");
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
        //bir sonraki masraftipi bilgilerini göstermek için.
        private void btnNext_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from deserved_Type");
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
        //yeni masraftipi eklemek için.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen Masrafın adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DB.ExecuteData("insert into deserved_Type values (" + txtID.Text + ",'" + txtName.Text + "')", "Yeni Masraf türü başarıyla eklendi.");
                AutoNumber();
            }
        }
        //yeni masraftipi ekleme işlemini aktifleştirmek.
        private void btnNew_Click(object sender, EventArgs e)
        {
            AutoNumber();
        }
        //yeni değişklikleri kaydetmek için.
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen Masrafın adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DB.ExecuteData("update deserved_Type set Name='" + txtName.Text + "' where Des_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");

        }
        //bir masraftipi silmek için
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("" + txtID.Text + " Nolu Masraf Türünü silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from deserved_Type where Des_ID=" + txtID.Text + "", "Masraf Türü başarıyla Silindi");
                AutoNumber();
            }
        }
        //tüm masraftipler silmek için.
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm Masraf Türleri silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from deserved_Type ", "Tüm Masraf Türleri başarıyla Silindi");
                AutoNumber();
            }
        }
    }
}