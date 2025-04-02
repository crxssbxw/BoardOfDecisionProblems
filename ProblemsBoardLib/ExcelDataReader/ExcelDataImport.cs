using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProblemsBoardLib.Models;
using System.IO;
using ClosedXML.Excel;

namespace ProblemsBoardLib.ExcelDataReader
{
    public class ExcelDataImport
    {
        private string filePath = "";
        private string allowedFormats = "Файлы Excel 2007 (*.xlsx)|*.xlsx";
        public bool IsPathAllowed
        {
            get => !string.IsNullOrEmpty(filePath);
        }
        public bool IsRightFormat
        {
            get
            {
                var workbook = new XLWorkbook(filePath);

                if (workbook.Worksheets.Count == 2 )
                {
                    if (workbook.Worksheets.Contains("Участки") && workbook.Worksheets.Contains("Сотрудники"))
                    {
                        var DepartmentWS = workbook.Worksheets.Where(a => a.Worksheet.Name == "Участки").FirstOrDefault();
                        var WorkersWS = workbook.Worksheets.Where(a => a.Worksheet.Name == "Сотрудники").FirstOrDefault();

                        if (DepartmentWS.ColumnsUsed().Count() == 3 && WorkersWS.ColumnsUsed().Count() == 6)
                            return true;
                    }
                }
                return false;
            }
        }

        public string FilePath
        {
            get => filePath;
        }

        public ObservableCollection<Department>? Departments { get; set; } = new();
        public ObservableCollection<Worker>? Workers { get; set; } = new();

        public void AskFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = allowedFormats;
            
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
        }

        public void GetDropped(string path)
        {
            filePath = path;
        }

        public void Read()
        {
            if (!IsPathAllowed)
            { 
                MessageBox.Show("Файл не открыт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); 
                return; 
            }
            if (!IsRightFormat)
            {
                MessageBox.Show("Неправильный формат данных для импорта!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var workbook = new XLWorkbook(filePath);

            var DepartmentWS = workbook.Worksheets.Where(a => a.Worksheet.Name == "Участки").FirstOrDefault();
            var WorkersWS = workbook.Worksheets.Where(a => a.Worksheet.Name == "Сотрудники").FirstOrDefault();

            bool isFirstRow = true;

            foreach (var row in DepartmentWS.RowsUsed())
            {
                if (isFirstRow)
                {
                    isFirstRow = false;
                    continue;
                }

                Department department = new()
                {
                    DepartmentId = row.Cell(1).GetValue<int>(),
                    Name = row.Cell(2).GetString(),
                    ViewerNumber = row.Cell(3).GetString(),
                };
                Departments.Add(department);
            }

            isFirstRow = true;

            foreach (var row in WorkersWS.RowsUsed())
            {
                if (isFirstRow)
                {
                    isFirstRow = false;
                    continue;
                }

                Worker worker = new()
                {
                    WorkerId = row.Cell(1).GetValue<int>(),
                    FirstName = row.Cell(2).GetString(),
                    SecondName = row.Cell(3).GetString(),
                    MiddleName = row.Cell(4).GetString(),
                    Post = row.Cell(5).GetString(),
                    DepartmentId = row.Cell(6).GetValue<int>(),
                };
                Workers.Add(worker);
            }
        }
    }
}
