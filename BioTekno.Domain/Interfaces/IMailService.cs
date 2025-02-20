using System;
namespace BioTekno.Domain.Interfaces
{
	public interface IMailService
	{
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    }
}

