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
    public partial class DetailCheck : Form
    {

        public Check check; //Объект несоответствия
        public JournalDBMainForm owner;

        private const string eventsQuery = "SELECT * FROM Events_View WHERE id = @id"; //Выбор мероприятия для детализации
        private const string insertShowQuery = "INSERT INTO shows (check_id, date, worker_id) VALUES (@check_id, @date, @worker_id)"; //Добавление записи об ознакомлении
        private const string bossShowQuery = "SELECT boss FROM grant_show WHERE subunit=@subunit AND sector=@sector"; //Выборка начальников проверенного участка

        public DetailCheck()
        {
            InitializeComponent();
        }

        private void DetailCheck_Shown(object sender, EventArgs e)
            /*
            Отображение формы
            Заполенение данными полей
            Инициализация данных об ознакомлениях и мероприятиях
            Проверка прав доступа для ознакомления
            */
        {
            //Заполнение данными полей
            checkNumTxt.Text = check.failCount;
            regDateTxt.Text = check.regDate.ToString("dd.MM.yyyy HH:mm");
            checkSubunitTxt.Text = check.regWorkerSubunit;
            regWorkerTxt.Text = check.regWorker;
            checkDateTxt.Text = check.checkDate.ToShortDateString();
            checkWorkerTxt.Text = check.checkWorker;
            subjSubunitTxt.Text = check.checkSubunit;
            TDKDTxt.Text = check.td_kd;
            controlIndicatorTxt.Text = check.controlIndicator;
            failDescrTxt.Text = check.failDescr;
            sectorTxt.Text = check.sector;

            //Заполнение таблицы мероприятий
            if (check.eventsDataSet != null)
            {
                eventsGridView.DataSource = check.eventsDataSet.Tables[0].DefaultView;

                eventsGridView.Columns[0].Visible = false;
                eventsGridView.Columns[1].Visible = false;
                eventsGridView.Columns[2].Visible = false;
                eventsGridView.Columns[3].Visible = false;
                eventsGridView.Columns[4].Visible = false;
                eventsGridView.Columns[5].Visible = false;
                eventsGridView.Columns[6].HeaderText = "Причина несоответствия";
                eventsGridView.Columns[7].HeaderText = "Описание мероприятия";
                eventsGridView.Columns[8].HeaderText = "Ответственный";
                eventsGridView.Columns[9].HeaderText = "Срок исполнения";
                eventsGridView.Columns[10].Visible = false;
                eventsGridView.Columns[11].Visible = false;
                eventsGridView.Columns[12].Visible = false;
                eventsGridView.Columns[13].HeaderText = "ФИО разработчика";
                eventsGridView.Columns[13].DisplayIndex = 0;
                eventsGridView.Columns[14].HeaderText = "Отчет";
                eventsGridView.Columns[15].HeaderText = "Подтверждающая информация";
                eventsGridView.Columns[16].Visible = false;
                eventsGridView.Columns[17].Visible = false;
                eventsGridView.Columns[18].Visible = false;
                eventsGridView.Columns[19].Visible = false;
                eventsGridView.Columns[20].Visible = false;
                eventsGridView.Columns[21].Visible = false;
                eventsGridView.Columns[22].Visible = false;
                eventsGridView.Columns[23].Visible = false;
                eventsGridView.Columns[24].Visible = false;
                eventsGridView.Columns[25].Visible = false;
                eventsGridView.Columns[26].Visible = false;
                eventsGridView.Columns[27].Visible = false;
                eventsGridView.Columns[28].Visible = false;
                eventsGridView.Columns[29].Visible = false;
            }

            //Заполнение таблицы ознакомлений
            if (check.showsDataAdapter != null)
            {
                DataSet showsDataSet = new DataSet();
                check.showsDataAdapter.Fill(showsDataSet);

                showsGridView.DataSource = showsDataSet.Tables[0].DefaultView;

                showsGridView.Columns[0].Visible = false;
                showsGridView.Columns[1].Visible = false;
                showsGridView.Columns[2].HeaderText = "Дата ознакомления";
                showsGridView.Columns[3].HeaderText = "ФИО сотрудника";
                showsGridView.Columns[4].HeaderText = "Должность";
                showsGridView.Columns[5].HeaderText = "Подразделение";
                showsGridView.Columns[6].Visible = false;

                //Если данный пользователь уже ознакомился, кнопка "Ознакомиться" блокируется
                foreach (DataGridViewRow row in showsGridView.Rows)
                {
                    if ((int)row.Cells[6].Value == owner.currentUserId)
                    {
                        acceptShowBtn.Enabled = false;
                        break;
                    }
                }
            }

            //Если нет прав на ознакомление
            if (!owner.currentUserAccess.Contains("SUBSHOW"))
                acceptShowBtn.Enabled = false;

            if (acceptShowBtn.Enabled == true)
            {
                acceptShowBtn.Enabled = false;
                SqlCommand bossShowCommand = owner.conn.CreateCommand();
                bossShowCommand.CommandText = bossShowQuery;
                bossShowCommand.Parameters.AddWithValue("@subunit", check.checkSubunit);
                bossShowCommand.Parameters.AddWithValue("@sector", check.sector);

                //Получение списка начальников
                SqlDataReader reader = bossShowCommand.ExecuteReader();
                List<int> bossIds = new List<int>();
                while (reader.Read())
                    bossIds.Add((int)reader[0]);
                reader.Close();

                //Если текущий пользователь является начальником
                if (bossIds.Contains(owner.currentUserId)) acceptShowBtn.Enabled = true;

                if (acceptShowBtn.Enabled != true)
                    foreach (DataGridViewRow row in showsGridView.Rows)
                    {
                        if (bossIds.Contains((int)row.Cells[6].Value))
                            acceptShowBtn.Enabled = true;
                    }
            }
        }

        private void acceptShowBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Ознакомиться"
            Запрос подтверждения у пользователя
            Добавление записи в таблицу
            */
        {
            if (MessageBox.Show("Ознакомиться с несоответствием от имени пользователя " + owner.currectUserName + "?", "Предупреждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            
            SqlCommand insertShowCommand = owner.conn.CreateCommand();
            insertShowCommand.CommandText = insertShowQuery;
            insertShowCommand.Parameters.AddWithValue("@check_id", check.id);
            insertShowCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss.fff"));
            insertShowCommand.Parameters.AddWithValue("@worker_id", owner.currentUserId);

            insertShowCommand.ExecuteNonQuery();

            DataSet showsDataSet = new DataSet();
            check.showsDataAdapter.Fill(showsDataSet);
            showsGridView.DataSource = showsDataSet.Tables[0].DefaultView;

            acceptShowBtn.Enabled = false;
        }

        private void closeBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Отмена"
            */
        {
            Close();
        }

        private void eventsGridView_DoubleClick(object sender, EventArgs e)
            /*
            Двойное нажатие по мероприятию
            Проверка прав доступа пользователя
            Получение данных о мероприятии
            Открытие формы детализации мероприятия
            */
        {
            if (eventsGridView.SelectedRows.Count != 1) return;

            if (!owner.currentUserAccess.Contains("EVENT_DETAIL") && !owner.currentUserAccess.Contains("CONTROL") && !owner.currentUserAccess.Contains("ADMIN")) return;

            int id = (int)eventsGridView.SelectedRows[0].Cells[0].Value;

            SqlCommand command = owner.conn.CreateCommand();
            command.CommandText = eventsQuery;
            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            Event eventt = new Event(reader);

            reader.Close();

            DetailEvent detailEventForm = new DetailEvent();
            detailEventForm.eventt = eventt;
            detailEventForm.owner = owner;
            detailEventForm.ShowDialog();
        }
    }
}
