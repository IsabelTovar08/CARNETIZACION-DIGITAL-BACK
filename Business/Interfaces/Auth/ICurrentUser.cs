namespace Business.Interfaces.Auth
{
    public interface ICurrentUser
    {
        string UserIdRaw { get; } 
        int UserId { get; }
        string? UserName { get; }

        /// <summary>
        /// Devuelve el valor del claim especificado o null si no existe.
        /// </summary>
        string? GetClaim(string claimType);
    }
}
