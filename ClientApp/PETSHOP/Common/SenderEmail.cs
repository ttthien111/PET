using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PETSHOP.Common
{
    public static class SenderEmail
    {
        public static void SendMail(string toAddress, string subject, string emailBody)
        {
			try
			{
				var builder = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json");

				var config = builder.Build();
				string username = config["Mail:username"];
				string password = config["Mail:password"];

				SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(username, password);

				MailMessage message = new MailMessage(username, toAddress, subject, emailBody);
				message.IsBodyHtml = true;
				message.BodyEncoding = UTF8Encoding.UTF8;

				client.Send(message);
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
