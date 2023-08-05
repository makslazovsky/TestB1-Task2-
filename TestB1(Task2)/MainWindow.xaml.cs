using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace TestB1_Task2_
{
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=b1db;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
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

                try
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    // Чтение данных из файла Excel
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            // Перемещение к нужному листу
                            reader.Read();

                            // Номер строки, с которой начинаются данные
                            int startRowNumber = 10;

                            int currentRowNumber = 0;
                            // Чтение данных из Excel и сохранение в таблицу DataTable
                            DataTable dataTable = new DataTable();
                            for (int i = 0; i < 8; i++)
                            {
                                dataTable.Columns.Add();
                            }

                            while (reader.Read())
                            {
                                if (currentRowNumber>=startRowNumber)
                                {
                                    DataRow row = dataTable.NewRow();

                                    for (int i = 0; i < 8; i++)
                                    {
                                        row[i] = reader.GetValue(i);
                                    }

                                    dataTable.Rows.Add(row);
                                }
                                currentRowNumber++;
                                
                            }

                            // Закрытие чтения файла Excel
                            reader.Close();

                            // Вставка в базу данных
                            await InsertDataAsync(filePath, dataTable);
                         
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при загрузке файла: " + ex.Message);
                }
            }
        }
        private async Task InsertDataAsync(string filePath, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Вставка данных в таблицу FileTable
                SqlCommand selectMaxFileCommand = new SqlCommand("SELECT MAX(Id) FROM FileTable", connection);
                int maxFileId = (int)selectMaxFileCommand.ExecuteScalar() + 1;

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                SqlCommand insertFileCommand = new SqlCommand("INSERT INTO FileTable (Id, FileName) VALUES (@Id, @FileName)", connection);
                insertFileCommand.Parameters.AddWithValue("@Id", maxFileId);
                insertFileCommand.Parameters.AddWithValue("@FileName", fileName);
                await insertFileCommand.ExecuteNonQueryAsync();

                int id = 0;
                // Вставка данных из DataTable в базу данных MSSQL
                SqlCommand selectMaxCommand = new SqlCommand("SELECT MAX(Id) FROM FileTable ", connection);
                int maxId = (int)selectMaxCommand.ExecuteScalar() + 1;
                id = maxId;

                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {

                        string accountNumber = row[0].ToString();
                        decimal openingBalanceAsset = decimal.Parse(row[1].ToString());
                        decimal openingBalanceLiability = decimal.Parse(row[2].ToString());
                        decimal debitTurnover = decimal.Parse(row[3].ToString());
                        decimal creditTurnover = decimal.Parse(row[4].ToString());
                        decimal closingBalanceAsset = decimal.Parse(row[5].ToString());
                        decimal closingBalanceLiability = decimal.Parse(row[6].ToString());

                        // Вставка данных в таблицу BalanceResult
                        SqlCommand insertCommand = new SqlCommand("INSERT INTO BalanceResult (Id, AccountNumber, OpeningBalanceAsset, OpeningBalanceLiability, DebitTurnover, CreditTurnover, ClosingBalanceAsset, ClosingBalanceLiability, FileID) " +
                                                                                     "VALUES (@Id, @AccountNumber, @OpeningBalanceAsset, @OpeningBalanceLiability, @DebitTurnover, @CreditTurnover, @ClosingBalanceAsset, @ClosingBalanceLiability, @FileID)", connection);
                        insertCommand.Parameters.AddWithValue("@Id", id);
                        insertCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        insertCommand.Parameters.AddWithValue("@OpeningBalanceAsset", openingBalanceAsset);
                        insertCommand.Parameters.AddWithValue("@OpeningBalanceLiability", openingBalanceLiability);
                        insertCommand.Parameters.AddWithValue("@DebitTurnover", debitTurnover);
                        insertCommand.Parameters.AddWithValue("@CreditTurnover", creditTurnover);
                        insertCommand.Parameters.AddWithValue("@ClosingBalanceAsset", closingBalanceAsset);
                        insertCommand.Parameters.AddWithValue("@ClosingBalanceLiability", closingBalanceLiability);
                        insertCommand.Parameters.AddWithValue("@FileID", maxFileId);
                        await insertCommand.ExecuteNonQueryAsync();
                        id++;
                    }
                    catch (Exception)
                    {
                    }

                }

                // Закрытие соединения с базой данных
                await connection.CloseAsync();

                MessageBox.Show("Файл успешно загружен в базу данных.");
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить выбранный элемент combobox
            var selectedFileName = fileComboBox.SelectedItem as string;

            // Подключиться к базе данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID FROM FileTable WHERE FileName = @FileName";
                SqlCommand scalarCommand = new SqlCommand(query, connection);
                scalarCommand.Parameters.AddWithValue("@FileName", selectedFileName);
                int idFile = (int)scalarCommand.ExecuteScalar();

                SqlCommand command = new SqlCommand($"SELECT AccountNumber, OpeningBalanceAsset, OpeningBalanceLiability, DebitTurnover, CreditTurnover, ClosingBalanceAsset, ClosingBalanceLiability FROM BalanceResult  WHERE FileID = {idFile}", connection); 

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                excelGrid.ItemsSource = dataTable.DefaultView;
            }

            
        }
        private void LoadFileList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT FileName FROM FileTable", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<string> fileList = new List<string>();

                    while (reader.Read())
                    {
                        fileList.Add(reader.GetString(0));
                    }

                    fileComboBox.ItemsSource = fileList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file list: " + ex.Message);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
           LoadFileList();
        }
    }
  
}
