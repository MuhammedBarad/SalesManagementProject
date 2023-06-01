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
    public partial class Frm_Sales : DevExpress.XtraEditors.XtraForm
    {
        private static Frm_Sales frm;
        static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }
        public static Frm_Sales GetForm
        {
            get
            {
                if (frm == null)
                {
                    frm = new Frm_Sales();
                    frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                }
                return frm;
            }
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();
        public Frm_Sales()
        {
            InitializeComponent();
            if (frm == null)
                frm = this;
        }
        //satın alma (satıcı) faturasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Order_ID) from Sales");//son eklenen fatura numarasını getir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk fatura 1 olarak numaralandır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni fatura numarasını son eklenen fatura numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            dtpDate.Text = DateTime.Now.ToShortDateString();//fatura tarihi
            dtpReminderDate.Text = DateTime.Now.ToShortDateString();//taksit ile satınca ödenecek tarih. 
            try
            {
                cbxItems.SelectedIndex = 0;//ürün listesinde ilk üründe dur.
                cbxCustomers.SelectedIndex = 0;//satıcı listesinde ik müşteride dur.
            }
            catch (Exception)
            {  }
            cbxItems.Text = "ürün seç";
            dgvSale.Rows.Clear();          
            txtBarcode.Clear();
            txtBarcode.Focus();
            txtTotal.Clear();
        }
        //comboboxtan ürün seçebilmek için.
        private void FillItems()
        {
            cbxItems.DataSource = DB.ReadData("select * from Products");
            cbxItems.DisplayMember = "Pro_Name";//gösterilecek ürün adları.
            cbxItems.ValueMember = "Pro_ID";//her bir gösterilecek ürün bir numarası var.
        }
        //comboboxtan müşteri seçebilmek için.
        public void FillCustomers()
        {
            cbxCustomers.DataSource = DB.ReadData("select * from Customers");
            cbxCustomers.DisplayMember = "Cust_Name";//gösterilecek müşteri adları.
            cbxCustomers.ValueMember = "Cust_ID";//her bir gösterilecek müşteri bir numarası var.
        }

        private void Frm_Sales_Load(object sender, EventArgs e)
        {              
            try
            {
                FillItems();
                FillCustomers();
                AutoNumber();
                lblUserName.Text = Properties.Settings.Default.USERNAME;
            }
            catch(Exception)
            {  }
       
        }
        //müşteri sayfası açmak ve müşteri ekleme veya müşteri bilgilerini düzeltmek için. 
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_Customers frm = new Frm_Customers();
                frm.ShowDialog();
            }
            catch (Exception)
            { }
        }
        //faturaya yeni bir ürün eklemek.
        private void btnItems_Click(object sender, EventArgs e)
        {
            if (cbxItems.Text == "ürün seç")
            {
                MessageBox.Show("lütfen ürün seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxItems.Items.Count <= 0)
            {
                MessageBox.Show("eklenecek bir ürün yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable tableItems = new DataTable();
            tableItems.Clear();
            tableItems = DB.ReadData("select * from Products where Pro_ID=" + cbxItems.SelectedValue + "");
            if (tableItems.Rows.Count >= 1)
            {
                try
                {
                    string Pro_ID = tableItems.Rows[0][0].ToString();
                    string Pro_Name = tableItems.Rows[0][1].ToString();
                    string Pro_Qty = "1";
                    string Pro_Price = tableItems.Rows[0][4].ToString();
                    decimal Discount = 0;
                    decimal Total = Convert.ToDecimal(Pro_Price) * Convert.ToDecimal(Pro_Qty);
                    //seçilen ürün bilgileri array olarak datagridviewe eklenecek. 
                    object[] data = { Pro_ID, Pro_Name, Pro_Qty, Pro_Price, Discount, Total };
                    dgvSale.Rows.Add(data);
                }
                catch (Exception)
                {   }
                decimal sum = 0;
                for (int i = 0; i < dgvSale.Rows.Count; i++)//faturanın tutarını hesaplayacak.
                {
                    sum += Convert.ToDecimal(dgvSale.Rows[i].Cells[5].Value);
                }
                txtTotal.Text = Math.Round(sum, 2).ToString();
                lblItemsCount.Text = dgvSale.Rows.Count.ToString();
            }
        }

        private void Frm_Sales_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F2)//f2 basınca ürün ekleyecek.
            {
                btnItems_Click(null,null);
            }
            else if(e.KeyCode==Keys.F1)//f1 basınca barkod ile ürün eklemek için.
            {
                txtBarcode.Clear();
                txtBarcode.Focus();
            }
            else if(e.KeyCode==Keys.F11)//fiyat,indirim veya ürün sayısını düzeltmek için.
            {
                updateorder();
            }
            else if(e.KeyCode==Keys.F12)//faturayı ödemek için.
            {
                orderpay();
            }
        }
        //ürün barkodu ile eklemek için.
        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {   
            if(e.KeyChar==13)
            {
                DataTable tableItems = new DataTable();
                tableItems.Clear();
                tableItems = DB.ReadData("select * from Products where BarCode='" + txtBarcode.Text + "'");
                if (tableItems.Rows.Count >= 1)
                {
                    try
                    {
                        string Pro_ID = tableItems.Rows[0][0].ToString();
                        string Pro_Name = tableItems.Rows[0][1].ToString();
                        string Pro_Qty = "1";
                        string Pro_Price = tableItems.Rows[0][4].ToString();
                        decimal Discount = 0;
                        decimal Total = Convert.ToDecimal(Pro_Price) * Convert.ToDecimal(Pro_Qty);
                        //seçilen ürün bilgileri array olarak datagridviewe eklenecek. 
                        object[] data = { Pro_ID, Pro_Name, Pro_Qty, Pro_Price, Discount, Total };
                        dgvSale.Rows.Add(data);
                    }
                    catch (Exception)
                    { }
                    decimal sum = 0;
                    for (int i = 0; i < dgvSale.Rows.Count; i++)//fatura tutarını hesaplayacak.
                    {
                        sum += Convert.ToDecimal(dgvSale.Rows[i].Cells[5].Value);
                    }
                    txtTotal.Text = Math.Round(sum, 2).ToString();
                    lblItemsCount.Text = dgvSale.Rows.Count.ToString();
                }
            }
           
        }
        //fatura ödeme yapmak.
        private void orderpay()
        {
            if (dgvSale.Rows.Count >= 1)
            {
                if (cbxCustomers.Items.Count <= 0)
                {
                    MessageBox.Show("önce Müşteri bilgileri giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    Properties.Settings.Default.TotalOrder = Convert.ToDecimal(txtTotal.Text);//fatura tutarını (TotalOrder) değişkene aktar.
                    Properties.Settings.Default.Paied = 0;//fatura ödenen para tutarı (Paied) değişkene aktar.
                    Properties.Settings.Default.Remainder = 0;//fatura daha sonra ödenmesi gereken para tutarı (Remainder) değişkene aktar.
                    Properties.Settings.Default.Save();
                    Frm_PaySale frm = new Frm_PaySale();
                    frm.ShowDialog();

                    if (Properties.Settings.Default.CheckButton == true)//kullancı faturayı ödemekten vazgeçmezse.
                    {
                        String d = dtpDate.Value.ToString("dd/MM/yyyy");//fatura tarihi
                        String dreminder = dtpReminderDate.Value.ToString("dd/MM/yyyy");//ödenecek tarih
                        DB.ExecuteData("insert into Sales values(" + txtID.Text + ",'" + d + "'," + cbxCustomers.SelectedValue + ")", "Fatura Eklendi");
                        for (int i = 0; i < dgvSale.Rows.Count; i++)
                        {
                            DB.ExecuteData("insert into Sales_Details values(" + txtID.Text + "," + cbxCustomers.SelectedValue + "," + dgvSale.Rows[i].Cells[0].Value + ",'" + d + "'," + dgvSale.Rows[i].Cells[2].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + dgvSale.Rows[i].Cells[3].Value + "," + dgvSale.Rows[i].Cells[4].Value + "," + dgvSale.Rows[i].Cells[5].Value + "," + txtTotal.Text + "," + Properties.Settings.Default.Paied + "," + Properties.Settings.Default.Remainder + ")", "");
                            DB.ExecuteData("update Products set Qty=Qty-" + dgvSale.Rows[i].Cells[2].Value + " where Pro_ID=" + dgvSale.Rows[i].Cells[0].Value + "", "");//ürünler sattıktan sonra satılan ürün miktarı mevcut miktardan çıkar.

                            decimal BuyPrice = 0;
                            BuyPrice = Convert.ToDecimal(DB.ReadData("select * from Products where Pro_ID=" + dgvSale.Rows[i].Cells[0].Value + "").Rows[0][3]);
                            DB.ExecuteData("insert into Sales_profit values(" + txtID.Text + "," + cbxCustomers.SelectedValue + "," + dgvSale.Rows[i].Cells[0].Value + ",'" + d + "'," + dgvSale.Rows[i].Cells[2].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + dgvSale.Rows[i].Cells[3].Value + "," + dgvSale.Rows[i].Cells[4].Value + "," + dgvSale.Rows[i].Cells[5].Value + "," + txtTotal.Text + "," + Properties.Settings.Default.Paied + "," + Properties.Settings.Default.Remainder + ","+BuyPrice+")", "");
                        }

                        if (rbtnCash.Checked == true)//peşin ödenince müşteri hesapları ödenen olarak kaydet. 
                        {                           
                            DB.ExecuteData("insert into Customer_Report values(" + txtID.Text + "," + Properties.Settings.Default.Paied + ",'" + d + "'," + cbxCustomers.SelectedValue + ")", "");
                        }
                        else if (rbtnSonra.Checked == true)//taksit ödenince müşteri hesapları ödenmeyen olarak kaydet.
                        {
                            DB.ExecuteData("insert into Customer_Money values(" + txtID.Text + "," + cbxCustomers.SelectedValue + "," + Properties.Settings.Default.Remainder + ",'" + d + "','" + dreminder + "')", "");
                            if (Properties.Settings.Default.Paied >= 1)//fatura tutarından bir miktar ödenmek istenince.
                            {
                                DB.ExecuteData("insert into Customer_Report values(" + txtID.Text + "," + Properties.Settings.Default.Paied + ",'" + d + "'," + cbxCustomers.SelectedValue + ")", "");
                            }

                        }
                        Print();
                        AutoNumber();
                    }
                }
                catch (Exception)
                {  }
            }
        }
        //fatura yazdırmak için.
        private void Print()
        {
            int id = Convert.ToInt32(txtID.Text);
            DataTable tablereport = new DataTable();
            tablereport.Clear();
            tablereport = DB.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',SD.Qty as'Adet',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kullancı Adı' ,Date as'Fatura Tarihi' FROM Sales_Details SD,Customers  C,Products P where SD.Cust_ID=C.Cust_ID and SD.Pro_ID=P.Pro_ID and Order_ID=" + txtID.Text + " ");
            try
            {
                Frm_Printing frm = new Frm_Printing();
                frm.crystalReportViewer1.RefreshReport();
                RptOrderSales rpt = new RptOrderSales();
                rpt.SetDatabaseLogon("", "", "DESKTOP-IPR2HMP", "Sales_System");

                rpt.SetDataSource(tablereport);
                rpt.SetParameterValue("ID", id);
                frm.crystalReportViewer1.ReportSource = rpt;
                //bilgisyara bağlı bir yazıcı var ise
                //System.Drawing.Printing.PrintDocument PrintDocument = new System.Drawing.Printing.PrintDocument();
                //rpt.PrintOptions.PrinterName = PrintDocument.PrinterSettings.PrinterName;
                //rpt.PrintToPrinter(1, true, 0, 0);

                frm.ShowDialog();

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }


        }
        //seçtiğimiz ürün miktarı,indirimi veya fiyatını düzeltmek. 
        private void updateorder()
        {
            if (dgvSale.Rows.Count >= 1)
            {
                int index = dgvSale.SelectedRows[0].Index;//seçtiğimiz satır indisi.

                Properties.Settings.Default.Item_Qty = Convert.ToDecimal(dgvSale.Rows[index].Cells[2].Value);//ürün miktarı (Item_Qty) değişkene aktar.
                Properties.Settings.Default.Item_SalePrice = Convert.ToDecimal(dgvSale.Rows[index].Cells[3].Value);//ürün fiyatı (Item_SalePrice) değişkene aktar.
                Properties.Settings.Default.Item_Discount = Convert.ToDecimal(dgvSale.Rows[index].Cells[4].Value);//ürün indirimi (Item_Discount) değişkene aktar.
                Properties.Settings.Default.Save();
                Frm_SaleQty frm = new Frm_SaleQty();
                frm.ShowDialog();
            }
        }
        //faturadan ürün silmek.
        private void btnDeleteitem_Click(object sender, EventArgs e)
        {
            if (dgvSale.Rows.Count >= 1)
            {
                if(dgvSale.CurrentRow!=null)
                {
                    dgvSale.Rows.Remove(dgvSale.CurrentRow);
                }
            }
            if (dgvSale.Rows.Count <= 0)
            {
                MessageBox.Show("silinecek bir satır yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblItemsCount.Text = "0";
                return;
            }
            decimal sum = 0;
            for (int i = 0; i < dgvSale.Rows.Count; i++)
            {
                sum += Convert.ToDecimal(dgvSale.Rows[i].Cells[5].Value);
            }
            txtTotal.Text = Math.Round(sum, 2).ToString();
            lblItemsCount.Text = dgvSale.Rows.Count.ToString();
        }
        //fiyat,ürün sayısı ve  indirim değiştirdikten sonra toplam yeniden yapacak.
        private void dgvSale_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            decimal Item_Qty, Item_SalePrice, Item_Discount;
            try
            {
                if (dgvSale.CurrentRow != null)
                {
                    Item_Qty = Convert.ToDecimal(dgvSale.CurrentRow.Cells[2].Value);
                    Item_SalePrice = Convert.ToDecimal(dgvSale.CurrentRow.Cells[3].Value);
                    Item_Discount = Convert.ToDecimal(dgvSale.CurrentRow.Cells[4].Value);

                    decimal Total = (Item_Qty * Item_SalePrice) - Item_Discount;
                    dgvSale.CurrentRow.Cells[5].Value = Total;

                }
                decimal TotalOrder = 0;
                for (int i = 0; i < dgvSale.Rows.Count; i++)
                {
                    TotalOrder += Convert.ToDecimal(dgvSale.Rows[i].Cells[5].Value);
                }
                txtTotal.Text = Math.Round(TotalOrder, 2).ToString();
            }
            catch (Exception)
            { }
        }
    }
}