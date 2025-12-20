
namespace HiloGame.Domain.Services
{
    public interface IDateProvider
    {
        DateTime UtcNow { get; }
    }
}