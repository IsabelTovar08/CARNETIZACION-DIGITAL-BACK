
namespace Business.Implementations.Notifications
{
    public interface INotificationReceivedData
    {
        Task<IEnumerable<object>> GetAllAsync();
    }
}