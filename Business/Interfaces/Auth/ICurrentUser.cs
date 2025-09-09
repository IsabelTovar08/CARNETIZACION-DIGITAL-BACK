namespace Business.Interfaces.Auth
{
    public interface ICurrentUser
    {
        string UserId { get; }        
        string? UserName { get; }  
    }
}
