using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface ISublocationService
    {
        DataWithError Add(SubLocation model);
        DataWithError UpdateSublocation(SubLocation model);
        DataWithError GetAll();
        bool CheckUniq(string value);
    }
}
