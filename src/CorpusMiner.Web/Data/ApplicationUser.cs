using Microsoft.AspNetCore.Identity;

namespace CorpusMiner.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    /// <summary>Nombre visible en el foro y menciones (@usuario). Si es null se usa la parte local del email.</summary>
    public string? DisplayName { get; set; }

    /// <summary>Un administrador debe aprobar la cuenta antes de poder iniciar sesion.</summary>
    public bool IsApproved { get; set; }

    public DateTimeOffset? ApprovedAtUtc { get; set; }

    public string? ApprovedByUserId { get; set; }

    /// <summary>Cultura preferida ("es" o "en"). Si es null se usa la deteccion automatica/cookie.</summary>
    public string? PreferredCulture { get; set; }
}
