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
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            try
            {
                textBox3.Text = Data.columSchedules[0];
                textBox1.Text = Data.columSchedules[2];
                label1.Text += Data.columSchedules[3];
                label2.Text += Data.columSchedules[4];                                //Заполняю данные выбранного рейса
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
            int price = Convert.ToInt32(textBox2.Text);                              
            double BPrice = Convert.ToDouble(price * 1.35);                          //рассчитываю стоимость бизнес билета
            double FPrice = Convert.ToDouble(BPrice * 1.3);                          //рассчитываю стоимость бизнес первого класса
            string theDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");           //конвертирую дату по нужный формат
            string sql = $"update Schedules set Date='{theDate}', Time='{textBox1.Text}', price={textBox2.Text}, Business_price={Math.Round(BPrice)}, First_class_price={Math.Round(FPrice)} where id={textBox3.Text};";
            Data.Request(sql);
        }
    }
}
