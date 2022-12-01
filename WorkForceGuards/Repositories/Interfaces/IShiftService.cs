using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IShiftService
    {
        List<ShiftBinding> GetAll();  //**
        DataWithError Add(ShiftBinding model);//**

        DataWithError Edit(ShiftBinding model);//**

        bool Delete(int id);//**

        ShiftBinding GetById(int id); //**

        bool CheckUniqeValue(ShiftBinding model); //**

        bool CheckValue(string value, string ignoreValue);//**

        int ConvertTimeToNumber(TimeSpan time);
        int ConvertTimeTostartInterval(TimeSpan Start, TimeSpan End);
        int ConvertTimeToendtInterval(TimeSpan Start, TimeSpan End);
        TimeSpan ConvertinttoDatetime(int IntervalId);

        TimeSpan ConvertNumberToTime(int number);

    }
}
