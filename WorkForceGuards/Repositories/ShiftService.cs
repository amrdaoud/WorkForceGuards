using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class ShiftService: IShiftService
    {
        private readonly ApplicationDbContext db;

        public ShiftService(ApplicationDbContext context)
        {
            db = context;
        }

        public List<ShiftBinding> GetAll()
        {
            List<ShiftBinding> Result = new List<ShiftBinding>();

            var shiftList= db.Shifts.ToList();

            foreach(var s in shiftList)
            {
                ShiftBinding data = new ShiftBinding();

                data.Id = s.Id;
                data.Name = s.Name;
                data.EarlyStart = ConvertinttoDatetime((int)s.EarlyStartIntervalId);
                data.LateEnd = ConvertinttoDatetime((int)s.LateEndIntervalId);
                data.ShiftDuration = s.Duration;

                Result.Add(data);

            }

            return Result;

        }

        public ShiftBinding GetById(int id)
        {
            ShiftBinding result = new ShiftBinding();

            var shift = db.Shifts.Find(id);

            result.Id = shift.Id;
            result.Name = shift.Name;
            result.EarlyStart = ConvertinttoDatetime((int)shift.EarlyStartIntervalId);
            result.LateEnd = ConvertinttoDatetime((int)shift.LateEndIntervalId);
            result.ShiftDuration = shift.Duration;
            return result;

        }
        public bool CheckUniqeValue(ShiftBinding model)
        {
            var data = db.Shifts.FirstOrDefault(x=>x.Name.ToLower()==model.Name.ToLower() && x.Id!=model.Id);
            if (data == null)
            {
                return true;
            }

            return false;
        }

        public DataWithError Add(ShiftBinding model)
        {
            DataWithError data = new DataWithError();
            if (CheckUniqeValue(model))
            {
                Shift InsertShift = new Shift();
                ShiftBinding ResponsetShift = new ShiftBinding();

                //var startQuarter = ConvertTimeToNumber(model.EarlyStart);
                //var endQuarter= ConvertTimeToNumber(model.LateEnd);
                var StartInterval = ConvertTimeTostartInterval(model.EarlyStart, model.LateEnd);
                var EndInterval= ConvertTimeToendtInterval(model.EarlyStart, model.LateEnd);

                InsertShift.Name = model.Name;
                InsertShift.EarlyStartIntervalId = StartInterval;
                InsertShift.LateEndIntervalId = EndInterval;
                InsertShift.Duration = model.ShiftDuration;

                db.Shifts.Add(InsertShift);
                db.SaveChanges();

                ResponsetShift.Id = InsertShift.Id;
                ResponsetShift.Name = model.Name;
                ResponsetShift.EarlyStart = ConvertinttoDatetime((int)InsertShift.EarlyStartIntervalId);
                ResponsetShift.LateEnd = ConvertinttoDatetime((int)(InsertShift.LateEndIntervalId));
                ResponsetShift.ShiftDuration = model.ShiftDuration;

                data.Result = ResponsetShift;
                data.ErrorMessage = null;
                return data;


            }

            data.Result = null;
            data.ErrorMessage = "Dublicated Shift Name Inserted";
            return data;

        }

        public int ConvertTimeToNumber(TimeSpan time)
        {
            var HourTime = time.Hours;
            var MinuteTime = time.Minutes;

            var ConvertHourToQuarter = HourTime * 4;
            var ConvertMinuteToQuater = Math.Round(MinuteTime / 15.0);

            var QuarterIndex = ConvertHourToQuarter + ConvertMinuteToQuater;

            return Convert.ToInt32(QuarterIndex);

        }

        public DataWithError Edit(ShiftBinding model)
        {
            ShiftBinding Response = new ShiftBinding();
            DataWithError data = new DataWithError();

            if(CheckUniqeValue(model))
            {
                Shift UpdatetShift = new Shift();
                var StartInterval = ConvertTimeTostartInterval(model.EarlyStart, model.LateEnd);
                var EndInterval = ConvertTimeToendtInterval(model.EarlyStart, model.LateEnd);
                UpdatetShift.Id = model.Id;
                UpdatetShift.Name = model.Name;
                UpdatetShift.EarlyStartIntervalId = StartInterval;
                UpdatetShift.LateEndIntervalId = EndInterval;
                UpdatetShift.Duration = model.ShiftDuration;
                db.Entry(UpdatetShift).State = EntityState.Modified;
                db.SaveChanges();

                Response.Id = model.Id;
                Response.Name = model.Name;
                Response.EarlyStart = ConvertinttoDatetime((int)UpdatetShift.EarlyStartIntervalId);
                Response.LateEnd = ConvertinttoDatetime((int)(UpdatetShift.LateEndIntervalId));
                Response.ShiftDuration = model.ShiftDuration;
                data.Result = Response;
                data.ErrorMessage = null;
                return data;


            }
            data.Result = null;
            data.ErrorMessage = "Dublicated Shift Name Inserted";
            return data;

        }


        public bool Delete(int id)
        {
            var shift = db.Shifts.Find(id);

            shift.Name = "#" + shift.Name + DateTime.Now;
            shift.IsDeleted = true;

            db.Entry(shift).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool CheckValue(string value, string ignoreValue)
        {
            var shift = db.Shifts.FirstOrDefault(x=>x.Name.ToLower()==value.ToLower() && x.Name.ToLower()!=ignoreValue.ToLower());

            if(shift==null)
            {
                return true;
            }
            return false;
        }

        public TimeSpan ConvertNumberToTime(int number)
        {
            var TimeHour = number / 4;
            var MinuteHour = (number % 4) * 15;

            TimeSpan ConverterTime = TimeSpan.Parse(TimeHour.ToString()+":"+MinuteHour.ToString());

            return ConverterTime;
        }

        public int ConvertTimeTostartInterval(TimeSpan Start, TimeSpan End)
        {
            //string hourMinute = Start.ToString("HH:mm");
            //TimeSpan time = TimeSpan.Parse(hourMinute);

            var data = db.Intervals.FirstOrDefault(x => x.TimeMap == Start);
            return data.Id;

        }
        public int ConvertTimeToendtInterval(TimeSpan Start, TimeSpan End)
        {
            //string hourStart = Start.ToString("HH:mm");
            //string hourEnd = End.ToString("HH:mm");

            //TimeSpan timeStart = TimeSpan.Parse(hourStart);
            //TimeSpan timeEnd = TimeSpan.Parse(hourEnd);
            var data = (Start > End) ? db.Intervals.Where(x => x.OrderMap >= 95).FirstOrDefault(x => x.TimeMap == End) : db.Intervals.FirstOrDefault(x => x.TimeMap == End);
            return data.Id;
            ////string hourMinute = End.ToString("HH:mm");
            ////TimeSpan time = TimeSpan.Parse(hourMinute);
            //if (Start > End)
            //{
            //    var data = db.Intervals.LastOrDefault(x => x.TimeMap == End);
            //    return data.Id;
            //}

            //var data2 = db.Intervals.FirstOrDefault(x => x.TimeMap == End);
            //return data2.Id;
        }
        public TimeSpan ConvertinttoDatetime(int IntervalId)
        {
            var data = db.Intervals.FirstOrDefault(x => x.Id == IntervalId);
            return data.TimeMap;
        }

    }
}
