using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    public partial class Admin : Form
    {
        MySqlConnection connection;
        private int kod;
        public Admin()
        {
            string sql = "Server=localhost;database=DataAmonic;uid=root;pwd=2003955eeeE_;";   // строка подключения к бд
            connection = new MySqlConnection(sql);
            InitializeComponent();  
            GetInfo(); GetOffice();                                                         // вызывает 2 метода 1 из которых загружает данные из базы в датагридвью(таблицу) и 2й фильтрует оффисы что бы не было повторов 
        }
        private void label1_Click(object sender, EventArgs e)
        {
            AddUser newUser = new AddUser();
            newUser.ShowDialog();                                                          //открываю форму newUser
            GetInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(kod != 0)
            {
                for(int i = 0; i < 6; i++)
                {
                    Data.dataColums.Add(Convert.ToString(dataGridView1[i, kod - 1].Value));  // передаю данные в класс 
                }
            }
            Change_Role Editrole = new Change_Role();
            Editrole.ShowDialog();                                                           //открываю форму role
            GetInfo(); Data.dataColums.Clear();                                              // удаляю данные которые передал в класс
        }
        private void GetInfo()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("select id, firstname, lastname, role, email, office, active, lastactive from UserData", connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];                                         // вывожу данные в датагридвью
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int i = 0; int index = 0;
            foreach (DataGridViewRow r in dataGridView1.Rows)                               //Достаю строки 
            {
                if (i == dataGridView1.RowCount - 1)                                        // что бы последняя пустая строка не была красная
                    break;
                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();
                command = new MySqlCommand($"select * from UserData where id={dataGridView1[0, i].Value} and state=1;", connection);
                adapter.SelectCommand = command;
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)                                                     // проверяю чему равен результат 1 - логин/пароль рабаотают и можно заходить 
                    index = 1;                                                             //  естли же результат 0 то аккаун не действителен
                else
                    index = 0;

                if (index == 1)                                                            // присваиваю цвет в зависимости от результата
                    r.DefaultCellStyle.BackColor = Color.White;                            // присаваю зеленый цвет фона работающим аккаунтам
                else                                                                       
                    r.DefaultCellStyle.BackColor = Color.Red;                              // и красный НЕ рабатающим
                i++;
            }
        }
        private void GetOffice()
        {
            List<string> a = new List<string>();                                            //Создаём лист
                                                                                           
            for (int i = 0; i < dataGridView1.RowCount; i++)                                //цикл для запичи всех офисов
                a.Add(Convert.ToString(dataGridView1[5,i].Value));                          //Добавление в лист строки
                                                                                           
            for (int i = 0; i < a.Count; i++)                                               //Циклы удаления лишних элементов
                for (int j = i + 1; j < a.Count; j++)                                      
                    if (a[i] == a[j])                                                      
                        a.RemoveAt(j);                                                     
            for(int i = 0; i < a.Count; i++)                                               
            {                                                                              
                comboBox1.Items.Add(a[i]);                                                  //Добавляем в комбобокс
                Data.office.Add(a[i]);                                                     
            }                                                                              
        }                                                                                  
                                                                                           
        private void Exit_Click(object sender, EventArgs e)                                
        {                                                                                  
            this.Close();                                                                   //закрыть форму
        }

        private void button2_Click(object sender, EventArgs e)                              // изменяю статус аккаунта
        {
            string sql = "";
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"select * from UserData where id={kod} and state=0;", connection);
            adapter.SelectCommand = command;
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)                                                          // проверяю статус аккаунт 
                sql = $"update UserData set state=1 where id={kod};";
            else if(dt.Rows.Count == 0)
                sql = $"update UserData set state=0 where id={kod};";
            command = new MySqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();                                                      // изменяю статус аккаунта
            connection.Close();
            GetInfo();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)                      //   отслеживаю изменения в выборекомбобокс
        {
            MySqlDataAdapter da = new MySqlDataAdapter($"select id, firstname, lastname, role, email, office, active, lastactive from UserData where office='{comboBox1.Text}'", connection);
            if(comboBox1.Text == "AllOffice")
                da = new MySqlDataAdapter($"select id, firstname, lastname, role, email, office, active, lastactive from UserData", connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];   
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];                       //  отслеживаю какого именно пользователя выбрали
                kod = Convert.ToInt32(row.Cells[0].Value.ToString());                       //  записываю его айди в переменную
                label2.Text = $"Выбран id: {kod}"; Data.id = kod;
            }
            catch
            {
            }
        }

    }
}
