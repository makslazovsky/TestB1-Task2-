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

        public async Task<BalanceInfoFile> GetFile(int fileId)
        {
            
            return await dbAccessor.GetFile(fileId);
        }

        public BalanceInfoFile GetFileByName(string fileName)
        {
            var test = dbAccessor.GetFileByName(fileName);
            return test;
        }

        public async Task<BalanceInfoRecord> GetRootRecordOfFileContent(int fileId)
        {
            var recordsByLevel = new Dictionary<int, BalanceInfoRecord>();
            var records = await dbAccessor.GetFileContent(fileId);
           
            foreach(var record in  records.OrderBy(x => x.Level))
            {
                recordsByLevel[record.AccountNumber] = record;
                if(record.ParentAccountNumber != null)
                {
                    recordsByLevel[record.ParentAccountNumber.Value].Children.Add(record);
                }
            }

            return recordsByLevel[0];
        }

        public async Task UploadFile(string filePath)
        {
            // Номер строки, с которой начинаются данные
            const int rowsToIgnore = 7;

            try
            {
                // Чтение данных из файла Excel
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    BalanceInfoFile fileInfo = new BalanceInfoFile();
                    fileInfo.FileName = Path.GetFileName(filePath);

                    List<BalanceInfoRecord> recorcds = new List<BalanceInfoRecord>();
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Перемещение к нужному листу
                        reader.Read();

                        // Пропускаем хидер документа
                        int currentRowNumber = 1;
                        while (reader.Read() && currentRowNumber < rowsToIgnore)
                        {
                            currentRowNumber++;
                        }

                        recorcds = ReadDocumentRows(reader);

                        // Закрытие чтения файла Excel
                        reader.Close();
                    }

                    if(recorcds != null)
                    {
                        await dbAccessor.UploadFile(fileInfo, recorcds);
                    }
                }
            }
            catch (Exception ex)
            {
                log.ShowError(ex.ToString());
            }
        }

        private List<BalanceInfoRecord> ReadDocumentRows(IExcelDataReader reader)
        {
            int level1AccountId = 0;
            int level2AcountId = 0;
            List<BalanceInfoRecord> records = new List<BalanceInfoRecord>();
            BalanceInfoRecord currentLevel0Record = new BalanceInfoRecord { Level = 0, AccountNumber = 0, ParentAccountNumber = null };
            BalanceInfoRecord currentLevel1Record = null;
            BalanceInfoRecord currentLevel2Record = null;
            try
            {
                while (reader.Read())
                {

                    bool readAllColumns = true;
                    string accountColumnText = reader.GetValue(0)?.ToString();

                    if (string.IsNullOrEmpty(accountColumnText))
                    {
                        continue;
                    }

                    BalanceInfoRecord infoRecord = new BalanceInfoRecord();
                    if (int.TryParse(accountColumnText, out var accountId))
                    {
                        var currentLevel2Number = accountId / 100;
                        if (currentLevel2Number > 0)
                        {
                            //строка уровня 4
                            if(level2AcountId != currentLevel2Number)
                            {
                                currentLevel2Record = new BalanceInfoRecord();
                                level2AcountId = currentLevel2Number;
                                currentLevel2Record.AccountNumber = currentLevel2Number;
                            }

                            infoRecord.Level = 4;
                            infoRecord.ParentAccountNumber = currentLevel2Record.AccountNumber;
                            records.Add(infoRecord);
                        }
                        else
                        {
                            //строка уровня 2
                            infoRecord = currentLevel2Record;
                            infoRecord.Level = 2;
                            infoRecord.ParentAccountNumber = currentLevel1Record.AccountNumber;
                            records.Add(infoRecord);
                        }

                        infoRecord.AccountNumber = accountId;
                    }
                    else
                    {
                        if (accountColumnText.Trim().StartsWith("КЛАСС  "))
                        {
                            //строка уровня 1
                            level1AccountId++;
                            infoRecord.AccountNumber = level1AccountId;
                            infoRecord.Level = 1;
                            readAllColumns = false;

                            currentLevel1Record = infoRecord;
                            currentLevel1Record.Description = accountColumnText;
                            infoRecord.ParentAccountNumber = currentLevel0Record.AccountNumber;
                            records.Add(infoRecord);
                        }
                        if (accountColumnText.Trim().StartsWith("БАЛАНС"))
                        {
                            //строка уровня 0
                            infoRecord = currentLevel0Record;
                            infoRecord.Description = "БАЛАНС";
                            records.Add(infoRecord);
                        }
                    }

                    if (readAllColumns)
                    {
                        infoRecord.OpeningBalanceAsset = Convert.ToDecimal(reader.GetValue(1));
                        infoRecord.OpeningBalanceLiability = Convert.ToDecimal(reader.GetValue(2));
                        infoRecord.DebitTurnover = Convert.ToDecimal(reader.GetValue(3));
                        infoRecord.CreditTurnover = Convert.ToDecimal(reader.GetValue(4));
                        infoRecord.ClosingBalanceAsset = Convert.ToDecimal(reader.GetValue(5));
                        infoRecord.ClosingBalanceLiability = Convert.ToDecimal(reader.GetValue(6));
                    }
                }

                return records;
            }
            catch (Exception)
            {
                log.ShowError("Неверный формат файла");
                return null;
            }
        }
    }
}
