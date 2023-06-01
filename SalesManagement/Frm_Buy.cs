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
    public partial class Frm_Buy : DevExpress.XtraEditors.XtraForm
    {
        private static Frm_Buy frm;
        static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }
        public static Frm_Buy GetForm
        {
            get
            {
                if(frm==null)
                {
                    frm = new Frm_Buy();
                    frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                }
                return frm;
            }
        }
        public Frm_Buy()
        {
            InitializeComponent();
            if (frm == null)
                frm = this;
        }
        DataBase DB = new DataBase();
        DataTable table = new DataTable();
        //satın alma (satıcı) faturasını otomatik olarak numaralandırmak için.
        public void AutoNumber()
        {
            table.Clear();
            table = DB.ReadData("select max(Order_ID) from Buy");//son eklenen fatura numarasını getir.
            if (table.Rows[0][0].ToString() == DBNull.Value.ToString())//tablo boş ise ilk fatura 1 olarak numaralandır.
            {
                txtID.Text = "1";
            }
            else//tablo boş değilse yeni fatura numarasını son eklenen fatura numarasınıa bir artarak numaralandır.
            {
                txtID.Text = (Convert.ToInt32(table.Rows[0][0]) + 1).ToString();
            }
            dtpDate.Text = DateTime.Now.ToShortDateString();//fatura tarihi
            dtpSonra.Text = DateTime.Now.ToShortDateString();//taksit ile satın alınınca ödenecek tarih. 
            try
            {
                cbxItems.SelectedIndex = 0;//ürün listesinde ilk üründe dur.
                cbxSuppliers.SelectedIndex = 0;//satıcı listesinde ik satıcıda dur.
            }
            catch(Exception)
            { }
            cbxItems.Text = "ürün seç";
            dgvBuy.Rows.Clear();
            rbtnCash.Checked = true;
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
        //comboboxtan satıcı seçebilmek için.
        public void FillSuppliers()
        {
            cbxSuppliers.DataSource = DB.ReadData("select * from Suppliers");
            cbxSuppliers.DisplayMember = "Sup_Name";//gösterilecek satıcı adları.
            cbxSuppliers.ValueMember = "Sup_ID";//her bir gösterilecek satıcı bir numarası var.
        }
        private void Frm_Buy_Load(object sender, EventArgs e)
        {
            try
            {
                FillItems();
                FillSuppliers();
                AutoNumber();
                lblUserName.Text = Properties.Settings.Default.USERNAME;
            }
            catch(Exception)
            { }
           
        }
        //satıcı sayfası açmak ve satıcı ekleme veya satıcı bilgilerini düzeltmek için. 
        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            Frm_Suppliers frm = new Frm_Suppliers();
            frm.ShowDialog();
        }
        //faturaya yeni bir ürün eklemek.
        private void btnItems_Click(object sender, EventArgs e)
        {
            if(cbxItems.Text== "ürün seç")//bir ürün seçmediysek.
            {
                MessageBox.Show("lütfen ürün seçin","Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(cbxItems.Items.Count<=0)//ürün listesi boş ise.
            {
                MessageBox.Show("eklenecek bir ürün yoktur", "Uyarı", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
               
            DataTable tableItems = new DataTable();
            tableItems.Clear();
            tableItems = DB.ReadData("select * from Products where Pro_ID=" + cbxItems.SelectedValue + "");//seçtiğimiz ürünün bilgileri getirilecek.
            if(tableItems.Rows.Count>=1)
            {
                 try
                 {
                     string Pro_ID = tableItems.Rows[0][0].ToString();
                     string Pro_Name = tableItems.Rows[0][1].ToString();
                     string Pro_Qty = "1";
                     string Pro_Price = tableItems.Rows[0][3].ToString();
                     decimal Discount = 0;
                     decimal Total = Convert.ToDecimal(Pro_Price) * Convert.ToDecimal(Pro_Qty);
                    //seçilen ürün bilgileri array olarak datagridviewe eklenecek. 
                     object[] data = { Pro_ID, Pro_Name, Pro_Qty, Pro_Price, Discount, Total };
                     dgvBuy.Rows.Add(data);
                 }
                 catch(Exception)
                 {  }
                 decimal sum = 0;
                 for (int i = 0; i < dgvBuy.Rows.Count; i++)//faturanın tutarını hesaplayacak.
                 {
                    sum += Convert.ToDecimal(dgvBuy.Rows[i].Cells[5].Value);
                    
                 }             
                    txtTotal.Text = Math.Round(sum, 2).ToString();
                    lblItemsCount.Text = dgvBuy.Rows.Count.ToString();
            }  
        }
        //faturadan ürün silmek.
        private void btnDeleteitem_Click(object sender, EventArgs e)
        {
            if(dgvBuy.Rows.Count>=1)
            {
                if(dgvBuy.CurrentRow!=null)
                {
                    dgvBuy.Rows.Remove(dgvBuy.CurrentRow);
                }
            }
            if(dgvBuy.Rows.Count<=0)
            {
                MessageBox.Show("silinecek bir satır yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblItemsCount.Text = "0";
                return;
            }
            decimal sum = 0;
            for (int i = 0; i < dgvBuy.Rows.Count; i++)//faturanın tutarını yeniden yapacak.
            {
                sum += Convert.ToDecimal(dgvBuy.Rows[i].Cells[5].Value);
            }
            txtTotal.Text = Math.Round(sum, 2).ToString();
            lblItemsCount.Text = dgvBuy.Rows.Count.ToString();
        }
        //ürün barkodu ile eklemek için.
        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                DataTable tableItems = new DataTable();
                tableItems.Clear();
                tableItems = DB.ReadData("select * from Products where BarCode='" +txtBarcode.Text + "'");
                if (tableItems.Rows.Count >= 1)
                {
                    try
                    {
                        string Pro_ID = tableItems.Rows[0][0].ToString();
                        string Pro_Name = tableItems.Rows[0][1].ToString();
                        string Pro_Qty = "1";
                        string Pro_Price = tableItems.Rows[0][3].ToString();
                        decimal Discount = 0;
                        decimal Total = Convert.ToDecimal(Pro_Price) * Convert.ToDecimal(Pro_Qty);
                        //seçilen ürün bilgileri array olarak datagridviewe eklenecek. 
                        object[] data = { Pro_ID, Pro_Name, Pro_Qty, Pro_Price, Discount, Total };
                        dgvBuy.Rows.Add(data);
                    }
                    catch (Exception)
                    { }
                    decimal sum = 0;
                    for (int i = 0; i < dgvBuy.Rows.Count; i++)//fatura tutarını hesaplayacak.
                    {
                        sum += Convert.ToDecimal(dgvBuy.Rows[i].Cells[5].Value);
                    }
                    txtTotal.Text = Math.Round(sum, 2).ToString();
                    lblItemsCount.Text = dgvBuy.Rows.Count.ToString();
                }
            }
           
        }
        //fatura ödeme yapmak.
        private void orderpay()
        {
            if (dgvBuy.Rows.Count >= 1)
            {
                if (cbxSuppliers.Items.Count <= 0)
                {
                    MessageBox.Show("önce Satıcı bilgileri giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    Properties.Settings.Default.TotalOrder = Convert.ToDecimal(txtTotal.Text);//fatura tutarını (TotalOrder) değişkene aktar.
                    Properties.Settings.Default.Paied = 0;//fatura ödenen para tutarı (Paied) değişkene aktar.
                    Properties.Settings.Default.Remainder = 0;//fatura daha sonra ödenmesi gereken para tutarı (Remainder) değişkene aktar.
                    Frm_PayBuy frm = new Frm_PayBuy();
                    frm.ShowDialog();
                   
                    if (Properties.Settings.Default.CheckButton == true) //kullancı faturayı ödemekten vazgeçmezse.
                    { 
                    String d = dtpDate.Value.ToString("dd/MM/yyyy");//fatura tarihi
                    String dreminder = dtpSonra.Value.ToString("dd/MM/yyyy");//ödenecek tarih
                    DB.ExecuteData("insert into Buy values(" + txtID.Text + ",'" + d + "'," + cbxSuppliers.SelectedValue + ")", "Fatura Eklendi");
                    for (int i = 0; i < dgvBuy.Rows.Count; i++)
                    {
                        DB.ExecuteData("insert into Buy_Details values(" + txtID.Text + "," + cbxSuppliers.SelectedValue + "," + dgvBuy.Rows[i].Cells[0].Value + ",'" + d + "'," + dgvBuy.Rows[i].Cells[2].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + dgvBuy.Rows[i].Cells[3].Value + "," + dgvBuy.Rows[i].Cells[4].Value + "," + dgvBuy.Rows[i].Cells[5].Value + ","+txtTotal.Text+","+Properties.Settings.Default.Paied+","+Properties.Settings.Default.Remainder+")", "");
                        DB.ExecuteData("update Products set Qty=Qty+"+dgvBuy.Rows[i].Cells[2].Value+ " where Pro_ID="+dgvBuy.Rows[i].Cells[0].Value+"", "");//ürünler satın aldıktan sonra yeni miktar mevcut miktara ekle.
                    }
                    if(rbtnCash.Checked==true)//peşin ödenince satıcı hesapları ödenen olarak kaydet. 
                    {
                         DB.ExecuteData("insert into Supplier_Report values("+txtID.Text+","+Properties.Settings.Default.Paied+",'"+d+"',"+cbxSuppliers.SelectedValue+")", "");
                    }
                    else if(rbtnSonra.Checked==true)//taksit ödenince satıcı hesapları ödenmeyen olarak kaydet.
                    {
                         DB.ExecuteData("insert into Supplier_Money values("+txtID.Text+","+cbxSuppliers.SelectedValue+","+Properties.Settings.Default.Remainder+",'"+d+"','"+dreminder+"')", "");
                         if(Properties.Settings.Default.Paied>=1)//fatura tutarından bir miktar ödenmek istenince.
                         {
                             DB.ExecuteData("insert into Supplier_Report values(" + txtID.Text + "," + Properties.Settings.Default.Paied + ",'" + d + "'," + cbxSuppliers.SelectedValue + ")", "");
                         }

                    }
                        Print();
                        AutoNumber();
                    }
                }
                catch (Exception)
                { }
            }
        }
        //fatura yazdırmak için.
        private void Print()
        {
            int id = Convert.ToInt32(txtID.Text);
            DataTable tablereport = new DataTable();
            tablereport.Clear();
            tablereport = DB.ReadData("SELECT [Order_ID] as'Fatura No',[Sup_Name] as'Satıcı Adı',[Pro_Name] as'Ürün Adı',[Date] as'Fatura Tarihi',BD.[Qty] as'Miktar',[User_Name] as'kasyer',[Price] as'Fiyat',[Discount] as'İndirim',[Total] as'Toplam',[Total_Order] as'Fatura Tutarı',[Paied] as'Ödenen Tutar',[Remainder] as'Kalan Tutar'FROM Buy_Details BD,Suppliers S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and Order_ID="+id+" ");
            try
            {
                Frm_Printing frm = new Frm_Printing();
                frm.crystalReportViewer1.RefreshReport();
                RptOrderBuy rpt = new RptOrderBuy();
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
            catch(Exception)
            { }          
        }
        //seçtiğimiz ürün miktarı,indirimi veya fiyatını düzeltmek. 
        private void updateorder()
        {
            if (dgvBuy.Rows.Count >= 1)
            {
                int index = dgvBuy.SelectedRows[0].Index;//seçtiğimiz satır indisi.

                Properties.Settings.Default.Item_Qty = Convert.ToDecimal(dgvBuy.Rows[index].Cells[2].Value);//ürün miktarı (Item_Qty) değişkene aktar.
                Properties.Settings.Default.Item_BuyPrice = Convert.ToDecimal(dgvBuy.Rows[index].Cells[3].Value);//ürün fiyatı (Item_BuyPrice) değişkene aktar.
                Properties.Settings.Default.Item_Discount = Convert.ToDecimal(dgvBuy.Rows[index].Cells[4].Value);//ürün indirimi (Item_Discount) değişkene aktar.
                Properties.Settings.Default.Save();
                Frm_BuyQty frm = new Frm_BuyQty();
                frm.ShowDialog();
            }
        }
        private void Frm_Buy_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F12)//faturayı ödemek için.
            {
                orderpay();
            }
            else if(e.KeyCode==Keys.F11)//fiyat,indirim veya ürün sayısını düzeltmek için.
            {
                updateorder();          
            }
            //f2 basınca ürün ekleyecek.
            else if(e.KeyCode==Keys.F2)
            {
                btnItems_Click(null, null);
            }
            //f1 basınca barkod ile ürün eklemek için.
            else if(e.KeyCode==Keys.F1)
            {
                txtBarcode.Clear();
                txtBarcode.Focus();
            }
        }
        //fiyat,ürün sayısı ve  indirim değiştirdikten sonra toplam yeniden yapacak.
        private void dgvBuy_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            decimal Item_Qty , Item_BuyPrice , Item_Discount;
            try
            {
                if(dgvBuy.CurrentRow!=null)
                { 
                Item_Qty = Convert.ToDecimal(dgvBuy.CurrentRow.Cells[2].Value);
                Item_BuyPrice = Convert.ToDecimal(dgvBuy.CurrentRow.Cells[3].Value);
                Item_Discount = Convert.ToDecimal(dgvBuy.CurrentRow.Cells[4].Value);

                decimal Total = (Item_Qty * Item_BuyPrice) - Item_Discount;
                dgvBuy.CurrentRow.Cells[5].Value =Total;
                   
                }
                decimal TotalOrder = 0;
                for (int i = 0; i < dgvBuy.Rows.Count; i++)
                {
                    TotalOrder += Convert.ToDecimal(dgvBuy.Rows[i].Cells[5].Value);
                }
                txtTotal.Text = Math.Round(TotalOrder,2).ToString();
            }
            catch(Exception)
            {   }
                
            
           
        }
    }
}