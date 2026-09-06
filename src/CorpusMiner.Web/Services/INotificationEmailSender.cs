namespace CorpusMiner.Web.Services;

/// <summary>Envio de correos de notificacion de la aplicacion (menciones, comentarios, etc.), fuera del flujo de Identity.</summary>
public interface INotificationEmailSender
{
    Task SendAsync(string toAddress, string subject, string htmlBody);
}
