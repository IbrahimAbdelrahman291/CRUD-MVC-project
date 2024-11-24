using DemoDAL.Models;
using System.Net;
using System.Net.Mail;

namespace DemoPL.Helpers
{
	public static class EmailSettings 
	{
		public static void SendEmail(Email email) 
		{
			var client = new SmtpClient(" smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("ibrahimabdelrahman2912003@gmail.com", "syscbfowsuaetswe ");
			client.Send("ibrahimabdelrahman2912003@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
