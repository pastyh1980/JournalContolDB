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
    public partial class DetailEvent : Form
    {

        public Event eventt;
        public JournalDBMainForm owner;

        public DetailEvent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Отмена"
            */
        {
            Close();
        }

        private void DetailEvent_Shown(object sender, EventArgs e)
            /*
            Отображение формы
            Заполнение данными полей
            */
        {
            if (eventt == null) return;

            failReasonTxt.Text = eventt.failReason;
            descriptionTxt.Text = eventt.description;
            responsWorkerTxt.Text = eventt.responsWorker;
            dueDateTxt.Text = eventt.dueDate.ToShortDateString();
            developDateTxt.Text = eventt.developDate.ToShortDateString();
            developSubunitTxt.Text = eventt.developerSubunit;
            developerTxt.Text = eventt.developer;
            reportTxt.Text = eventt.report;
            proofInfTxt.Text = eventt.proofInf;
            if (reportTxt.Text != "")
                reportDateTxt.Text = eventt.reportDate.ToShortDateString();
            reportSubunitTxt.Text = eventt.reportWorkerSubunit;
            reportWorkerTxt.Text = eventt.reportWorker;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void reportTxt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
