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

        private List<string[]> rows = new List<string[]>();
        private List<string[]> filteredList = null;


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

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Products", nrthwndCon);
            DataSet dataBase = new DataSet();
            dataAdapter.Fill(dataBase);

            dataGridView2.DataSource = dataBase.Tables[0];


            SqlDataReader dataReader = null;

            string[] row = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products",
                    nrthwndCon);

                dataReader = sqlCommand.ExecuteReader();

                while(dataReader.Read())
                {
                    row = new string[]
                    {
                        Convert.ToString(dataReader["ProductName"]),
                        Convert.ToString(dataReader["QuantityPerUnit"]),
                        Convert.ToString(dataReader["UnitPrice"])
                    };

                    rows.Add(row);
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
            RefreshList(rows);
        }

        private void RefreshList(List<string[]> list)
        {
            listView2.Items.Clear();
            foreach (string[] s in list)
            {
                listView2.Items.Add(new ListViewItem(s));
            }
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

                while (dataReader.Read())
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
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <= 10";
                    break;
                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <= 10 AND UnitsInStock <= 50";
                    break;
                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 50";
                    break;
                case 3:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            filteredList = rows.Where((x) =>
            x[0].ToLower().Contains(textBox9.Text.ToLower())).ToList();

            RefreshList(filteredList);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    filteredList = rows.Where((x) =>
                    double.Parse(x[2]) <= 10).ToList();

                    RefreshList(filteredList);

                    break;
                case 1:
                    filteredList = rows.Where((x) =>
                    double.Parse(x[2]) > 10 && double.Parse(x[2]) <= 100).ToList();

                    RefreshList(filteredList);

                    break;
                case 2:
                    filteredList = rows.Where((x) =>
                    double.Parse(x[2]) > 100).ToList();

                    RefreshList(filteredList);

                    break;
                case 3:

                    RefreshList(filteredList);

                    break;
            }
        }
    }
}
