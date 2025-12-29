using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resend;

namespace Infrastructure.Email;

public class EmailSender(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory) : IEmailSender<User>
{
    

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject="Confirm your email address";
        var body=$@"
            <P>Hi {user.DisplayName}</p>
            <P>please confirm your email by clicking the link below</p>
            <P><a href='{confirmationLink}'>Click here to verify email </a></p>
            <P>Thanks</P>
        ";
        await SendMailAsync(email,subject,body);
    }

   

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

     private async Task SendMailAsync(string email, string subject, string body)
    {
        var message= new EmailMessage
        {
            From="whatever@resend.dev",
            HtmlBody=body,
            Subject=subject
        };
        message.To.Add(email);
        Console.WriteLine(message.HtmlBody);
        //await Task.CompletedTask;
        using var scope = serviceProvider.CreateScope();
        var options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ResendClientOptions>>();
        var resend = new ResendClient(options, httpClientFactory.CreateClient());
         await resend.EmailSendAsync(message);
    }
}
