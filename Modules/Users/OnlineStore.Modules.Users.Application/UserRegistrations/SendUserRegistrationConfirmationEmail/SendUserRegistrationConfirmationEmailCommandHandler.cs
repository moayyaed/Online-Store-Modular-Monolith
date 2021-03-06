using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Modules.Users.Application.UserRegistrations.SendUserRegistrationConfirmationEmail
{
    internal class SendUserRegistrationConfirmationEmailCommandHandler : ICommandHandler<SendUserRegistrationConfirmationEmailCommand>
    {
        private readonly IEmailSender _emailSender;

        public SendUserRegistrationConfirmationEmailCommandHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task<Unit> Handle(SendUserRegistrationConfirmationEmailCommand command, CancellationToken cancellationToken)
        {
            var emailMessage = new EmailMessage(
                command.Email,
                "MyMeetings - Please confirm your registration",
                $"This should be link to confirmation page. For now, please execute HTTP request PATCH http://localhost:5000/userAccess/userRegistrations/{command.UserRegistrationId.Value}/confirm");

            _emailSender.SendEmail(emailMessage);

            return Unit.Task;
        }
    }
}