using ClosedXML.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemsBoardLib.ExcelDataReader
{
	public class ExcelDataExport
	{
		private string filePath = "";

		private List<Problem> Problems { get; set; } = new();

		public void GenerateReport()
		{
			var workbook = new XLWorkbook();

			var worksheet = workbook.Worksheets.Add();

			bool isFirstRow = true;

			using (DatabaseContext databaseContext = new())
			{
				foreach(var problem in databaseContext.Problems.Include(a => a.Department).Include(a => a.Theme).Include(a => a.Responsible).Include(a => a.Header))
				{
					Problems.Add(problem);
				}
			}

			for (int i = 1, j = 0; j < Problems.Count; i++, j++) 
			{
				var row = worksheet.Row(i);
				if (isFirstRow)
				{
					isFirstRow = false;

					row.Cell(1).Value = "ID";
					row.Cell(2).Value = "Тема";
					row.Cell(3).Value = "Дата возникновения";
					row.Cell(4).Value = "Описание";
					row.Cell(5).Value = "Решение";
					row.Cell(6).Value = "Дата решения";
					row.Cell(7).Value = "Кто добавил";
					row.Cell(8).Value = "Ответственный по решению проблемы";
					row.Cell(9).Value = "Участок";
					row.Cell(10).Value = "Время решения";
					row.Cell(11).Value = "Статус";

					row.Style.Font.Bold = true;
					row.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
					continue;
				}

				row.Cell(1).Value = Problems[j].ProblemId;
				row.Cell(2).Value = Problems[j].Theme.ToString() ?? "???";
				row.Cell(3).Value = Problems[j].DateOccurance.ToShortDateString();
				row.Cell(4).Value = Problems[j].Description;
				row.Cell(5).Value = Problems[j].Decision ?? "---";
				row.Cell(6).Value = Problems[j].DateElimination.ToString() ?? "---";
				row.Cell(7).Value = Problems[j].Header.WorkerInfo ?? "---";
				row.Cell(8).Value = Problems[j].Responsible.WorkerInfo ?? "---";
				row.Cell(9).Value = Problems[j].Department.ToString() ?? "???";
				row.Cell(10).Value = Problems[j].DecisionTime;
				row.Cell(11).Value = Problems[j].Status;
			}

			worksheet.Columns().AdjustToContents();

			// SaveDatabaseReport(workbook);
			SaveWorkbook(workbook);
		}

		private void SaveWorkbook(XLWorkbook workbook)
		{
			SaveFileDialog saveFileDialog = new();
			string allowedFormats = "Файлы Excel 2007 (*.xlsx)|*.xlsx";
			saveFileDialog.Filter = allowedFormats;

			if (saveFileDialog.ShowDialog() == true)
			{
				workbook.SaveAs(saveFileDialog.FileName);
			}
		}

		private void SaveDatabaseReport(XLWorkbook workbook)
		{
			string type = "ExcelЭкспорт";
			using (DatabaseContext databaseContext = new())
			{
				int nextNumber = 1;
				if (databaseContext.Reports.Any())
				{
					nextNumber = databaseContext.Reports
						.Where(r => r.Type == type)
						.Count() + 1;
				}

				byte[] bytes;
				using (MemoryStream stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					bytes = stream.ToArray();
				}

				Report report = new()
				{
					Type = type,
					Number = nextNumber.ToString("D4"),
					ReportFile = bytes,
					CreatedAt = DateTime.Today
				};

				databaseContext.Add(report);
				try
				{
					databaseContext.SaveChanges();
				}
				catch(Exception ex)
				{
					return;
				}
			}
		}
	}
}
