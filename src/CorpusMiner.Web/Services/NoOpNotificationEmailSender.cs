using Microsoft.Extensions.Logging;

namespace CorpusMiner.Web.Services;

/// <summary>Usado cuando no hay Azure Communication Services configurado (p. ej. desarrollo local sin infra desplegada).</summary>
internal sealed class NoOpNotificationEmailSender(ILogger<NoOpNotificationEmailSender> logger) : INotificationEmailSender
{
    public Task SendAsync(string toAddress, string subject, string htmlBody)
    {
        logger.LogWarning("Envio de correo omitido (no hay proveedor configurado). Para: {ToAddress}, Asunto: {Subject}", toAddress, subject);
        return Task.CompletedTask;
    }
}
