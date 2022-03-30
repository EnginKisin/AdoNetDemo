using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDemo
{
    public class ProductDal
    {
        //connection'u metotlarin disinda tanimladigimiz icin basina alt cizgi koyduk.
        SqlConnection _connection = new SqlConnection(@"Server = (localdb)\mssqllocaldb;initial catalog=ETrade;integrated security=true");
        public List<Product> GetAll()
        {
            //@'in anlami burdaki her seyi string olarak kabul et.
            //initial catalog= hangi veritabanina baglanicagini soyler.
            //integrated security = veritabanina  Windows Authentication'ile (windows kimlik dogrulamasi) baglan demek.
            //uzaktaki bir veritabanina baglanmak icin=>  integrated security=false;uid=engin;password=12345 seklinde yazabiliriz.
            //baglanti nesnesini olusturduk.


            //baglanti kapaliysa ac.
            ConnectionControl();

            SqlCommand command = new SqlCommand("Select * from Products", _connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Product> products = new List<Product>();

            //reader dan gelen kayıtları tek tek oku.Okuya bildigin surecede dongunun icine calistir.
            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    StockAmount = Convert.ToInt32(reader["StockAmount"]),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
                };
                products.Add(product);
            }

            reader.Close();
            _connection.Close();
            return products;
        }

        private void ConnectionControl()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }


        public DataTable GetAll2()
        {
            //@'in anlami burdaki her seyi string olarak kabul et.
            //initial catalog= hangi veritabanina baglanicagini soyler.
            //integrated security = veritabanina  Windows Authentication'ile (windows kimlik dogrulamasi) baglan demek.
            //uzaktaki bir veritabanina baglanmak icin=>  integrated security=false;uid=engin;password=12345 seklinde yazabiliriz.
            //baglanti nesnesini olusturduk.


            //baglanti kapaliysa ac.
            //ConnectionState enum sabitidir.
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            SqlCommand command = new SqlCommand("Select * from Products", _connection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            _connection.Close();
            return dataTable;
        }

        public void Add(Product product)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Insert into Products Values(@name,@unitPrice,@stockAmount)", _connection);
            //string kullanarak da yapilabilinir ama SQL Injection saldirilari icin parametre kullanmak önemli.
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@stockAmount", product.StockAmount);

            //etkilenen kayit sayisini dondurur.kayit oldumu olmadımı diye kontrol yapmak icin kullanabiliriz.
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void Update(Product product)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Update Products set Name=@name, UnitPrice=@unitPrice, StockAmount=@stockAmount where Id=@id", _connection);
            //where kosulu yazmadan guncelleme! yoksa tum kayitlari gunceller.
            //string kullanarak da yapilabilinir ama SQL Injection saldirilari icin parametre kullanmak önemli.
            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            command.Parameters.AddWithValue("@id", product.Id);

            //etkilenen kayit sayisini dondurur.kayit oldumu olmadımı diye kontrol yapmak icin kullanabiliriz.
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void Delete(int id)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand(
                   "Delete from Products where Id=@id", _connection);

            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            _connection.Close();
        }

    }
}
