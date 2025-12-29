using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Resend;

namespace Infrastructure.Email;

public class EmailSender(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, IConfiguration config) : IEmailSender<User>
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

   

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
         var subject="Reset you password";
        var body=$@"
            <P>Hi {user.DisplayName}</p>
            <P>please click this link to reset your password</p>
            <P><a href='{config["ClientAppUrl"]}/reset-password?email={email}&code={resetCode}'>
            Click to reset your password
            </a></p>
            <P>If you didi not request this, you can ignore this email</P>
        ";
        await SendMailAsync(email,subject,body);
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
