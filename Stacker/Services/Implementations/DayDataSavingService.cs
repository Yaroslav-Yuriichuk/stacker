using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace Stacker.Services
{
    public class DayDataSavingService : IDayDataSavingService
    {
        #region Constants

        private const string TodayDataFileName = "TodayDayData.dat";
        private const string AllDataFileName = "AllDaysData.dat";

        #endregion

        #region Methods

        public IEnumerable<DayData> LoadAllData()
        {
            if (!File.Exists(AllDataFileName))
            {
                return Enumerable.Empty<DayData>();
            }

            IEnumerable<DayData> allData;

            allData = new JavaScriptSerializer().Deserialize<List<DayData>>(File.ReadAllText(AllDataFileName));
            allData ??= Enumerable.Empty<DayData>();

            return allData;
        }

        public DayData LoadTodayData()
        {
            if (!File.Exists(TodayDataFileName))
            {
                return new DayData(DateTime.Now);
            }

            DayData todayData =
                new JavaScriptSerializer().Deserialize<DayData>(File.ReadAllText(TodayDataFileName));

            if (todayData.Date.Date != DateTime.Now.Date)
            {
                return new DayData(DateTime.Now);
            }

            return todayData;
        }

        public void SaveAllData(IEnumerable<DayData> dayDatas)
        {
            File.WriteAllText(AllDataFileName, new JavaScriptSerializer().Serialize(dayDatas));
        }

        public void SaveTodayData(DayData dayData)
        {
            File.WriteAllText(TodayDataFileName, new JavaScriptSerializer().Serialize(dayData));
        }

        #endregion
    }
}
