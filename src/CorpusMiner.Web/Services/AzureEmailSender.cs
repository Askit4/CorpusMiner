using Azure;
using Azure.Communication.Email;
using CorpusMiner.Web.Data;
using Microsoft.AspNetCore.Identity;

namespace CorpusMiner.Web.Services;

/// <summary>
/// Envia correos reales via Azure Communication Services Email. Cubre tanto los correos
/// transaccionales de Identity (confirmacion, reset de password) como las notificaciones
/// de la aplicacion (menciones, comentarios) a traves de <see cref="INotificationEmailSender"/>.
/// </summary>
internal sealed class AzureEmailSender(EmailClient emailClient, IConfiguration configuration, ILogger<AzureEmailSender> logger)
    : IEmailSender<ApplicationUser>, INotificationEmailSender
{
    private string SenderAddress => configuration["Acs:SenderAddress"]
        ?? throw new InvalidOperationException("Falta configurar 'Acs:SenderAddress'.");

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendAsync(email, "Confirma tu cuenta - CorpusMiner", $"<p>Confirma tu cuenta haciendo <a href=\"{confirmationLink}\">clic aqui</a>.</p>");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendAsync(email, "Restablece tu contrasena - CorpusMiner", $"<p>Restablece tu contrasena haciendo <a href=\"{resetLink}\">clic aqui</a>.</p>");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendAsync(email, "Codigo para restablecer tu contrasena - CorpusMiner", $"<p>Tu codigo es: <strong>{resetCode}</strong></p>");

    public Task SendAsync(string toAddress, string subject, string htmlBody) =>
        SendCoreAsync(toAddress, subject, htmlBody);

    private async Task SendCoreAsync(string toAddress, string subject, string htmlBody)
    {
        try
        {
            var message = new EmailMessage(SenderAddress, toAddress, new EmailContent(subject) { Html = htmlBody });
            await emailClient.SendAsync(WaitUntil.Started, message);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "No se pudo enviar el correo a {ToAddress}", toAddress);
            throw;
        }
    }
}
