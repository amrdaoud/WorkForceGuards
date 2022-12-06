using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface ITransportationRouteService
    {
        List<TransportationRouteBinding> GetAll();//GET()
        List<TransportationRouteBinding> GetById(int id);// GET(ID)
        DataWithError Add(TransportationRouteBinding model); //POST(Location)
        // DataWithError Update(TransportationRouteBinding model); //PUT(ID,Location)
        bool Delete(int id);//Delete(ID)
        bool CheckUniqValue(TransportationRouteBinding value);
        int ConvertTimeTostartInterval(DateTime Start, DateTime End);

        int ConvertTimeToendtInterval(DateTime Start, DateTime End);
        DateTime ConvertintoDatetime(int IntervalId);
        bool CheckUniqValue(int Id, string Name);
        bool checkSublocValue(TransportationBinding model);
        DataWithError AddGuard(TransportationBinding model);
        DataWithError UpdateGuard(TransportationBinding model);
    }
}
