using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace MSSQL
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlCon = null;

        private SqlConnection nrthwndCon = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DBLink"].ConnectionString);
            sqlCon.Open();

            //if (sqlCon.State == ConnectionState.Open)
            //{
            //    MessageBox.Show("Connection set");
            //}

            nrthwndCon = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthWindDB"].ConnectionString);
            nrthwndCon.Open();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SqlCommand command = new SqlCommand($"INSERT INTO [Students] (Name, Surname, Birthday) VALUES (N'{textBox1.Text}', N'{textBox2.Text}', '{textBox3.Text}')",
            //   sqlCon);

            SqlCommand command = new SqlCommand(
                $"INSERT INTO [Students] (Name, Surname, Birthday, Birth_place, Phone, Email) VALUES (@Name, @Surname, @Birthday, @Birth_place, @Phone, @Email)",
                sqlCon);



            //DateTime date = DateTime.Parse(textBox3.Text);
            // command.Parameters.AddWithValue("Birthday", $"{date.Month}/{date.Day}/{date.Year}");

            DateTimePicker dateTimePicker = new DateTimePicker();

            command.Parameters.AddWithValue("Name", textBox1.Text);
            command.Parameters.AddWithValue("Surname", textBox2.Text);
            command.Parameters.AddWithValue("Birthday", dateTimePicker.Value);
            command.Parameters.AddWithValue("Birth_place", textBox4.Text);
            command.Parameters.AddWithValue("Phone", textBox5.Text);
            command.Parameters.AddWithValue("Email", textBox6.Text);

            MessageBox.Show(command.ExecuteNonQuery().ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(textBox7.Text, nrthwndCon);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader dataReader = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products",
                    nrthwndCon);

                dataReader = sqlCommand.ExecuteReader();

                ListViewItem item = null;

                while(dataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(dataReader["ProductName"]), 
                        Convert.ToString(dataReader["QuantityPerUnit"]), 
                        Convert.ToString(dataReader["UnitPrice"]) });

                    listView1.Items.Add(item);
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }
    }
}
