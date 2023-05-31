using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    public partial class AddUser : Form
    {
        public AddUser()
        {
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
            string sql = $"insert into UserData(email, firstname,lastname,office,birthdate, password,role,state) values ('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{comboBox1.Text}', '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}','{textBox5.Text}', 'User', 1)";
            Data.Request(sql);
        }
    }
}
