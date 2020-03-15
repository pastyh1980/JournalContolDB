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
    public partial class CheckSelect : Form
        /*
        Форма для выбора определенного несоответствия из таблицы
        */
    {

        public int checkId; //Айди несоответствия
        public string checkNum; //Номер несоответствия
        public string checkResult;//Результат контроля
        public string controlIndicator;//Объект контроля
        public JournalDBMainForm owner;

        //Запрос на выборку проверок с несоответствием, с которым уже ознакомился начальник проверяемого подразделения
        private const string checkQuery = "SELECT * FROM Check_View WHERE Check_View.isActive = 1 " +
            "AND Check_View.isCorrect = 1 AND Check_View.isFail = 1 AND (SELECT count(id) FROM shows WHERE shows.check_id = Check_View.id) <> 0";


        public CheckSelect()
        {
            InitializeComponent();
        }

        private void CheckSelect_Shown(object sender, EventArgs e)
            /*
            Отображение формы
            Формирование данных для таблицы
            */
        {
            SqlDataAdapter adapter = new SqlDataAdapter(checkQuery, owner.conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            checkGrid.DataSource = ds.Tables[0].DefaultView;

            checkGrid.Columns[0].Visible = false;
            checkGrid.Columns[1].HeaderText = "Номер несоответствия";
            checkGrid.Columns[2].HeaderText = "Проверяемое подразделение";
            checkGrid.Columns[3].HeaderText = "Проверяемый участок";
            checkGrid.Columns[4].HeaderText = "Дата проверки";
            checkGrid.Columns[5].Visible = false;
            checkGrid.Columns[6].Visible = false;
            checkGrid.Columns[7].HeaderText = "Объект контроля";
            checkGrid.Columns[8].HeaderText = "Результат контроля";
            checkGrid.Columns[9].Visible = false;
            checkGrid.Columns[10].Visible = false;
            checkGrid.Columns[11].Visible = false;
            checkGrid.Columns[12].Visible = false;
            checkGrid.Columns[13].Visible = false;
            checkGrid.Columns[14].Visible = false;
            checkGrid.Columns[15].HeaderText = "Проверяющее подразделение";
            checkGrid.Columns[16].Visible = false;
            checkGrid.Columns[17].Visible = false;
            checkGrid.Columns[18].Visible = false;
            checkGrid.Columns[19].Visible = false;
            checkGrid.Columns[20].Visible = false;
            checkGrid.Columns[21].Visible = false;

            checkGrid.Columns[1].DisplayIndex = 0;
            checkGrid.Columns[15].DisplayIndex = 1;
            checkGrid.Columns[4].DisplayIndex = 2;
            checkGrid.Columns[2].DisplayIndex = 3;
            checkGrid.Columns[3].DisplayIndex = 4;
            checkGrid.Columns[7].DisplayIndex = 5;
            checkGrid.Columns[8].DisplayIndex = 6;
        }

        private void checkGrid_DoubleClick(object sender, EventArgs e)
            /*
            Двойное нажатие по строке
            Сохранение айди, номера несоответствия и результата контроля
            Закрытие формы
            */
        {
            if (checkGrid.SelectedRows.Count != 1) return;
            checkId = (int)checkGrid.SelectedRows[0].Cells[0].Value;
            checkNum = checkGrid.SelectedRows[0].Cells[1].Value.ToString();
            checkResult = checkGrid.SelectedRows[0].Cells[8].Value.ToString();
            controlIndicator = checkGrid.SelectedRows[0].Cells[7].Value.ToString();

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
