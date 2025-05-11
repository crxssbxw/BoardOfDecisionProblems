using DocumentFormat.OpenXml.Wordprocessing;
using ProblemsBoardLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProblemsBoardLib.Tools
{
	public class MailSender
	{
		private class SmtpConfig
		{
			public string MailFrom { get; set; }
			public string Password { get; set; }
		}

		private SmtpClient _Client { get; set; }
		private string smtpServer = "smtp.mail.ru";
		private int port = 25;
		private SmtpConfig smtpConfig;
		public string RecepientEmail { get; set; }

		public MailSender()
		{
			string jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "SmtpConfig.json");
			string jsonString = File.ReadAllText(jsonFile);
			smtpConfig = JsonSerializer.Deserialize<SmtpConfig>(jsonString)!;

			_Client = new SmtpClient(smtpServer, port)
			{
				Credentials = new NetworkCredential(smtpConfig.MailFrom, smtpConfig.Password),
				EnableSsl = true
			};
		}

		public void SendEmail(string body, string subject, string recepientEmail, bool isHtml)
		{
			MailMessage mail = new(smtpConfig.MailFrom, recepientEmail, subject, body);
			mail.IsBodyHtml = isHtml;
			mail.Headers.Add("Precedence", "bulk");
			_Client.Send(mail);
		}

		public void SendAuthorizationData(string login, string password, Worker worker, string destination)
		{
			string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "AuthorizationDataTemplate.html");
			string emailTemplate = File.ReadAllText(templatePath);
			emailTemplate = emailTemplate.Replace("{WorkerSecondName}", worker.SecondName);
			emailTemplate = emailTemplate.Replace("{WorkerName}", worker.FirstName);
			emailTemplate = emailTemplate.Replace("{WorkerMiddleName}", worker.MiddleName);
			emailTemplate = emailTemplate.Replace("{Destination}", destination);
			emailTemplate = emailTemplate.Replace("{Department}", worker.DepartmentNumber);
			emailTemplate = emailTemplate.Replace("{Login}", login);
			emailTemplate = emailTemplate.Replace("{Password}", password);

			SendEmail(emailTemplate, "Назначение на доску решения проблем", RecepientEmail, true);
		}
	}
}
