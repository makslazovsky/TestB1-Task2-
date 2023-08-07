using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TestB1_Task2_.DAL;
using TestB1_Task2_.Models;

namespace TestB1_Task2_
{
    public partial class MainWindow : Window, ILog
    {
        IFilesDbContextFactory dbContextFactory;
        IDBAccessor dbAccessor;
        IFileManagmentService fileManagmentService;

        public MainWindow()
        {
            InitializeComponent();
            dbContextFactory = new FilesDbContextFactory();
            dbAccessor = new DBAccessor(dbContextFactory);
            fileManagmentService = new FileManagmentService(dbAccessor, this);

            LoadFileList();
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            // Выбор файла Excel для загрузки
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                await fileManagmentService.UploadFile(filePath);
            }
        }
        //private async Task InsertDataAsync(string filePath, DataTable dataTable)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();

        //        // Вставка данных в таблицу FileTable
        //        SqlCommand selectMaxFileCommand = new SqlCommand("SELECT MAX(Id) FROM FileTable", connection);
        //        int maxFileId = (int)selectMaxFileCommand.ExecuteScalar() + 1;

        //        string fileName = Path.GetFileNameWithoutExtension(filePath);
        //        SqlCommand insertFileCommand = new SqlCommand("INSERT INTO FileTable (Id, FileName) VALUES (@Id, @FileName)", connection);
        //        insertFileCommand.Parameters.AddWithValue("@Id", maxFileId);
        //        insertFileCommand.Parameters.AddWithValue("@FileName", fileName);
        //        await insertFileCommand.ExecuteNonQueryAsync();

        //        int id = 0;
        //        // Вставка данных из DataTable в базу данных MSSQL
        //        SqlCommand selectMaxCommand = new SqlCommand("SELECT MAX(Id) FROM FileTable ", connection);
        //        int maxId = (int)selectMaxCommand.ExecuteScalar() + 1;
        //        id = maxId;

        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            try
        //            {

        //                string accountNumber = row[0].ToString();
        //                decimal openingBalanceAsset = decimal.Parse(row[1].ToString());
        //                decimal openingBalanceLiability = decimal.Parse(row[2].ToString());
        //                decimal debitTurnover = decimal.Parse(row[3].ToString());
        //                decimal creditTurnover = decimal.Parse(row[4].ToString());
        //                decimal closingBalanceAsset = decimal.Parse(row[5].ToString());
        //                decimal closingBalanceLiability = decimal.Parse(row[6].ToString());

        //                // Вставка данных в таблицу BalanceResult
        //                SqlCommand insertCommand = new SqlCommand("INSERT INTO BalanceResult (Id, AccountNumber, OpeningBalanceAsset, OpeningBalanceLiability, DebitTurnover, CreditTurnover, ClosingBalanceAsset, ClosingBalanceLiability, FileID) " +
        //                                                                             "VALUES (@Id, @AccountNumber, @OpeningBalanceAsset, @OpeningBalanceLiability, @DebitTurnover, @CreditTurnover, @ClosingBalanceAsset, @ClosingBalanceLiability, @FileID)", connection);
        //                insertCommand.Parameters.AddWithValue("@Id", id);
        //                insertCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
        //                insertCommand.Parameters.AddWithValue("@OpeningBalanceAsset", openingBalanceAsset);
        //                insertCommand.Parameters.AddWithValue("@OpeningBalanceLiability", openingBalanceLiability);
        //                insertCommand.Parameters.AddWithValue("@DebitTurnover", debitTurnover);
        //                insertCommand.Parameters.AddWithValue("@CreditTurnover", creditTurnover);
        //                insertCommand.Parameters.AddWithValue("@ClosingBalanceAsset", closingBalanceAsset);
        //                insertCommand.Parameters.AddWithValue("@ClosingBalanceLiability", closingBalanceLiability);
        //                insertCommand.Parameters.AddWithValue("@FileID", maxFileId);
        //                await insertCommand.ExecuteNonQueryAsync();
        //                id++;
        //            }
        //            catch (Exception)
        //            {
        //            }

        //        }

        //        // Закрытие соединения с базой данных
        //        await connection.CloseAsync();

        //        MessageBox.Show("Файл успешно загружен в базу данных.");
        //    }
        //}
        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить выбранный элемент combobox
            var selectedFileName = fileComboBox.SelectedItem as string;
            var fileInfo = await fileManagmentService.GetFile(0); //TODO: pass id
            var records = await fileManagmentService.GetFileContent(0); //TODO: pass id
            BindFileInfoAsync(fileInfo, records);


            //TODO: bind fileInfo + fileRecords to UI
            //excelGrid.ItemsSource = dataTable.DefaultView;

            // Подключиться к базе данных
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    string query = "SELECT ID FROM FileTable WHERE FileName = @FileName";
            //    SqlCommand scalarCommand = new SqlCommand(query, connection);
            //    scalarCommand.Parameters.AddWithValue("@FileName", selectedFileName);
            //    int idFile = (int)scalarCommand.ExecuteScalar();

            //    SqlCommand command = new SqlCommand($"SELECT AccountNumber, OpeningBalanceAsset, OpeningBalanceLiability, DebitTurnover, CreditTurnover, ClosingBalanceAsset, ClosingBalanceLiability FROM BalanceResult  WHERE FileID = {idFile}", connection); 

            //    SqlDataAdapter adapter = new SqlDataAdapter(command);
            //    DataTable dataTable = new DataTable();
            //    adapter.Fill(dataTable);

            //    excelGrid.ItemsSource = dataTable.DefaultView;
            //}
        }
        private async void LoadFileList()
        {
            try
            {
                var fileInfos = await fileManagmentService.GetFiles();
                //TODO: bind to fileComboBox
                fileComboBox.ItemsSource = fileInfos;

                //TODO: get curently selected item and bind it
                var fileinfo = fileInfos.FirstOrDefault();
                if (fileinfo != null)
                {
                    var records = await fileManagmentService.GetFileContent(fileinfo.Id);
                    BindFileInfoAsync(fileinfo, records);
                }


                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                //    connection.Open();

                //    SqlCommand command = new SqlCommand("SELECT FileName FROM FileTable", connection);
                //    SqlDataReader reader = command.ExecuteReader();

                //    List<string> fileList = new List<string>();

                //    while (reader.Read())
                //    {
                //        fileList.Add(reader.GetString(0));
                //    }

                //    fileComboBox.ItemsSource = fileList;
                //}
            }
            catch (Exception ex)
            {
                ShowError("Error loading file list: " + ex.Message);
            }
        }

        private void BindFileInfoAsync(BalanceInfoFile info, List<BalanceInfoRecord> records)
        {
            //TODO:
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFileList();
        }

        public void ShowError(string info)
        {
            MessageBox.Show(info);
            //TODO: show dialog box
        }
    }

}
