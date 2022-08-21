namespace GymApp14V1.Extensions
{
    public static class DateTimeExtension
    {

        /// <summary>
        /// Set Daytime e.g 9am, 12:00, afternoon and evening
        /// Args: 0 = Morning time from 09:00
        /// Args: 1 = LunchTime from 12:15
        /// Args: 2 = EveningTime from 19:00
        /// Args: 3 = Current date, 2 hours ahead
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dayTimeOpt"></param>
        /// <returns></returns>
        public static DateTime SetDayTime(this DateTime value, int dayTimeOpt = 0)
        {
            var dateTimeStr = string.Empty;

            switch (dayTimeOpt)
            {
                case 1:
                    dateTimeStr = $"{value.ToString("yyyy-MM-ddT09:00")}";
                    break;
                case 2:
                    dateTimeStr = $"{value.ToString("yyyy-MM-ddT12:15")}";
                    break;
                case 3:
                    dateTimeStr = $"{value.ToString("yyyy-MM-ddT19:00")}";
                    break;
                default:
                    dateTimeStr = $"{DateTime.Now.AddHours(2).ToString("yyyy-MM-ddThh:mm")}";
                    break;
            }

            return DateTime.Parse(dateTimeStr);
        }

    }
}
