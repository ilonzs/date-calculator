using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Test
{
    [TestClass]
    public class CalculatorTests
    {
        const string SubmitTimeErrorMessage = "Submit time is not a valid working time";
        const string TurnaroundTimeErrorMessage = "Turnaround time should be a positive integer";

        #region Argument_Testing

        [TestMethod]
        public void CalculateDueDate_WeekendSubmitTime_OnSundayEvening_ShouldThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 15, 13, 45, 0);

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, 0);

                // Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Assert
                Assert.IsTrue(e is ArgumentException);
                Assert.IsTrue(e.Message.StartsWith(SubmitTimeErrorMessage));
            }
        }

        [TestMethod]
        public void CalculateDueDate_WeekdayButNotWorkingHour_TuesdayMorning_at_8_30_ShouldThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 17, 08, 30, 0);

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, 0);

                // Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Assert
                Assert.IsTrue(e is ArgumentException);
                Assert.IsTrue(e.Message.StartsWith(SubmitTimeErrorMessage));
            }
        }

        [TestMethod]
        public void CalculateDueDate_WeekdayButNotWorkingHourTuesday_at_5_00_PM_ShouldNotThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 17, 17, 0, 18);

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, 8);
            }
            catch (Exception)
            {
                // Assert
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CalculateDueDate_WeekdayButNotWorkingHourTuesday_5_15_PM_ShouldThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 17, 17, 15, 0);

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, 0);

                // Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Assert
                Assert.IsTrue(e is ArgumentException);
                Assert.IsTrue(e.Message.StartsWith(SubmitTimeErrorMessage));
            }
        }

        [TestMethod]
        public void CalculateDueDate_WeekdayButNotWorkingHour_TuesdayEvening_ShouldThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 17, 18, 30, 0);

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, 0);

                // Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Assert
                Assert.IsTrue(e is ArgumentException);
                Assert.IsTrue(e.Message.StartsWith(SubmitTimeErrorMessage));
            }
        }

        [TestMethod]
        public void CalculateDueDate_NegativeTurnaroundTime_ShouldThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 17, 11, 20, 0);
            var turnaroundTime = -1;

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, turnaroundTime);

                // Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Assert
                Assert.IsTrue(e is ArgumentException);
                Assert.IsTrue(e.Message.StartsWith(TurnaroundTimeErrorMessage));
            }
        }

        [TestMethod]
        public void CalculateDueDate_ZeroTurnaroundTime_ShouldThrowException()
        {
            // Arrange
            var date = new DateTime(2020, 3, 17, 11, 20, 0);
            var turnaroundTime = 0;

            // Act
            try
            {
                var result = Calculator.CalculateDueDate(date, turnaroundTime);

                // Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                // Assert
                Assert.IsTrue(e is ArgumentException);
                Assert.IsTrue(e.Message.StartsWith(TurnaroundTimeErrorMessage));
            }
        }

        #endregion Argument_Testing

        #region Validation_Testing_WholeDays

        [TestMethod]
        public void CalculateDueDate_ThreeWholeWorkDaysLater_WithWeekend()
        {
            // Arrange
            var date = new DateTime(2020, 4, 16, 11, 20, 0);
            var turnaroundTime = 3 * 8;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(21, result.Day);
        }

        [TestMethod]
        public void CalculateDueDate_ThreeWholeWorkDaysLater_WithoutWeekend()
        {
            // Arrange
            var date = new DateTime(2020, 4, 21, 11, 20, 0);
            var turnaroundTime = 3 * 8;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(24, result.Day);

        }

        [TestMethod]
        public void CalculateDueDate_OneWorkDayLater_OnFriday()
        {
            // Arrange
            var date = new DateTime(2020, 5, 8, 11, 20, 0);
            var turnaroundTime = 1 * 8;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(11, result.Day);
        }

        [TestMethod]
        public void CalculateDueDate_TwoWeekAndOneDayLater_StartingTuesday_ShouldEndOnWednesday2WeeksLater()
        {
            // Arrange
            var date = new DateTime(2020, 4, 21, 11, 20, 0);
            var turnaroundTime = 2 * 40 + 1 * 8;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(5, result.Month);
            Assert.AreEqual(6, result.Day);

        }

        #endregion Validation_Testing_WholeDays

        #region Validation_Testing_FractionOfDays

        [TestMethod]
        public void CalculateDueDate_Starting_9_15_With_2_Hours_Turnaraound_ShouldResultIn_SameDay_2HoursLater()
        {
            // Arrange
            var date = new DateTime(2020, 5, 4, 9, 15, 25);
            var turnaroundTime = 2;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day, result.Day);
            Assert.AreEqual(date.Hour + 2, result.Hour);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_11_00_With_4_Hours_Turnaraound()
        {
            // Arrange
            var date = new DateTime(2020, 5, 4, 11, 0, 25);
            var turnaroundTime = 4;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day, result.Day);
            Assert.AreEqual(date.Hour + 4, result.Hour);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_11_00_With_6_Hours_Turnaraound_ShouldResultInNextMorning()
        {
            // Arrange
            var date = new DateTime(2020, 5, 4, 11, 00, 25);
            var turnaroundTime = 6;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day + 1, result.Day);
            Assert.AreEqual(9, result.Hour);
            Assert.AreEqual(0, result.Minute);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_12_25_With_6_Hours_Turnaraound_ShouldResultInNextMorning()
        {
            // Arrange
            var date = new DateTime(2020, 5, 4, 12, 25, 25);
            var turnaroundTime = 6;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day + 1, result.Day);
            Assert.AreEqual(10, result.Hour);
            Assert.AreEqual(25, result.Minute);
            Assert.AreEqual(25, result.Second);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_On_Friday_16_30_With_3_Hours_Turnaraound_ShouldResultInMondayMorning()
        {
            // Arrange
            var date = new DateTime(2020, 5, 8, 16, 30, 25);
            var turnaroundTime = 3;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day + 3, result.Day);
            Assert.AreEqual(11, result.Hour);
            Assert.AreEqual(30, result.Minute);
            Assert.AreEqual(25, result.Second);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_On_Friday_16_30_With_1_day_3_Hours_Turnaraound_ShouldResultInMondayMorning()
        {
            // Arrange
            var date = new DateTime(2020, 5, 8, 16, 30, 25);
            var turnaroundTime = 8 + 3;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day + 4, result.Day);
            Assert.AreEqual(11, result.Hour);
            Assert.AreEqual(30, result.Minute);
            Assert.AreEqual(25, result.Second);
        }

        #endregion Validation_Testing_FractionOfDays

        #region Validation_Testing_Miscellaneous

        [TestMethod]
        public void CalculateDueDate_Starting_On_Wednesday_10_15_With_3_Days_7_Hours_Turnaraound_ShouldResultInNextTuesday()
        {
            // Arrange
            var date = new DateTime(2020, 5, 13, 10, 15, 25);
            var turnaroundTime = 3 * 8 + 7;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(date.Day + 6, result.Day);
            Assert.AreEqual(9, result.Hour);
            Assert.AreEqual(15, result.Minute);
            Assert.AreEqual(25, result.Second);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_On_Wednesday_10_15_With_2_Weeks_3_Days_7_Hours_Turnaraound_ShouldResultInTuesday2WeeksLater()
        {
            // Arrange
            var date = new DateTime(2020, 5, 13, 10, 15, 25);
            var turnaroundTime = 2 * 40 + 3 * 8 + 7;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month + 1, result.Month);
            Assert.AreEqual(2, result.Day);
            Assert.AreEqual(9, result.Hour);
            Assert.AreEqual(15, result.Minute);
            Assert.AreEqual(25, result.Second);
        }

        [TestMethod]
        public void CalculateDueDate_YearChange()
        {
            // Arrange
            var date = new DateTime(2020, 12, 25, 13, 15, 35);
            var turnaroundTime = 1 * 40 + 8 + 2;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year + 1, result.Year);
            Assert.AreEqual(1, result.Month);
            Assert.AreEqual(4, result.Day);
            Assert.AreEqual(15, result.Hour);
            Assert.AreEqual(15, result.Minute);
            Assert.AreEqual(35, result.Second);
        }

        [TestMethod]
        public void CalculateDueDate_Starting_On_Thu_15_30_With_4_day_2_Hours_Turnaraound_ShouldResultInNextTueMorning()
        {
            // Arrange
            var date = new DateTime(2020, 5, 14, 15, 37, 0);
            var turnaroundTime = 4 * 8 + 2;

            // Act
            var result = Calculator.CalculateDueDate(date, turnaroundTime);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(date.Year, result.Year);
            Assert.AreEqual(date.Month, result.Month);
            Assert.AreEqual(21, result.Day);
            Assert.AreEqual(9, result.Hour);
            Assert.AreEqual(37, result.Minute);
            Assert.AreEqual(00, result.Second);
        }

        #endregion Validation_Testing_Miscellaneous
    }
}
