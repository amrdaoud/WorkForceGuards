using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WorkForceManagementV0.Repositories
{
    public class ForecastService : IForecastService
    {
        private readonly ApplicationDbContext _db;
        public ForecastService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<DataWithError> GenerateForecastAsync(ForecastBindingModel model)
        {
            DataWithError data = new DataWithError();
            if(CheckUniqeValue(model))

            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone);
                model.EndDate= TimeZoneInfo.ConvertTimeFromUtc(model.EndDate, timezone);
                model.ExceptDates = model.ExceptDates.Select(x => TimeZoneInfo.ConvertTimeFromUtc(x, timezone)).ToList();

                var days = Enumerable.Range(0, 1 + model.EndDate.Subtract(model.StartDate).Days)
                .Select(offset => model.StartDate.AddDays(offset).Date)
                .Where(x => model.ExceptDates == null || !model.ExceptDates.Contains(x)).ToList();
                var weekDays = days.Select(x => x.DayOfWeek).Distinct().ToList();
                if (weekDays.Count != 7)
                {
                    data.Result = null;
                    data.ErrorMessage="the range date is not contain 7 day";
                    return data;
                    //new SuccessWithMessage { Message = "Not all week days included!", Succeded = false };
                }
                var forecastDetails = _db.ForecastHistories.Where(x => days.Contains(x.Day)).ToList().GroupBy(x => new { x.Day.DayOfWeek, x.IntervalId }).Select(g => new
                {
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    g.Key.IntervalId,
                    Duration = g.Average(s => s.Duration),
                    Offered = g.Average(s => s.Offered)
                }
                    )
                    .Select(x => new ForecastDetails(0, x.DayOfWeek, x.IntervalId, AgentCount(model.ServiceLevel, model.ServiceTime, x.Offered * (1 + model.OfferedTolerance), x.Duration * (1 + model.DurationTolerance))))
                    .ToList();
                if (forecastDetails.Count < 672)
                {
                    data.Result = null;
                    data.ErrorMessage = "Missing Intervals";
                    return data;
                }
                var result = forecastDetails.Union(forecastDetails.Select(x => new ForecastDetails(0, GetPastDayOfWeek(x.DayoffWeek), x.IntervalId + 96, x.EmployeeCount))).ToList();
                //_db.ForecastDetails.RemoveRange(_db.TmpForecastDetails.ToList());
                //_db.ForecastDetails.AddRange(result);
                //await _db.SaveChangesAsync();
                
                var forecast = new Forecast(model);
                forecast.ForecastDetails = result;

                _db.Forecasts.Add(forecast);
                await _db.SaveChangesAsync();
                var finalresult = _db.Forecasts.Include(x => x.ForecastDetails).ThenInclude(x => x.Interval).FirstOrDefault(x => x.Id == forecast.Id);
                finalresult.ForecastDetails = finalresult.ForecastDetails.Where(x => x.IntervalId < 97).ToList()
                    .OrderBy(x => x.Interval.TimeMap).OrderBy(x => GetWeekDayNumber(x.DayoffWeek)).ToList();
                data.Result=finalresult;
                data.ErrorMessage = null;
                return data;

            }
            data.Result = null;
            data.ErrorMessage = "duplicate with forecast name";
            return data;

            // return new SuccessWithMessage { Message = "Forecast generated", Succeded = true };
        }
        private int GetWeekDayNumber(string dayOfWeek)
        {
            var result = 1;
            switch (dayOfWeek)
            {
                case "Saturday": result = 7; break;
                case "Sunday": result = 1; break;
                case "Monday": result = 2; break;
                case "Tuesday": result = 3; break;
                case "Wednesday": result = 4; break;
                case "Thursday": result = 5; break;
                case "Friday": result = 6; break;
                default: result = 1; break;
            }
            return result;
        }
        private string GetPastDayOfWeek(string dayOfWeek)
        {
            var result = "Sunday";
            switch (dayOfWeek)
            {
                case "Saturday": result = "Friday"; break;
                case "Sunday": result = "Saturday"; break;
                case "Monday": result = "Sunday"; break;
                case "Tuesday": result = "Monday"; break;
                case "Wednesday": result = "Tuesday"; break;
                case "Thursday": result = "Wednesday"; break;
                case "Friday": result = "Thursday"; break;
                default: result = "Sunday"; break;
            }
            return result;
        }
        public int AgentCount(double serviceLevel, int serviceTime, double offered, double duration)
        {
            //Copyright © T&C Limited 1996, 1999, 2001
            //Calculate the number of agents required to service a given number of calls to meet the service level
            // SLA is the % of calls to be answered within the ServiceTime period  e.g. 0.95  (95%)
            // ServiceTime is target answer time in seconds e.g. 15
            // CallsPerHour is the number of calls received in one hour period
            // AHT is the call duration including after call work in seconds  e.g 180
            double BirthRate;
            double DeathRate;
            double TrafficRate;
            double Erlangs;
            double Utilisation;
            double C;
            double SLQueued;
            int NoAgents = 0;
            long MaxIterate;
            long Count;
            double Server;

            double MaxAccuracy = 0.00001;

            try
            {
                if (serviceLevel > 1)
                    serviceLevel = 1;
                BirthRate = offered;
                DeathRate = 900 / (double)duration;
                // calculate the traffic intensity
                TrafficRate = BirthRate / (double)DeathRate;
                // calculate the number of Erlangs/hours
                Erlangs = Math.Truncate((BirthRate * (duration)) / (double)900 + 0.5);
                // start at number of agents for 100% utilisation
                if (Erlangs < 1)
                    NoAgents = 1;
                else
                    NoAgents = (int)Math.Truncate(Erlangs);
                Utilisation = TrafficRate / (double)NoAgents;
                // now get real and get number below 100%
                while (Utilisation >= 1)
                {
                    NoAgents = NoAgents + 1;
                    Utilisation = TrafficRate / (double)NoAgents;
                }
                MaxIterate = NoAgents * 100;
                // try each number of agents until the correct SLA is reached
                for (Count = 1; Count <= MaxIterate; Count++)
                {
                    Utilisation = TrafficRate / (double)NoAgents;
                    if (Utilisation < 1)
                    {
                        Server = NoAgents;
                        C = ErlangC(Server, TrafficRate);
                        // find the level of SLA with this number of agents
                        SLQueued = 1 - C * Math.Exp((TrafficRate - Server) * serviceTime / (double)duration);
                        if (SLQueued < 0)
                            SLQueued = 0;
                        if (SLQueued >= serviceLevel)
                            Count = MaxIterate;
                        // put a limit on the accuracy required (it will never actually get to 100%)
                        if (SLQueued > (1 - MaxAccuracy))
                            Count = MaxIterate;
                    }
                    if (Count != MaxIterate)
                        NoAgents = NoAgents + 1;
                }

                return NoAgents;
            }
            catch
            {
                return 0;
            }
        }
        private double ErlangC(double Servers, double Intensity)
        {
            // Copyright © T&C Limited 1996, 1999
            // This formula gives the percentage likelyhood of the caller being
            // placed in a queue.
            // Servers = Number of agents
            // Intensity = Arrival rate of calls / Completion rate of calls
            // Arrival rate = the number of calls arriving per hour
            // Completion rate = the number of calls completed per hour
            double B;
            double C;
            try
            {
                if ((Servers < 0) | (Intensity < 0))
                {
                    return 0;
                }
                B = ErlangB(Servers, Intensity);
                C = B / (double)(((Intensity / (double)Servers) * B) + (1 - (Intensity / (double)Servers)));
                return MinMax(C, 0, 1);
            }
            catch
            {
                return MinMax(0, 0, 1);
            }
        }
        private double ErlangB(double Servers, double Intensity)
        {
            // Copyright © T&C Limited 1996, 1999
            // The Erlang B formula calculates the percentage likelyhood of the call
            // being blocked, that is that all the trunks are in use and the caller
            // will receive a busy signal.
            // Servers = Number of telephone lines
            // Intensity = Arrival rate of calls / Completion rate of calls
            // Arrival rate = the number of calls arriving per hour
            // Completion rate = the number of calls completed per hour
            double Val;
            double Last;
            double B = 0;
            long Count;
            long MaxIterate;
            try
            {
                if ((Servers < 0) | (Intensity < 0))
                {
                    return 0;
                }
                MaxIterate = (long)Math.Truncate(Servers);
                Val = Intensity;
                Last = 1; // for server = 0
                for (Count = 1; Count <= MaxIterate; Count++)
                {
                    B = (Val * Last) / (double)(Count + (Val * Last));
                    Last = B;
                }
                return MinMax(B, 0, 1);
            }
            catch
            {
                return MinMax(0, 0, 1);
            }
        }
        private double MinMax(double Val, double Min, double Max)
        {
            // Apply minimum and maximum bounds to a value
            if (Val < Min)
                return Min;
            if (Val > Max)
                return Max;
            return Val;
        }


       

        public DataWithError GetAll()
        {
            DataWithError data = new DataWithError();
            var result = _db.Forecasts;
            data.Result= result;
            data.ErrorMessage = null;
            return data;
        }

        public DataWithError GetById(int id)
        {
            DataWithError data = new DataWithError();
            var result = _db.Forecasts.Include(x => x.ForecastDetails).ThenInclude(x => x.Interval).FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                data.Result = null;
                data.ErrorMessage = "Forecast is not found ";
                return data;
            }
            result.ForecastDetails = result.ForecastDetails.Where(x => x.IntervalId < 97).ToList()
                    .OrderBy(x => x.Interval.TimeMap).OrderBy(x => GetWeekDayNumber(x.DayoffWeek)).ToList();

            data.Result = result;
            data.ErrorMessage = null;
            return data;

        }

        public DataWithError GetSaved()
        {
            DataWithError result = new DataWithError();
            var data = _db.Forecasts.Where(x => x.IsSaved == true)
                .ToList();

            //.OrderBy(x => x.Interval.TimeMap)
            //.OrderBy(x => GetWeekDayNumber(x.DayoffWeek))
            //.ToList();
            if (data.Count == 0)
            {
                result.Result = null;
                result.ErrorMessage = "there is not  Forecast";
                return result;
            }
            result.Result = data;
            result.ErrorMessage = null;
            return result;



        }
        public DataWithError GetUnSaved()
        {

            DataWithError result = new DataWithError();
            var data = _db.Forecasts.Where(x => x.IsSaved == false)
                .ToList();
            //.OrderBy(x => x.Interval.TimeMap)
            //.OrderBy(x => GetWeekDayNumber(x.DayoffWeek))
            //.ToList();
            if (data.Count == 0)
            {
                result.Result = null;
                result.ErrorMessage = "there is not forecast Unsaved";
                return result;
            }
            result.Result = data;
            result.ErrorMessage = null;
            return result;


        }
        public DataWithError Remove(int id)
        {
            DataWithError final = new DataWithError();
            var forecast = _db.Forecasts.FirstOrDefault(x=> x.Id==id);
            if (forecast == null)
            {
                final.Result = false;
                final.ErrorMessage = "id is not found ";
                return final;
            }
            //if(forecast.IsSaved)
            //{
            //    return new DataWithError(null, "Cannot remove saved forecast");
            //}
            var scheduleFound = _db.Schedules.FirstOrDefault(s => s.ForecastId == id);
            if(scheduleFound != null)
            {
                return new DataWithError(null, "Cannot delete. Related Schedule Found!");
            }

            _db.Forecasts.Remove(forecast);
            _db.SaveChanges();
            final.Result = true;
            final.ErrorMessage = null;
            return final;

        }
        public DataWithError PutForecast(int id, ForecastBindingModel model)
        {
            DataWithError finalresult = new DataWithError();
            var forecast = _db.Forecasts.Include(x => x.ForecastDetails).FirstOrDefault(x=> x.Id==id);
            if (forecast == null)
            {
                finalresult.Result = null;
                finalresult.ErrorMessage = "forecast_id is not found ";
                return finalresult;
            }
            //_db.Entry(forecast).CurrentValues.SetValues(new Forecast(model));
            if (CheckUniqeValue(model) && forecast.IsSaved==false)

            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone);
                model.EndDate = TimeZoneInfo.ConvertTimeFromUtc(model.EndDate, timezone);
                model.ExceptDates = model.ExceptDates.Select(x => TimeZoneInfo.ConvertTimeFromUtc(x, timezone)).ToList();

                var days = Enumerable.Range(0, 1 + model.EndDate.Subtract(model.StartDate).Days)
                   .Select(offset => model.StartDate.AddDays(offset).Date)
                   .Where(x => model.ExceptDates == null || !model.ExceptDates.Contains(x)).ToList();
                var weekDays = days.Select(x => x.DayOfWeek).Distinct().ToList();
                if (weekDays.Count != 7)
                {
                    finalresult.Result = null;
                    finalresult.ErrorMessage = "this range is not contain 7 dayweek";
                    return finalresult;

                }
                var forecastDetails = _db.ForecastHistories.Where(x => days.Contains(x.Day)).ToList().GroupBy(x => new { x.Day.DayOfWeek, x.IntervalId }).Select(g => new
                {
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    g.Key.IntervalId,
                    Duration = g.Average(s => s.Duration),
                    Offered = g.Average(s => s.Offered)
                }
                    )
                    .Select(x => new ForecastDetails(0, x.DayOfWeek, x.IntervalId, AgentCount(model.ServiceLevel, model.ServiceTime, x.Offered * (1 + model.OfferedTolerance), x.Duration * (1 + model.DurationTolerance))))
                    .ToList();
                if(forecastDetails.Count < 672)
                {
                    finalresult.Result = null;
                    finalresult.ErrorMessage = "Missing Intervals";
                    return finalresult;
                }
                var result = forecastDetails.Union(forecastDetails.Select(x => new ForecastDetails(0, GetPastDayOfWeek(x.DayoffWeek), x.IntervalId + 96, x.EmployeeCount))).ToList();
                forecast.ForecastDetails.Clear();
                forecast.ForecastDetails = result;
                forecast.Id =forecast.Id;
                forecast.Name = model.Name;
                forecast.StartDate = model.StartDate;
                forecast.EndDate = model.EndDate;
                forecast.ExceptDates = model.ExceptDates == null ? "" : string.Join(",", model.ExceptDates.Select(x => x.ToString("dd/MM/yyyy")).ToArray());
                forecast.DurationTolerance = model.DurationTolerance;
                forecast.ServiceLevel = model.ServiceLevel;
                forecast.ServiceTime = model.ServiceTime;
                forecast.OfferedTolerance = model.OfferedTolerance;
                forecast.IsSaved = false;
                _db.Entry(forecast).State = EntityState.Modified;
                _db.SaveChanges();
                // forecast.ForecastDetails = result.Where(x => x.IntervalId < 97).ToList().OrderBy(x => x.Interval.TimeMap).OrderBy(x => GetWeekDayNumber(x.DayoffWeek)).ToList();
                var data = _db.Forecasts.Include(x => x.ForecastDetails).ThenInclude(x => x.Interval).FirstOrDefault(x => x.Id == forecast.Id);
                data.ForecastDetails = data.ForecastDetails.Where(x => x.IntervalId < 97).ToList()
                    .OrderBy(x => x.Interval.TimeMap).OrderBy(x => GetWeekDayNumber(x.DayoffWeek)).ToList();
                finalresult.Result = data;
                finalresult.ErrorMessage = null;
                return finalresult;
            }
                finalresult.Result = null;
                finalresult.ErrorMessage = "Duplicate with forecast Name or forcast is Saved ";
                return finalresult;
         
        }
        public DataWithError PutOneInterval(int id, EditForecastBinding model)

        {
            DataWithError data = new DataWithError();
            
            var forecastdetail = _db.ForecastDetails.FirstOrDefault(x => x.Id == model.Id);
            if (forecastdetail == null)
            {
                data.Result = null;
                data.ErrorMessage = "Interval Id is not found";
                return data;
            }
            else
            {
                var forecast = _db.Forecasts.FirstOrDefault(x => x.Id == forecastdetail.ForecastId);
                if (forecast.IsSaved == false && forecast != null)
                {

                    forecastdetail.EmployeeCount = model.NewEmployeeCount;
                    _db.SaveChanges();

                    var newforecast = _db.ForecastDetails.FirstOrDefault(x => x.Id == model.Id);
                    data.Result = newforecast;
                    data.ErrorMessage = null;
                    return data;
                }

                data.Result = null;
                data.ErrorMessage = "Sorry Forecast is saved or not found ";
                return data;
            }


        }
        public bool CheckUniqeValue(ForecastBindingModel model)
        {
            var data = _db.Forecasts.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);

            if (data == null)
            {
                return true;
            }
            return false;
        }
        public bool CheckValue(string name, string ignoreName)
        {
            
                var forecast = _db.Forecasts.FirstOrDefault(x => x.Name.ToLower() != ignoreName.ToLower() && x.Name.ToLower() == name.ToLower());

                if (forecast == null)
                {
                    return true;
                }
                return false;
            
        }
        public DataWithError SaveForecast(int id)
        {
            DataWithError data = new DataWithError();
            var result = _db.Forecasts.FirstOrDefault(x => x.Id == id);
            //result.ForecastDetails.Count() == 0
            if (result ==null )
            {
                data.Result = null;
                data.ErrorMessage = "Forecast id is not found ";
                return data;
            }
            result.IsSaved = true;
            _db.SaveChanges();

            data.Result = result;
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError GetIntervals()
        {
            DataWithError data = new DataWithError();
            var intervals = _db.Intervals.ToList();

             data.Result = intervals;
            data.ErrorMessage = null;
            return data;
            
        }
        public DataWithError EditTolerance( EditToleranceBinding model)
        {
            DataWithError data = new DataWithError();
            EditToleranceBinding response = new EditToleranceBinding();
            var intervaldata = _db.Intervals.FirstOrDefault(x => x.Id == model.IntervalId);
            var secondinterval = _db.Intervals.FirstOrDefault(x => x.Id == model.IntervalId + 96);
            if(intervaldata != null)
            {
                intervaldata.Tolerance = model.Tolerance;
                secondinterval.Tolerance = model.Tolerance;


                _db.SaveChanges();
                response.IntervalId = model.IntervalId;
                response.Tolerance = model.Tolerance;

                data.Result = response;
                data.ErrorMessage = null;
                return data;

            }
                data.Result = null;
                data.ErrorMessage = "Id is not found ";
                return data;
        
        }
        public DataWithError EditAllTolerance(double tolerance)
        {
            foreach(var interval in _db.Intervals)
            {
                interval.Tolerance = tolerance;
            }
            _db.SaveChanges();
            return new DataWithError { Result = true, ErrorMessage = null };
        }
        public DataWithError RecommendAllTolerances(List<RecommendAll> recommendAll)
        {
            var intervals = _db.Intervals.Where(x => recommendAll.Select(y => y.IntervalId).Contains(x.Id) || recommendAll.Select(y => y.IntervalId + 96).Contains(x.Id));
           foreach(var interval in intervals)
            {
                interval.Tolerance = recommendAll.FirstOrDefault(x => x.IntervalId == interval.Id || x.IntervalId + 96 == interval.Id).Tolerance;
            }
            _db.SaveChanges();
            return new DataWithError(true, "");
        }

    }
}
