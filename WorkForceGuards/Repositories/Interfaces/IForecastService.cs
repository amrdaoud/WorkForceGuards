using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IForecastService
    {
        Task<DataWithError> GenerateForecastAsync(ForecastBindingModel model);
        //SuccessWithMessage SaveForecast(ForecastBindingModel model);
        // DataWithError GetUnSaved();
        int AgentCount(double serviceLevel, int serviceTime, double offered, double duration);
        DataWithError PutOneInterval(int id, EditForecastBinding model);
        DataWithError GetAll();
        DataWithError GetById(int id);

        DataWithError GetSaved();
        DataWithError GetUnSaved();
        DataWithError Remove(int id);
        DataWithError PutForecast(int id, ForecastBindingModel model);
        bool CheckUniqeValue(ForecastBindingModel model);
        bool CheckValue(string name, string ignoreName);
        DataWithError SaveForecast(int id);
        DataWithError GetIntervals();
        DataWithError EditTolerance(EditToleranceBinding model);
        DataWithError EditAllTolerance(double tolerance);

        DataWithError RecommendAllTolerances(List<RecommendAll> recommendAll);

    }
}

