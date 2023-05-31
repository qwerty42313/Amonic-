using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Amonic
{
    internal class Data
    {
        /// ////////////////////////User
        static MySqlConnection connection = new MySqlConnection("Server=localhost;database=DataAmonic;uid=root;pwd=2003955eee;");
        public static int id; 
        public static List<string> office = new List<string>();
        public static List<string> dataColums = new List<string>();
        
        /////////////////////////// Schedules
        public static List<string> columSchedules = new List<string>();

        public static void Request(string sql)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();                                                //   выполняю запрос
                MessageBox.Show("Done");
                connection.Close();
            }
            catch
            {
                MessageBox.Show("incorrect data entereda");                              //Сообщаю пользателю о ошибки
            }
        }
        public static void GetData(int id, int i)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (i == 0)
            {
                MySqlCommand commad = new MySqlCommand($"update UserData set active='{date}' where id={id}", connection);
                connection.Open(); commad.ExecuteNonQuery(); connection.Close();
            }
            else if (i == 1)
            {
                MySqlCommand commad = new MySqlCommand($"update UserData set lastactive='{date}' where id={id}", connection);
                connection.Open(); commad.ExecuteNonQuery(); connection.Close();
            }
        }
    }
}
