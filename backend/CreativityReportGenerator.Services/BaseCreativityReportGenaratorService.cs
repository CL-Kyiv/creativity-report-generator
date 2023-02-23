using System;
using System.Collections.Generic;
using System.Text;

namespace CreativityReportGenerator.Services
{
    public class BaseCreativityReportGenaratorService
    {
        /// <summary>
        /// Calculate working time per day.
        /// </summary>
        /// <param name="startWorkingHours">Working day start time.</param>
        /// <param name="endWorkingHours">Working day end time.</param>
        /// <returns>Working time per day.</returns>
        protected int CalculateWorkingTimePerDay(int startWorkingHours, int endWorkingHours)
        {
            int workingHoursPerDay;

            if (startWorkingHours <= endWorkingHours)
            {
                workingHoursPerDay = endWorkingHours - startWorkingHours;
            }
            else
            {
                workingHoursPerDay = endWorkingHours + 24 - startWorkingHours;
            }

            return workingHoursPerDay;
        }

        /// <summary>
        /// Get time difference between commits.
        /// </summary>
        /// <param name="com">Current commit.</param>
        /// <param name="previousCom">Previous commit.</param>
        /// <param name="startWorkingHours">Working day start time.</param>
        /// <param name="endWorkingHours">Working day end time.</param>
        /// <returns>Time difference between commits.</returns>
        protected int GetTimeDifferenceBetweenCommits(DateTimeOffset end, DateTimeOffset start, int startWorkingHours, int endWorkingHours)
        {
            int hours = 0;

            if (startWorkingHours <= endWorkingHours)
            {
                while (start < end)
                {
                    start = start.AddHours(1);
                    if (start.Hour > startWorkingHours &&
                       start.Hour <= endWorkingHours &&
                       start.DayOfWeek != DayOfWeek.Saturday &&
                       start.DayOfWeek != DayOfWeek.Sunday)
                    {
                        hours++;
                    }
                }
            }
            else
            {
                while (start < end)
                {
                    start = start.AddHours(1);
                    if ((start.Hour > startWorkingHours &&
                       start.Hour <= 24) || (start.Hour >= 0 &&
                       start.Hour <= endWorkingHours) &&
                       start.DayOfWeek != DayOfWeek.Saturday &&
                       start.DayOfWeek != DayOfWeek.Sunday)
                    {
                        hours++;
                    }
                }
            }

            return hours;
        }
    }
}
