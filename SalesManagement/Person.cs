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
    public  partial class Person : DevExpress.XtraEditors.XtraForm
    {
        private string name;
        private String Phone;
        private String Address;

        public Person(String Name,String Phone,String Address)
        {
            this.name = Name;
            this.Phone = Phone;
            this.Address = Address;
        }

        public Person()
        {
            InitializeComponent();
        }
        public String getset_Name
        {
            get { return name; }
            set { this.name = value; }
        }
        public String getset_Phone
        {
            get { return Phone; }
            set { this.Phone = value; }
        }
        public String getset_Address
        {
            get { return Address; }
            set { this.Address = value; }
        }

        //protected abstract void Insert();
        //protected abstract void Delete();
        //protected abstract void Update();
        //protected abstract void DeleteAll();

    }
}