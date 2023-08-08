using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить выбранный элемент combobox
            //var selectedFileInfo = fileComboBox.SelectedItem as BalanceInfoFile;
            var selectedFileInfo = fileManagmentService.GetFileByName(fileComboBox.SelectedItem.ToString());
            if (selectedFileInfo != null)
            {
                try
                {
                    var fileInfo = await fileManagmentService.GetFile(selectedFileInfo.Id);
                    var rootRecord = await fileManagmentService.GetRootRecordOfFileContent(selectedFileInfo.Id);
                    BindFileInfoAsync(fileInfo, rootRecord);
                }
                catch (Exception ex)
                {
                    ShowError("Ошибка при смене файла: " + ex.Message);
                }
            }
        }
        private async void LoadFileList()
        {
            try
            {
                var fileInfos = await fileManagmentService.GetFiles();
                List<string> files = new List<string>();
                foreach (var file in fileInfos)
                {
                    files.Add(file.FileName);
                }
                fileComboBox.ItemsSource = files;

                //fileComboBox.ItemsSource = fileInfos;

                //Получить корневой элемент для отображения
                var fileinfo = fileInfos.FirstOrDefault();
                if (fileinfo != null)
                {
                    var record = await fileManagmentService.GetRootRecordOfFileContent(fileinfo.Id);
                    BindFileInfoAsync(fileinfo, record);
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при загрузке списка файлов: " + ex.Message);
            }
        }

        private void BindFileInfoAsync(BalanceInfoFile info, BalanceInfoRecord rootRecord)
        {
            excelGrid.ItemsSource = new[] { rootRecord };
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFileList();
        }

        public void ShowError(string info)
        {
            MessageBox.Show(info);
        }
    }

}
