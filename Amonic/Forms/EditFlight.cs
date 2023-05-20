using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Amonic
{
    public partial class EditFlight : Form
    {
        MySqlConnection connection;
        public EditFlight()
        {
            connection = new MySqlConnection("Server=localhost;database=DataAmonic;uid=root;pwd=2003955eeeE_;");
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            try
            {
                textBox3.Text = Data.columSchedules[0];
                textBox1.Text = Data.columSchedules[2];
                label1.Text += Data.columSchedules[3];
                label2.Text += Data.columSchedules[4];
                label3.Text += Data.columSchedules[6];
                textBox2.Text += Data.columSchedules[7];
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int price = Convert.ToInt32(textBox2.Text);
                double BPrice = Convert.ToDouble(price * 1.35);
                double FPrice = Convert.ToDouble(BPrice * 1.3);
                string theDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string sql = $"update Schedules set Date='{theDate}', Time='{textBox1.Text}', price={textBox2.Text}, Business_price={Math.Round(BPrice)}, First_class_price={Math.Round(FPrice)} where id={textBox3.Text};";
                MySqlCommand command = new MySqlCommand(sql, connection);
                connection.Open();
                command.ExecuteNonQuery();                                                //   выполняю запрос
                MessageBox.Show("Done");
                connection.Close();
            }
            catch 
            {
                MessageBox.Show("Enter correct data");
            }
            
        }
    }
}
