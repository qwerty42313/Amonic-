using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    public partial class AddUser : Form
    {
        MySqlConnection connection;
        public AddUser()
        {
            string sql = $"Server=localhost;database=DataAmonic;uid=root;pwd=2003955eeeE_;";   // строка подключения к бд
            connection = new MySqlConnection(sql);
            InitializeComponent();
            for(int i = 0; i < Data.office.Count; i++)
            {
                comboBox1.Items.Add(Data.office[i]);                                        //   заливаю данные в комбобокс
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();                                                                  //  закрываю форму
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = $"insert into UserData(email, firstname,lastname,office,birthdate, password,role,state) values ('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{comboBox1.Text}', '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}','{textBox5.Text}', 'Users', 1)";
                MySqlCommand command = new MySqlCommand(sql, connection);
                connection.Open();
                command.ExecuteNonQuery();                                                //   выполняю запрос
                MessageBox.Show("Done");
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Enter correct data");                                    //  сообщение о том что данные введене неправильно
            }
        }
    }
}
