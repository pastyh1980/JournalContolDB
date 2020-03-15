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
    public partial class NewCheckForm : Form
    {
        public Check check;
        public List<string> sectors;
        public List<string> subunits;

        public NewCheckForm()
        {
            InitializeComponent();
        }

        public NewCheckForm(List<string> subunits, List<string> sectors) : this()
            /*
            Инициализация формы
            Добавление информации о подразделениях и участках
            */
        {
            checkSubunitCmbBx.Items.AddRange(subunits.ToArray());
            this.sectors = sectors;
            this.subunits = subunits;
        }

        private void saveBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Сохранить"
            Проверка заполненности полей
            Формирование объекта несоответствия
            Закрытие формы, если все заполнены
            */
        {
            List<string> errors = new List<string>();
            if (checkDatePicker.Text == "")
                errors.Add("Дата проверки");
            if (checkWorkerTxt.Text == "")
                errors.Add("ФИО проверяющего");
            if (checkSubunitCmbBx.Text == "")
                errors.Add("Проверяющее подразделение");
            if (controlIndicatorTxt.Text == "")
                errors.Add("Объект контроля");
            if (failDescrTxt.Text == "")
                errors.Add("Описание несоответствия");
            if (TDKDTxt.Text == "")
                errors.Add("Обозначение комплекта документов (ТД, КД)");
            if (sectorCmbBx.Text == "")
                errors.Add("Проверяемый участок");

            if (errors.Count != 0)
            {
                string msg = "Пожалуйста, заполните следующие поля: " + errors[0];
                for (int i = 1; i < errors.Count; ++i)
                    msg += ", " + errors[i];
                msg += "!";

                MessageBox.Show(msg, "Ошибка");
                return;
            }

            check = new Check();
            check.checkDate = checkDatePicker.Value;
            check.checkWorker = checkWorkerTxt.Text;
            check.checkSubunit = checkSubunitCmbBx.Text;
            check.controlIndicator = controlIndicatorTxt.Text;
            check.failDescr = failDescrTxt.Text;
            check.td_kd = TDKDTxt.Text;
            check.sector = sectorCmbBx.Text;
            check.isFail = true;

            if (MessageBox.Show("Изменить данные будет невозможно! Вы уверены, что хотите сохранить?", "Предупреждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Отмена"
            */
        {
            Close();
        }

        private void checkSubunitCmbBx_SelectedIndexChanged(object sender, EventArgs e)
            /*
            Выбор подразделения
            Обновление списка участков
            */
        {
            int index = subunits.FindIndex(str => str == checkSubunitCmbBx.Text);
            if (index >= 0)
            {
                sectorCmbBx.Items.Clear();
                sectorCmbBx.Items.AddRange(sectors[index].Split(';'));
            }
        }

        private void saveNotFailBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Зарегестрировать отсутствие несоответствия"
            Проверка полей на заполненность
            Формирование объекта несоответствия
            Закрытие формы
            */
        {
            List<string> errors = new List<string>();
            if (checkDatePicker.Text == "")
                errors.Add("Дата проверки");
            if (checkWorkerTxt.Text == "")
                errors.Add("ФИО проверяющего");
            if (checkSubunitCmbBx.Text == "")
                errors.Add("Проверяющее подразделение");
            if (controlIndicatorTxt.Text == "")
                errors.Add("Объект контроля");
            if (TDKDTxt.Text == "")
                errors.Add("Обозначение комплекта документов (ТД, КД)");
            if (sectorCmbBx.Text == "")
                errors.Add("Проверяемый участок");

            if (errors.Count != 0)
            {
                string msg = "Пожалуйста, заполните следующие поля: " + errors[0];
                for (int i = 1; i < errors.Count; ++i)
                    msg += ", " + errors[i];
                msg += "!";

                MessageBox.Show(msg, "Ошибка");
                return;
            }

            check = new Check();
            check.checkDate = checkDatePicker.Value;
            check.checkWorker = checkWorkerTxt.Text;
            check.checkSubunit = checkSubunitCmbBx.Text;
            check.controlIndicator = controlIndicatorTxt.Text;
            check.td_kd = TDKDTxt.Text;
            check.failDescr = "Несоответствий не обнаружено";
            check.sector = sectorCmbBx.Text;
            check.isFail = false;

            if (MessageBox.Show("Изменить данные будет невозможно! Вы уверены, что хотите сохранить?", "Предупреждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
