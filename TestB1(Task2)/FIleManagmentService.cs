using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestB1_Task2_.DAL;
using TestB1_Task2_.Models;
using BalanceInfoFile = TestB1_Task2_.Models.BalanceInfoFile;

namespace TestB1_Task2_
{
    public class FileManagmentService: IFileManagmentService
    {
        private IDBAccessor dbAccessor;
        ILog log;
        // Номер строки, с которой начинаются данные
        const int startRowNumber = 10;

        public FileManagmentService(IDBAccessor dbAccessor, ILog log)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            this.dbAccessor = dbAccessor;
            this.log = log;
        }

        public async Task<List<BalanceInfoFile>> GetFiles()
        {
            var list = await dbAccessor.GetFiles();
            return list;
        }
        public Task<BalanceInfoFile> GetFile(int fileId)
        {
            
            return dbAccessor.GetFile(fileId);
        }

        public async Task<List<BalanceInfoRecord>> GetFileContent(int fileId)
        {
            return await dbAccessor.GetFileContent(fileId);
        }

        public async Task UploadFile(string filePath)
        {
            try
            {
                // Чтение данных из файла Excel
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    BalanceInfoFile fileInfo = new BalanceInfoFile();
                    List<BalanceInfoRecord> fileRecords = new List<BalanceInfoRecord>();

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Перемещение к нужному листу
                        reader.Read();

                        int currentRowNumber = 0;

                        fileInfo.FileName = Path.GetFileName(filePath);

                        while (reader.Read())
                        {
                            if (currentRowNumber >= startRowNumber)
                            {
                                try
                                {
                                    BalanceInfoRecord infoRecord = new BalanceInfoRecord();
                                    infoRecord.AccountNumber = Convert.ToString(reader.GetValue(0));
                                    infoRecord.OpeningBalanceAsset = Convert.ToDecimal(reader.GetValue(1));
                                    infoRecord.OpeningBalanceLiability = Convert.ToDecimal(reader.GetValue(2));
                                    infoRecord.DebitTurnover = Convert.ToDecimal(reader.GetValue(3));
                                    infoRecord.CreditTurnover = Convert.ToDecimal(reader.GetValue(4));
                                    infoRecord.ClosingBalanceAsset = Convert.ToDecimal(reader.GetValue(5));
                                    infoRecord.ClosingBalanceLiability = Convert.ToDecimal(reader.GetValue(6));
                                    fileRecords.Add(infoRecord);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            currentRowNumber++;
                        }
                        // Закрытие чтения файла Excel
                        reader.Close();
                    }

                    await dbAccessor.UploadFile(fileInfo, fileRecords);
                }
            }
            catch (Exception ex)
            {
                log.ShowError(ex.ToString());
            }
        }
    }
}
