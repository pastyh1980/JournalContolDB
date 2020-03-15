using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

using Excel = Microsoft.Office.Interop.Excel;

namespace JournalControlDB
{
    public partial class JournalDBMainForm : Form
    {
        Excel.Application excelApp; //Приложение Excel
        double version; //Версия установленного приложения Excel

        public SqlConnection conn = new SqlConnection(); //Объект подключения к БД
        private SqlDataAdapter checkDataAdapter; //DataAdapter для вывода несоответствий
        private SqlDataAdapter eventsDataAdapter; //DataAdapter для вывода мероприятий
        private SqlDataAdapter reportCheckDataAdapter; //DataAdapter для вывода отчета

        public int currentUserId; //АйДи текущего пользователя программы
        private string currentUserSubunit; //Подразделение текущего пользователя программы
        public string[] currentUserAccess; //Доступ пользователя
        public string currectUserName; //ФИО текущего пользователя

        private int selectedCheckId = -1;
        private int selectedEventId = -1;

        public const string LongDateFormat = "yyyy-dd-MM HH:mm:ss.fff";
        public const string ShortDateFormat = "yyyy-MM-dd";

        #region Запросы к БД
        private const string checkQuery = "SELECT * FROM Check_View WHERE isActive = 1 AND isCorrect = 1"; //Выбора активных несоответствий
        private const string reportCheckQuery = "SELECT * FROM Check_View"; //Вывод всех несоответствий
        private const string checkQueryWithCondition = "SELECT * FROM Check_View WHERE check_subunit = @check_subunit AND check_date = @check_date " + 
            "AND check_worker = @check_worker AND td_kd = @td_kd AND control_indicator = @control_indicator AND fail_description = @fail_description " + 
            "AND isActive = 1 AND isCorrect = 1"; //Поиск одинаковых несоответствий
        private const string allSubunitsQuery = "SELECT * FROM subunits"; //Выборка подразделений
        private const string authUserIdQuery = "SELECT id, subunit, access, SUBSTRING(name, 1, 1) + '.' + SUBSTRING(otch, 1, 1) + '. ' + family AS userName FROM workers WHERE login = @login"; //Поиск пользователя по логину
        private const string insertCheckQuery = "INSERT INTO [check] VALUES(@id, @reg_worker, NULL, @reg_date, @check_date, @check_worker," +
            "@check_subunit, @td_kd, @control_indicator, @count_operations, @fail_count, @fail_description, 1, 1, NULL, @sector, @isFail)"; //Добавление записи о несоответствии в БД
        private const string nextNumOfCheckQuery = "SELECT MAX(id) FROM [check]"; //Выбор номера последнего несоответствия
        private const string findCheckByFailCountQuery = "SELECT id FROM [check] WHERE fail_count=@fail_count"; //Поиск несоответствия по номеру
        /*private const string updateCheckWhenShowQuery = "UPDATE [check] SET show_worker = @showWorkerId, show_date = @showDate, " +
            "solution_subunit = @solutionSubunit WHERE id = @id";*/ //Добавление записи об ознакомлении в БД
        private const string eventsQuery = "SELECT * FROM Events_View WHERE isActive = 1 AND isCorrect = 1"; //Выборка активных мероприятий
        private const string countEventsQuery = "SELECT * FROM Events_View WHERE isCorrect = 1"; //Выборка неошибочных мероприятий
        private const string reportEventsQuery = "SELECT * FROM Events_View"; //Выборка всех мероприятий
        private const string showsQuery = "SELECT * FROM Shows_View"; //Выборка ознакомлений
        private const string checkSubunitQuery = "SELECT check_subunit FROM [check] WHERE id = @id"; //Выбор проверяемого подразделения по айди
        private const string bossShowQuery = "SELECT boss FROM grant_show WHERE subunit=@subunit AND sector=@sector"; //Выборка начальников подразделений
        private const string deactiveateCheckQuery = "UPDATE [check] SET isActive = 0, delete_worker = @worker_id, delete_date = @date " +
            "WHERE id = @id"; //Устранение несоответствия
        private const string incorrectCheckQuery = "UPDATE [check] SET isCorrect = 0, delete_worker = @worker_id, delete_date = @date " +
            "WHERE id = @id"; //Ошибка в несоответствии
        private const string insertEventQuery = "INSERT INTO events (check_id, developer, fail_reason, description, respons_worker, due_date, develop_date, isActive, isCorrect) " +
            "VALUES (@checkId, @developerId, @failReason, @description, @responsWorker, @dueDate, @developDate, 1, 1)"; //Добавление записи о мероприятии в БД
        private const string findEventByIdQuery = "SELECT * FROM Events_View WHERE id = @id"; //Поиск мероприятия по айди
        private const string updateEventQuery = "UPDATE events SET report = @report, proof_inf = @proofInf, report_date = @reportDate, report_worker = @reportWorker " +
            "WHERE id = @id"; //Добавление отчета о мероприятии
        private const string deactivateEventQuery = "UPDATE events SET isActive = 0, delete_worker = @worker_id, delete_date = @date WHERE check_id = @check_id AND isCorrect = 1"; //Устранение мероприятия
        private const string incorrectEventQuery = "UPDATE events SET isCorrect = 0, delete_worker = @worker_id, delete_date = @delete_date WHERE id = @id"; //Ошибка в мероприятии
        private const string countEventsByCheckIdQuery = "SELECT COUNT(id) FROM events WHERE check_id = @id AND isActive = 1 AND isCorrect = 1 AND report IS NULL"; //Количество невыполненных мероприятий по несоответствию
        private const string countShowsByCheckIdQuery = "SELECT COUNT(id) FROM shows WHERE check_id = @id"; //Количество несоответствий по ознакомлению
        private const string regIdWorkerQuery = "SELECT reg_worker FROM [check] WHERE id = @id";
        private const string bossShowBySubunitQuery = "SELECT boss FROM grant_show WHERE subunit=@subunit";
        private const string devIdWorkerQuery = "SELECT developer FROM [events] WHERE id = @id";
        private const string countCompleteEventsByCheckIdQuery = "SELECT COUNT(id) FROM events WHERE check_id = @id AND isActive = 1 AND isCorrect = 1 AND report IS NOT NULL";
        #endregion

        public JournalDBMainForm()
            /*Конструктор класса главной формы
            Проверка версии Excel (было проверено на версии 12 2007 года)
            Инициализация пароля для подключение к БД для каждого пользователя на ПК
            Инициализация подключения к БД и проверка
            Авторизация пользователя в форме
            Проверка прав доступа пользователя
            Заполнение GridView данными
            */
        {
            InitializeComponent();

            //Инициализация строки подключения
            /*StreamWriter writer = new StreamWriter("settings.conf");
            writer.Write(Data Source=DESKTOP-09J0MMK\\SQLEXPRESS;Initial Catalog=journal;Persist Security Info=True;User ID=sa;");
            writer.WriteLine();
            writer.Close();*/

            //Проверка версии Excel
            try
            {
                excelApp = new Excel.Application();
                System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                version = Convert.ToDouble(excelApp.Version, provider);
            }
            catch
            {
                version = 0;
            }

            if (version < 12) //Проверено на версии Excel 2007
            {
                reportToXlsBtn.Enabled = false; //Блокировка кнопки "Сформировать отчет в Excel"
                MessageBox.Show("Недопустимая версия Excel!", "Ошибка");
            }

            //Если пароль в Properties пустой, показывает форму для ввода пароля
            if (Properties.Settings.Default.Pass == null || Properties.Settings.Default.Pass == "")
            {
                PasswdForm passwdForm = new PasswdForm();
                if (passwdForm.ShowDialog() != DialogResult.OK) Environment.Exit(0);
                Properties.Settings.Default.Pass = passwdForm.pass;
                Properties.Settings.Default.Save(); //Сохранение нового пароля в Properties
                //Необходим ввод пароля для каждого пользователя
            }

            //Считавание строки подключения к базе данных из файла settings.conf в одной папке с исполняемым файлом
            StreamReader reader = new StreamReader("settings.conf");
            conn.ConnectionString = reader.ReadLine() + "Password=" + Properties.Settings.Default.Pass;

            //Проверка подключения
            try
            {
                conn.Open();
            }
            catch (SqlException ex)
            {
                //Если неверный пароль, очищает пароль и закрывает программу
                if (ex.Number == 18456) //Код ошибки - ошибка входа пользователя
                {
                    Properties.Settings.Default.Pass = "";
                    Properties.Settings.Default.Save(); //Очистка пароля
                }
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }

            try
            {
                //Получение логина пользователя, зарегистрированного в системе (старая версия)
                //string login = Environment.UserName;
                /* string login = "user1";


                 //Поиск соответствующей записи о пользователе в БД
                 SqlCommand command = conn.CreateCommand();
                 command.CommandText = authUserIdQuery;
                 command.Parameters.AddWithValue("@login", login);
                 SqlDataReader sqlReader = command.ExecuteReader();

                 //Если пользователь не найден, приложение закрывается
                 if (!sqlReader.Read())
                 {
                     MessageBox.Show("Пользователь не найден в базе данных. Пожалуйста, обратитесь к системному администратору", "Ошибка");
                     Environment.Exit(0);
                 }

                 currentUserId = (int)sqlReader[0]; //АйДи пользователя в БД
                 currentUserSubunit = (string)sqlReader[1]; //Подразделение пользователя в БД
                 currentUserAccess = sqlReader[2].ToString().Split(';'); //Права доступа пользователя
                 currectUserName = sqlReader[3].ToString(); //ФИО пользователя

                 sqlReader.Close();*/

                //Инциализция формы входа в приложение
                LoginForm loginForm = new LoginForm();
                loginForm.owner = this;

                if (loginForm.ShowDialog() != DialogResult.OK) //Если нажата кнопка "Отмена"
                {
                    conn.Close();
                    Environment.Exit(0);
                }

                currentUserId = loginForm.currentUserId; //АйДи пользователя в БД
                currentUserSubunit = loginForm.currentUserSubunit; //Подразделение пользователя в БД
                currentUserAccess = loginForm.currentUserAccess; //Права доступа пользователя
                currectUserName = loginForm.currectUserName; //ФИО пользователя

                Text += " от имени пользователя " + currectUserName;

                if (currentUserAccess.Contains("CHECK_DETAIL") || currentUserAccess.Contains("CONTROL"))
                    checkDataAdapter = new SqlDataAdapter(checkQuery, conn); //Инициализация данных о несоответствиях
                else
                    checkDataAdapter = new SqlDataAdapter(checkQuery + " AND id = 0", conn); //Пустая таблица

                if (currentUserAccess.Contains("EVENT_DETAIL") || currentUserAccess.Contains("CONTROL"))
                    eventsDataAdapter = new SqlDataAdapter(eventsQuery, conn); //Инициализация данных о мероприятиях
                else
                    eventsDataAdapter = new SqlDataAdapter(eventsQuery + " AND id = 0", conn); //Пустая таблица

                if (currentUserAccess.Contains("ADMIN"))
                {
                    reportCheckDataAdapter = new SqlDataAdapter(reportCheckQuery, conn); //Инициализация данных для отчёта
                }
                else
                {
                    //Проверка прав доступа пользователя

                    if (!currentUserAccess.Contains("REG")) //Блокировка кнопки "Зарегистрировать несоответствие"
                        newCheckBtn.Enabled = false;

                    if (!currentUserAccess.Contains("DEVEL")) //Блокировка кнопки "Зарегистрировать мероприятие"
                        newEventBtn.Enabled = false;

                    if (currentUserAccess.Contains("CONTROL") || currentUserAccess.Contains("REPORT_SHOW")) //Получение данных для отчета
                    {
                        reportCheckDataAdapter = new SqlDataAdapter(reportCheckQuery, conn);
                        reportTabPage.Select();
                    }
                }

                refreshViews(); //Заполнение данными GridViews

                //Выбор видимых колонок и инициализация их заголовков
                //checkGrid - несоответствия
                checkGrid.RowHeadersWidth = 60;

                checkGrid.Columns[0].Visible = false;
                checkGrid.Columns[1].HeaderText = "Номер несоответствия";

                checkGrid.Columns[2].HeaderText = "Проверяемое подразделение";
                
                checkGrid.Columns[3].HeaderText = "Проверяемый участок";
                
                checkGrid.Columns[4].HeaderText = "Дата проверки";
                
                checkGrid.Columns[5].HeaderText = "ФИО проверяющего";
                
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

                //eventsGrid - мероприятия
                eventsGridView.Columns[0].Visible = false;
                eventsGridView.Columns[1].Visible = false;
                eventsGridView.Columns[2].HeaderText = "Номер несоответствия";
                eventsGridView.Columns[3].HeaderText = "Проверяемое подразделение";
                eventsGridView.Columns[4].HeaderText = "Проверяемый участок";
                eventsGridView.Columns[5].HeaderText = "Результат контроля";
                eventsGridView.Columns[6].Visible = false;
                eventsGridView.Columns[7].HeaderText = "Описание мероприятия";
                eventsGridView.Columns[8].HeaderText = "Ответственный";
                eventsGridView.Columns[9].HeaderText = "Срок исполнения";
                eventsGridView.Columns[10].Visible = false;
                eventsGridView.Columns[11].HeaderText = "Подразделение разработчика";
                eventsGridView.Columns[12].Visible = false;
                eventsGridView.Columns[13].HeaderText = "ФИО разработчика";
                eventsGridView.Columns[14].HeaderText = "Отчет";
                eventsGridView.Columns[15].Visible = false;
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
                eventsGridView.Columns[27].HeaderText = "Дата проверки";
                eventsGridView.Columns[28].HeaderText = "Объект контроля";
                eventsGridView.Columns[29].Visible = false;

                //reportCheckGrid - отчет по несоответствиям
                if (reportCheckDataAdapter != null)
                {
                    reportCheckGrid.Columns[0].Visible = false;
                    reportCheckGrid.Columns[1].HeaderText = "Номер несоответствия";
                    reportCheckGrid.Columns[2].HeaderText = "Проверяемое подразделение";
                    reportCheckGrid.Columns[3].HeaderText = "Проверяемый участок";
                    reportCheckGrid.Columns[4].HeaderText = "Дата проверки";
                    reportCheckGrid.Columns[5].HeaderText = "ФИО проверяющего";
                    reportCheckGrid.Columns[6].HeaderText = "Обозначение комплекта документов (ТД, КД)";
                    reportCheckGrid.Columns[7].HeaderText = "Объект контроля";
                    reportCheckGrid.Columns[8].HeaderText = "Результат контроля";
                    reportCheckGrid.Columns[9].HeaderText = "Дата регистрации несоответствия";
                    reportCheckGrid.Columns[10].HeaderText = "Количество проверенных операций";
                    reportCheckGrid.Columns[11].Visible = false;
                    reportCheckGrid.Columns[12].Visible = false;
                    reportCheckGrid.Columns[13].HeaderText = "ФИО регистрирующего сотрудника";
                    reportCheckGrid.Columns[14].HeaderText = "Должность регистрирующего сотрудника";
                    reportCheckGrid.Columns[15].HeaderText = "Подразделение регистрирующего сотрудника";
                    reportCheckGrid.Columns[16].HeaderText = "Дата удаления несоответствия";
                    reportCheckGrid.Columns[17].HeaderText = "Причина удаления несоответствия";
                    reportCheckGrid.Columns[18].HeaderText = "ФИО удалившего несоответствие сотрудника";
                    reportCheckGrid.Columns[19].HeaderText = "Должность удалившего несоответствие сотрудника";
                    reportCheckGrid.Columns[20].HeaderText = "Подразделение удалившего несоответствие сотрудника";
                    reportCheckGrid.Columns[21].Visible = false;
                }
                else
                {
                    reportTabPage.Parent = null;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void newCheckBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Зарегистрировать несоответствие"
            Открытие формы регистрации несоответствия, получение данных о новом несоответствии и добавление записи в БД
            */
        {
            try
            {
                conn.Open();
                //Получение списка подразделений с участками
                SqlCommand commandSubunits = conn.CreateCommand();
                commandSubunits.CommandText = allSubunitsQuery;
                SqlDataReader reader = commandSubunits.ExecuteReader();
                List<string> subunits = new List<string>();
                List<string> sectors = new List<string>();
                while (reader.Read())
                {
                    subunits.Add(reader[0].ToString());
                    sectors.Add(reader[1].ToString());
                }
                reader.Close();

                //Инициализация формы регистрации мероприятия
                NewCheckForm newCheckForm = new NewCheckForm(subunits, sectors);
                newCheckForm.Text += " от имени пользователя " + currectUserName;
                if (newCheckForm.ShowDialog() != DialogResult.OK)
                {
                    conn.Close();
                    return;
                }

                Check check = newCheckForm.check; //Получение данных о несоответствии из формы

                //Проверка на существование такого же несоответствия в БД
                SqlCommand checkRecord = conn.CreateCommand();
                checkRecord.CommandText = checkQueryWithCondition;
                check.addParsWithCondition(checkRecord);
                object existCheck = checkRecord.ExecuteScalar();
                if (existCheck != null)
                {
                    MessageBox.Show("Данное несоответствие уже зарегистрированно!", "Ошибка");
                    conn.Close();
                    return;
                }

                SqlCommand commandNextId = conn.CreateCommand();
                commandNextId.CommandText = nextNumOfCheckQuery;
                object nextId = commandNextId.ExecuteScalar(); //Получение номера последнего несоответствия и выбор следующего номера
                int id;
                if (!int.TryParse(nextId.ToString(), out id))
                    check.id = 1;
                else
                    check.id = id + 1;

                //Данные о пользователе, зарегистрировавшем несоответствие
                check.regDate = DateTime.Now;
                check.regWorkerId = currentUserId;

                //Формирование номера несоответствия
                SqlCommand commandByFailCount = conn.CreateCommand();
                commandByFailCount.CommandText = findCheckByFailCountQuery;

                int num = 0;
                string date = check.checkDate.ToString("yyMMdd");
                string failCount;

                do
                {
                    if (commandByFailCount.Parameters.Count != 0) commandByFailCount.Parameters.Clear();
                    ++num;
                    failCount = date + num.ToString().PadLeft(2, '0');
                    commandByFailCount.Parameters.AddWithValue("@fail_count", failCount);
                } while (commandByFailCount.ExecuteScalar() != null);

                check.failCount = failCount;

                //Добавление записи о несоответствии в БД
                SqlCommand commandInsertCheck = conn.CreateCommand();
                commandInsertCheck.CommandText = insertCheckQuery;

                check.addPars(commandInsertCheck);

                commandInsertCheck.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                GC.Collect();

                Environment.Exit(0);
            }
        }

        private void checkGrid_DoubleClick(object sender, EventArgs e)
            /*
            Двойное нажатие на несоответствие в GridView
            Проверка прав доступа пользователя
            Формирование данных для формы детализации несоответствия
            */
        {
            try
            {
                //Если выбрано не одно несоответствие
                if (checkGrid.SelectedRows.Count != 1) return;

                //Проверка прав доступа польователя
                if (!currentUserAccess.Contains("CHECK_DETAIL") && !currentUserAccess.Contains("CONTROL") && !currentUserAccess.Contains("ADMIN")) return;

                //Получение айди несоответствия
                int id = (int)checkGrid.SelectedRows[0].Cells[0].Value;

                //Выбор данных о несоответствии
                string query = checkQuery + " AND id = @id";

                conn.Open();

                SqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);

                SqlDataReader dataReader = command.ExecuteReader();

                //Формирование данных несоответствия из dataReader
                Check check = new Check(dataReader);

                dataReader.Close();

                //Получение всех мероприятий текущего несоответствия
                string eventsByCheckId = eventsQuery + " AND check_id = " + check.id.ToString();

                SqlDataAdapter eventsDataAdapter = new SqlDataAdapter(eventsByCheckId, conn);
                check.eventsDataSet = new DataSet();
                eventsDataAdapter.Fill(check.eventsDataSet);

                //Получение всех ознакомлений с текущим несоответствием
                string showsByCheckId = showsQuery + " WHERE check_id = " + check.id.ToString();

                SqlDataAdapter showsDataAdapter = new SqlDataAdapter(showsByCheckId, conn);
                check.showsDataAdapter = showsDataAdapter;

                //Инициализация формы детализации несоответствия
                DetailCheck detailCheckForm = new DetailCheck();
                detailCheckForm.check = check;
                detailCheckForm.owner = this;

                detailCheckForm.ShowDialog();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void JournalDBMainForm_Shown(object sender, EventArgs e)
            /*
            Показ главной формы пользователю
            Обновление всех GridView
            */
        {
            try
            {
                conn.Open();
                refreshViews();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void refreshViews()
            /*
            Процедура обновления данных в GridView
            Выделение несоответствий без мероприятий красным в checkGrid
            */
        {
            try
            {
                //Обновление данных в checkGrid
                DataSet checkDS = new DataSet();
                checkDataAdapter.Fill(checkDS);
                checkGrid.DataSource = checkDS.Tables[0].DefaultView;

                checkGrid.Columns[1].DisplayIndex = 0;
                checkGrid.Columns[15].DisplayIndex = 1;
                checkGrid.Columns[5].DisplayIndex = 2;
                checkGrid.Columns[4].DisplayIndex = 3;
                checkGrid.Columns[2].DisplayIndex = 4;
                checkGrid.Columns[3].DisplayIndex = 5;
                checkGrid.Columns[7].DisplayIndex = 6;
                checkGrid.Columns[8].DisplayIndex = 7;

                //Подсчет и выделение несоответствий без мероприятий
                SqlCommand eventByCheckIdCommand = conn.CreateCommand();
                eventByCheckIdCommand.CommandText = countEventsQuery + " AND check_id = @check_id";

                int withoutEventCheckCount = 0;

                foreach (DataGridViewRow row in checkGrid.Rows)
                {
                    //Инициализация номеров строк
                    row.HeaderCell.Value = (checkGrid.Rows.IndexOf(row) + 1).ToString();
                    row.DefaultCellStyle.BackColor = Color.White;
                    eventByCheckIdCommand.Parameters.Clear();
                    eventByCheckIdCommand.Parameters.AddWithValue("@check_id", (int)row.Cells[0].Value);
                    object eventt = eventByCheckIdCommand.ExecuteScalar();

                    if (eventt == null && (bool)row.Cells[21].Value) //Если нет мероприятий и обнаружено несоответствие
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                        ++withoutEventCheckCount;
                    }
                }

                if (withoutEventCheckCount != 0)
                    unshownCheckLbl.Text = "Количество несоответствий, для которых не разработаны мероприятия: " + withoutEventCheckCount.ToString();
                else
                    unshownCheckLbl.Text = "";

                if (selectedCheckId != -1)
                    foreach (DataGridViewRow row in checkGrid.Rows)
                        if ((int)row.Cells[0].Value == selectedCheckId)
                        {
                            row.Selected = true;
                            break;
                        }

                //Обновление eventsGridView
                DataSet eventsDS = new DataSet();
                eventsDataAdapter.Fill(eventsDS);
                eventsGridView.DataSource = eventsDS.Tables[0].DefaultView;


                eventsGridView.Columns[2].DisplayIndex = 0;
                eventsGridView.Columns[27].DisplayIndex = 1;
                eventsGridView.Columns[28].DisplayIndex = 2;
                eventsGridView.Columns[3].DisplayIndex = 3;
                eventsGridView.Columns[4].DisplayIndex = 4;
                eventsGridView.Columns[5].DisplayIndex = 5;
                eventsGridView.Columns[7].DisplayIndex = 6;
                eventsGridView.Columns[11].DisplayIndex = 7;
                eventsGridView.Columns[13].DisplayIndex = 8;
                eventsGridView.Columns[9].DisplayIndex = 9;
                eventsGridView.Columns[8].DisplayIndex = 10;
                eventsGridView.Columns[14].DisplayIndex = 11;

                //Обновление reportCheckGrid
                if (reportCheckDataAdapter != null)
                {
                    DataSet reportCheckDS = new DataSet();
                    reportCheckDataAdapter.Fill(reportCheckDS);
                    reportCheckGrid.DataSource = reportCheckDS.Tables[0].DefaultView;
                }

                if (selectedEventId != -1)
                    foreach (DataGridViewRow row in eventsGridView.Rows)
                        if ((int)row.Cells[0].Value == selectedEventId)
                        {
                            row.Selected = true;
                            break;
                        }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void deactivateCheckBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Устранение несоответствия"
            Окно с вопросом пользователю
            Проверка прав доступа
            Подсчёт мероприятий. Если хотя бы одно активное есть, устранить нельзя
            */
        {
            try
            {
                if (checkGrid.SelectedRows.Count != 1) return; //Если выбрана не одна строка

                string msg = "Зарегистрировать устранение несоответствия №" + checkGrid.SelectedRows[0].Cells[1].Value.ToString() +
                    "; " + checkGrid.SelectedRows[0].Cells[8].Value.ToString() + " от " + checkGrid.SelectedRows[0].Cells[4].Value.ToString().Substring(0, 10) +
                    " от имени пользователя " + currectUserName + "?";

                if (MessageBox.Show(msg, "Устранить несоответствие", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                conn.Open();

                //Получение списка руководителей данного участка
                SqlCommand checkSubunitCommand = conn.CreateCommand();
                checkSubunitCommand.CommandText = bossShowQuery;
                checkSubunitCommand.Parameters.AddWithValue("@subunit", checkGrid.SelectedRows[0].Cells[2].Value.ToString());
                checkSubunitCommand.Parameters.AddWithValue("@sector", checkGrid.SelectedRows[0].Cells[3].Value.ToString());

                SqlDataReader reader = checkSubunitCommand.ExecuteReader();

                List<int> bossIds = new List<int>();
                while (reader.Read())
                    bossIds.Add((int)reader[0]);
                reader.Close();

                if (!bossIds.Contains(currentUserId)) //Если пользователь не является начальником, устранить невозможно
                {
                    MessageBox.Show("Вы не можете зарегистрировать устранение данного несоответствия, так как у вас нет прав на его устранение!", "Ошибка");
                    conn.Close();
                    return;
                }

                //Проверка количества мероприятий
                SqlCommand countEventsByCheckIdCommand = conn.CreateCommand();
                countEventsByCheckIdCommand.CommandText = countEventsByCheckIdQuery;
                countEventsByCheckIdCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);

                int eventsCount = (int)countEventsByCheckIdCommand.ExecuteScalar();

                if (eventsCount != 0)
                {
                    conn.Close();
                    MessageBox.Show("Для данного несоответствия существуют невыполненные мероприятия! Устраните сначала мероприятия!", "Ошибка");
                    return;
                }

                SqlCommand countCompleteEventsByCheckIdCommand = conn.CreateCommand();
                countCompleteEventsByCheckIdCommand.CommandText = countCompleteEventsByCheckIdQuery;
                countCompleteEventsByCheckIdCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);

                int completeEventsCount = (int)countCompleteEventsByCheckIdCommand.ExecuteScalar();

                if (completeEventsCount < 1 && (bool)checkGrid.SelectedRows[0].Cells[21].Value)
                {
                    conn.Close();
                    MessageBox.Show("Для данного несоответствия не выполнено ни одного мероприятия! Устранение невозможно!", "Ошибка");
                    return;
                }

                //Обновление записи о несоответствии в БД
                SqlCommand deactivateCheckCommand = conn.CreateCommand();
                deactivateCheckCommand.CommandText = deactiveateCheckQuery;
                deactivateCheckCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);
                deactivateCheckCommand.Parameters.AddWithValue("@worker_id", currentUserId);
                deactivateCheckCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString(LongDateFormat));

                deactivateCheckCommand.ExecuteNonQuery();

                SqlCommand deactivateEventsCommand = conn.CreateCommand();
                deactivateEventsCommand.CommandText = deactivateEventQuery;
                deactivateEventsCommand.Parameters.AddWithValue("@check_id", (int)checkGrid.SelectedRows[0].Cells[0].Value);
                deactivateEventsCommand.Parameters.AddWithValue("@worker_id", currentUserId);
                deactivateEventsCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString(LongDateFormat));

                deactivateEventsCommand.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void incorrectCheckBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Ошибка в несоответствии"
            Вопрос пользователю
            Проверка ознакомившихся: если есть хотя бы одно ознакомление, удаление невозможно
            Проверка мероприятий: если есть хотя бы одно активное, удаление невозможно
            Обновление записи о несоответствии в БД
            */
        {
            try
            {
                if (checkGrid.SelectedRows.Count != 1) return;

                string msg = "Удалить ошибочно зарегистрированное несоответствие №" + checkGrid.SelectedRows[0].Cells[1].Value.ToString() +
                    "; " + checkGrid.SelectedRows[0].Cells[8].Value.ToString() + " от " + checkGrid.SelectedRows[0].Cells[4].Value.ToString().Substring(0, 10) +
                    " от имени пользователя " + currectUserName + "?";

                if (MessageBox.Show(msg, "Устранить несоответствие", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                conn.Open();
                
                //Получение номера регистрирующего пользователя
                SqlCommand regWorkerIdCommand = conn.CreateCommand();
                regWorkerIdCommand.CommandText = regIdWorkerQuery;
                regWorkerIdCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);

                List<int> bossIds = new List<int>();

                object reg_id = regWorkerIdCommand.ExecuteScalar();
                if (reg_id != null)
                    bossIds.Add((int)reg_id);

                //Получение списка руководителей данного участка
                SqlCommand checkSubunitCommand = conn.CreateCommand();
                checkSubunitCommand.CommandText = bossShowBySubunitQuery;
                checkSubunitCommand.Parameters.AddWithValue("@subunit", checkGrid.SelectedRows[0].Cells[15].Value);

                SqlDataReader reader = checkSubunitCommand.ExecuteReader();

                while (reader.Read())
                    bossIds.Add((int)reader[0]);
                reader.Close();

                if (!bossIds.Contains(currentUserId)) //Если пользователь не является начальником, устранить невозможно
                {
                    MessageBox.Show("Вы не можете зарегистрировать ошибку в данном несоответствии, так как у вас нет на это прав!", "Ошибка");
                    conn.Close();
                    return;
                }

                //Подсчет ознакомлений
                SqlCommand countShowsByCheckIdCommand = conn.CreateCommand();
                countShowsByCheckIdCommand.CommandText = countShowsByCheckIdQuery;
                countShowsByCheckIdCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);

                int showsCount = (int)countShowsByCheckIdCommand.ExecuteScalar();

                if (showsCount != 0)
                {
                    conn.Close();
                    MessageBox.Show("С данным несоответствием уже ознакомились! Удаление невозможно!", "Ошибка");
                    return;
                }

                //Подсчет мероприятий
                SqlCommand countEventsByCheckIdCommand = conn.CreateCommand();
                countEventsByCheckIdCommand.CommandText = countEventsByCheckIdQuery;
                countEventsByCheckIdCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);

                int eventsCount = (int)countEventsByCheckIdCommand.ExecuteScalar();

                if (eventsCount != 0)
                {
                    conn.Close();
                    MessageBox.Show("Для данного несоответствия существуют неудаленные мероприятия! Удалите сначала мероприятия!", "Ошибка");
                    return;
                }

                //Обновление записи
                SqlCommand incorrectCheckCommand = conn.CreateCommand();
                incorrectCheckCommand.CommandText = incorrectCheckQuery;
                incorrectCheckCommand.Parameters.AddWithValue("@id", (int)checkGrid.SelectedRows[0].Cells[0].Value);
                incorrectCheckCommand.Parameters.AddWithValue("@worker_id", currentUserId);
                incorrectCheckCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString(LongDateFormat));

                incorrectCheckCommand.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void newEventBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Зарегистрировать мероприятие"
            Отрытие формы создания мероприятия
            Получения данных о мероприятии из формы
            Создание записи о мероприятии в БД
            */
        {
            try
            {
                conn.Open();

                //Инициализация формы создания мероприятия
                NewEvent newEventForm = new NewEvent();
                newEventForm.owner = this;
                newEventForm.Text += " от имени пользователя " + currectUserName;

                if (newEventForm.ShowDialog() != DialogResult.OK)
                {
                    conn.Close();
                    return;
                }

                //Информация о мероприятии
                Event eventt = newEventForm.eventt;

                eventt.developDate = DateTime.Now;
                eventt.developerId = currentUserId;

                //Добавление записи в БД
                SqlCommand insertEventCommand = conn.CreateCommand();
                insertEventCommand.CommandText = insertEventQuery;
                eventt.addInsertParams(insertEventCommand);

                insertEventCommand.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void eventsGridView_DoubleClick(object sender, EventArgs e)
            /*
            Двойное нажатие на строку в eventsGrid
            Проверка прав доступа
            Открытие формы с детализацией мероприятие и с возможностью добавить отчет
            Если отчет добавлен, обновление записи в БД
            */
        {
            try
            {
                if (eventsGridView.SelectedRows.Count != 1) return; //Выделена только одна строка

                //Проверка прав
                if (!currentUserAccess.Contains("EVENT_DETAIL") && !currentUserAccess.Contains("CONTROL") && !currentUserAccess.Contains("ADMIN")) return;

                int id = (int)eventsGridView.SelectedRows[0].Cells[0].Value;

                conn.Open();

                //Получение данных о мероприятии
                SqlCommand findEventCommand = conn.CreateCommand();
                findEventCommand.CommandText = findEventByIdQuery;
                findEventCommand.Parameters.AddWithValue("@id", id);

                SqlDataReader eventReader = findEventCommand.ExecuteReader();

                Event eventt = new Event(eventReader);

                eventReader.Close();

                //Получение данных о несоответствии
                string findCheckById = checkQuery + " AND id = @id";
                SqlCommand findCheckCommand = conn.CreateCommand();
                findCheckCommand.CommandText = findCheckById;
                findCheckCommand.Parameters.AddWithValue("@id", eventt.checkId);

                SqlDataReader checkReader = findCheckCommand.ExecuteReader();

                Check check = new Check(checkReader);

                checkReader.Close();

                //Инициализация формы с отчетом о мероприятии
                ReportEvent reportEventForm = new ReportEvent();
                reportEventForm.eventt = eventt;
                reportEventForm.check = check;
                reportEventForm.owner = this;
                reportEventForm.Text += " от имени пользователя " + currectUserName;

                if (reportEventForm.ShowDialog() != DialogResult.OK)
                {
                    conn.Close();
                    return;
                }

                eventt = reportEventForm.eventt;

                eventt.reportDate = DateTime.Now;
                eventt.reportWorkerId = currentUserId;

                //Обновление записи в БД
                SqlCommand updateEventCommand = conn.CreateCommand();
                updateEventCommand.CommandText = updateEventQuery;
                eventt.addUpdateParams(updateEventCommand);

                updateEventCommand.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void deactivateEvent_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Устранить мероприятие"
            Вопрос пользователю
            Проверка прав доступа
            Обновление записи в БД
            */
        {
            try
            {
                if (eventsGridView.SelectedRows.Count != 1) return; //Выделена только одна строка

                string msg = "Зарегистрировать устранение меропрития для несоответствия №" + eventsGridView.SelectedRows[0].Cells[2].Value.ToString() +
                    "; " + eventsGridView.SelectedRows[0].Cells[5].Value.ToString() +
                    " от имени пользователя " + currectUserName + "?";

                if (MessageBox.Show(msg, "Устранить меропритие", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                conn.Open();

                if (!currentUserAccess.Contains("REPORT"))
                {
                    MessageBox.Show("Вы не можете зарегистрировать устранение данного мероприятия, так как у вас нет прав на его устранение!", "Ошибка");
                    conn.Close();
                    return;
                }

                //Команда на обновление записи в БД
                SqlCommand deactivateEventCommand = conn.CreateCommand();
                deactivateEventCommand.CommandText = deactivateEventQuery;
                deactivateEventCommand.Parameters.AddWithValue("@id", (int)eventsGridView.SelectedRows[0].Cells[0].Value);
                deactivateEventCommand.Parameters.AddWithValue("@worker_id", currentUserId);
                deactivateEventCommand.Parameters.AddWithValue("@delete_date", DateTime.Now.ToString(LongDateFormat));

                deactivateEventCommand.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void incorrectEvent_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Ошибка в мероприятии"
            Вопрос пользователю
            Проверка наличия отчета
            Обновление записи в БД
            */
        {
            try
            {
                if (eventsGridView.SelectedRows.Count != 1) return;

                string msg = "Удалить ошибочно назначенное мероприятие для несоответствия №" + eventsGridView.SelectedRows[0].Cells[2].Value.ToString() +
                    "; " + eventsGridView.SelectedRows[0].Cells[5].Value.ToString() +
                    " от имени пользователя " + currectUserName + "?";

                if (MessageBox.Show(msg, "Устранить меропритие", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                if (eventsGridView.SelectedRows[0].Cells[14].Value.ToString() != "")
                {
                    MessageBox.Show("Невозможно удалить меропритие, для которого предоставлен отчет!", "Ошибка");
                    return;
                }

                conn.Open();

                //Получение номера разрабатывающего пользователя
                SqlCommand devWorkerIdCommand = conn.CreateCommand();
                devWorkerIdCommand.CommandText = devIdWorkerQuery;
                devWorkerIdCommand.Parameters.AddWithValue("@id", (int)eventsGridView.SelectedRows[0].Cells[0].Value);

                List<int> bossIds = new List<int>();

                object dev_id = devWorkerIdCommand.ExecuteScalar();
                if (dev_id != null)
                    bossIds.Add((int)dev_id);

                //Получение списка руководителей данного участка
                SqlCommand checkSubunitCommand = conn.CreateCommand();
                checkSubunitCommand.CommandText = bossShowBySubunitQuery;
                checkSubunitCommand.Parameters.AddWithValue("@subunit", eventsGridView.SelectedRows[0].Cells[11].Value);

                SqlDataReader reader = checkSubunitCommand.ExecuteReader();

                while (reader.Read())
                    bossIds.Add((int)reader[0]);
                reader.Close();

                if (!bossIds.Contains(currentUserId)) //Если пользователь не является начальником, устранить невозможно
                {
                    MessageBox.Show("Вы не можете зарегистрировать ошибку в данном мероприятии, так как у вас нет на это прав!", "Ошибка");
                    conn.Close();
                    return;
                }

                //Команда для обновления записи в БД
                SqlCommand incorrectEventCommand = conn.CreateCommand();
                incorrectEventCommand.CommandText = incorrectEventQuery;
                incorrectEventCommand.Parameters.AddWithValue("@id", (int)eventsGridView.SelectedRows[0].Cells[0].Value);
                incorrectEventCommand.Parameters.AddWithValue("@worker_id", currentUserId);
                incorrectEventCommand.Parameters.AddWithValue("@delete_date", DateTime.Now.ToString(LongDateFormat));

                incorrectEventCommand.ExecuteNonQuery();

                refreshViews();

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void checkGrid_Sorted(object sender, EventArgs e)
            /*
            Сортировка checkGrid
            Выделение красным несоответствий без мероприятий
            */
        {
            try
            {
                conn.Open();

                SqlCommand eventByCheckIdCommand = conn.CreateCommand();
                eventByCheckIdCommand.CommandText = eventsQuery + " AND check_id = @check_id";

                foreach (DataGridViewRow row in checkGrid.Rows)
                {
                    //Номер строки
                    row.HeaderCell.Value = (checkGrid.Rows.IndexOf(row) + 1).ToString();
                    row.DefaultCellStyle.BackColor = Color.White;
                    eventByCheckIdCommand.Parameters.Clear();
                    eventByCheckIdCommand.Parameters.AddWithValue("@check_id", (int)row.Cells[0].Value);
                    object eventt = eventByCheckIdCommand.ExecuteScalar();

                    if (eventt == null && (bool)row.Cells[21].Value) //Нет мероприятий и обнаружено несоответствие
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void reportCheckGrid_SelectionChanged(object sender, EventArgs e)
            /*
            Изменение выбора на вкладке "Отчеты"
            Поиск мероприятий и ознакомлений для выбранного несоответствия
            Инициализация столбцов для ознакомлений и мероприятий
            */
        {
            try
            {
                if (reportCheckGrid.SelectedRows.Count != 1) return; //Выбрана только одна строка

                //Выборка ознакомлений по несоответствию
                string reportShowQuery = showsQuery + " WHERE check_id = " + reportCheckGrid.SelectedRows[0].Cells[0].Value.ToString();

                SqlDataAdapter reportShowsDataAdapter = new SqlDataAdapter(reportShowQuery, conn);

                DataSet reportShowsDS = new DataSet();
                reportShowsDataAdapter.Fill(reportShowsDS);

                reportShowsGrid.DataSource = reportShowsDS.Tables[0].DefaultView;

                //Инициализация столбцов ознакомления
                reportShowsGrid.Columns[0].Visible = false;
                reportShowsGrid.Columns[1].Visible = false;
                reportShowsGrid.Columns[2].HeaderText = "Дата ознакомления";
                reportShowsGrid.Columns[3].HeaderText = "ФИО ознакомившегося";
                reportShowsGrid.Columns[4].HeaderText = "Должность ознакомившегося";
                reportShowsGrid.Columns[5].HeaderText = "Подразделение ознакомившегося";
                reportShowsGrid.Columns[6].Visible = false;

                //Выборка мероприятия по несоответствию
                string reportEventsQry = reportEventsQuery + " WHERE check_id = " + reportCheckGrid.SelectedRows[0].Cells[0].Value.ToString();

                SqlDataAdapter reportEventsDataAdapter = new SqlDataAdapter(reportEventsQry, conn);

                DataSet reportEventsDS = new DataSet();
                reportEventsDataAdapter.Fill(reportEventsDS);

                reportsEventGrid.DataSource = reportEventsDS.Tables[0].DefaultView;

                //Инициализация столбцов мероприятия
                reportsEventGrid.Columns[0].Visible = false;
                reportsEventGrid.Columns[1].Visible = false;
                reportsEventGrid.Columns[2].Visible = false;
                reportsEventGrid.Columns[3].Visible = false;
                reportsEventGrid.Columns[4].Visible = false;
                reportsEventGrid.Columns[5].Visible = false;
                reportsEventGrid.Columns[6].HeaderText = "Причина несоответствия";
                reportsEventGrid.Columns[7].HeaderText = "Описание мероприятия";
                reportsEventGrid.Columns[8].HeaderText = "Ответственный";
                reportsEventGrid.Columns[9].HeaderText = "Срок исполнения";
                reportsEventGrid.Columns[10].HeaderText = "Дата разработки";
                reportsEventGrid.Columns[11].HeaderText = "Подразделение разработчика";
                reportsEventGrid.Columns[12].HeaderText = "Должность разработчика";
                reportsEventGrid.Columns[13].HeaderText = "ФИО разработчика";
                reportsEventGrid.Columns[14].HeaderText = "Отчет";
                reportsEventGrid.Columns[15].HeaderText = "Подтверждающая информация";
                reportsEventGrid.Columns[16].HeaderText = "Дата отчета";
                reportsEventGrid.Columns[17].HeaderText = "Подразделение сотрудника, предоставившего отчет";
                reportsEventGrid.Columns[18].HeaderText = "Должность сотрудника, предоставившего отчет";
                reportsEventGrid.Columns[19].HeaderText = "ФИО сотрудника, предоставившего отчет";
                reportsEventGrid.Columns[20].Visible = false;
                reportsEventGrid.Columns[21].Visible = false;
                reportsEventGrid.Columns[22].HeaderText = "Дата удаления";
                reportsEventGrid.Columns[23].HeaderText = "Причина удаления";
                reportsEventGrid.Columns[24].HeaderText = "ФИО удалившего мероприятие сотрудника";
                reportsEventGrid.Columns[25].HeaderText = "Должность удалившего мероприятие сотрудника";
                reportsEventGrid.Columns[26].HeaderText = "Подразделение удалившего мероприятие сотрудника";
                reportsEventGrid.Columns[27].Visible = false;
                reportsEventGrid.Columns[28].Visible = false;
                reportsEventGrid.Columns[29].Visible = false;

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Обновить"
            Вызов процедуры refreshViews
            */
        {
            try
            {
                conn.Open();
                refreshViews();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void reportToXls(List<TableString> data)
            /*
            Процедура для формирования отчета в Excel
            Принимает данные в списке объектов TableString
            Открывает приложение Excel с книгой с одним листом
            Заносит данные из списка в Excel, рисует рамки
            */
        {
            try
            {
                //Инициализация переменной приложения Excel
                excelApp = new Excel.Application();
                //Приложение не видно пользователю
                excelApp.Visible = false;
                //Один лист в новой книге
                excelApp.SheetsInNewWorkbook = 1;
                //Отключение вывода сообщений пользователю (нужно для объединения ячеек)
                excelApp.DisplayAlerts = false;
                //Создание книги
                excelApp.Workbooks.Add();

                //Переменная рабочего листа
                Excel.Worksheet worksheet = excelApp.Worksheets[1];
                //Переменная ячеек (будет менять значение)
                Excel.Range cells;

                #region Шапка таблицы

                cells = worksheet.Cells[1, 1]; //Выбор ячейки
                cells.Value2 = "Номер несоответствия"; //Текст
                cells.EntireColumn.ColumnWidth = 16; //Ширина в относительных единицах Excel (не пиксели и не милиметры)
                createBorders(cells); //Создание границ ячеек

                cells = worksheet.Cells[1, 3];
                cells.Value2 = "Проверяемое подразделение";
                cells.EntireColumn.ColumnWidth = 16;
                createBorders(cells);

                cells = worksheet.Cells[1, 4];
                cells.Value2 = "Проверяемый участок";
                cells.EntireColumn.ColumnWidth = 16;
                createBorders(cells);

                cells = worksheet.Cells[1, 2];
                cells.Value2 = "Дата проверки";
                cells.EntireColumn.ColumnWidth = 11;
                createBorders(cells);

                cells = worksheet.Cells[1, 8];
                cells.Value2 = "ФИО проверяющего";
                cells.EntireColumn.ColumnWidth = 16;
                createBorders(cells);

                cells = worksheet.Cells[1, 7];
                cells.Value2 = "Обозначение комплекта документов (ТД, КД)";
                cells.EntireColumn.ColumnWidth = 14;
                createBorders(cells);

                cells = worksheet.Cells[1, 5];
                cells.Value2 = "Объект контроля";
                cells.EntireColumn.ColumnWidth = 24;
                createBorders(cells);

                cells = worksheet.Cells[1, 6];
                cells.Value2 = "Результат контроля";
                cells.EntireColumn.ColumnWidth = 28;
                createBorders(cells);

                cells = worksheet.Cells[1, 11];
                cells.Value2 = "Дата регистрации несоответствия";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 10];
                cells.Value2 = "ФИО и должность регистрирующего сотрудника";
                cells.EntireColumn.ColumnWidth = 21;
                createBorders(cells);

                cells = worksheet.Cells[1, 9];
                cells.Value2 = "Подразделение регистрирующего сотрудника";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 12];
                cells.Value2 = "Дата удаления несоответствия";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 13];
                cells.Value2 = "Причина удаления несоответствия";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 14];
                cells.Value2 = "ФИО и должность удалившего несоответствие сотрудника";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 15];
                cells.Value2 = "Подразделение удалившего несоответствие сотрудника";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 16];
                cells.Value2 = "Дата ознакомления";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 17];
                cells.Value2 = "ФИО и должность ознакомившегося";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 18];
                cells.Value2 = "Подразделение ознакомившегося";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 19];
                cells.Value2 = "Причина несоответствия";
                cells.EntireColumn.ColumnWidth = 24;
                createBorders(cells);

                cells = worksheet.Cells[1, 20];
                cells.Value2 = "Описание мероприятия";
                cells.EntireColumn.ColumnWidth = 28;
                createBorders(cells);

                cells = worksheet.Cells[1, 21];
                cells.Value2 = "Ответственный";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 22];
                cells.Value2 = "Срок исполнения";
                cells.EntireColumn.ColumnWidth = 14;
                createBorders(cells);

                cells = worksheet.Cells[1, 23];
                cells.Value2 = "Дата разработки";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 24];
                cells.Value2 = "ФИО и должность разработчика";
                cells.EntireColumn.ColumnWidth = 16;
                createBorders(cells);

                cells = worksheet.Cells[1, 25];
                cells.Value2 = "Подразделение разработчика";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 26];
                cells.Value2 = "Отчет";
                cells.EntireColumn.ColumnWidth = 13;
                createBorders(cells);

                cells = worksheet.Cells[1, 27];
                cells.Value2 = "Подтверждающая информация";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                cells = worksheet.Cells[1, 28];
                cells.Value2 = "Дата отчета";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 29];
                cells.Value2 = "ФИО и должность сотрудника, предоставившего отчет";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                cells = worksheet.Cells[1, 30];
                cells.Value2 = "Подразделение сотрудника, предоставившего отчет";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                cells = worksheet.Cells[1, 31];
                cells.Value2 = "Дата удаления";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 32];
                cells.Value2 = "Причина удаления";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                cells = worksheet.Cells[1, 33];
                cells.Value2 = "ФИО и должность удалившего мероприятие сотрудника";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                cells = worksheet.Cells[1, 34];
                cells.Value2 = "Подразделение удалившего мероприятие сотрудника";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                #endregion
                //strNum - Текущая строка
                //startStr - Первая строка каждого несоответствия
                //endStr - последняя строка каждого несоответствия
                //showStrNum - Текущая строка ознакомления
                //eventStrNum - Текущая строка мероприятия
                int strNum = 2, startStr = 2, endStr, showStrNum = 2, eventStrNum = 2;

                foreach (TableString str in data)
                {
                    if (strNum != 2) //Для каждого несоответствия, кроме первого
                    {
                        //Последняя строка - большая из ознакомлений и мероприятий минус один
                        endStr = showStrNum > eventStrNum ? showStrNum - 1 : eventStrNum - 1;
                        if (strNum < showStrNum || strNum < eventStrNum) //Присвоение strNum следующего значения, если он меньше ознакомлений или мероприятий
                            strNum = showStrNum > eventStrNum ? showStrNum : eventStrNum;
                        
                        //endStr не должен быть меньше startStr
                        if (endStr < startStr) endStr = startStr;

                        #region Объединение ячеек
                        cells = worksheet.Range["A" + startStr.ToString(), "A" + endStr.ToString()]; //Выбор всех ячеек одного столбца текущего несоответствия
                        cells.Merge(); //Объединение ячеек
                        createBorders(cells); //Создание общей рамки

                        cells = worksheet.Range["B" + startStr.ToString(), "B" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["C" + startStr.ToString(), "C" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["D" + startStr.ToString(), "D" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["E" + startStr.ToString(), "E" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["F" + startStr.ToString(), "F" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["G" + startStr.ToString(), "G" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["H" + startStr.ToString(), "H" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["I" + startStr.ToString(), "I" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["J" + startStr.ToString(), "J" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["K" + startStr.ToString(), "K" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["L" + startStr.ToString(), "L" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["M" + startStr.ToString(), "M" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["N" + startStr.ToString(), "N" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["O" + startStr.ToString(), "O" + endStr.ToString()];
                        cells.Merge();
                        createBorders(cells);

                        cells = worksheet.Range["P" + startStr.ToString(), "P" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["Q" + startStr.ToString(), "Q" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["R" + startStr.ToString(), "R" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["S" + startStr.ToString(), "S" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["T" + startStr.ToString(), "T" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["U" + startStr.ToString(), "U" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["V" + startStr.ToString(), "V" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["W" + startStr.ToString(), "W" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["X" + startStr.ToString(), "X" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["Y" + startStr.ToString(), "Y" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["Z" + startStr.ToString(), "Z" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AA" + startStr.ToString(), "AA" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AB" + startStr.ToString(), "AB" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AC" + startStr.ToString(), "AC" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AD" + startStr.ToString(), "AD" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AE" + startStr.ToString(), "AE" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AF" + startStr.ToString(), "AF" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AG" + startStr.ToString(), "AG" + endStr.ToString()];
                        createBorders(cells);

                        cells = worksheet.Range["AH" + startStr.ToString(), "AH" + endStr.ToString()];
                        createBorders(cells);
                        #endregion

                        startStr = strNum; //Новый startStr
                    }

                    #region Заполнение данными
                    cells = worksheet.Cells[strNum, 1];
                    cells.Value2 = str.check.failCount;

                    cells = worksheet.Cells[strNum, 3];
                    cells.Value2 = str.check.checkSubunit;

                    cells = worksheet.Cells[strNum, 4];
                    cells.Value2 = str.check.sector;

                    cells = worksheet.Cells[strNum, 2];
                    cells.Value2 = str.check.checkDate.ToShortDateString();

                    cells = worksheet.Cells[strNum, 8];
                    cells.Value2 = str.check.checkWorker;

                    cells = worksheet.Cells[strNum, 7];
                    cells.Value2 = str.check.td_kd;

                    cells = worksheet.Cells[strNum, 5];
                    cells.Value2 = str.check.controlIndicator;

                    cells = worksheet.Cells[strNum, 6];
                    cells.Value2 = str.check.failDescr;

                    cells = worksheet.Cells[strNum, 11];
                    cells.Value2 = str.check.regDate.ToShortDateString() + " " + str.check.regDate.ToShortTimeString();

                    cells = worksheet.Cells[strNum, 10];
                    cells.Value2 = str.check.regWorker;

                    cells = worksheet.Cells[strNum, 9];
                    cells.Value2 = str.check.regWorkerSubunit;

                    if (str.check.deleteReason != "-") //Пустая дата отображается некорректно
                    {
                        cells = worksheet.Cells[strNum, 12];
                        cells.Value2 = str.check.deleteDate.ToShortDateString() + " " + str.check.deleteDate.ToShortTimeString();

                        cells = worksheet.Cells[strNum, 14];
                        cells.Value2 = str.check.deleteWorker;
                    }

                    cells = worksheet.Cells[strNum, 13];
                    cells.Value2 = str.check.deleteReason;

                    cells = worksheet.Cells[strNum, 15];
                    cells.Value2 = str.check.deleteSubunit;

                    showStrNum = strNum;

                    foreach (Show show in str.shows)
                    {
                        cells = worksheet.Cells[showStrNum, 16];
                        cells.Value2 = show.date.ToShortDateString() + " " + show.date.ToShortTimeString();

                        cells = worksheet.Cells[showStrNum, 17];
                        cells.Value2 = show.worker;

                        cells = worksheet.Cells[showStrNum, 18];
                        cells.Value2 = show.subunit;

                        ++showStrNum;
                    }

                    eventStrNum = strNum;

                    foreach (Event eventt in str.events)
                    {
                        cells = worksheet.Cells[eventStrNum, 19];
                        cells.Value2 = eventt.failReason;

                        cells = worksheet.Cells[eventStrNum, 20];
                        cells.Value2 = eventt.description;

                        cells = worksheet.Cells[eventStrNum, 21];
                        cells.Value2 = eventt.responsWorker;

                        cells = worksheet.Cells[eventStrNum, 22];
                        cells.Value2 = eventt.dueDate.ToShortDateString();

                        cells = worksheet.Cells[eventStrNum, 23];
                        cells.Value2 = eventt.developDate.ToShortDateString() + " " + eventt.developDate.ToShortTimeString();

                        cells = worksheet.Cells[eventStrNum, 24];
                        cells.Value2 = eventt.developer;

                        cells = worksheet.Cells[eventStrNum, 25];
                        cells.Value2 = eventt.developerSubunit;

                        cells = worksheet.Cells[eventStrNum, 26];
                        cells.Value2 = eventt.report;

                        cells = worksheet.Cells[eventStrNum, 27];
                        cells.Value2 = eventt.proofInf;

                        if (eventt.report != null && eventt.report != "")
                        {
                            cells = worksheet.Cells[eventStrNum, 28];
                            cells.Value2 = eventt.reportDate.ToShortDateString() + " " + eventt.reportDate.ToShortTimeString();

                            cells = worksheet.Cells[eventStrNum, 29];
                            cells.Value2 = eventt.reportWorker;
                        }

                        cells = worksheet.Cells[eventStrNum, 30];
                        cells.Value2 = eventt.reportWorkerSubunit;

                        if (eventt.deleteReason != "-")
                        {
                            cells = worksheet.Cells[eventStrNum, 32];
                            cells.Value2 = eventt.deleteReason;

                            cells = worksheet.Cells[eventStrNum, 33];
                            cells.Value2 = eventt.deleteWorker;

                            cells = worksheet.Cells[eventStrNum, 31];
                            cells.Value2 = eventt.deleteDate.ToShortDateString() + " " + eventt.deleteDate.ToShortTimeString();
                        }

                        cells = worksheet.Cells[eventStrNum, 34];
                        cells.Value2 = eventt.deleteSubunit;

                        ++eventStrNum;
                    }
                    #endregion
                    ++strNum;
                }

                if (strNum != 2) //Объединение ячеек последнего несоответствия
                {
                    endStr = showStrNum > eventStrNum ? showStrNum - 1 : eventStrNum - 1;
                    if (endStr < strNum - 1) endStr = strNum - 1;

                    #region Объединение ячеек
                    cells = worksheet.Range["A" + startStr.ToString(), "A" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["B" + startStr.ToString(), "B" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["C" + startStr.ToString(), "C" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["D" + startStr.ToString(), "D" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["E" + startStr.ToString(), "E" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["F" + startStr.ToString(), "F" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["G" + startStr.ToString(), "G" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["H" + startStr.ToString(), "H" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["I" + startStr.ToString(), "I" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["J" + startStr.ToString(), "J" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["K" + startStr.ToString(), "K" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["L" + startStr.ToString(), "L" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["M" + startStr.ToString(), "M" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["N" + startStr.ToString(), "N" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["O" + startStr.ToString(), "O" + endStr.ToString()];
                    cells.Merge();
                    createBorders(cells);

                    cells = worksheet.Range["P" + startStr.ToString(), "P" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["Q" + startStr.ToString(), "Q" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["R" + startStr.ToString(), "R" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["S" + startStr.ToString(), "S" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["T" + startStr.ToString(), "T" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["U" + startStr.ToString(), "U" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["V" + startStr.ToString(), "V" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["W" + startStr.ToString(), "W" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["X" + startStr.ToString(), "X" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["Y" + startStr.ToString(), "Y" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["Z" + startStr.ToString(), "Z" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AA" + startStr.ToString(), "AA" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AB" + startStr.ToString(), "AB" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AC" + startStr.ToString(), "AC" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AD" + startStr.ToString(), "AD" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AE" + startStr.ToString(), "AE" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AF" + startStr.ToString(), "AF" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AG" + startStr.ToString(), "AG" + endStr.ToString()];
                    createBorders(cells);

                    cells = worksheet.Range["AH" + startStr.ToString(), "AH" + endStr.ToString()];
                    createBorders(cells);
                    #endregion
                }

                cells = worksheet.Range["A1", "AI65536"];//Выбор всех ячеек
                cells.WrapText = true;//Включение переноса по словам
                //Выравнивание по центру ячейки
                cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                //Включение отображения сообщений пользователю
                excelApp.DisplayAlerts = true;
                //Отображение приложения пользователю
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                if (excelApp != null) excelApp.Quit();
                Environment.Exit(0);
            }
        }

        private void createBorders(Excel.Range cells)
            //Создание внешних границ выбранного диапазона ячеек Excel
        {
            Excel.XlBordersIndex BorderIndex;

            BorderIndex = Excel.XlBordersIndex.xlEdgeLeft;
            cells.Borders[BorderIndex].Weight = Excel.XlBorderWeight.xlThin;
            cells.Borders[BorderIndex].LineStyle = Excel.XlLineStyle.xlContinuous;
            cells.Borders[BorderIndex].ColorIndex = 0;


            BorderIndex = Excel.XlBordersIndex.xlEdgeTop;
            cells.Borders[BorderIndex].Weight = Excel.XlBorderWeight.xlThin;
            cells.Borders[BorderIndex].LineStyle = Excel.XlLineStyle.xlContinuous;
            cells.Borders[BorderIndex].ColorIndex = 0;


            BorderIndex = Excel.XlBordersIndex.xlEdgeBottom;
            cells.Borders[BorderIndex].Weight = Excel.XlBorderWeight.xlThin;
            cells.Borders[BorderIndex].LineStyle = Excel.XlLineStyle.xlContinuous;
            cells.Borders[BorderIndex].ColorIndex = 0;

            BorderIndex = Excel.XlBordersIndex.xlEdgeRight;
            cells.Borders[BorderIndex].Weight = Excel.XlBorderWeight.xlThin;
            cells.Borders[BorderIndex].LineStyle = Excel.XlLineStyle.xlContinuous;
            cells.Borders[BorderIndex].ColorIndex = 0;
        }

        private void reportToXlsBtn_Click(object sender, EventArgs e)
            /*
            Нажатие на кнопку "Сформировать отчет в Excel"
            Выборка всех несоответствий, мероприятий и ознакомлений
            Формирования списка объектов TableString
            Вызов процедуры reportToXls
            */
        {
            try
            {

                List<TableString> data = new List<TableString>(); //Все данные

                conn.Open();

                //Команда на выборку всех несооответствий
                SqlCommand allCheckCommand = conn.CreateCommand();
                allCheckCommand.CommandText = reportCheckQuery;

                SqlDataReader allCheckReader = allCheckCommand.ExecuteReader();

                do
                {
                    Check check = new Check(allCheckReader);
                    if (check.failCount == null || check.failCount == "") break; //Пока не получено пустое несоответствие
                    data.Add(new TableString(check));
                } while (true);

                allCheckReader.Close();

                foreach (TableString tableString in data) //Перебор найденных несоответствий
                {
                    //Команда на выборку мероприятий для каждого несоответствия
                    SqlCommand eventByCheckIdCommand = conn.CreateCommand();
                    eventByCheckIdCommand.CommandText = reportEventsQuery + " WHERE check_id = " + tableString.check.id.ToString();

                    SqlDataReader eventReader = eventByCheckIdCommand.ExecuteReader();

                    while (true)
                    {
                        Event eventt = new Event(eventReader);
                        if (eventt.description == null || eventt.description == "") break; //Пока не встречено пустое мероприятие
                        tableString.events.Add(eventt);
                    }

                    eventReader.Close();

                    //Коменда для выборки ознакомлений для каждого несоответствия
                    SqlCommand showsByCheckIdCommand = conn.CreateCommand();
                    showsByCheckIdCommand.CommandText = showsQuery + " WHERE check_id = " + tableString.check.id.ToString();

                    SqlDataReader showsReader = showsByCheckIdCommand.ExecuteReader();

                    while (true)
                    {
                        Show show = new Show(showsReader);
                        if (show.worker == null || show.worker == "") break; //Пока не получено пустое ознакомление
                        tableString.shows.Add(show);
                    }

                    showsReader.Close();
                }

                reportToXls(data);

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
            /*
            Переключение вкладок
            Происходит обновление таблиц
            */
        {
            refreshBtn_Click(refreshBtn, null);
        }

        private void changeUserBtn_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                LoginForm loginForm = new LoginForm();
                loginForm.owner = this;

                if (loginForm.ShowDialog() != DialogResult.OK) //Если нажата кнопка "Отмена"
                {
                    conn.Close();
                    Environment.Exit(0);
                }

                currentUserId = loginForm.currentUserId; //АйДи пользователя в БД
                currentUserSubunit = loginForm.currentUserSubunit; //Подразделение пользователя в БД
                currentUserAccess = loginForm.currentUserAccess; //Права доступа пользователя
                currectUserName = loginForm.currectUserName; //ФИО пользователя

                Text = "Журнал контроля технологической дисциплины от имени пользователя " + currectUserName;

                if (currentUserAccess.Contains("CHECK_DETAIL") || currentUserAccess.Contains("CONTROL"))
                    checkDataAdapter = new SqlDataAdapter(checkQuery, conn); //Инициализация данных о несоответствиях
                else
                    checkDataAdapter = new SqlDataAdapter(checkQuery + " AND id = 0", conn); //Пустая таблица

                if (currentUserAccess.Contains("EVENT_DETAIL") || currentUserAccess.Contains("CONTROL"))
                    eventsDataAdapter = new SqlDataAdapter(eventsQuery, conn); //Инициализация данных о мероприятиях
                else
                    eventsDataAdapter = new SqlDataAdapter(eventsQuery + " AND id = 0", conn); //Пустая таблица

                if (currentUserAccess.Contains("ADMIN"))
                {
                    reportCheckDataAdapter = new SqlDataAdapter(reportCheckQuery, conn); //Инициализация данных для отчёта
                }
                else
                {
                    //Проверка прав доступа пользователя

                    if (!currentUserAccess.Contains("REG")) //Блокировка кнопки "Зарегистрировать несоответствие"
                        newCheckBtn.Enabled = false;
                    else
                        newCheckBtn.Enabled = true;

                    if (!currentUserAccess.Contains("DEVEL")) //Блокировка кнопки "Зарегистрировать мероприятие"
                        newEventBtn.Enabled = false;
                    else
                        newEventBtn.Enabled = true;

                    if (currentUserAccess.Contains("CONTROL") || currentUserAccess.Contains("REPORT_SHOW")) //Получение данных для отчета
                    {
                        reportCheckDataAdapter = new SqlDataAdapter(reportCheckQuery, conn);
                    }
                    else
                    {
                        reportCheckDataAdapter = null;
                    }
                }

                refreshViews(); //Заполнение данными GridViews

                conn.Close();

                if (!currentUserAccess.Contains("CONTROL") && !currentUserAccess.Contains("REPORT_SHOW")) 
                {
                    reportTabPage.Parent = null;
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void checkGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (checkGrid.SelectedRows.Count == 1)
                selectedCheckId = (int)checkGrid.SelectedRows[0].Cells[0].Value;
        }

        private void eventsGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (eventsGridView.SelectedRows.Count == 1)
                selectedEventId = (int)eventsGridView.SelectedRows[0].Cells[0].Value;
        }

        private void shortReportToXlsBtn_Click(object sender, EventArgs e)
        {
            try
            {

                List<TableString> data = new List<TableString>(); //Все данные

                conn.Open();

                //Команда на выборку всех несооответствий
                SqlCommand allCheckCommand = conn.CreateCommand();
                allCheckCommand.CommandText = reportCheckQuery;

                SqlDataReader allCheckReader = allCheckCommand.ExecuteReader();

                do
                {
                    Check check = new Check(allCheckReader);
                    if (check.failCount == null || check.failCount == "") break; //Пока не получено пустое несоответствие
                    data.Add(new TableString(check));
                } while (true);

                allCheckReader.Close();

                foreach (TableString tableString in data) //Перебор найденных несоответствий
                {
                    //Команда на выборку мероприятий для каждого несоответствия
                    SqlCommand eventByCheckIdCommand = conn.CreateCommand();
                    eventByCheckIdCommand.CommandText = reportEventsQuery + " WHERE check_id = " + tableString.check.id.ToString();

                    SqlDataReader eventReader = eventByCheckIdCommand.ExecuteReader();

                    while (true)
                    {
                        Event eventt = new Event(eventReader);
                        if (eventt.description == null || eventt.description == "") break; //Пока не встречено пустое мероприятие
                        tableString.events.Add(eventt);
                    }

                    eventReader.Close();
                }

                shortReportToXls(data);

                conn.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                Environment.Exit(0);
            }
        }

        private void shortReportToXls(List<TableString> data)
        /*
        Процедура для формирования краткого отчета в Excel
        Принимает данные в списке объектов TableString
        Открывает приложение Excel с книгой с одним листом
        Заносит данные из списка в Excel, рисует рамки
        */
        {
            try
            {
                //Инициализация переменной приложения Excel
                excelApp = new Excel.Application();
                //Приложение не видно пользователю
                excelApp.Visible = false;
                //Один лист в новой книге
                excelApp.SheetsInNewWorkbook = 1;
                //Отключение вывода сообщений пользователю (нужно для объединения ячеек)
                excelApp.DisplayAlerts = false;
                //Создание книги
                excelApp.Workbooks.Add();

                //Переменная рабочего листа
                Excel.Worksheet worksheet = excelApp.Worksheets[1];
                //Переменная ячеек (будет менять значение)
                Excel.Range cells;

                #region Шапка таблицы

                cells = worksheet.Cells[1, 1]; //Выбор ячейки
                cells.Value2 = "Номер несоответствия"; //Текст
                cells.EntireColumn.ColumnWidth = 16; //Ширина в относительных единицах Excel (не пиксели и не милиметры)
                createBorders(cells); //Создание границ ячеек

                cells = worksheet.Cells[1, 2];
                cells.Value2 = "Дата проверки";
                cells.EntireColumn.ColumnWidth = 11;
                createBorders(cells);

                cells = worksheet.Cells[1, 3];
                cells.Value2 = "Проверяемое подразделение";
                cells.EntireColumn.ColumnWidth = 16;
                createBorders(cells);

                cells = worksheet.Cells[1, 4];
                cells.Value2 = "Проверяемый участок";
                cells.EntireColumn.ColumnWidth = 16;
                createBorders(cells);

                cells = worksheet.Cells[1, 5];
                cells.Value2 = "Объект контроля";
                cells.EntireColumn.ColumnWidth = 24;
                createBorders(cells);

                cells = worksheet.Cells[1, 6];
                cells.Value2 = "Результат контроля";
                cells.EntireColumn.ColumnWidth = 28;
                createBorders(cells);

                cells = worksheet.Cells[1, 7];
                cells.Value2 = "Обозначение комплекта документов (ТД, КД)";
                cells.EntireColumn.ColumnWidth = 14;
                createBorders(cells);

                cells = worksheet.Cells[1, 8];
                cells.Value2 = "Подразделение регистрирующего сотрудника";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 9];
                cells.Value2 = "Причина несоответствия";
                cells.EntireColumn.ColumnWidth = 24;
                createBorders(cells);

                cells = worksheet.Cells[1, 10];
                cells.Value2 = "Описание мероприятия";
                cells.EntireColumn.ColumnWidth = 28;
                createBorders(cells);

                cells = worksheet.Cells[1, 11];
                cells.Value2 = "Ответственный";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 12];
                cells.Value2 = "Срок исполнения";
                cells.EntireColumn.ColumnWidth = 14;
                createBorders(cells);

                cells = worksheet.Cells[1, 13];
                cells.Value2 = "Дата разработки";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 14];
                cells.Value2 = "Подразделение разработчика";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 15];
                cells.Value2 = "Отчет";
                cells.EntireColumn.ColumnWidth = 13;
                createBorders(cells);

                cells = worksheet.Cells[1, 16];
                cells.Value2 = "Подтверждающая информация";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                cells = worksheet.Cells[1, 17];
                cells.Value2 = "Дата отчета";
                cells.EntireColumn.ColumnWidth = 17;
                createBorders(cells);

                cells = worksheet.Cells[1, 18];
                cells.Value2 = "ФИО и должность сотрудника, предоставившего отчет";
                cells.EntireColumn.ColumnWidth = 20;
                createBorders(cells);

                #endregion
                //strNum - Текущая строка
                //startStr - Первая строка каждого несоответствия
                //endStr - последняя строка каждого несоответствия
                //showStrNum - Текущая строка ознакомления
                //eventStrNum - Текущая строка мероприятия
                int strNum = 2, startStr = 2, endStr, showStrNum = 2, eventStrNum = 2;

                #region Заполнение данными
                foreach (TableString str in data)
                {
                    if (str.events.Count == 0)
                    {
                        cells = worksheet.Cells[strNum, 1];
                        cells.Value2 = str.check.failCount;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 2];
                        cells.Value2 = str.check.checkDate.ToShortDateString();
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 3];
                        cells.Value2 = str.check.checkSubunit;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 4];
                        cells.Value2 = str.check.sector;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 5];
                        cells.Value2 = str.check.controlIndicator;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 6];
                        cells.Value2 = str.check.failDescr;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 7];
                        cells.Value2 = str.check.td_kd;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 8];
                        cells.Value2 = str.check.regWorkerSubunit;
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 9];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 10];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 11];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 12];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 13];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 14];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 15];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 16];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 17];
                        createBorders(cells);

                        cells = worksheet.Cells[strNum, 18];
                        createBorders(cells);

                        ++strNum;
                    }
                    else foreach (Event eventt in str.events)
                        {
                            cells = worksheet.Cells[strNum, 1];
                            cells.Value2 = str.check.failCount;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 2];
                            cells.Value2 = str.check.checkDate.ToShortDateString();
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 3];
                            cells.Value2 = str.check.checkSubunit;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 4];
                            cells.Value2 = str.check.sector;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 5];
                            cells.Value2 = str.check.controlIndicator;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 6];
                            cells.Value2 = str.check.failDescr;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 7];
                            cells.Value2 = str.check.td_kd;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 8];
                            cells.Value2 = str.check.regWorkerSubunit;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 9];
                            cells.Value2 = eventt.failReason;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 10];
                            cells.Value2 = eventt.description;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 11];
                            cells.Value2 = eventt.responsWorker;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 12];
                            cells.Value2 = eventt.dueDate.ToShortDateString();
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 13];
                            cells.Value2 = eventt.developDate.ToShortDateString() + " " + eventt.developDate.ToShortTimeString();
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 14];
                            cells.Value2 = eventt.developerSubunit;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 15];
                            cells.Value2 = eventt.report;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 16];
                            cells.Value2 = eventt.proofInf;
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 17];
                            createBorders(cells);

                            cells = worksheet.Cells[strNum, 18];
                            createBorders(cells);

                            if (eventt.report != null && eventt.report != "")
                            {
                                cells = worksheet.Cells[strNum, 17];
                                cells.Value2 = eventt.reportDate.ToShortDateString() + " " + eventt.reportDate.ToShortTimeString();
                                createBorders(cells);

                                cells = worksheet.Cells[strNum, 18];
                                cells.Value2 = eventt.reportWorker;
                                createBorders(cells);
                            }

                            ++strNum;
                        }
                }
                #endregion

                cells = worksheet.Range["A1", "AI65536"];//Выбор всех ячеек
                cells.WrapText = true;//Включение переноса по словам
                //Выравнивание по центру ячейки
                cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                //Включение отображения сообщений пользователю
                excelApp.DisplayAlerts = true;
                //Отображение приложения пользователю
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("В ходе работы программы возникла ошибка! Обратитесь к системному администратору!" +
                    "\nСкопировать сообщение об ошибке в буфер обмена?", "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Clipboard.SetText(ex.Message);

                if (excelApp != null) excelApp.Quit();
                Environment.Exit(0);
            }
        }
    }

    public class Check //Несоответствие
    {
        public int id;//Идентификатор несоответствия
        public int regWorkerId; //Номер сотрудника, зарегистрировавшего несоответствие
        public DateTime regDate; //Дата регистрации
        public DateTime checkDate; //Дата проверка
        public string checkWorker; //Проверяющий
        public string checkSubunit; //Проверяемое подразделение
        public string sector; //Проверяемый участок
        public string td_kd; //Обозначение комплекта документов (ТД, КД)
        public string controlIndicator; //Объект контроля
        public int operCount; //Количество проверенных операций
        public string failCount; //Номер несоответствия
        public string failDescr; //Результат контроля

        public string regWorker; //ФИО и должность регистрирующего сотрудника
        public string regWorkerSubunit; //Подразделение регистрирующего сотрудника

        public DateTime deleteDate; //Дата удаления
        public string deleteReason; //Причина удаления
        public string deleteWorker; //ФИО и должность удалившего
        public string deleteSubunit; //Подразделение удалившего

        public bool isFail; //Обнаружены несоответствия?

        public DataSet eventsDataSet; //Мероприятия из БД
        public SqlDataAdapter showsDataAdapter; //Ознакомления из БД

        public Check()
        {
            operCount = 0;
        }

        public Check(SqlDataReader reader) //Формирование несоответствия из DataReader
        {
            if (!reader.Read())
            {
                //reader.Close();
                return;
            }

            id = (int)reader["id"];
            regDate = DateTime.Parse(reader["reg_date"].ToString());
            checkDate = DateTime.Parse(reader["check_date"].ToString());
            checkWorker = reader["check_worker"].ToString();
            checkSubunit = reader["check_subunit"].ToString();
            sector = reader["sector"].ToString();
            td_kd = reader["td_kd"].ToString();
            controlIndicator = reader["control_indicator"].ToString();
            operCount = (int)reader["count_operations"];
            failCount = reader["fail_count"].ToString();
            failDescr = reader["fail_description"].ToString();
            regWorker = reader["reg_worker"].ToString() + ", " + reader["post"].ToString();
            regWorkerSubunit = reader["subunit"].ToString();

            deleteReason = reader["delete_reason"].ToString();
            deleteWorker = reader["Delete_Worker"].ToString() + ", " + reader["Delete_Post"].ToString();
            deleteSubunit = reader["Delete_Subunit"].ToString();

            if (deleteReason != "-")
                deleteDate = DateTime.Parse(reader["delete_date"].ToString());

            //reader.Close();
        }

        public void addPars(SqlCommand command) //Формирования параметров для команды добавления записи о несоответствии в БД
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@reg_worker", regWorkerId);
            command.Parameters.AddWithValue("@reg_date", regDate.ToString(JournalDBMainForm.LongDateFormat));
            command.Parameters.AddWithValue("@check_date", checkDate.ToString(JournalDBMainForm.ShortDateFormat));
            command.Parameters.AddWithValue("@check_worker", checkWorker);
            command.Parameters.AddWithValue("@check_subunit", checkSubunit);
            command.Parameters.AddWithValue("@sector", sector);
            command.Parameters.AddWithValue("@td_kd", td_kd);
            command.Parameters.AddWithValue("@control_indicator", controlIndicator);
            command.Parameters.AddWithValue("@count_operations", operCount);
            command.Parameters.AddWithValue("@fail_count", failCount);
            command.Parameters.AddWithValue("@fail_description", failDescr);
            command.Parameters.AddWithValue("@isFail", isFail ? 1 : 0);
        }

        public void addParsWithCondition(SqlCommand command) //Формирование параметров для проверки существования несоответствия в БД
        {
            command.Parameters.AddWithValue("@check_date", checkDate.ToString(JournalDBMainForm.ShortDateFormat));
            command.Parameters.AddWithValue("@check_worker", checkWorker);
            command.Parameters.AddWithValue("@check_subunit", checkSubunit);
            command.Parameters.AddWithValue("@td_kd", td_kd);
            command.Parameters.AddWithValue("@control_indicator", controlIndicator);
            command.Parameters.AddWithValue("@fail_description", failDescr);
        }
    }

    public class Event //Мероприятие
    {
        public int id; //Идентификатор мероприятия
        public int checkId; //Идентификатор несоответствия
        public int developerId; //Номер разработчика
        public int reportWorkerId; //Номер предоставившего отчет
        public string failReason; //Причина несоответствия
        public string description; //Описание мероприятия
        public string responsWorker; //Ответственный
        public DateTime dueDate; //Срок исполнения
        public DateTime developDate; //Дата разработки
        public string report; //Отчет
        public string proofInf; //Подтверждающая информация
        public DateTime reportDate; //Дата отчета

        public string developerSubunit; //Подразделение разработчика
        public string developer; //ФИО и должность разработчика
        public string reportWorkerSubunit; //Подразделение предоставившего отчет
        public string reportWorker; //ФИО и должность предоставившего отчет

        public DateTime deleteDate; //Дата удаления
        public string deleteReason; //Причина удаление
        public string deleteWorker; //ФИО и должность удалившего
        public string deleteSubunit; //Подразделение удалившего

        public Event()
        { }

        public Event(SqlDataReader reader) //Формирование данных мероприятия из DataReader
        {
            if (!reader.Read())
            {
                //reader.Close();
                return;
            }

            id = (int)reader["id"];
            developerId = (int)reader["dev_id"];
            checkId = (int)reader["check_id"];
            failReason = reader["fail_reason"].ToString();
            description = reader["description"].ToString();
            responsWorker = reader["respons_worker"].ToString();
            dueDate = DateTime.Parse(reader["due_date"].ToString());
            developDate = DateTime.Parse(reader["develop_date"].ToString());
            developerSubunit = reader["Devel_subunit"].ToString();
            developer = reader["Developer"].ToString() + ", " + reader["Devel_post"].ToString();
            report = reader["report"].ToString();
            proofInf = reader["proof_inf"].ToString();
            
            deleteReason = reader["delete_reason"].ToString();
            deleteWorker = reader["Delete_Worker"].ToString() + ", " + reader["Delete_Post"].ToString();
            deleteSubunit = reader["Delete_Subunit"].ToString();

            if (deleteReason != "-")
                deleteDate = DateTime.Parse(reader["delete_date"].ToString());

            if (report != null && report != "")
            {
                reportDate = DateTime.Parse(reader["report_date"].ToString());
                reportWorkerSubunit = reader["Rep_subunit"].ToString();
                reportWorker = reader["Rep_worker"].ToString() + ", " + reader["Rep_post"].ToString();
            }

            //reader.Close();
        }

        public void addInsertParams(SqlCommand command) //Формирование параметров для добавления записи в БД
        {
            command.Parameters.AddWithValue("@checkId", checkId);
            command.Parameters.AddWithValue("@developerId", developerId);
            command.Parameters.AddWithValue("@failReason", failReason);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@responsWorker", responsWorker);
            command.Parameters.AddWithValue("@dueDate", dueDate.ToString(JournalDBMainForm.ShortDateFormat));
            command.Parameters.AddWithValue("@developDate", developDate.ToString(JournalDBMainForm.LongDateFormat));
        }

        public void addUpdateParams(SqlCommand command) //Формирование параметров для обновления записи в БД
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@report", report);
            command.Parameters.AddWithValue("@proofInf", proofInf);
            command.Parameters.AddWithValue("@reportDate", reportDate.ToString(JournalDBMainForm.LongDateFormat));
            command.Parameters.AddWithValue("@reportWorker", reportWorkerId);
        }
    }

    public class Show //Ознакомление
    {
        public int id; //Идентификатор ознакомления
        public int checkId; //Идентификатор несоответствия
        public DateTime date; //Дата ознакомления
        public string worker; //ФИО и должность ознакомившегося
        public string subunit; //Подразделение ознакомившегося

        public Show() { }

        public Show(SqlDataReader reader) //Формирование данных из DataReader
        {
            if (!reader.Read())
                return;

            id = int.Parse(reader["id"].ToString());
            checkId = int.Parse(reader["check_id"].ToString());
            date = DateTime.Parse(reader["date"].ToString());
            worker = reader["worker"].ToString() + ", " + reader["post"].ToString();
            subunit = reader["subunit"].ToString();
        }
    }

    public class TableString //Строка данных в Excel
    {
        public Check check; //Несоответствие
        public List<Event> events; //Список мероприятий
        public List<Show> shows; //Список ознакомлений

        public TableString()
        {
            events = new List<Event>();
            shows = new List<Show>();
        }

        public TableString(Check check) : this()
        {
            this.check = check;
        }

        public TableString(Check check, List<Event> events, List<Show> shows)
        {
            this.check = check;
            this.events = events;
            this.shows = shows;
        }
    }
}

