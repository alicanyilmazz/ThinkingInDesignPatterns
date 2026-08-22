using Behavioral.Patterns.Command.BackgroundJob.Commands.Abstracts;
using Behavioral.Patterns.Command.BackgroundJob.Services;

namespace Behavioral.Patterns.Command.BackgroundJob.Commands;

public class SendEmailCommand : ICommand
{
    private readonly EmailService _emailService;

    private readonly string _email;

    private readonly string _message;

    public SendEmailCommand(EmailService emailService, string email, string message)
    {
        _emailService = emailService;
        _email = email;
        _message = message;
    }

    public void Execute()
    {
        _emailService.SendEmail(_email, _message);
    }
}