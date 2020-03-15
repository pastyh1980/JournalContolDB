namespace JournalControlDB
{
    partial class NewEvent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkNumTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkSelectBtn = new System.Windows.Forms.Button();
            this.failReasonTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.descriptionTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.responsTxt = new System.Windows.Forms.TextBox();
            this.dueDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.saveBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkResultTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.controlIndicatorTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkNumTxt
            // 
            this.checkNumTxt.Location = new System.Drawing.Point(12, 30);
            this.checkNumTxt.Name = "checkNumTxt";
            this.checkNumTxt.ReadOnly = true;
            this.checkNumTxt.Size = new System.Drawing.Size(167, 20);
            this.checkNumTxt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Номер несоответствия";
            // 
            // checkSelectBtn
            // 
            this.checkSelectBtn.Location = new System.Drawing.Point(12, 56);
            this.checkSelectBtn.Name = "checkSelectBtn";
            this.checkSelectBtn.Size = new System.Drawing.Size(75, 23);
            this.checkSelectBtn.TabIndex = 2;
            this.checkSelectBtn.Text = "Выбрать";
            this.checkSelectBtn.UseVisualStyleBackColor = true;
            this.checkSelectBtn.Click += new System.EventHandler(this.checkSelectBtn_Click);
            // 
            // failReasonTxt
            // 
            this.failReasonTxt.Location = new System.Drawing.Point(12, 175);
            this.failReasonTxt.Multiline = true;
            this.failReasonTxt.Name = "failReasonTxt";
            this.failReasonTxt.Size = new System.Drawing.Size(487, 55);
            this.failReasonTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Причина несоответствия";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Описание мероприятия";
            // 
            // descriptionTxt
            // 
            this.descriptionTxt.Location = new System.Drawing.Point(12, 249);
            this.descriptionTxt.Multiline = true;
            this.descriptionTxt.Name = "descriptionTxt";
            this.descriptionTxt.Size = new System.Drawing.Size(487, 51);
            this.descriptionTxt.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 313);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Ответственный";
            // 
            // responsTxt
            // 
            this.responsTxt.Location = new System.Drawing.Point(12, 329);
            this.responsTxt.Name = "responsTxt";
            this.responsTxt.Size = new System.Drawing.Size(197, 20);
            this.responsTxt.TabIndex = 7;
            // 
            // dueDatePicker
            // 
            this.dueDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dueDatePicker.Location = new System.Drawing.Point(302, 329);
            this.dueDatePicker.Name = "dueDatePicker";
            this.dueDatePicker.Size = new System.Drawing.Size(197, 20);
            this.dueDatePicker.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(299, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Срок исполнения";
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(12, 367);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 11;
            this.saveBtn.TabStop = false;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(424, 367);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 12;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Результат контроля";
            // 
            // checkResultTxt
            // 
            this.checkResultTxt.Location = new System.Drawing.Point(12, 105);
            this.checkResultTxt.Multiline = true;
            this.checkResultTxt.Name = "checkResultTxt";
            this.checkResultTxt.ReadOnly = true;
            this.checkResultTxt.Size = new System.Drawing.Size(487, 51);
            this.checkResultTxt.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(184, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Объект контроля";
            // 
            // controlIndicatorTxt
            // 
            this.controlIndicatorTxt.Location = new System.Drawing.Point(187, 30);
            this.controlIndicatorTxt.Multiline = true;
            this.controlIndicatorTxt.Name = "controlIndicatorTxt";
            this.controlIndicatorTxt.ReadOnly = true;
            this.controlIndicatorTxt.Size = new System.Drawing.Size(312, 51);
            this.controlIndicatorTxt.TabIndex = 2;
            // 
            // NewEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 399);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.controlIndicatorTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkResultTxt);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dueDatePicker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.responsTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.descriptionTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.failReasonTxt);
            this.Controls.Add(this.checkSelectBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkNumTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NewEvent";
            this.Text = "Регистрация мероприятия";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox checkNumTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button checkSelectBtn;
        private System.Windows.Forms.TextBox failReasonTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox descriptionTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox responsTxt;
        private System.Windows.Forms.DateTimePicker dueDatePicker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox checkResultTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox controlIndicatorTxt;
    }
}