using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxNest.Application.Contract;

namespace LuxNest.Infrastructure.Emails
{
    public class EmailService : IEmailService
    {
        public Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
