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
    public partial class ReportEvent : Form
    {
        public Check check;
        public Event eventt;
        public JournalDBMainForm owner;

        private const string bossShowBySubunitQuery = "SELECT boss FROM grant_show WHERE subunit=@subunit";

        public ReportEvent()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Отмена"
            */
        {
            Close();
        }

        private void ReportEvent_Shown(object sender, EventArgs e)
            /*
            Отображение формы
            Заполнение данными полей
            Проверка прав доступа
            */
        {
            checkNumTxt.Text = check.failCount;
            checkDateTxt.Text = check.checkDate.ToShortDateString();
            subjSubunitTxt.Text = check.checkSubunit;
            checkWorkerTxt.Text = check.checkWorker;
            controlIndicatorTxt.Text = check.controlIndicator;
            failDescrTxt.Text = check.failDescr;
            descriptionTxt.Text = eventt.description;
            responsWorkerTxt.Text = eventt.responsWorker;
            dueDateTxt.Text = eventt.dueDate.ToShortDateString();
            failReasonTxt.Text = eventt.failReason;
            developDateTxt.Text = eventt.developDate.ToShortDateString();
            developSubunitTxt.Text = eventt.developerSubunit;
            developerTxt.Text = eventt.developer;
            sectorTxt.Text = check.sector;
            TDKDTxt.Text = check.td_kd;

            SqlCommand bossCommand = owner.conn.CreateCommand();
            bossCommand.CommandText = bossShowBySubunitQuery;
            bossCommand.Parameters.AddWithValue("@subunit", eventt.developerSubunit);

            SqlDataReader reader = bossCommand.ExecuteReader();

            List<int> bossIds = new List<int>();
            while (reader.Read())
                bossIds.Add((int)reader[0]);
            reader.Close();

            if (eventt.report != null && eventt.report != "")
            {
                reportCmbBx.Text = eventt.report;
                reportCmbBx.Enabled = false;
                reportDate.Text = eventt.reportDate.ToShortDateString();
                reportSubunitTxt.Text = eventt.reportWorkerSubunit;
                reportWorker.Text = eventt.reportWorker;
                proofInfTxt.Text = eventt.proofInf;
                proofInfTxt.ReadOnly = true;
                saveBtn.Enabled = false;
            }
            //Если пользователь не имеет права написать отчет по этому мероприятию, и отчет ещё никем не написан, скрываются поля отчета
            else if (!owner.currentUserAccess.Contains("REPORT") || (owner.currentUserId != eventt.developerId) && !bossIds.Contains(owner.currentUserId))
            {
                reportPanel.Visible = false;
                Height -= 143;
                saveBtn.Enabled = false;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Сохранить"
            Проверка заполненности полей
            Формирование объекта мероприятия
            */
        {
            List<string> errors = new List<string>();

            if (reportCmbBx.Text == "") errors.Add("Отчет");
            if (proofInfTxt.Text == "") errors.Add("Подтверждающая информация");

            if (errors.Count != 0)
            {
                string msg = "Пожалуйста, заполните следующие поля: " + errors[0];
                for (int i = 1; i < errors.Count; ++i)
                    msg += ", " + errors[i];
                msg += "!";

                MessageBox.Show(msg, "Ошибка");
                return;
            }

            if (MessageBox.Show("После сохранения изменить данные будет невозможно! Вы уверены, что хотите сохранить?", "Предупреждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            eventt.report = reportCmbBx.Text;
            eventt.proofInf = proofInfTxt.Text;

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
