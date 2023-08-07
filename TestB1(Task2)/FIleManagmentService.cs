using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestB1_Task2_.DAL;
using TestB1_Task2_.Models;
using FileInfo = TestB1_Task2_.Models.FileInfo;

namespace TestB1_Task2_
{
    public class FileManagmentService: IFileManagmentService
    {
        private IDBAccessor dbAccessor;
        ILog log;

        public FileManagmentService(IDBAccessor dbAccessor, ILog log)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            this.dbAccessor = dbAccessor;
            this.log = log;
        }

        public Task<List<FileInfo>> GetFiles()
        {
            return dbAccessor.GetFiles();
        }
        public Task<FileInfo> GetFile(int fileId)
        {
            return dbAccessor.GetFile(fileId);
        }

        public async Task<List<FileRecord>> GetFileContent(int fileId)
        {
            var records =  await dbAccessor.GetFileContent(fileId);
            return records.OrderBy(x => x.AccountNumber).ToList();
        }

        public async Task UploadFile(string filePath)
        {
            try
            {
                // Чтение данных из файла Excel
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    FileInfo fileInfo = null;
                    List<FileRecord> fileRecords = null;

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Перемещение к нужному листу
                        reader.Read();

                        // Номер строки, с которой начинаются данные
                        int startRowNumber = 10;

                        int currentRowNumber = 0;

                        //TODO:

                        //// Чтение данных из Excel и сохранение в таблицу DataTable
                        //DataTable dataTable = new DataTable();
                        //for (int i = 0; i < 8; i++)
                        //{
                        //    dataTable.Columns.Add();
                        //}

                        while (reader.Read())
                        {
                            if (currentRowNumber >= startRowNumber)
                            {
                                //DataRow row = dataTable.NewRow();

                                //for (int i = 0; i < 8; i++)
                                //{
                                //    row[i] = reader.GetValue(i);
                                //}

                                //dataTable.Rows.Add(row);
                            }
                            currentRowNumber++;

                        }

                        // Закрытие чтения файла Excel
                        reader.Close();

                        //// Вставка в базу данных
                        //await InsertDataAsync(filePath, dataTable);
                    }

                    await dbAccessor.UploadFile(fileInfo, fileRecords);
                }
            }
            catch (Exception ex)
            {
                log.ShowError("Произошла ошибка при загрузке файла: " + ex.Message);
            }
        }
    }
}
