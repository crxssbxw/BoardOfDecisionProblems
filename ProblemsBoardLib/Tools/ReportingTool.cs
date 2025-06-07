using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Office.Interop.Word;
using ProblemsBoardLib.Models;

namespace ProblemsBoardLib.Tools 
{
    public class ReportingTool : IDisposable
    {
        private Application wordApp;
        private Document document;
        private DateTime Today { get; } = DateTime.Today.Date;
        private DatabaseContext databaseContext = new();

        public FileInfo Xps { get; set; }

        public ReportingTool(bool visibility)
        {
            wordApp = new Application()
            {
                Visible = visibility
            };
            document = wordApp.Documents.Add();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                document?.Close(WdSaveOptions.wdDoNotSaveChanges);
                wordApp?.Quit();
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(wordApp);
                GC.Collect();
            }
        }

        private void SetStyle()
        {
            Style style = document.Styles[WdBuiltinStyle.wdStyleNormal];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;
            style.ParagraphFormat.SpaceAfter = 0;
            style.ParagraphFormat.SpaceBefore = 0;
            style.ParagraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5;
        }

        private string BuildProblemText(Problem problem)
        {
            if (problem.Status == "Решена")
            {
                return BuildSolvedProblemText(problem);
            }
            if (problem.DaysLeft < 1 && problem.Status == "Решается")
            {
                return BuildOverdueProblemText(problem);
            }
            if (problem.Status == "Решается")
            {
                return BuildSolvingProblemText(problem);
            }
            return "Неизвестно";
        }

        private string BuildOverdueProblemText(Problem problem)
        {
            return $"Проблема с ID {problem.ProblemId} " +
                    $"(тема: {problem.ThemeName}), " +
                    $"возникшая {problem.DateOccurance.ToShortDateString()}, просрочена. " +
                    $"Срок: {problem.Theme.DaysToDecide} дней. " +
                    //$"Ответственный: {problem.Responsible.FullName} " +
                    $"(участок {problem.Department.ViewerNumber} - {problem.Department.Name}). " +
                    $"Описание: {problem.Description}. " +
                    $"Текущий статус: {problem.Status}. ";
        }

        private string BuildSolvingProblemText(Problem problem)
        {
            return $"Проблема с ID {problem.ProblemId} " +
                    $"(тема: {problem.ThemeName}), " +
                    $"возникшая {problem.DateOccurance.ToShortDateString()}, " +
                    $"находится в процессе решения. " +
                    $"На решение потрачено {problem.DecisionTime} дней из {problem.Theme.DaysToDecide} дн. " +
                    $"Осталось {problem.DaysLeft} дней. " +
                    //$"Ответственный: {problem.Responsible.FullName} " +
                    $"(участок {problem.Department.ViewerNumber} - {problem.Department.Name}). " +
                    $"Описание: {problem.Description}. ";
        }

        private string BuildSolvedProblemText(Problem problem)
        {
            return $"Проблема с ID {problem.ProblemId} " +
                    $"(тема: {problem.ThemeName}), " +
                    $"возникшая {problem.DateOccurance.ToShortDateString()}, " +
                    $"была успешно решена {problem.DateElimination?.ToShortDateString()} " +
                    $"за {problem.DecisionTime} дней. " +
                    $"Описание: {problem.Description}. " +
                    //$"Ответственный: {problem.Responsible.FullName} " +
                    $"(участок {problem.Department.ViewerNumber} - {problem.Department.Name}). " +
                    $"Принятое решение: {problem.Decision}. ";
        }

        public void GenerateProblemReport(Problem problem)
        {
            // Установка стиля
            SetStyle();
            document.Content.Paragraphs.Format.RightIndent = 250;
            document.Content.Paragraphs.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            // Шапка 
            Paragraph header = document.Content.Paragraphs.Add();
            header.Range.Text = $"Акционерное общество \rКовровский Электромеханический Завод \r";
            header.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            header.Format.RightIndent = 250;

            // Дата под шапкой
            string datepar = $"Отчет от {Today.ToShortDateString()} \rг. Ковров\r";
            Paragraph date = document.Content.Paragraphs.Add();
            date.Range.Text = datepar;
            date.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            date.Format.RightIndent = 0;

            Paragraph paragraph = document.Content.Paragraphs.Add();

			Table problemTable = document.Tables.Add(paragraph.Range, 5, 2); // Например, 5 строк и 2 столбца

			problemTable.Columns[1].Width = wordApp.InchesToPoints(2.5f);
			problemTable.Columns[2].Width = wordApp.InchesToPoints(4.5f);

			problemTable.Cell(1, 1).Range.Text = "Параметр";
			problemTable.Cell(1, 2).Range.Text = "Значение";

			problemTable.Cell(2, 1).Range.Text = "ID";
			problemTable.Cell(2, 2).Range.Text = problem.ProblemId.ToString();

			problemTable.Cell(3, 1).Range.Text = "Описание";
			problemTable.Cell(3, 2).Range.Text = problem.Description;

			problemTable.Cell(4, 1).Range.Text = "Дата возникновения";
			problemTable.Cell(4, 2).Range.Text = problem.DateOccurance.ToShortDateString();

			problemTable.Cell(5, 1).Range.Text = "Статус";
			problemTable.Cell(5, 2).Range.Text = problem.Status.ToString();

			problemTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
			problemTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

			SaveXpsFile();
        }

        public void GenerateStatisticReport(string general, string responsible = "", string theme = "")
        {
            // Установка стиля
            SetStyle();
            document.Content.Paragraphs.Format.RightIndent = 250;
            document.Content.Paragraphs.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            // Шапка 
            Paragraph header = document.Content.Paragraphs.Add();
            header.Range.Text = $"Акционерное общество \rКовровский Электромеханический Завод \r";

            // Дата под шапкой
            string datepar = $"Отчет от {Today.ToShortDateString()} \rг. Ковров\r";
            Paragraph date = document.Content.Paragraphs.Add();
            date.Range.Text = datepar;
            date.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            date.FirstLineIndent = wordApp.CentimetersToPoints(1.25f);
            date.Format.RightIndent = 0;
            date.Format.SpaceBefore = 8;


            // Основной параграф
            Paragraph generalpar = document.Content.Paragraphs.Add();
            if (!string.IsNullOrEmpty(general))
            {
                generalpar.Range.Text = general;
                generalpar.SpaceBefore = 0;
            }

            if (!string.IsNullOrEmpty(responsible))
            {
                Paragraph responsiblepar = document.Content.Paragraphs.Add();
                responsiblepar.Range.Text = responsible;
            }

            if (!string.IsNullOrEmpty(theme))
            {
                Paragraph paragraph = document.Content.Paragraphs.Add();
                paragraph.Range.Text = theme;
            }

            SaveXpsFile();
        }

        private void ResetDocument()
        {
            try
            {
                document?.Close();
            }
            catch
            { }
            finally
            {
                document = wordApp.Documents.Add();
            }
        }

        private void SaveXpsFile()
        {
            if (document != null)
            {
                FileInfo fileInfo = new("xpsReport.xps");

                document.SaveAs2(FileName: fileInfo.FullName, FileFormat: WdSaveFormat.wdFormatXPS);

                Xps = fileInfo;
            }
        }

        public void SaveToDatabase(string type)
        {
            FileInfo fileInfo = new("report.docx");

            document.SaveAs2(FileName: fileInfo.FullName, FileFormat: WdSaveFormat.wdFormatDocumentDefault);
            document.Close(WdSaveOptions.wdSaveChanges);

            byte[] bytes = File.ReadAllBytes(fileInfo.FullName);

            int nextNumber = 1;
            if (databaseContext.Reports.Any())
            {
                nextNumber = databaseContext.Reports
                    .Where(r => r.Type == type)
                    .Count() + 1;
            }

            Report report = new()
            {
                Type = type,
                Number = nextNumber.ToString("D4"),
                ReportFile = bytes,
                CreatedAt = Today
            };
            try
            {
                databaseContext.Reports.Add(report);
                databaseContext.SaveChanges();

                LoggingTool.ReportCreated(report);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                ResetDocument();
            }
        }
    }
}
