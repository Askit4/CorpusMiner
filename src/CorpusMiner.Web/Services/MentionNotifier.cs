using System.Text.RegularExpressions;
using CorpusMiner.Web.Data;
using Microsoft.AspNetCore.Identity;

namespace CorpusMiner.Web.Services;

/// <summary>
/// Detecta menciones "@handle" en un texto y notifica por correo a los usuarios mencionados.
/// El "handle" de un usuario es su DisplayName sin espacios, o la parte local de su email si no tiene DisplayName.
/// </summary>
public sealed partial class MentionNotifier(UserManager<ApplicationUser> userManager, INotificationEmailSender emailSender, ILogger<MentionNotifier> logger)
{
    [GeneratedRegex(@"@([A-Za-z0-9_.-]+)")]
    private static partial Regex MentionPattern();

    public async Task NotifyMentionsAsync(string commentBody, ApplicationUser author, string datasetTitle, string datasetUrl)
    {
        var handles = MentionPattern().Matches(commentBody)
            .Select(m => m.Groups[1].Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (handles.Count == 0)
        {
            return;
        }

        foreach (var user in userManager.Users.ToList())
        {
            if (user.Id == author.Id || string.IsNullOrEmpty(user.Email))
            {
                continue;
            }

            var handle = GetHandle(user);
            if (!handles.Any(h => string.Equals(h, handle, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            try
            {
                await emailSender.SendAsync(
                    user.Email,
                    $"{GetHandle(author)} te menciono en '{datasetTitle}' - CorpusMiner",
                    $"<p>{GetHandle(author)} te menciono en un comentario de <a href=\"{datasetUrl}\">{datasetTitle}</a>.</p>");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "No se pudo notificar la mencion a {Email}", user.Email);
            }
        }
    }

    public static string GetHandle(ApplicationUser user)
    {
        if (!string.IsNullOrWhiteSpace(user.DisplayName))
        {
            return user.DisplayName.Replace(" ", "", StringComparison.Ordinal);
        }

        var email = user.Email ?? user.UserName ?? "usuario";
        var atIndex = email.IndexOf('@');
        return atIndex > 0 ? email[..atIndex] : email;
    }
}
