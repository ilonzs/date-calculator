using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public static class Calculator
    {
        /// <summary>
        /// Function to calculate the due date of a given issue.
        /// </summary>
        /// <param name="submitTime">The sumbit date/time of the issue.</param>
        /// <param name="turnAroundTime">The turnaround time of the issue defined in hours</param>
        /// <returns>The due date of the specified issue</returns>
        public static DateTime CalculateDueDate(DateTime submitTime, int turnAroundTime)
        {
            if (!IsValidWorkingTime(submitTime))
            {
                throw new ArgumentException("Submit time is not a valid working time.");
            }
            if (turnAroundTime <= 0)
            {
                throw new ArgumentException("Turnaround time should be a positive integer.");
            }

            int extraWorkingHours = turnAroundTime % 8;
            int wholeWorkingDays = turnAroundTime / 8;
            int wholeWorkingWeeks = wholeWorkingDays / 5;
            int extraWorkingDays = wholeWorkingDays % 5;

            return submitTime
                .AddWorkWeeks(wholeWorkingWeeks)
                .AddWorkDays(extraWorkingDays)
                .AddWorkHours(extraWorkingHours);
        }

        #region Private_Methods

        /// <summary>
        /// Returns true for a valid working time (between monday-friday, between 9AM-5PM)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static bool IsValidWorkingTime(DateTime time)
        {
            return ((int)time.DayOfWeek > 0 &&
                    (int)time.DayOfWeek < 6 &&
                    (time.TimeOfDay.Hours >= 9 &&
                        time.TimeOfDay.Hours <= 16 ||
                    time.TimeOfDay.Hours == 17 &&
                        time.TimeOfDay.Minutes == 0));
        }

        private static DateTime AddWorkWeeks(this DateTime dateTime, int workWeeks)
        {
            return dateTime.AddDays(workWeeks * 7);
        }

        private static DateTime AddWorkDays(this DateTime dateTime, int workDays)
        {
            if ((int)dateTime.DayOfWeek + workDays > 5)
            {
                workDays += 2;
            }

            return dateTime.AddDays(workDays);
        }

        private static DateTime AddWorkHours(this DateTime dateTime, int workHours)
        {
            var result = dateTime;
            if (dateTime.TimeOfDay.Hours + workHours >= 17)
            {
                result = result.AddWorkDays(1).AddHours(-8);
            }
            return result.AddHours(workHours);
        }

        #endregion Private_Methods
    }
}
