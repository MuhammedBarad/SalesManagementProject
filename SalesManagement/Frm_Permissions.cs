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
    public partial class Frm_Permissions : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Permissions()
        {
            InitializeComponent();
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();
        DataTable tblsearch = new DataTable();//kullancı bilgileri datagrdviewe akatarmak için.
        //kullancı numarasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            tblsearch.Clear();
            tblsearch= DB.ReadData("SELECT [User_ID] as'Kullancı No',[User_Name]as'Kullancı Adı',[Type]as'Kullancı Tipi'FROM [dbo].[Users]");
            dgvSearch.DataSource = tblsearch;
            table.Clear();
            table = DB.ReadData("select max(User_ID) from Users");
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise.
            {
                txtID.Text = "1";//kullancı numarası bir olarak numaralandır.
            }
            else//tablo boş değilse yeni kullancı No en son eklenen kullancı adı numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            try
            {
                txtPassword.Clear();
                txtUserName.Clear();
                FillUsers();
                cbxType.SelectedIndex = 0;
                btnAdd.Enabled = true;
                btnNew.Enabled = true;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
            }
            catch(Exception)
            { }
        }
        int row;//kullancılar arasında geçiş yapabilmek için kullanılan bir global variable.
        private void Show()//kullancılar arasında geçiş yapabilmek için.
        {
            table.Clear();
            table = DB.ReadData("select * from Users");
            if (table.Rows.Count <= 0)
            {
                MessageBox.Show("Bu ekranda gösterilecek bir bilgi yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                txtID.Text = table.Rows[row][0].ToString();
                txtUserName.Text = table.Rows[row][1].ToString();
                txtPassword.Text = table.Rows[row][2].ToString();
                cbxType.Text = table.Rows[row][3].ToString();
            }
            btnAdd.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = true;
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        //bir kullancı seçmek için çalışan adlarını comboboxa doldur
        private void FillUsers()
        {
            //ayarlar sayfasında kullancılar comboboxa akatar.
            cbxUsers1.DataSource = DB.ReadData("select * from Users");
            cbxUsers1.DisplayMember = "User_Name";
            cbxUsers1.ValueMember = "User_ID";
            //müşteri sayfasında kullancılar comboboxa akatar.
            cbxUsers2.DataSource = DB.ReadData("select * from Users");
            cbxUsers2.DisplayMember = "User_Name";
            cbxUsers2.ValueMember = "User_ID";
            //satıcı sayfasında kullancılar comboboxa akatar.
            cbxUsers3.DataSource = DB.ReadData("select * from Users");
            cbxUsers3.DisplayMember = "User_Name";
            cbxUsers3.ValueMember = "User_ID";
            //satın alma sayfasında kullancılar comboboxa akatar.
            cbxUsers4.DataSource = DB.ReadData("select * from Users");
            cbxUsers4.DisplayMember = "User_Name";
            cbxUsers4.ValueMember = "User_ID";
            //satış sayfasında kullancılar comboboxa akatar.
            cbxUsers5.DataSource = DB.ReadData("select * from Users");
            cbxUsers5.DisplayMember = "User_Name";
            cbxUsers5.ValueMember = "User_ID";
            //iade sayfasında kullancılar comboboxa akatar.
            cbxUsers6.DataSource = DB.ReadData("select * from Users");
            cbxUsers6.DisplayMember = "User_Name";
            cbxUsers6.ValueMember = "User_ID";
            //masraf sayfasında kullancılar comboboxa akatar.
            cbxUsers7.DataSource = DB.ReadData("select * from Users");
            cbxUsers7.DisplayMember = "User_Name";
            cbxUsers7.ValueMember = "User_ID";
            //çalışan sayfasında kullancılar comboboxa akatar.
            cbxUsers8.DataSource = DB.ReadData("select * from Users");
            cbxUsers8.DisplayMember = "User_Name";
            cbxUsers8.ValueMember = "User_ID";

        }
        //ilk kullancı bilgilerini göstermek için.
        private void btnFirst_Click(object sender, EventArgs e)
        {
            row = 0;
            Show();
        }
        //bir önceki kullancı bilgilerini göstermek için.
        private void btnPrev_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Users");
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
        //bir sonraki kullancı bilgilerini göstermek için.
        private void btnNext_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Users");
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
        //son kullancı bilgilerini göstermek için.
        private void btnLast_Click(object sender, EventArgs e)
        {
            table.Clear();
            table = DB.ReadData("select count(*) from Users");
            row = Convert.ToInt32(table.Rows[0][0]) - 1;
            Show();
        }

        private void Frm_Permissions_Load(object sender, EventArgs e)
        {
            try
            {
                AutoNumber();
            }
            catch(Exception)
            { }     
        }
        //yeni kullancı eklemek için.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || txtPassword.Text=="")
            {
                MessageBox.Show("Lütfen kullancı adı ve şifresini giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }      
            else//kullancı eklediğimizde tüm sayfalara erişim kapalı olacak(0).
            {
                DB.ExecuteData("insert into Users values (" + txtID.Text + ",'" + txtUserName.Text + "','" + txtPassword.Text + "','"+cbxType.Text+"')", "Yeni Kullancı başarıyla eklendi.");
                DB.ExecuteData("insert into User_Setting values (" + txtID.Text + ",0,0,0)", "");
                DB.ExecuteData("insert into User_Customer values (" + txtID.Text + ",0,0,0)", "");
                DB.ExecuteData("insert into User_Supplier values (" + txtID.Text + ",0,0,0)", "");
                DB.ExecuteData("insert into User_Buy values (" + txtID.Text + ",0,0)", "");
                DB.ExecuteData("insert into User_Sale values (" + txtID.Text + ",0,0,0)", "");
                DB.ExecuteData("insert into User_Return values (" + txtID.Text + ",0,0)", "");
                DB.ExecuteData("insert into User_Deserved values (" + txtID.Text + ",0,0,0)", "");
                DB.ExecuteData("insert into User_Employee values (" + txtID.Text + ",0,0,0)", "");

                AutoNumber();
            }
        }
        //yeni kullancı ekleme işlemini aktifleştirmek.
        private void btnNew_Click(object sender, EventArgs e)
        {
            AutoNumber();
        }
        //yeni değişklikleri kaydetmek için.
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Lütfen kullancı adı ve şifresini giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DB.ExecuteData("update Users set User_Name='" + txtUserName.Text + "',Password='" + txtPassword.Text + "',Type='" + cbxType.Text + "' where User_ID=" + txtID.Text + " ", "Yeni değişiklikler başarıyla kaydedildi");
        }
        //bir kullancı silmek için
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("" + txtID.Text + " Nolu Kullancı silmekten emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.ExecuteData("delete from Users where User_ID=" + txtID.Text + "", "Kullancı başarıyla Silindi");
                DataTable tableusers = new DataTable();
                tableusers = DB.ReadData("select * from Users");
                if(tableusers.Rows.Count<=0)//kullancı tablosu boş ise defualt olarak bir kullancı olması gerek.
                {
                    DB.ExecuteData("insert into Users values (1,'123','123','Manager')", "");
                }        
            }
        }
        DataTable tablesearch = new DataTable();
        //ayarlar sayfasında seçilen kullancı izinleri göster.
        private void cbxUsers1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Setting where User_ID=" + cbxUsers1.SelectedValue+"");
                if(tablesearch.Rows.Count>=1)
                {
                    //kullancı izinleri kontrol et
                    if(Convert.ToInt32(tablesearch.Rows[0][1])==1)
                    {
                        checkPermissions.Checked = true;
                    }
                    else
                    {
                        checkPermissions.Checked = false;
                    }
                    //ürün ekleme kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkAddProduct.Checked = true;
                    }
                    else
                    {
                        checkAddProduct.Checked = false;
                    }
                    //ürün gösterme kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][3]) == 1)
                    {
                        checkShowProduct.Checked = true;
                    }
                    else
                    {
                        checkShowProduct.Checked = false;
                    }
                }
            }
            catch(Exception)
            { }
        }
        //ayarlar sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet.
        private void btnSave1_Click(object sender, EventArgs e)
        {
            try
            {
                int Permissions = 0, AddProduct = 0, ShowProduct = 0;
                if(checkPermissions.Checked==true)
                {
                    Permissions = 1;
                }
                else
                {
                    Permissions = 0;
                }
                if (checkAddProduct.Checked == true)
                {
                    AddProduct = 1;
                }
                else
                {
                    AddProduct = 0;
                }
                if(checkShowProduct.Checked==true)
                {
                    ShowProduct = 1;
                }
                else
                {
                    ShowProduct = 0;
                }
                DB.ExecuteData("update User_Setting set User_Permissions="+ Permissions + ",Add_Pro="+ AddProduct + ",Show_Pro="+ ShowProduct + " where User_ID="+cbxUsers1.SelectedValue+"", "Yeni değişiklikler kaydedildi");
                cbxUsers1.SelectedIndex = 0;
            }
            catch(Exception)
            { }
        }
        //müşteri sayfasında seçilen kullancı izinleri göster.
        private void cbxUsers2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Customer where User_ID=" + cbxUsers2.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //müşteri ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkCustomer.Checked = true;
                    }
                    else
                    {
                        checkCustomer.Checked = false;
                    }
                    //müşteri hesapları-ödenmeyen kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkCustomerMoney.Checked = true;
                    }
                    else
                    {
                        checkCustomerMoney.Checked = false;
                    }
                    //müşteri hesapları-ödenen kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][3]) == 1)
                    {
                        checkCustomerReport.Checked = true;
                    }
                    else
                    {
                        checkCustomerReport.Checked = false;
                    }
                }
            }
            catch (Exception)
            { }
        }
        //müşteri sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet.
        private void btnSave2_Click(object sender, EventArgs e)
        {

            try
            {
                int Customer = 0, Customer_Money = 0, Customer_Report = 0;
                if (checkCustomer.Checked == true)
                {
                    Customer = 1;
                }
                else
                {
                    Customer = 0;
                }
                if (checkCustomerMoney.Checked == true)
                {
                    Customer_Money = 1;
                }
                else
                {
                    Customer_Money = 0;
                }
                if (checkCustomerReport.Checked == true)
                {
                    Customer_Report = 1;
                }
                else
                {
                    Customer_Report = 0;
                }
                DB.ExecuteData("update User_Customer set Customer=" + Customer + ",Customer_Money=" + Customer_Money + ",Customer_Report=" + Customer_Report + " where User_ID=" + cbxUsers2.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers2.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
        //satıcı sayfasında seçilen kullancı izinleri göster.
        private void cbxUsers3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Supplier where User_ID=" + cbxUsers3.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //satıcı ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkSupplier.Checked = true;
                    }
                    else
                    {
                        checkSupplier.Checked = false;
                    }
                    //satıcı hesapları-ödenmeyen kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkSupplierMoney.Checked = true;
                    }
                    else
                    {
                        checkSupplierMoney.Checked = false;
                    }
                    //satıcı hesapları-ödenen kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][3]) == 1)
                    {
                        checkSupplierReport.Checked = true;
                    }
                    else
                    {
                        checkSupplierReport.Checked = false;
                    }
                }
            }
            catch (Exception)
            {  }
        }
        //satıcı sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet.
        private void btnSave3_Click(object sender, EventArgs e)
        {
            try
            {
                int Supplier = 0, Supplier_Money = 0, Supplier_Report = 0;
                if (checkSupplier.Checked == true)
                {
                    Supplier = 1;
                }
                else
                {
                    Supplier = 0;
                }
                if (checkSupplierMoney.Checked == true)
                {
                    Supplier_Money = 1;
                }
                else
                {
                    Supplier_Money = 0;
                }
                if (checkSupplierReport.Checked == true)
                {
                    Supplier_Report = 1;
                }
                else
                {
                    Supplier_Report = 0;
                }
                DB.ExecuteData("update User_Supplier set Supplier=" + Supplier + ",Supplier_Money=" + Supplier_Money + ",Supplier_Report=" + Supplier_Report + " where User_ID=" + cbxUsers3.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers3.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
        //satın alma sayfasında seçilen kullancı izinleri göster.
        private void cbxUsers4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Buy where User_ID=" + cbxUsers4.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //satın alma ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkBuy.Checked = true;
                    }
                    else
                    {
                        checkBuy.Checked = false;
                    }
                    //satın alma raporu kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkBuyReport.Checked = true;
                    }
                    else
                    {
                        checkBuyReport.Checked = false;
                    }
                    
                }
            }
            catch (Exception)
            { }
        }
        //satın alma sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet.
        private void btnSave4_Click(object sender, EventArgs e)
        {
            try
            {
                int Buy = 0, Buy_Report = 0;
                if (checkBuy.Checked == true)
                {
                    Buy = 1;
                }
                else
                {
                    Buy = 0;
                }
                if (checkBuyReport.Checked == true)
                {
                    Buy_Report = 1;
                }
                else
                {
                    Buy_Report = 0;
                }
               
                DB.ExecuteData("update User_Buy set Buy=" + Buy + ",Buy_Report=" + Buy_Report + " where User_ID=" + cbxUsers4.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers4.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
        //satış sayfasında seçilen kullancı izinleri göster.
        private void cbxUsers5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Sale where User_ID=" + cbxUsers5.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //satış ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkSale.Checked = true;
                    }
                    else
                    {
                        checkSale.Checked = false;
                    }
                    //satış raporu kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkSaleReport.Checked = true;
                    }
                    else
                    {
                        checkSaleReport.Checked = false;
                    }
                    //satış karı raporu kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][3]) == 1)
                    {
                        checkSaleProfit.Checked = true;
                    }
                    else
                    {
                        checkSaleProfit.Checked = false;
                    }

                }
            }
            catch (Exception)
            { }
        }
        //satış sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet
        private void btnSave5_Click(object sender, EventArgs e)
        {
            try
            {
                int Sale = 0, Sale_Report = 0, Sale_profit = 0;
                if (checkSale.Checked == true)
                {
                    Sale = 1;
                }
                else
                {
                    Sale = 0;
                }
                if (checkSaleReport.Checked == true)
                {
                    Sale_Report = 1;
                }
                else
                {
                    Sale_Report = 0;
                }
                if (checkSaleProfit.Checked == true)
                {
                    Sale_profit = 1;
                }
                else
                {
                    Sale_profit = 0;
                }

                DB.ExecuteData("update User_Sale set Sale=" + Sale + ",Sale_Report=" + Sale_Report + ",Sale_profit="+ Sale_profit + " where User_ID=" + cbxUsers5.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers5.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
        //iade sayfasında seçilen kullancı izinleri göster
        private void cbxUsers6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Return where User_ID=" + cbxUsers6.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //iade ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkReturn.Checked = true;
                    }
                    else
                    {
                        checkReturn.Checked = false;
                    }
                    //iade raporu kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkReturnReport.Checked = true;
                    }
                    else
                    {
                        checkReturnReport.Checked = false;
                    }

                }
            }
            catch (Exception) { }

        }
        //iade sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet
        private void btnSave6_Click(object sender, EventArgs e)
        {
            try
            {
                int Returnn = 0, Return_Report = 0;
                if (checkReturn.Checked == true)
                {
                    Returnn = 1;
                }
                else
                {
                    Returnn = 0;
                }
                if (checkReturnReport.Checked == true)
                {
                    Return_Report = 1;
                }
                else
                {
                    Return_Report = 0;
                }
              

                DB.ExecuteData("update User_Return set Returnn=" + Returnn + ",Return_Report=" + Return_Report + " where User_ID=" + cbxUsers6.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers6.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
        //masraf sayfasında seçilen kullancı izinleri göster
        private void cbxUsers7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Deserved where User_ID=" + cbxUsers7.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //masraf tipi ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkDeservedType.Checked = true;
                    }
                    else
                    {
                        checkDeservedType.Checked = false;
                    }
                    //masraf yönetim kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkDeserved.Checked = true;
                    }
                    else
                    {
                        checkDeserved.Checked = false;
                    }
                    //masraf raporları kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkDeservedReport.Checked = true;
                    }
                    else
                    {
                        checkDeservedReport.Checked = false;
                    }

                }
            }
            catch (Exception) { }
        }
        //masraf sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet
        private void btnSave7_Click(object sender, EventArgs e)
        {
            try
            {
                int deservedtype = 0, deserved = 0, deservedreport = 0;
                if (checkDeservedType.Checked == true)
                {
                    deservedtype = 1;
                }
                else
                {
                    deservedtype = 0;
                }
                if (checkDeserved.Checked == true)
                {
                    deserved = 1;
                }
                else
                {
                    deserved = 0;
                }
                if (checkDeservedReport.Checked == true)
                {
                    deservedreport = 1;
                }
                else
                {
                    deservedreport = 0;
                }

                DB.ExecuteData("update User_Deserved set Deserved_Type=" + deservedtype + ",Deserved=" + deserved + ",Deserved_Report=" + deservedreport + " where User_ID=" + cbxUsers7.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers7.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
        //çalışan sayfasında seçilen kullancı izinleri göster
        private void cbxUsers8_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tablesearch.Clear();
                tablesearch = DB.ReadData("select *from User_Employee where User_ID=" + cbxUsers8.SelectedValue + "");
                if (tablesearch.Rows.Count >= 1)
                {
                    //çalışan  ekranı kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][1]) == 1)
                    {
                        checkEmployee.Checked = true;
                    }
                    else
                    {
                        checkEmployee.Checked = false;
                    }
                    //çalışan maaşları kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkEmployeeSalary.Checked = true;
                    }
                    else
                    {
                        checkEmployeeSalary.Checked = false;
                    }
                    //çalışan raporları kontrol et
                    if (Convert.ToInt32(tablesearch.Rows[0][2]) == 1)
                    {
                        checkEmployeeSalaryReport.Checked = true;
                    }
                    else
                    {
                        checkEmployeeSalaryReport.Checked = false;
                    }

                }
            }
            catch (Exception) { }
        }

        //çalışan sayfasında seçilen kullancı için seçilen kullancı izinlerini kaydet
        private void btnSave8_Click(object sender, EventArgs e)
        {
            try
            {
                int Employee = 0, EmployeeSalary = 0, EmployeeReport = 0;
                if (checkEmployee.Checked == true)
                {
                    Employee = 1;
                }
                else
                {
                    Employee = 0;
                }
                if (checkEmployeeSalary.Checked == true)
                {
                    EmployeeSalary = 1;
                }
                else
                {
                    EmployeeSalary = 0;
                }
                if (checkEmployeeSalaryReport.Checked == true)
                {
                    EmployeeReport = 1;
                }
                else
                {
                    EmployeeReport = 0;
                }


                DB.ExecuteData("update User_Employee set Employee=" + Employee + ",Employee_Salary=" + EmployeeSalary + ",Employee_Report=" + EmployeeReport + " where User_ID=" + cbxUsers8.SelectedValue + "", "Yeni değişiklikler kaydedildi");
                cbxUsers8.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }
    }
}