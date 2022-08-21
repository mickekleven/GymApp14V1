namespace GymApp14V1.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime SetDayTime(this DateTime value, int dayTimeOpt = 0)
        {
            var dateTimeNow = DateTime.Now.AddDays(1);

            var dateTimeStr = string.Empty;

            switch (dayTimeOpt)
            {
                case 1:
                    dateTimeStr = $"{dateTimeNow.ToString("yyyy-MM-ddT09:00")}";
                    break;
                case 2:
                    dateTimeStr = $"{dateTimeNow.ToString("yyyy-MM-ddT12:15")}";
                    break;
                case 3:
                    dateTimeStr = $"{dateTimeNow.ToString("yyyy-MM-ddT19:00")}";
                    break;
                default:
                    dateTimeStr = $"{DateTime.Now.AddHours(2).ToString("yyyy-MM-ddThh:mm")}";
                    break;
            }

            return DateTime.Parse(dateTimeStr);
        }

    }
}
