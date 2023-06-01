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
    public partial class Frm_Return : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Return()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        DataBase db = new DataBase();
        private void Frm_Return_Load(object sender, EventArgs e)
        {
            dtpReturnDate.Text = DateTime.Now.ToShortDateString();
            txtOrderID.Focus();
        }
        //müşteri siparişini iade etmek isterse
        private void SalesReturn()
        {
            table = db.ReadData("SELECT Order_ID as'Fatura No',C.Cust_Name as'Müşteri Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',SD.Qty as'Adet',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kasiyer'FROM Sales_Details SD,Customers  C,Products P where SD.Cust_ID=C.Cust_ID and SD.Pro_ID=P.Pro_ID and Order_ID=" + txtOrderID.Text + "");
            if(table.Rows.Count<=0)
            {
                MessageBox.Show("Fatura bulunmadı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dgvSearch.DataSource = table;
            decimal total = 0, paied=0, reminder=0;
            for(int i=0; i<dgvSearch.Rows.Count; i++)
            {
                total += Convert.ToDecimal(dgvSearch.Rows[i].Cells[7].Value);
            }
            try
            {
                paied = Convert.ToDecimal(dgvSearch.Rows[0].Cells[9].Value);
                reminder = Convert.ToDecimal(dgvSearch.Rows[0].Cells[10].Value);
                txtTotal.Text = (Math.Round(total, 2)).ToString();
                txtpaied.Text = (Math.Round(paied, 2)).ToString();
                txtReminder.Text = (Math.Round(reminder, 2)).ToString();
                txtName.Text = (dgvSearch.Rows[0].Cells[1].Value).ToString();           
            }
            catch(Exception)
            {   }
         
           
        }
        //satıcıya siparişimizi iade etmek istersek
        private void BuyReturn()
        {
            table.Clear();
            table = db.ReadData("SELECT Order_ID as'Fatura No',S.Sup_Name as'Satıcı Adı',P.Pro_Name as'Ürün Adı',Date as'Fatura Tarihi',BD.Qty as'Miktar',Price as'Fiyat',Discount as'indirim',Total as'Toplam',Total_Order as'Fatura Tutarı',Paied as'Ödenen Miktar',Remainder as'Kalan Miktar',User_Name as'Kasiyer'FROM Buy_Details BD,Suppliers  S,Products P where BD.Sup_ID=S.Sup_ID and BD.Pro_ID=P.Pro_ID and Order_ID=" + txtOrderID.Text + "");
            if (table.Rows.Count <= 0)
            {
                MessageBox.Show("Fatura bulunmadı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dgvSearch.DataSource = table;
            decimal total = 0, paied = 0, reminder = 0;
            for (int i = 0; i < dgvSearch.Rows.Count; i++)
            {
                total += Convert.ToDecimal(dgvSearch.Rows[i].Cells[7].Value);
            }
            try
            {               
                 paied = Convert.ToDecimal(dgvSearch.Rows[0].Cells[9].Value);
                 reminder = Convert.ToDecimal(dgvSearch.Rows[0].Cells[10].Value);
                 txtTotal.Text = (Math.Round(total, 2)).ToString();
                 txtpaied.Text = (Math.Round(paied, 2)).ToString();
                 txtReminder.Text = (Math.Round(reminder, 2)).ToString();
                 txtName.Text = (dgvSearch.Rows[0].Cells[1].Value).ToString();                 
            }
            catch (Exception)
            { }

        }
        private void txtOrderID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                if(txtOrderID.Text=="")
                {
                    MessageBox.Show("lütfen fatura numarasını giriniz", "Uayarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(rbtnSales.Checked==true)
                {
                    SalesReturn();
                }
                else if(rbtnBuy.Checked==true)
                {
                    BuyReturn();
                }
            }
        }

        private void rbtnSales_CheckedChanged(object sender, EventArgs e)
        {
            ResetSupCust();
        }

        private void rbtnBuy_CheckedChanged(object sender, EventArgs e)
        {
            ResetSupCust();
        }
        private void ResetSupCust()
        {
            if (rbtnSales.Checked == true)
            {
                lblName.Text = "Müşteri Adı:";
            }
            else if (rbtnBuy.Checked == true)
            {
                lblName.Text = "Satıcı Adı:";
            }
        }
        //müşterinin faturasını tam olarak iade et.
        private void returnAllSales()
        {
            if(txtName.Text=="")
            {
                MessageBox.Show("lütfen Müşteri adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            db.ExecuteData("delete from Sales where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "");
            db.ExecuteData("delete from Sales_Details where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "");
            db.ExecuteData("delete from Sales_profit where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "");
            string d = dtpReturnDate.Value.ToString("dd/MM/yyyy");
            db.ExecuteData("insert into Returns (Order_Date,Order_Type) values ('"+d+"','satış')","");
            int id = 1;
            try
            {
                id = Convert.ToInt32(db.ReadData("select max(Order_ID) from Returns").Rows[0][0]);//iade işleminin numarasını getir.
            }
            catch(Exception)
            {  }
            for(int i=0; i<dgvSearch.Rows.Count; i++)//iade işlemi detayları veritabanına kaydet.
            {
                db.ExecuteData("insert into Returns_Details values ("+id+",'','"+txtName.Text+"','"+dgvSearch.Rows[i].Cells[2].Value+"','"+d+"',"+ dgvSearch.Rows[i].Cells[4].Value + ","+ dgvSearch.Rows[i].Cells[5].Value + ","+ dgvSearch.Rows[i].Cells[7].Value + ",'"+Properties.Settings.Default.USERNAME+"',"+txtTotal.Text+","+txtpaied.Text+","+txtReminder.Text+")", "");
                int Pro_ID = 1;
                Pro_ID=Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='"+dgvSearch.Rows[i].Cells[2].Value+"'"));//iadde edilen ürün numarası getir.
                db.ExecuteData("update Products set Qty=Qty+"+dgvSearch.Rows[i].Cells[4].Value+ "where Pro_ID="+ Pro_ID + " ", "");//müşteriden iade edile ürün miktarı mevcut miktara ekle.

            }
            MessageBox.Show("Sipariş iade edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            table.Clear();
            dgvSearch.DataSource = table;
            txtOrderID.Clear();
            txtName.Clear();
            txtTotal.Clear();
            txtpaied.Clear();
            txtReminder.Clear();
            txtOrderID.Focus();
        }
        //satıcının faturasını tam olarak iade et.
        private void returnAllBuy()
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("lütfen Satıcı adını giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            db.ExecuteData("delete from Buy where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "");
            db.ExecuteData("delete from Buy_Details where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + "", "");
            string d = dtpReturnDate.Value.ToString("dd/MM/yyyy");
            db.ExecuteData("insert into Returns (Order_Date,Order_Type) values ('" + d + "','satın alma')", "");
            int id = 1;
            try
            {
                id = Convert.ToInt32(db.ReadData("select max(Order_ID) from Returns").Rows[0][0]);//iade işleminin numarasını getir.
            }
            catch (Exception)
            {   }
            for (int i = 0; i < dgvSearch.Rows.Count; i++)//iade işlemi detayları veritabanına kaydet.
            {
                db.ExecuteData("insert into Returns_Details values (" + id + ",'"+txtName.Text+"','','" + dgvSearch.Rows[i].Cells[2].Value + "','" + d + "'," + dgvSearch.Rows[i].Cells[4].Value + "," + dgvSearch.Rows[i].Cells[5].Value + "," + dgvSearch.Rows[i].Cells[7].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + txtTotal.Text + "," + txtpaied.Text + "," + txtReminder.Text + ")", "");
                int Pro_ID = 1;
                Pro_ID = Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='" + dgvSearch.Rows[i].Cells[2].Value + "'").Rows[0][0]);//iadde edilen ürün numarası getir.
                db.ExecuteData("update Products set Qty=Qty-" + dgvSearch.Rows[i].Cells[4].Value + "where Pro_ID=" + Pro_ID + " ", "");//satıcıya iade edilen ürün miktarı mevcut miktara çıkar.

            }
            MessageBox.Show("Sipariş iade edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            table.Clear();
            dgvSearch.DataSource = table;
            txtOrderID.Clear();
            txtName.Clear();
            txtTotal.Clear();
            txtpaied.Clear();
            txtReminder.Clear();
            txtOrderID.Focus();
        }
        //satın alma veya  satış faturası iade.
        private void btnReturnAll_Click(object sender, EventArgs e)
        {
            if(dgvSearch.Rows.Count>=1)
            {
                if(rbtnSales.Checked==true)
                {
                    returnAllSales();
                }
                else if(rbtnBuy.Checked==true)
                {
                    returnAllBuy();
                }
            }
        }
        //faturadan sadece bir ürün iade(satış)
        private void returnitemsaleonly()
        {
            int Pro_ID = 1;
            Pro_ID = Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='" + dgvSearch.CurrentRow.Cells[2].Value + "'").Rows[0][0]);//iade edilecek ürünün numarasını getir.
            db.ExecuteData("delete from Sales_Details where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Pro_ID="+Pro_ID+" and Qty=" + dgvSearch.CurrentRow.Cells[4].Value + " and Price="+ dgvSearch.CurrentRow.Cells[5].Value + "", "");
            db.ExecuteData("delete from Sales_profit where  Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Pro_ID=" + Pro_ID + " and Qty=" + dgvSearch.CurrentRow.Cells[4].Value + " and Price=" + dgvSearch.CurrentRow.Cells[5].Value + "", "");
            string d = dtpReturnDate.Value.ToString("dd/MM/yyyy");
            db.ExecuteData("insert into Returns (Order_Date,Order_Type) values ('" + d + "','satış')", "");
            int id = 1;
            try
            {
                id = Convert.ToInt32(db.ReadData("select max(Order_ID) from Returns").Rows[0][0]);//iade işleminin numarasını getir.
            }
            catch (Exception)
            { }                      
            db.ExecuteData("insert into Returns_Details values (" + id + ",'','" + txtName.Text + "','" + dgvSearch.CurrentRow.Cells[2].Value + "','" + d + "'," + dgvSearch.CurrentRow.Cells[4].Value + "," + dgvSearch.CurrentRow.Cells[5].Value + "," + dgvSearch.CurrentRow.Cells[7].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + txtTotal.Text + "," + txtpaied.Text + "," + txtReminder.Text + ")", "");//iade işlemi detayları veritabanına kaydet.
            db.ExecuteData("update Products set Qty=Qty+" + dgvSearch.CurrentRow.Cells[4].Value + "where Pro_ID=" + Pro_ID + " ", "");//müşteriden iade edile ürün miktarı mevcut miktara ekle.            
            MessageBox.Show("seçilen ürün iade edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            table.Clear();
            dgvSearch.DataSource = table;
            txtOrderID.Clear();
            txtName.Clear();
            txtTotal.Clear();
            txtpaied.Clear();
            txtReminder.Clear();
            txtOrderID.Focus();
        }

        //faturadan sadece bir ürün iade(satın alma)
        private void returnitemBuyonly()
        {
            int Pro_ID = 1;
            Pro_ID = Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='" + dgvSearch.CurrentRow.Cells[2].Value + "'").Rows[0][0]);//iade edilecek ürünün numarasını getir.
            db.ExecuteData("delete from Buy_Details where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Pro_ID=" + Pro_ID + " and Qty=" + dgvSearch.CurrentRow.Cells[4].Value + " and Price=" + dgvSearch.CurrentRow.Cells[5].Value + "", "");
            string d = dtpReturnDate.Value.ToString("dd/MM/yyyy");
            db.ExecuteData("insert into Returns (Order_Date,Order_Type) values ('" + d + "','satın alma')", "");
            int id = 1;
            try
            {
                id = Convert.ToInt32(db.ReadData("select max(Order_ID) from Returns").Rows[0][0]);//iade işleminin numarasını getir.
            }
            catch (Exception)
            { }
            db.ExecuteData("insert into Returns_Details values (" + id + ",'" + txtName.Text + "','','" + dgvSearch.CurrentRow.Cells[2].Value + "','" + d + "'," + dgvSearch.CurrentRow.Cells[4].Value + "," + dgvSearch.CurrentRow.Cells[5].Value + "," + dgvSearch.CurrentRow.Cells[7].Value + ",'123'," + txtTotal.Text + "," + txtpaied.Text + "," + txtReminder.Text + ")", "");//iade işlemi detayları veritabanına kaydet.
            int ID = 1;
            ID = Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='" + dgvSearch.CurrentRow.Cells[2].Value + "'").Rows[0][0]);
            db.ExecuteData("update Products set Qty=Qty-" + dgvSearch.CurrentRow.Cells[4].Value + "where Pro_ID=" + Pro_ID + " ", "");//satıcıya iade edilen ürün miktarı mevcut miktara çıkar.  
            MessageBox.Show("seçilen ürün iade edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            table.Clear();
            dgvSearch.DataSource = table;
            txtOrderID.Clear();
            txtName.Clear();
            txtTotal.Clear();
            txtpaied.Clear();
            txtReminder.Clear();
            txtOrderID.Focus();
        }

        //müşteri ürün tam olarak değil de üründen bir kaç adet iade etmek istenince
        private void returnQtysaleonly()
        {
            int Pro_ID = 1;
            Pro_ID = Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='" + dgvSearch.CurrentRow.Cells[2].Value + "'").Rows[0][0]);
            if(nudQty.Value>=Convert.ToDecimal(dgvSearch.CurrentRow.Cells[4].Value))
            {
                MessageBox.Show("iade etmek istediğiniz miktar sattığnız miktar daha büyüktür", "Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            db.ExecuteData("update Sales_Details set Qty=Qty-"+nudQty.Value+" where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Pro_ID=" + Pro_ID + " and Qty=" + dgvSearch.CurrentRow.Cells[4].Value + " and Price=" + dgvSearch.CurrentRow.Cells[5].Value + "", "");
            db.ExecuteData("update Sales_profit set Qty=Qty-" + nudQty.Value + " where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Pro_ID=" + Pro_ID + " and Qty=" + dgvSearch.CurrentRow.Cells[4].Value + " and Price=" + dgvSearch.CurrentRow.Cells[5].Value + "", "");
            string d = dtpReturnDate.Value.ToString("dd/MM/yyyy");
            db.ExecuteData("insert into Returns (Order_Date,Order_Type) values ('" + d + "','satış')", "");
            int id = 1;
            try
            {
                id = Convert.ToInt32(db.ReadData("select max(Order_ID) from Returns").Rows[0][0]);
            }
            catch (Exception)
            {  }
            db.ExecuteData("insert into Returns_Details values (" + id + ",'','" + txtName.Text + "','" + dgvSearch.CurrentRow.Cells[2].Value + "','" + d + "'," + nudQty.Value + "," + dgvSearch.CurrentRow.Cells[5].Value + "," + dgvSearch.CurrentRow.Cells[7].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + txtTotal.Text + "," + txtpaied.Text + "," + txtReminder.Text + ")", "");
            db.ExecuteData("update Products set Qty=Qty+" + nudQty.Value + "where Pro_ID=" + Pro_ID + " ", "");
            MessageBox.Show("girdiğiniz miktar iade edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            table.Clear();
            dgvSearch.DataSource = table;
            txtOrderID.Clear();
            txtName.Clear();
            txtTotal.Clear();
            txtpaied.Clear();
            txtReminder.Clear();
            txtOrderID.Focus();
        }
        //satucıya ürün tam olarak değil de bir kaç adet iade etmek istenince
        private void returnQtyBuyonly()
        {
            int Pro_ID = 1;
            Pro_ID = Convert.ToInt32(db.ReadData("select Pro_ID from Products where Pro_Name='" + dgvSearch.CurrentRow.Cells[2].Value + "'").Rows[0][0]);
            if (nudQty.Value >= Convert.ToDecimal(dgvSearch.CurrentRow.Cells[4].Value))
            {
                MessageBox.Show("iade etmek istediğiniz miktar satın aldığınız miktar daha büyüktür", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            db.ExecuteData("update Buy_Details set Qty=Qty-" + nudQty.Value + " where Order_ID=" + dgvSearch.CurrentRow.Cells[0].Value + " and Pro_ID=" + Pro_ID + " and Qty=" + dgvSearch.CurrentRow.Cells[4].Value + " and Price=" + dgvSearch.CurrentRow.Cells[5].Value + "", "");
            string d = dtpReturnDate.Value.ToString("dd/MM/yyyy");
            db.ExecuteData("insert into Returns (Order_Date,Order_Type) values ('" + d + "','satın alma')", "");
            int id = 1;
            try
            {
                id = Convert.ToInt32(db.ReadData("select max(Order_ID) from Returns").Rows[0][0]);
            }
            catch (Exception)
            {   }
            db.ExecuteData("insert into Returns_Details values (" + id + ",'','" + txtName.Text + "','" + dgvSearch.CurrentRow.Cells[2].Value + "','" + d + "'," + nudQty.Value + "," + dgvSearch.CurrentRow.Cells[5].Value + "," + dgvSearch.CurrentRow.Cells[7].Value + ",'"+Properties.Settings.Default.USERNAME+"'," + txtTotal.Text + "," + txtpaied.Text + "," + txtReminder.Text + ")", "");      
            db.ExecuteData("update Products set Qty=Qty-" + nudQty.Value + "where Pro_ID=" + Pro_ID + " ", "");
            MessageBox.Show("girdiğiniz miktar iade edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            table.Clear();
            dgvSearch.DataSource = table;
            txtOrderID.Clear();
            txtName.Clear();
            txtTotal.Clear();
            txtpaied.Clear();
            txtReminder.Clear();
            txtOrderID.Focus();
        }
        private void btnReturnitemOnly_Click(object sender, EventArgs e)
        {
            if (dgvSearch.Rows.Count >= 1)
            {
                if (rbtnReturnitem.Checked == true)//bir ürün tam olarak iade. 
                {
                    if(rbtnSales.Checked==true)//satış
                    {
                        returnitemsaleonly();
                    }
                    else if(rbtnBuy.Checked==true)//satın alma
                    {
                        returnitemBuyonly();
                    }
                }
                else if(rbtnReturnQtyOnly.Checked==true)//bir üründen belirli bir miktar iade.
                {
                    if (rbtnSales.Checked == true)//satış.
                    {
                        returnQtysaleonly();
                    }
                    else if (rbtnBuy.Checked == true)//satın alma.
                    {
                        returnQtyBuyonly();
                    }
                } 
            }
        }
    }
}