namespace JournalControlDB
{
    partial class NewCheckForm
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
            this.checkDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.checkWorkerTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkSubunitCmbBx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.controlIndicatorTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.failDescrTxt = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.TDKDTxt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.sectorCmbBx = new System.Windows.Forms.ComboBox();
            this.saveNotFailBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkDatePicker
            // 
            this.checkDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.checkDatePicker.Location = new System.Drawing.Point(12, 25);
            this.checkDatePicker.Name = "checkDatePicker";
            this.checkDatePicker.Size = new System.Drawing.Size(160, 20);
            this.checkDatePicker.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Дата проверки";
            // 
            // checkWorkerTxt
            // 
            this.checkWorkerTxt.Location = new System.Drawing.Point(12, 80);
            this.checkWorkerTxt.Multiline = true;
            this.checkWorkerTxt.Name = "checkWorkerTxt";
            this.checkWorkerTxt.Size = new System.Drawing.Size(160, 49);
            this.checkWorkerTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ФИО проверяющего";
            // 
            // checkSubunitCmbBx
            // 
            this.checkSubunitCmbBx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.checkSubunitCmbBx.FormattingEnabled = true;
            this.checkSubunitCmbBx.Location = new System.Drawing.Point(234, 28);
            this.checkSubunitCmbBx.Name = "checkSubunitCmbBx";
            this.checkSubunitCmbBx.Size = new System.Drawing.Size(160, 21);
            this.checkSubunitCmbBx.TabIndex = 4;
            this.checkSubunitCmbBx.SelectedIndexChanged += new System.EventHandler(this.checkSubunitCmbBx_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(231, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Проверяемое подразделение";
            // 
            // controlIndicatorTxt
            // 
            this.controlIndicatorTxt.Location = new System.Drawing.Point(234, 65);
            this.controlIndicatorTxt.Multiline = true;
            this.controlIndicatorTxt.Name = "controlIndicatorTxt";
            this.controlIndicatorTxt.Size = new System.Drawing.Size(403, 36);
            this.controlIndicatorTxt.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(231, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Объект контроля";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Результат контроля";
            // 
            // failDescrTxt
            // 
            this.failDescrTxt.Location = new System.Drawing.Point(12, 157);
            this.failDescrTxt.Multiline = true;
            this.failDescrTxt.Name = "failDescrTxt";
            this.failDescrTxt.Size = new System.Drawing.Size(625, 76);
            this.failDescrTxt.TabIndex = 10;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(12, 253);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 12;
            this.saveBtn.TabStop = false;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(573, 253);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 13;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(231, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(242, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Обозначение комплекта документов (ТД, КД)";
            // 
            // TDKDTxt
            // 
            this.TDKDTxt.Location = new System.Drawing.Point(234, 120);
            this.TDKDTxt.Name = "TDKDTxt";
            this.TDKDTxt.Size = new System.Drawing.Size(403, 20);
            this.TDKDTxt.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(436, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Проверяемый участок";
            // 
            // sectorCmbBx
            // 
            this.sectorCmbBx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectorCmbBx.FormattingEnabled = true;
            this.sectorCmbBx.Location = new System.Drawing.Point(439, 28);
            this.sectorCmbBx.Name = "sectorCmbBx";
            this.sectorCmbBx.Size = new System.Drawing.Size(198, 21);
            this.sectorCmbBx.TabIndex = 5;
            // 
            // saveNotFailBtn
            // 
            this.saveNotFailBtn.Location = new System.Drawing.Point(187, 253);
            this.saveNotFailBtn.Name = "saveNotFailBtn";
            this.saveNotFailBtn.Size = new System.Drawing.Size(267, 23);
            this.saveNotFailBtn.TabIndex = 18;
            this.saveNotFailBtn.TabStop = false;
            this.saveNotFailBtn.Text = "Зарегистрировать отсутствие несоответствия";
            this.saveNotFailBtn.UseVisualStyleBackColor = true;
            this.saveNotFailBtn.Click += new System.EventHandler(this.saveNotFailBtn_Click);
            // 
            // NewCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 288);
            this.Controls.Add(this.saveNotFailBtn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.sectorCmbBx);
            this.Controls.Add(this.TDKDTxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.failDescrTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.controlIndicatorTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkSubunitCmbBx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkWorkerTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkDatePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "NewCheckForm";
            this.Text = "Регистрация несоответствия";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker checkDatePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox checkWorkerTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox checkSubunitCmbBx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox controlIndicatorTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox failDescrTxt;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TDKDTxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox sectorCmbBx;
        private System.Windows.Forms.Button saveNotFailBtn;
    }
}