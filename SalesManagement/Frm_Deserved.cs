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
    public partial class Frm_Deserved : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Deserved()
        {
            InitializeComponent();
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içeren tablodur.
        //Masraf numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Des_ID) from deserved");//en son eklenen masraf numarasını getrir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk masraf bir olarak numaralndır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni masraf No en son eklenen masraf numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            txtNot.Clear();
            nudPrice.Value = 1;
            dtpDate.Text = DateTime.Now.ToShortDateString();
            btnAdd.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            btnSave.Enabled = false;
        }
        int row;//masraf arasında geçiş yapabilmek için kullanılan bir global variable.
        private void Show()//masrafler arasında geçiş yapabilmek için.
        {
            table.Clear();
            table = DB.ReadData("select * from deserved");
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
                    nudPrice.Value = Convert.ToDecimal(table.Rows[row][1]);
                    //tarih formatları farklı olduğu için tarih belirli format olarak gösterilecektir.
                    this.Text = table.Rows[row][2].ToString();
                    DateTime dt = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);
                    dtpDate.Value = dt;

                    txtNot.Text = table.Rows[row][3].ToString();
                    cbxType.SelectedValue = Convert.ToInt32(table.Rows[row][4]);
                }
                catch (Exception) { }
                
            }
            //istenen masraf bilgilerini getirdikten sonra delete,update ve deleteall butonları kullanmaya aç.
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
            btnSave.Enabled = true;
        }
        //masraf(ödeme) yapmak ve bu masraf ait olduğu tipi seçmek için masraf tipleri comboboxa doldur
        private void FillType()
        {
            cbxType.DataSource =DB.ReadData("select * from deserved_Type");//veritabanından masraftipleri getir.
            cbxType.DisplayMember = "Name";//combobox içinde masraftipileri gösterilecek.
            cbxType.ValueMember = "Des_ID";//her gösterilecek masraftipinin bir numarası vardır.
        }

        private void Deserved_Load(object sender, EventArgs e)
        {
            try
            {
                FillType();
                AutoNumber();
            }
            catch (Exception) { }     
        }
        //yeni masraf eklemek (ödeme yapmak)için.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbxType.Items.Count<=0)
            {
                MessageBox.Show("Lütfen önce Masraf türleri giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //tarih formatları farklı olduğu için tarih belirli format olarak veritabanına kaydedilecektir.
                String d = dtpDate.Value.ToString("dd/MM/yyyy");
                DB.ExecuteData("insert into deserved values (" + txtID.Text + ","+nudPrice.Value+",'"+d+"','"+txtNot.Text+"',"+cbxType.SelectedValue+")", "yapılan Yeni ödeme başarıyla eklendi.");
                AutoNumber();
            }
        }
        //yeni masraf ekleme işlemini aktifleştirmek.
        private void btnNew_Click(object sender, EventArgs e)
        {
            FillType();
            AutoNumber();         
        }
        //yeni değişklikleri kaydetmek için.
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(nudPrice.Value<=0)
            {
                MessageBox.Show("Lütfen gerçek bir ödeme miktarı giriniz", "Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            String d = dtpDate.Value.ToString("dd/MM/yyyy");
            DB.ExecuteData("update deserved set Price="+nudPrice.Value+ ",Date='"+d+ "',Notes='"+txtNot.Text+ "',Type_ID="+cbxType.SelectedValue+" where Des_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");

        }
        //bir masraf silmek için
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("" + txtID.Text + " Nolu Ödemeyi silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from deserved where Des_ID=" + txtID.Text + "", "Ödeme başarıyla Silindi");
                AutoNumber();
            }
        }
        //tüm masrafler silmek için.
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm Ödemeler silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from deserved ", "Tüm Ödemeler başarıyla Silindi");
                AutoNumber();
            }
        }
        //ilk masraf bilgilerini göstermek için.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            row = 0;
            Show();
        }
        //bir önceki masraf bilgilerini göstermek için.
        private void btnPrev_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from deserved");
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
            table = DB.ReadData("select count(*) from deserved");
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
        //son masraf bilgilerini göstermek için.
        private void btnLast_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from deserved");
            row = Convert.ToInt32(table.Rows[0][0]) - 1;
            Show();
        }
    }
}