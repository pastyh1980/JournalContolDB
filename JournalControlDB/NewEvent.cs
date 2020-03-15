using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JournalControlDB
{
    public partial class NewEvent : Form
    {

        public JournalDBMainForm owner;
        public Event eventt;

        public NewEvent()
        {
            InitializeComponent();

            eventt = new Event();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Отмена"
            */
        {
            Close();
        }

        private void checkSelectBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Выбрать несоответствие"
            Открытие формы с выбором несоответствия
            Получение данных о выбранном несоответствии
            Заполнение полей на форме
            */
        {
            CheckSelect checkSelectForm = new CheckSelect();
            checkSelectForm.owner = owner;

            if (checkSelectForm.ShowDialog() != DialogResult.OK) return;

            eventt.checkId = checkSelectForm.checkId;

            checkNumTxt.Text = checkSelectForm.checkNum;

            checkResultTxt.Text = checkSelectForm.checkResult;

            controlIndicatorTxt.Text = checkSelectForm.controlIndicator;
        }

        private void saveBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Сохранить"
            Проверка заполенности полей
            Формирование объекта мероприятия
            Закрытие формы
            */
        {
            List<string> errors = new List<string>();
            if (checkNumTxt.Text == "") errors.Add("Номер несоответствия");
            if (failReasonTxt.Text == "") errors.Add("Причина несоответствия");
            if (descriptionTxt.Text == "") errors.Add("Описание мероприятия");
            if (responsTxt.Text == "") errors.Add("Ответственный");

            if (errors.Count != 0)
            {
                string msg = "Пожалуйста, заполните следующие поля: " + errors[0];
                for (int i = 1; i < errors.Count; ++i)
                    msg += ", " + errors[i];
                msg += "!";

                MessageBox.Show(msg, "Ошибка");
                return;
            }

            eventt.failReason = failReasonTxt.Text;
            eventt.description = descriptionTxt.Text;
            eventt.responsWorker = responsTxt.Text;
            eventt.dueDate = dueDatePicker.Value;

            if (MessageBox.Show("Изменить данные будет невозможно! Вы уверены, что хотите сохранить?", "Предупреждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
