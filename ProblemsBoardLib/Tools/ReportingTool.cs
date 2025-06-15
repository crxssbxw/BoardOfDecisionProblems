using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
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

        public void GenerateProblemReport(Problem problem)
        {
            // Установка стиля
            SetStyle();
            document.Content.Paragraphs.Format.RightIndent = 250;
            document.Content.Paragraphs.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            // Шапка 
            Paragraph header = document.Content.Paragraphs.Add();
            header.Range.Text = $"Акционерное общество \rКовровский Электромеханический Завод \r";
            header.Range.Bold = 1;
            header.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            header.Format.RightIndent = 250;

            // Дата под шапкой
            string datepar = $"Отчет от {Today.ToShortDateString()} \rг. Ковров\r";
            Paragraph date = document.Content.Paragraphs.Add();
            date.Range.Text = datepar;
            date.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            date.Format.RightIndent = 0;
            date.Range.Bold = 0;
            date.Format.SpaceBefore = 0;
            date.Format.SpaceAfter = 0;
            date.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;

            Paragraph paragraph = document.Content.Paragraphs.Add();

            Table problemTable = document.Tables.Add(paragraph.Range, 10, 2);

            problemTable.Columns[1].Width = wordApp.InchesToPoints(2.5f);
            problemTable.Columns[2].Width = wordApp.InchesToPoints(4.5f);

            problemTable.Cell(1, 1).Range.Text = "Свойство проблемы";
            problemTable.Cell(1, 2).Range.Text = "Значение";
            problemTable.Cell(1, 2).Range.Font.Bold = 1;
            problemTable.Cell(1, 1).Range.Font.Bold = 1;
            problemTable.Cell(1, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            problemTable.Cell(1, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            problemTable.Cell(2, 1).Range.Text = "Идентификатор";
            problemTable.Cell(2, 2).Range.Text = problem.ProblemId.ToString("D4");

            problemTable.Cell(3, 1).Range.Text = "Описание";
            problemTable.Cell(3, 2).Range.Text = problem.Description ?? "";

            problemTable.Cell(4, 1).Range.Text = "Дата возникновения";
            problemTable.Cell(4, 2).Range.Text = problem.DateOccurance.ToShortDateString();

            problemTable.Cell(5, 1).Range.Text = "Статус";
            problemTable.Cell(5, 2).Range.Text = problem.Status ?? "---";

            problemTable.Cell(6, 1).Range.Text = "Дата устранения";
            problemTable.Cell(6, 2).Range.Text = problem.DateElimination?.ToShortDateString() ?? "---";

            problemTable.Cell(7, 1).Range.Text = "Решение";
            problemTable.Cell(7, 2).Range.Text = problem.Decision ?? "---";

            problemTable.Cell(8, 1).Range.Text = "Время решения (дни)";
            problemTable.Cell(8, 2).Range.Text = problem.DecisionTime?.ToString() ?? "---";

            problemTable.Cell(9, 1).Range.Text = "Отдел";
            problemTable.Cell(9, 2).Range.Text = $"[{problem.Department?.ViewerNumber}] {problem.Department?.Name}" ?? "";

            problemTable.Cell(10, 1).Range.Text = "Тема";
            problemTable.Cell(10, 2).Range.Text = problem.Theme?.Name ?? "";

            problemTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
			problemTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = $"Отчет по проблеме";
			saveFileDialog.Filter = "Документ MS Office Word 2007(*.docx)|*.docx";

			if (saveFileDialog.ShowDialog() == true)
			{
				document.SaveAs2(saveFileDialog.FileName);
				LoggingTool.ReportSaved(saveFileDialog.FileName, "по проблеме");
				var result = System.Windows.MessageBox.Show("Открыть папку с файлом?", "Подтвердждение действия", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
				if (result == System.Windows.MessageBoxResult.Yes)
				{
					string argument = "/select, \"" + saveFileDialog.FileName + "\"";
					System.Diagnostics.Process.Start("explorer.exe", argument);
				}
			}
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

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = $"Отчет по статистике";
			saveFileDialog.Filter = "Документ MS Office Word 2007(*.docx)|*.docx";

			if (saveFileDialog.ShowDialog() == true)
			{
				document.SaveAs2(saveFileDialog.FileName);
				LoggingTool.ReportSaved(saveFileDialog.FileName, "по статистике");
				var result = System.Windows.MessageBox.Show("Открыть папку с файлом?", "Подтвердждение действия", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
				if (result == System.Windows.MessageBoxResult.Yes)
				{
					string argument = "/select, \"" + saveFileDialog.FileName + "\"";
					System.Diagnostics.Process.Start("explorer.exe", argument);
				}
			}
        }
    }
}
