using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesManagement
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataBase db = new DataBase();
        int User_ID = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                User_ID = Convert.ToInt32(db.ReadData("select * from Users where User_Name='"+Properties.Settings.Default.USERNAME+"'").Rows[0][0]);

            }
            catch (Exception) { }
        }
        private bool CheckUser(string colum,string table)
        {
            DataTable tablesearch = new DataTable();
            tablesearch = db.ReadData("select "+ colum + " from "+ table + " where User_ID=" + User_ID + "");
            if (Convert.ToDecimal(tablesearch.Rows[0][0]) == 0)
            {
                MessageBox.Show("Bu sayfaya erişim hakkınız yoktur", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Customer", "User_Customer"))
            {
                Frm_Customers frm = new Frm_Customers();
                frm.ShowDialog();
            }
            
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Supplier", "User_Supplier"))
            {
                Frm_Suppliers frm = new Frm_Suppliers();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Deserved_Type", "User_Deserved"))
            {
                Frm_DeservedType frm = new Frm_DeservedType();
                frm.ShowDialog();
            }
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Deserved", "User_Deserved"))
            {
                Frm_Deserved frm = new Frm_Deserved();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Deserved_Report", "User_Deserved"))
            {
                Frm_deservedReport frm = new Frm_deservedReport();
                frm.ShowDialog();
            }
                
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Add_Pro", "User_Setting"))
            {
                Frm_Products frm = new Frm_Products();
                frm.ShowDialog();
            }            
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Buy", "User_Buy"))
            {
                Frm_Buy frm = new Frm_Buy();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Supplier_Money", "User_Supplier"))
            {
                Frm_SupplierMoney frm = new Frm_SupplierMoney();
                frm.ShowDialog();
            }
              
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Supplier_Report", "User_Supplier"))
            {
                Frm_SuppliersReport frm = new Frm_SuppliersReport();
                frm.ShowDialog();
            }
            
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Buy_Report", "User_Buy"))
            {
                Frm_BuyReport frm = new Frm_BuyReport();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Sale", "User_Sale"))
            {
                Frm_Sales frm = new Frm_Sales();
                frm.ShowDialog();
            }
              
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Customer_Money", "User_Customer"))
            {
                Frm_CustomerMoney frm = new Frm_CustomerMoney();
                frm.ShowDialog();
            }
             
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Customer_Report", "User_Customer"))
            {
                Frm_CustomersReport frm = new Frm_CustomersReport();
                frm.ShowDialog();
            }
             
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Sale_Report", "User_Sale"))
            {
                Frm_SalesReport frm = new Frm_SalesReport();
                frm.ShowDialog();
            }
              
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Returnn", "User_Return"))
            {
                Frm_Return frm = new Frm_Return();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Sale_profit", "User_Sale"))
            {
                Frm_SalesProfit frm = new Frm_SalesProfit();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Employee", "User_Employee"))
            {
                Frm_Employee frm = new Frm_Employee();
                frm.ShowDialog();
            }
                
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Employee_Salary", "User_Employee"))
            {
                Frm_EmployeeSalary frm = new Frm_EmployeeSalary();
                frm.ShowDialog();
            }
              
        }

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Employee_Report", "User_Employee"))
            {
                Frm_EmployeeSalaryRrport frm = new Frm_EmployeeSalaryRrport();
                frm.ShowDialog();
            }
               
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void tileItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("User_Permissions", "User_Setting"))
            {
                Frm_Permissions frm = new Frm_Permissions();
                frm.ShowDialog();
            }
        }

        private void tileItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Supplier_Money", "User_Supplier"))
            {
                Frm_SupplierMoney frm = new Frm_SupplierMoney();
                frm.ShowDialog();
            }
        }

        private void tileItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Add_Pro", "User_Setting"))
            {
                Frm_Products frm = new Frm_Products();
                frm.ShowDialog();
            }
        }

        private void tileItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Customer_Money", "User_Customer"))
            {
                Frm_CustomerMoney frm = new Frm_CustomerMoney();
                frm.ShowDialog();
            }
        }

        private void tileItem5_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
        }

        private void tileItem6_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           if( CheckUser("User_Permissions", "User_Setting"))
            {
                Frm_Permissions frm = new Frm_Permissions();
                frm.ShowDialog();
            }    
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Show_Pro", "User_Setting"))
            {
                Frm_ShowProducts frm = new Frm_ShowProducts();
                frm.ShowDialog();
            }
               
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckUser("Return_Report", "User_Return"))
            {
                Frm_ReturnReport frm = new Frm_ReturnReport();
                frm.ShowDialog();
            }
              
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem26_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void tileControl1_Click(object sender, EventArgs e)
        {

        }

        private void tileItem7_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }

        private void tileItem8_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }

        private void tileItem9_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Returnn", "User_Return"))
            {
                Frm_Return frm = new Frm_Return();
                frm.ShowDialog();
            }
        }

        private void tileItem10_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Sale_profit", "User_Sale"))
            {
                Frm_SalesProfit frm = new Frm_SalesProfit();
                frm.ShowDialog();
            }
        }

        private void tileItem8_ItemClick_1(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Deserved", "User_Deserved"))
            {
                Frm_Deserved frm = new Frm_Deserved();
                frm.ShowDialog();
            }
        }

        private void tileItem11_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Employee", "User_Employee"))
            {
                Frm_Employee frm = new Frm_Employee();
                frm.ShowDialog();
            }
        }

        private void tileItem5_ItemClick_1(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Sale", "User_Sale"))
            {
                Frm_Sales frm = new Frm_Sales();
                frm.ShowDialog();
            }
        }

        private void tileItem6_ItemClick_1(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (CheckUser("Buy", "User_Buy"))
            {
                Frm_Buy frm = new Frm_Buy();
                frm.ShowDialog();
            }
        }

      
    }
}
