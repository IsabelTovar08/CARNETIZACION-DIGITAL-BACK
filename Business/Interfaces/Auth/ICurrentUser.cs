namespace Business.Interfaces.Auth
{
    public interface ICurrentUser
    {
        string UserIdRaw { get; } 
        int UserId { get; }
        string? UserName { get; }  
    }
}
