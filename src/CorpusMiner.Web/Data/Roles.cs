namespace CorpusMiner.Web.Data;

/// <summary>Nombres de los roles de aplicacion. Unica fuente de verdad para evitar strings sueltos.</summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Contributor = "Contributor";
    public const string Read = "Read";

    /// <summary>Roles con permiso para publicar datasets en el foro.</summary>
    public const string Publishers = Admin + "," + Contributor;

    public static readonly IReadOnlyList<string> All = [Admin, Contributor, Read];
}
