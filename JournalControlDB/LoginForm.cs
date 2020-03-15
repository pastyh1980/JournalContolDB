using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace JournalControlDB
{
    public partial class LoginForm : Form
    {
        public JournalDBMainForm owner;
        public string login;
        public int currentUserId;
        public string currentUserSubunit;
        public string[] currentUserAccess;
        public string currectUserName;

		//Текст запроса в БД
        private const string authUserIdQuery = "SELECT id, subunit, access, family + ' ' + SUBSTRING(name, 1, 1) + '.' + SUBSTRING(otch, 1, 1) + '.' AS userName, passwd "
            + "FROM workers WHERE login = @login";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (loginTxt.Text == "") //Проверка заполненности поля Логин на форме
            {
                MessageBox.Show("Поле \"Логин\" не заполнено!", "Ошибка");
                return;
            }

			//Создание команды
            SqlCommand authUserIdCommand = owner.conn.CreateCommand();
            authUserIdCommand.CommandText = authUserIdQuery;
            authUserIdCommand.Parameters.AddWithValue("@login", loginTxt.Text);

			//Выполнение и создание ридера
            SqlDataReader userReader = authUserIdCommand.ExecuteReader();
            
            if (!userReader.Read()) //Считывание первой строки. Если она пустая, пользователя нет
            {
                MessageBox.Show("Пользователь с таким логином не найден в базе данных!", "Ошибка");
                userReader.Close();
                return;
            }

			//Сверка пароля из ридера и пароля из формы
            if (userReader["passwd"].ToString() != passwordTxt.Text)
            {
                MessageBox.Show("Введен неверный пароль!", "Ошибка");
                userReader.Close();
                return;
            }

            login = loginTxt.Text;
            currentUserId = (int)userReader[0]; //АйДи пользователя в БД
            currentUserSubunit = userReader[1].ToString(); //Подразделение пользователя в БД
            currentUserAccess = userReader[2].ToString().Split(';'); //Права доступа пользователя
            currectUserName = userReader[3].ToString(); //ФИО пользователя

            userReader.Close();

            DialogResult = DialogResult.OK;

            MessageBox.Show("Вы вошли как пользователь " + currectUserName, "Информация");

            Close();
        }

        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                loginBtn.PerformClick();
        }
    }
}
