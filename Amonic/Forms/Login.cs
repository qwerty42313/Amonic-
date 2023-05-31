using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    public partial class Login : Form
    {
        private MySqlConnection connect;
        private int i = 0; int idStatic;
        private static DataTable dt;
        private static MySqlDataAdapter adapter;
        private static MySqlCommand command;
        public Login()
        {
            string sql = "Server=localhost;database=DataAmonic;uid=root;pwd=2003955eee;";    // срока подключения к базе
            connect = new MySqlConnection(sql);
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);         // метод вызывается когда приложения закрывается
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (i < 3)
            {
                
                command = new MySqlCommand($"select * from UserData where email='{textBox1.Text}' and password='{textBox2.Text}' and role='{comboBox1.Text}';", connect);
                adapter = new MySqlDataAdapter(); adapter.SelectCommand = command;
                dt = new DataTable();  adapter.Fill(dt);
                if (dt.Rows.Count > 0)                                                       // проверяю логин и пароль а так же правильно ли указана роль
                {
                    command = new MySqlCommand($"select * from UserData where email='{textBox1.Text}' and password='{textBox2.Text}' and state=1;", connect);
                    adapter = new MySqlDataAdapter(); adapter.SelectCommand = command;
                    dt = new DataTable(); adapter.Fill(dt);
                    if (dt.Rows.Count > 0 && comboBox1.Text == "Admin")                      // проверяю статус аккаунта
                    {
                        getIdUser(0);
                        Admin admin = new Admin();
                        admin.ShowDialog();
                    }
                    else if (dt.Rows.Count > 0 && comboBox1.Text == "User")
                    {
                        getIdUser(0);
                        User user = new User();
                        user.ShowDialog();
                    }
                    else
                        MessageBox.Show("Your account is not valid, contact the administration"); // отправляет сообщение о том что аккаунту запрещено заходить
                }
                else
                    MessageBox.Show("Incorrect login or password"); i++;                     //отправляет сообщение о том что логин или пароль введены непралвильно
            }
            else
            {
                getBlock();
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
               Data.GetData(idStatic, 1);                                                   // вызываю метода для записи в бд когда пользователь вышел из приложения
            }
            catch
            {

            }
        }
        private async void getBlock()                                                        // метод который блокирует часть интерфейса при неудачном вводе данных
        {
            label3.Text = "Error login";
            Application.DoEvents();
            textBox1.Enabled = false; button1.Enabled = false; comboBox1.Enabled = false; textBox2.Enabled = false;
            await Task.Delay(10000);                                                         // блокировка интерфейса на 10 секунд
            textBox1.Enabled = true; button1.Enabled = true; comboBox1.Enabled = true; textBox2.Enabled = true;
            label3.Text = "";
            i = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();                                                              // выход их приложения
        }
        private void getIdUser(int point)
        {
            for(int id = 0; id < 1000; id++)
            {
                try
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand command = new MySqlCommand($"select * from UserData where email='{textBox1.Text}' and password='{textBox2.Text}' and id={id}", connect);
                    adapter.SelectCommand = command;
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)                                                   //поиск айди юзера который входи в систему
                    {
                        idStatic = id;
                        Data.GetData(id, 0);                                                 //передает в метод айди пользователя что бы записать дату и время входа
                        break; 
                    }    
                }
                catch
                {
                    continue;
                }
            };
        }
    }
}
































































































                                                                                                                                                                                                                      //Creator Жаров Константин В012б
