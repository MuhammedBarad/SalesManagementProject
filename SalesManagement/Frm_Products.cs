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
    public partial class Frm_Products : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Products()
        {
            InitializeComponent();
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();//veritabanından gelen verileri içerecek tablodur.
        //ürün numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Pro_ID) from Products");//en son eklenen ürün numarasını getirir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk ürün bir olarak numaralndır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni ürün No en son eklenen çalışan numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            txtName.Clear();
            txtSearch.Clear();
            txtBarcode.Clear();
            nudBuyPrice.Value = 1;
            nudSalesPrice.Value = 1;
            nudMinQty.Value = 0;
            nudMaxDiscount.Value = 0;
            try
            {
                FillType();
                cbxProducts.SelectedIndex = 0;
            }
            catch (Exception) { }
          
            btnAdd.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = false;
            btnDeleteAll.Enabled = false;
            btnSave.Enabled = false;
        }
        int row;//ürünler arasında geçiş yapabilmek için kullanılan bir global variable
        private void Show()//ürünler arasında geçiş yapabilmek için.
        {
            table.Clear();
            table = DB.ReadData("select * from Products");
            if (table.Rows.Count <= 0)//tablo boş ise uyarı göster.
            {
                MessageBox.Show("Bu ekranda gösterilecek bir bilgi yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                try
                {
                    txtID.Text = table.Rows[row][0].ToString();
                    txtName.Text = table.Rows[row][1].ToString();
                    nudQty.Value = Convert.ToDecimal(table.Rows[row][2]);
                    nudBuyPrice.Value = Convert.ToDecimal(table.Rows[row][3]);
                    nudSalesPrice.Value = Convert.ToDecimal(table.Rows[row][4]);
                    txtBarcode.Text = table.Rows[row][5].ToString();
                    nudMinQty.Value = Convert.ToDecimal(table.Rows[row][6]);
                    nudMaxDiscount.Value = Convert.ToDecimal(table.Rows[row][7]);
                }
                catch(Exception)
                { }
               
            }
            //istenen ürün bilgilerini getirdikten sonra delete,update ve deleteall butonları kullanmaya aç.
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnDeleteAll.Enabled = true;
            btnSave.Enabled = true;
        }
        //bir ürün seçmek için çalışan adlarını comboboxa doldur
        private void FillType()
        {
            cbxProducts.DataSource = DB.ReadData("select * from Products");//veritabanından ürünler getir.
            cbxProducts.DisplayMember = "Pro_Name";//combobox içinde ürün adları gösterilecek.
            cbxProducts.ValueMember = "Pro_ID";//her gösterilecek ürünün bir numarası vardır.
        }
        private void Frm_Products_Load(object sender, EventArgs e)
        {
            try
            {
                AutoNumber();
            }
            catch(Exception) {  }
            
        }
        //ilk ürün bilgilerini göstermek için.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            row = 0;
            Show();
        }
        //bir önceki ürün bilgilerini göstermek için.
        private void btnPrev_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Products");
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
        //bir sonraki ürün bilgilerini göstermek için.
        private void btnNext_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Products");
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
        //son ürün bilgilerini göstermek için.
        private void btnLast_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Products");
            row = Convert.ToInt32(table.Rows[0][0]) - 1;
            Show();
        }
        //yeni ürün eklemek için.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text=="")
            {
                MessageBox.Show("Lütfen önce Üürün Adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudSalesPrice.Value <=0)
            {
                MessageBox.Show("Lütfen doğru bir satış fiyatını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudBuyPrice.Value <= 0)
            {
                MessageBox.Show("Lütfen doğru bir alış fiyatını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudMaxDiscount.Value>nudBuyPrice.Value)
            {
                MessageBox.Show("indirim miktarı alış fiyatından daha yüksek olamaz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudSalesPrice.Value < nudBuyPrice.Value)
            {
                MessageBox.Show("satış fiyatı alış fiyatından daha düşük olamaz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DB.ExecuteData("insert into Products values ("+txtID.Text+",'" + txtName.Text+"',"+nudQty.Value+","+nudBuyPrice.Value+","+nudSalesPrice.Value+",'"+txtBarcode.Text+"',"+nudMinQty.Value+","+nudMaxDiscount.Value+")", "Yeni Ürün başarıyla eklendi.");
                AutoNumber();
            }
        }
        //yeni ürün ekleme işlemini aktifleştirmek.
        private void btnNew_Click(object sender, EventArgs e)
        {
            AutoNumber();
        }
        //yeni değişklikleri kaydetmek için.
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Lütfen önce Üürün Adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudSalesPrice.Value <= 0)
            {
                MessageBox.Show("Lütfen doğru bir satış fiyatını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudBuyPrice.Value <= 0)
            {
                MessageBox.Show("Lütfen doğru bir alış fiyatını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (nudMaxDiscount.Value > nudBuyPrice.Value)
            {
                MessageBox.Show("indirim miktarı alış fiyatından daha yüksek olamaz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (nudSalesPrice.Value < nudBuyPrice.Value)
            {
                MessageBox.Show("satış fiyatı alış fiyatından daha düşük olamaz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DB.ExecuteData("update Products set Pro_Name='" + txtName.Text + "',Qty="+nudQty.Value+ ",Buy_Price=" + nudBuyPrice.Value + ",Sale_Price=" + nudSalesPrice.Value+ ",BarCode='"+txtBarcode.Text+ "',MinQty="+nudMinQty.Value+ ",MaxDiscount="+nudMaxDiscount.Value+" where Pro_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");

            }
        }
        //bir ürün silmek için
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("" + txtID.Text + " Nolu Ürünü silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Products where Pro_ID=" + txtID.Text + "", "Ürünü başarıyla Silindi");
                AutoNumber();
            }
        }
        //tüm ürünler silmek için.
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm Ürünler silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Products", "Tüm Ürünler başarıyla Silindi");
                AutoNumber();
            }
        }
        //bir ürün aramak ve bilgilerini göstermek için.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtSearch.Text!="")
            {
                DataTable tablesearch = new DataTable();
                tablesearch.Clear();
                tablesearch = DB.ReadData("select * from Products where Pro_Name like '%" + txtSearch.Text + "%'");
                if (tablesearch.Rows.Count <= 0)
                {
                    MessageBox.Show("aramak istediğiniz ürün listede bulunmamaktadır", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    try
                    {
                        txtID.Text = tablesearch.Rows[0][0].ToString();
                        txtName.Text = tablesearch.Rows[0][1].ToString();
                        nudQty.Value = Convert.ToDecimal(tablesearch.Rows[0][2]);
                        nudBuyPrice.Value = Convert.ToDecimal(tablesearch.Rows[0][3]);
                        nudSalesPrice.Value = Convert.ToDecimal(tablesearch.Rows[0][4]);
                        txtBarcode.Text = tablesearch.Rows[0][5].ToString();
                        nudMinQty.Value = Convert.ToDecimal(tablesearch.Rows[0][6]);
                        nudMaxDiscount.Value = Convert.ToDecimal(tablesearch.Rows[0][7]);
                    }
                    catch (Exception)
                    { }

                }
                btnAdd.Enabled = false;
                btnNew.Enabled = true;
                btnDelete.Enabled = true;
                btnDeleteAll.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Lütfen aramak istediğiniz ürün adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
        }
        //comboboxtan bir ürün seçtikten sonra o ürünün bilgilerini gösterecek.
        private void cbxProducts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbxProducts.Items.Count>=1)
            {
                DataTable tablesearch = new DataTable();
                tablesearch.Clear();
                tablesearch = DB.ReadData("select * from Products where Pro_ID=" + cbxProducts.SelectedValue + "");
                if (tablesearch.Rows.Count <= 0)
                {
                    MessageBox.Show("aramak istediğiniz ürün listede bulunmamaktadır", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    try
                    {
                        txtID.Text = tablesearch.Rows[0][0].ToString();
                        txtName.Text = tablesearch.Rows[0][1].ToString();
                        nudQty.Value = Convert.ToDecimal(tablesearch.Rows[0][2]);
                        nudBuyPrice.Value = Convert.ToDecimal(tablesearch.Rows[0][3]);
                        nudSalesPrice.Value = Convert.ToDecimal(tablesearch.Rows[0][4]);
                        txtBarcode.Text = tablesearch.Rows[0][5].ToString();
                        nudMinQty.Value = Convert.ToDecimal(tablesearch.Rows[0][6]);
                        nudMaxDiscount.Value = Convert.ToDecimal(tablesearch.Rows[0][7]);
                    }
                    catch (Exception)
                    { }

                }
                btnAdd.Enabled = false;
                btnNew.Enabled = true;
                btnDelete.Enabled = true;
                btnDeleteAll.Enabled = true;
                btnSave.Enabled = true;
            }
           
        }
    }
}