using Inventory.Domain.Utilities;

namespace Inventory.Domain
{
    public interface IUnitOfWork
    {
        ISqlUtility SqlUtility { get; }
        void Save();
        Task SaveAsync();
    }
}
