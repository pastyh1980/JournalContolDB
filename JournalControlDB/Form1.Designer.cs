namespace JournalControlDB
{
    partial class JournalDBMainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.checkTabPage = new System.Windows.Forms.TabPage();
            this.incorrectCheckBtn = new System.Windows.Forms.Button();
            this.deactivateCheckBtn = new System.Windows.Forms.Button();
            this.unshownCheckLbl = new System.Windows.Forms.Label();
            this.newCheckBtn = new System.Windows.Forms.Button();
            this.checkGrid = new System.Windows.Forms.DataGridView();
            this.eventsTabPage = new System.Windows.Forms.TabPage();
            this.incorrectEvent = new System.Windows.Forms.Button();
            this.deactivateEvent = new System.Windows.Forms.Button();
            this.newEventBtn = new System.Windows.Forms.Button();
            this.eventsGridView = new System.Windows.Forms.DataGridView();
            this.reportTabPage = new System.Windows.Forms.TabPage();
            this.shortReportToXlsBtn = new System.Windows.Forms.Button();
            this.reportToXlsBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.reportsEventGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.reportShowsGrid = new System.Windows.Forms.DataGridView();
            this.reportCheckGrid = new System.Windows.Forms.DataGridView();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.changeUserBtn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.checkTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkGrid)).BeginInit();
            this.eventsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).BeginInit();
            this.reportTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportsEventGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportShowsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportCheckGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.checkTabPage);
            this.tabControl1.Controls.Add(this.eventsTabPage);
            this.tabControl1.Controls.Add(this.reportTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(785, 497);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // checkTabPage
            // 
            this.checkTabPage.Controls.Add(this.incorrectCheckBtn);
            this.checkTabPage.Controls.Add(this.deactivateCheckBtn);
            this.checkTabPage.Controls.Add(this.unshownCheckLbl);
            this.checkTabPage.Controls.Add(this.newCheckBtn);
            this.checkTabPage.Controls.Add(this.checkGrid);
            this.checkTabPage.Location = new System.Drawing.Point(4, 22);
            this.checkTabPage.Name = "checkTabPage";
            this.checkTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.checkTabPage.Size = new System.Drawing.Size(777, 471);
            this.checkTabPage.TabIndex = 0;
            this.checkTabPage.Text = "Несоответствия";
            this.checkTabPage.UseVisualStyleBackColor = true;
            // 
            // incorrectCheckBtn
            // 
            this.incorrectCheckBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.incorrectCheckBtn.Location = new System.Drawing.Point(566, 429);
            this.incorrectCheckBtn.Name = "incorrectCheckBtn";
            this.incorrectCheckBtn.Size = new System.Drawing.Size(198, 39);
            this.incorrectCheckBtn.TabIndex = 4;
            this.incorrectCheckBtn.Text = "Зарегистрировать ошибку в несоответствии";
            this.incorrectCheckBtn.UseVisualStyleBackColor = true;
            this.incorrectCheckBtn.Click += new System.EventHandler(this.incorrectCheckBtn_Click);
            // 
            // deactivateCheckBtn
            // 
            this.deactivateCheckBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deactivateCheckBtn.Location = new System.Drawing.Point(285, 429);
            this.deactivateCheckBtn.Name = "deactivateCheckBtn";
            this.deactivateCheckBtn.Size = new System.Drawing.Size(198, 39);
            this.deactivateCheckBtn.TabIndex = 3;
            this.deactivateCheckBtn.Text = "Зарегистрировать устранение несоответствия";
            this.deactivateCheckBtn.UseVisualStyleBackColor = true;
            this.deactivateCheckBtn.Click += new System.EventHandler(this.deactivateCheckBtn_Click);
            // 
            // unshownCheckLbl
            // 
            this.unshownCheckLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unshownCheckLbl.AutoSize = true;
            this.unshownCheckLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.unshownCheckLbl.ForeColor = System.Drawing.Color.Red;
            this.unshownCheckLbl.Location = new System.Drawing.Point(6, 396);
            this.unshownCheckLbl.Name = "unshownCheckLbl";
            this.unshownCheckLbl.Size = new System.Drawing.Size(414, 20);
            this.unshownCheckLbl.TabIndex = 2;
            this.unshownCheckLbl.Text = "Количество непросмотренных несоответствий:";
            // 
            // newCheckBtn
            // 
            this.newCheckBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newCheckBtn.Location = new System.Drawing.Point(6, 442);
            this.newCheckBtn.Name = "newCheckBtn";
            this.newCheckBtn.Size = new System.Drawing.Size(198, 23);
            this.newCheckBtn.TabIndex = 1;
            this.newCheckBtn.Text = "Зарегистрировать несоответствие";
            this.newCheckBtn.UseVisualStyleBackColor = true;
            this.newCheckBtn.Click += new System.EventHandler(this.newCheckBtn_Click);
            // 
            // checkGrid
            // 
            this.checkGrid.AllowUserToAddRows = false;
            this.checkGrid.AllowUserToDeleteRows = false;
            this.checkGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.checkGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.checkGrid.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.checkGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.checkGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.checkGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.checkGrid.Location = new System.Drawing.Point(6, 6);
            this.checkGrid.MultiSelect = false;
            this.checkGrid.Name = "checkGrid";
            this.checkGrid.ReadOnly = true;
            this.checkGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.checkGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.checkGrid.Size = new System.Drawing.Size(765, 387);
            this.checkGrid.TabIndex = 0;
            this.checkGrid.Sorted += new System.EventHandler(this.checkGrid_Sorted);
            this.checkGrid.Click += new System.EventHandler(this.checkGrid_SelectionChanged);
            this.checkGrid.DoubleClick += new System.EventHandler(this.checkGrid_DoubleClick);
            // 
            // eventsTabPage
            // 
            this.eventsTabPage.Controls.Add(this.incorrectEvent);
            this.eventsTabPage.Controls.Add(this.deactivateEvent);
            this.eventsTabPage.Controls.Add(this.newEventBtn);
            this.eventsTabPage.Controls.Add(this.eventsGridView);
            this.eventsTabPage.Location = new System.Drawing.Point(4, 22);
            this.eventsTabPage.Name = "eventsTabPage";
            this.eventsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.eventsTabPage.Size = new System.Drawing.Size(777, 471);
            this.eventsTabPage.TabIndex = 1;
            this.eventsTabPage.Text = "Мероприятия";
            this.eventsTabPage.UseVisualStyleBackColor = true;
            // 
            // incorrectEvent
            // 
            this.incorrectEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.incorrectEvent.Location = new System.Drawing.Point(566, 424);
            this.incorrectEvent.Name = "incorrectEvent";
            this.incorrectEvent.Size = new System.Drawing.Size(198, 39);
            this.incorrectEvent.TabIndex = 6;
            this.incorrectEvent.Text = "Зарегистрировать ошибку в мероприятии";
            this.incorrectEvent.UseVisualStyleBackColor = true;
            this.incorrectEvent.Click += new System.EventHandler(this.incorrectEvent_Click);
            // 
            // deactivateEvent
            // 
            this.deactivateEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deactivateEvent.Location = new System.Drawing.Point(279, 424);
            this.deactivateEvent.Name = "deactivateEvent";
            this.deactivateEvent.Size = new System.Drawing.Size(198, 39);
            this.deactivateEvent.TabIndex = 5;
            this.deactivateEvent.Text = "Зарегистрировать устранение мероприятия";
            this.deactivateEvent.UseVisualStyleBackColor = true;
            this.deactivateEvent.Visible = false;
            this.deactivateEvent.Click += new System.EventHandler(this.deactivateEvent_Click);
            // 
            // newEventBtn
            // 
            this.newEventBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newEventBtn.Location = new System.Drawing.Point(3, 440);
            this.newEventBtn.Name = "newEventBtn";
            this.newEventBtn.Size = new System.Drawing.Size(190, 23);
            this.newEventBtn.TabIndex = 4;
            this.newEventBtn.Text = "Зарегистрировать мероприятие";
            this.newEventBtn.UseVisualStyleBackColor = true;
            this.newEventBtn.Click += new System.EventHandler(this.newEventBtn_Click);
            // 
            // eventsGridView
            // 
            this.eventsGridView.AllowUserToAddRows = false;
            this.eventsGridView.AllowUserToDeleteRows = false;
            this.eventsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.eventsGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.eventsGridView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.eventsGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.eventsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.eventsGridView.Location = new System.Drawing.Point(6, 6);
            this.eventsGridView.Name = "eventsGridView";
            this.eventsGridView.ReadOnly = true;
            this.eventsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eventsGridView.Size = new System.Drawing.Size(765, 398);
            this.eventsGridView.TabIndex = 1;
            this.eventsGridView.Click += new System.EventHandler(this.eventsGridView_SelectionChanged);
            this.eventsGridView.DoubleClick += new System.EventHandler(this.eventsGridView_DoubleClick);
            // 
            // reportTabPage
            // 
            this.reportTabPage.Controls.Add(this.shortReportToXlsBtn);
            this.reportTabPage.Controls.Add(this.reportToXlsBtn);
            this.reportTabPage.Controls.Add(this.label2);
            this.reportTabPage.Controls.Add(this.reportsEventGrid);
            this.reportTabPage.Controls.Add(this.label1);
            this.reportTabPage.Controls.Add(this.reportShowsGrid);
            this.reportTabPage.Controls.Add(this.reportCheckGrid);
            this.reportTabPage.Location = new System.Drawing.Point(4, 22);
            this.reportTabPage.Name = "reportTabPage";
            this.reportTabPage.Size = new System.Drawing.Size(777, 471);
            this.reportTabPage.TabIndex = 2;
            this.reportTabPage.Text = "Отчет";
            this.reportTabPage.UseVisualStyleBackColor = true;
            // 
            // shortReportToXlsBtn
            // 
            this.shortReportToXlsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shortReportToXlsBtn.Location = new System.Drawing.Point(602, 427);
            this.shortReportToXlsBtn.Name = "shortReportToXlsBtn";
            this.shortReportToXlsBtn.Size = new System.Drawing.Size(166, 41);
            this.shortReportToXlsBtn.TabIndex = 7;
            this.shortReportToXlsBtn.Text = "Сформировать сокращенный отчет в Excel";
            this.shortReportToXlsBtn.UseVisualStyleBackColor = true;
            this.shortReportToXlsBtn.Click += new System.EventHandler(this.shortReportToXlsBtn_Click);
            // 
            // reportToXlsBtn
            // 
            this.reportToXlsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reportToXlsBtn.Location = new System.Drawing.Point(3, 427);
            this.reportToXlsBtn.Name = "reportToXlsBtn";
            this.reportToXlsBtn.Size = new System.Drawing.Size(128, 41);
            this.reportToXlsBtn.TabIndex = 6;
            this.reportToXlsBtn.Text = "Сформировать полный отчет в Excel";
            this.reportToXlsBtn.UseVisualStyleBackColor = true;
            this.reportToXlsBtn.Click += new System.EventHandler(this.reportToXlsBtn_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Мероприятия";
            // 
            // reportsEventGrid
            // 
            this.reportsEventGrid.AllowUserToAddRows = false;
            this.reportsEventGrid.AllowUserToDeleteRows = false;
            this.reportsEventGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportsEventGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reportsEventGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.reportsEventGrid.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.reportsEventGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reportsEventGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.reportsEventGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.reportsEventGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.reportsEventGrid.Location = new System.Drawing.Point(3, 288);
            this.reportsEventGrid.MultiSelect = false;
            this.reportsEventGrid.Name = "reportsEventGrid";
            this.reportsEventGrid.ReadOnly = true;
            this.reportsEventGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.reportsEventGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.reportsEventGrid.Size = new System.Drawing.Size(765, 133);
            this.reportsEventGrid.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ознакомились";
            // 
            // reportShowsGrid
            // 
            this.reportShowsGrid.AllowUserToAddRows = false;
            this.reportShowsGrid.AllowUserToDeleteRows = false;
            this.reportShowsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportShowsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reportShowsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.reportShowsGrid.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.reportShowsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reportShowsGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.reportShowsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.reportShowsGrid.DefaultCellStyle = dataGridViewCellStyle4;
            this.reportShowsGrid.Location = new System.Drawing.Point(3, 188);
            this.reportShowsGrid.MultiSelect = false;
            this.reportShowsGrid.Name = "reportShowsGrid";
            this.reportShowsGrid.ReadOnly = true;
            this.reportShowsGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.reportShowsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.reportShowsGrid.Size = new System.Drawing.Size(765, 80);
            this.reportShowsGrid.TabIndex = 2;
            // 
            // reportCheckGrid
            // 
            this.reportCheckGrid.AllowUserToAddRows = false;
            this.reportCheckGrid.AllowUserToDeleteRows = false;
            this.reportCheckGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportCheckGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reportCheckGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.reportCheckGrid.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.reportCheckGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reportCheckGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.reportCheckGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.reportCheckGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.reportCheckGrid.Location = new System.Drawing.Point(3, 3);
            this.reportCheckGrid.MultiSelect = false;
            this.reportCheckGrid.Name = "reportCheckGrid";
            this.reportCheckGrid.ReadOnly = true;
            this.reportCheckGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.reportCheckGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.reportCheckGrid.Size = new System.Drawing.Size(765, 165);
            this.reportCheckGrid.TabIndex = 1;
            this.reportCheckGrid.SelectionChanged += new System.EventHandler(this.reportCheckGrid_SelectionChanged);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshBtn.Location = new System.Drawing.Point(16, 518);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(75, 23);
            this.refreshBtn.TabIndex = 1;
            this.refreshBtn.Text = "Обновить";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // changeUserBtn
            // 
            this.changeUserBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.changeUserBtn.Location = new System.Drawing.Point(649, 518);
            this.changeUserBtn.Name = "changeUserBtn";
            this.changeUserBtn.Size = new System.Drawing.Size(144, 23);
            this.changeUserBtn.TabIndex = 2;
            this.changeUserBtn.Text = "Сменить пользователя";
            this.changeUserBtn.UseVisualStyleBackColor = true;
            this.changeUserBtn.Click += new System.EventHandler(this.changeUserBtn_Click);
            // 
            // JournalDBMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 553);
            this.Controls.Add(this.changeUserBtn);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(816, 592);
            this.Name = "JournalDBMainForm";
            this.Text = "Журнал контроля технологической дисциплины";
            this.Shown += new System.EventHandler(this.JournalDBMainForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.checkTabPage.ResumeLayout(false);
            this.checkTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkGrid)).EndInit();
            this.eventsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).EndInit();
            this.reportTabPage.ResumeLayout(false);
            this.reportTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportsEventGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportShowsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportCheckGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage checkTabPage;
        private System.Windows.Forms.DataGridView checkGrid;
        private System.Windows.Forms.TabPage eventsTabPage;
        private System.Windows.Forms.Button newCheckBtn;
        private System.Windows.Forms.DataGridView eventsGridView;
        private System.Windows.Forms.Button newEventBtn;
        private System.Windows.Forms.Label unshownCheckLbl;
        private System.Windows.Forms.Button deactivateCheckBtn;
        private System.Windows.Forms.Button incorrectCheckBtn;
        private System.Windows.Forms.Button incorrectEvent;
        private System.Windows.Forms.Button deactivateEvent;
        private System.Windows.Forms.TabPage reportTabPage;
        private System.Windows.Forms.DataGridView reportCheckGrid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView reportsEventGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView reportShowsGrid;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Button reportToXlsBtn;
        private System.Windows.Forms.Button changeUserBtn;
        private System.Windows.Forms.Button shortReportToXlsBtn;
    }
}

