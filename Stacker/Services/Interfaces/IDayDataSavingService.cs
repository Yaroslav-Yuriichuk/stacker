using System.Collections.Generic;

namespace Stacker.Services
{
    public interface IDayDataSavingService
    {
        public void SaveTodayData(DayData dayData);
        public void SaveAllData(IEnumerable<DayData> dayDatas);

        public DayData LoadTodayData();
        public IEnumerable<DayData> LoadAllData();
    }
}
