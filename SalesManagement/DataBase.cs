using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace SalesManagement
{
    //veritabanına bağlanmak ve(insert,delete,update,select)işlemleri yapabilmek için bir class oluştutduk.
    class DataBase
    {
        SqlConnection Conn = new SqlConnection("server=DESKTOP-1SC5SJI; database=Sales_System; Integrated Security=True");
        SqlCommand Cmd = new SqlCommand();
        //veritabanındaki verileri okuyacaktır(select statement)
        //ReadData fonkisyonu istediğimiz verileri tablo olarak (datatable) döndürecektir. 
        public DataTable ReadData(String stmt)
        {
            DataTable table = new DataTable();
            try { 
            Cmd.Connection = Conn;
            Cmd.CommandText = stmt;
            Conn.Open();    
            table.Load(Cmd.ExecuteReader());//verileri veri tabanından tabloya yükleyecektir.
            Conn.Close();
            }catch(SqlException exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                //durum ne olursa olsun veritabanı ile bağlanmayı kapat.
                Conn.Close();
            }
            return table;
        }
        //veritabanındaki veriler üzerinde (insert update delete) işlemler yapacaktır.
        public bool ExecuteData(String stmt,String msg)
        {
            try
            {
                Cmd.Connection = Conn;
                Cmd.CommandText = stmt;
                Conn.Open();
                Cmd.ExecuteNonQuery();//parameter olarak gönderilien sql komut uyglayacaktır.
                Conn.Close();
                if(msg!="")
                {
                    MessageBox.Show(msg, "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //yukarıdaki kod doğru bir şekilde çalıştıysa true değerini döndür.
                return true;
            }
            catch(SqlException exp)
            {
                MessageBox.Show(exp.Message);
                //yukarıdaki kod doğru bir şekilde çalışmadıysa false değerini döndür.
                return false;
            }
            finally
            {
                //durum ne olursa olsun veritabanı ile bağlanmayı kapat.
                Conn.Close();
            }
        }
    }
}
