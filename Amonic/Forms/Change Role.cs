using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    public partial class Change_Role : Form
    {
        MySqlConnection connect;
        public Change_Role()
        {
            string sql = "Server=localhost;database=DataAmonic;uid=root;pwd=2003955eeeE_;";   //строка подключения к бд
            connect = new MySqlConnection(sql);
            InitializeComponent();
            for (int i = 0; i < Data.office.Count; i++)
            {
                comboBox1.Items.Add(Data.office[i]);                                        // заливаю данные в комбобокс с класса
            }
            if(Data.id != 0)
            {
                textBox1.Text = Convert.ToString(Data.id);                                  // присваиваю значения текстбоксам и комбобоксам
                textBox2.Text = Data.dataColums[4];
                textBox3.Text = Data.dataColums[1];
                textBox4.Text = Data.dataColums[2];
                comboBox1.Text = Data.dataColums[5];
                comboBox2.Text = Data.dataColums[3];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();                                                                   //закрываю форму
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.Text == "Admin" || comboBox2.Text == "User")                  //проверяю что бы в роле были только админ или юзер
                {
                    string sql = $"update UserData set email='{textBox2.Text}', firstname='{textBox3.Text}', lastname='{textBox4.Text}', office='{comboBox1.Text}', role='{comboBox2.Text}' where id={textBox1.Text};";
                    MySqlCommand command = new MySqlCommand(sql, connect);
                    connect.Open();                                                         // открываю бд
                    command.ExecuteNonQuery();                                              // выполняю запрос
                    connect.Close();                                                        // закрываю бд
                    MessageBox.Show("Done");
                }
                else
                    MessageBox.Show("Incorrect role");
            }
            catch
            {
                MessageBox.Show("Enter correct data");
            }
            
        }
    }
}
