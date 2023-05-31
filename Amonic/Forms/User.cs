using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    public partial class User : Form
    {
        MySqlConnection connection; int kod; int point;
        public User()
        {
            connection = new MySqlConnection("Server=localhost;database=DataAmonic;uid=root;pwd=2003955eee;");
            InitializeComponent();
            getInfo(); GetReice();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }
        private void GetReice()
        {
            List<string> a = new List<string>();                                            //Создаём лист
            List<string> b = new List<string>();

            for (int i = 0; i < dataGridView1.RowCount; i++)                                //цикл для запичи всех офисов
            {
                a.Add(Convert.ToString(dataGridView1[3, i].Value));                         //Добавление в лист строки
                b.Add(Convert.ToString(dataGridView1[4, i].Value));
            }
            for (int i = 0; i < a.Count; i++)                                               //Циклы удаления лишних элементов
                for (int j = i + 1; j < a.Count; j++)
                    if (a[i] == a[j])
                        a.RemoveAt(j);
            for (int i = 0; i < b.Count; i++)                                               //Циклы удаления лишних элементов
                for (int j = i + 1; j < b.Count; j++)
                    if (a[i] == a[j])
                        b.RemoveAt(j);
            for (int i = 0; i < a.Count; i++)
            {
                comboBox1.Items.Add(a[i]);
                comboBox2.Items.Add(b[i]);                                                  //Добавляем в комбобокс
                Data.office.Add(a[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (kod != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Data.columSchedules.Add(Convert.ToString(dataGridView1[i, kod - 1].Value));  // передаю данные в класс 
                }
            }
            EditFlight flight = new EditFlight();
            flight.ShowDialog();
            getInfo();  Data.columSchedules.Clear();
        }
        private void getInfo()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("select id, Date, Time, From_, To_, Flight_Number, Aircraft, price, Business_price, First_class_price from Schedules", connection);
            DataSet ds = new DataSet(); 
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];                                            // вывожу данные в датагридвью
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string date = DateTime.Now.ToString("dd-MM-yyyy");                                  //сегодняшняя дата
                string theDate = dateTimePicker1.Value.ToString("dd-MM-yyyy");                      //дата которую выбрал пользователь
                string theDate2 = dateTimePicker1.Value.ToString("yyyy-MM-dd");                     //дата которая будет для запроса запроса
                string sql = $"select id, Date, Time, From_, To_, Flight_Number, Aircraft, price, Business_price, First_class_price from Schedules where";
                if (comboBox1.Text != "" && comboBox2.Text != "")
                    sql += $" from_='{comboBox1.Text}' and to_='{comboBox2.Text}'";
                if (textBox1.Text != "")
                    sql += $" and Flight_Number={textBox1.Text}";
                if (theDate != date)
                    sql += $" and Date='{theDate2}'";
                if (textBox2.Text != "")
                    sql += $" and price={textBox2.Text}";
                MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch 
            {
                MessageBox.Show("From and To required to fill in");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];   //  отслеживаю какого именно пользователя выбрали
                kod = Convert.ToInt32(row.Cells[0].Value.ToString());   //  записываю его айди в переменную
                label6.Text = $"Выбран id: {kod}"; Data.id = kod;
            }
            catch
            {
            }
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
                command = new MySqlCommand($"select * from Schedules where id={dataGridView1[0, i].Value} and state=1;", connection);
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

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "";
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"select * from Schedules where id={kod} and state=0;", connection);
            adapter.SelectCommand = command;
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)                                                          // проверяю статус аккаунт 
                sql = $"update Schedules set state=1 where id={kod};";
            else if(dt.Rows.Count == 0)
                sql = $"update Schedules set state=0 where id={kod};";
            command = new MySqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();                                                      // изменяю статус аккаунта
            connection.Close();
            getInfo();
        }
    }
}
