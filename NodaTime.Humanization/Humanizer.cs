using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    public static class Humanizer
    {
        public static string GetRelativeTime(Instant instant, int maxTerms = 1)
        {
            return GetRelativeTime(instant, SystemClock.Instance, maxTerms);
        }

        public static string GetRelativeTime(Instant instant, IClock clock, int maxTerms = 1)
        {
            var now = clock.Now;

            if (instant == now)
                return Properties.Resources.RightNow;

            var periodText = GetRelativeTime(instant, now, maxTerms);

            return instant < now
                ? String.Format(Properties.Resources.PeriodPast, periodText)
                : String.Format(Properties.Resources.PeriodFuture, periodText);
        }

        public static string GetRelativeTime(Instant start, Instant end, int maxTerms = 1)
        {
            return GetRelativeTime(start.InUtc().LocalDateTime, end.InUtc().LocalDateTime, maxTerms);
        }

        public static string GetRelativeTime(OffsetDateTime start, OffsetDateTime end, int maxTerms = 1)
        {
            return GetRelativeTime(start.LocalDateTime, end.SwitchOffset(start.Offset).LocalDateTime, maxTerms);
        }

        public static string GetRelativeTime(ZonedDateTime start, ZonedDateTime end, int maxTerms = 1)
        {
            return GetRelativeTime(start.LocalDateTime, end.ToOffsetDateTime().SwitchOffset(start.Offset).LocalDateTime, maxTerms);
        }

        public static string GetRelativeTime(LocalDate start, LocalDate end, int maxTerms = 1)
        {
            if (maxTerms < 1 || maxTerms > 3)
                throw new ArgumentOutOfRangeException("maxTerms", "maxTerms must be between 1 and 3 when passing LocalDate values.");

            return GetRelativeTime(start.AtMidnight(), end.AtMidnight(), maxTerms);
        }

        public static string GetRelativeTime(LocalTime start, LocalTime end, int maxTerms = 1)
        {
            return GetRelativeTime(start, end, PeriodUnits.DateAndTime, maxTerms);
        }

        public static string GetRelativeTime(LocalTime start, LocalTime end, PeriodUnits unitsToDisplay, int maxTerms = 1)
        {
            if (maxTerms < 1 || maxTerms > 3)
                throw new ArgumentOutOfRangeException("maxTerms", "maxTerms must be between 1 and 3 when passing LocalTime values.");

            return GetRelativeTime(start.LocalDateTime, end.LocalDateTime, unitsToDisplay, maxTerms);
        }

        public static string GetRelativeTime(LocalDateTime start, LocalDateTime end, int maxTerms = 1)
        {
            return GetRelativeTime(start, end, PeriodUnits.DateAndTime, maxTerms);
        }

        public static string GetRelativeTime(LocalDateTime start, LocalDateTime end, PeriodUnits unitsToDisplay, int maxTerms = 1)
        {
            if (start > end)
            {
                var t = end;
                end = start;
                start = t;
            }

            var period = Period.Between(start, end, unitsToDisplay);
            var periodBuilder = period.ToBuilder();

            int terms = 0;
            var sb = new StringBuilder();
            foreach (var unit in GetIndividualUnits(unitsToDisplay))
            {
                var value = (decimal)periodBuilder[unit];
                if (value == 0)
                    continue;

                if (sb.Length > 0)
                {
                    sb.Replace(String.Format(" {0} ", Properties.Resources.And), ", ");
                    sb.AppendFormat(" {0} ", Properties.Resources.And);
                }

                terms++;
                if (terms == maxTerms)
                {
                    switch (unit)
                    {
                        case PeriodUnits.Years:
                            value += period.Months / 12M;
                            break;

                        case PeriodUnits.Months:
                            // when counting by months, we give fractions in terms of the month we didn't complete.
                            var daysInEndMonth = end.Calendar.GetDaysInMonth(end.Year, end.Month);
                            value += period.Days / (decimal)daysInEndMonth;
                            break;

                        case PeriodUnits.Weeks:
                            // when counting weeks, we need to take partial days into account to get values like "1.5 weeks"
                            value += (period.Days + (period.Hours / 24M)) / 7M;
                            break;

                        case PeriodUnits.Days:
                            value += period.Hours / 24M;
                            break;

                        case PeriodUnits.Hours:
                            value += period.Minutes / 60M;
                            break;

                        case PeriodUnits.Minutes:
                            value += period.Seconds / 60M;
                            break;

                        case PeriodUnits.Seconds:
                            // no fractional seconds in results
                            value += period.Milliseconds < 500 ? 0 : 1;
                            break;
                    }
                }

                var textValue = value.ToString("0.#");

                sb.Append(GetTextForUnit(unit, textValue));

                if (terms == maxTerms) break;
            }

            return sb.ToString();
        }

        private static IEnumerable<PeriodUnits> GetIndividualUnits(PeriodUnits units)
        {
            foreach (PeriodUnits unit in Enum.GetValues(typeof(PeriodUnits)))
            {
                if ((units & unit) == unit) //Logical AND to find the present units
                {
                    //Only return the units that are useful, the rest is ignored
                    switch (unit)
                    {
                        case PeriodUnits.Years:
                            yield return PeriodUnits.Years;
                            break;
                        case PeriodUnits.Months:
                            yield return PeriodUnits.Months;
                            break;
                        case PeriodUnits.Weeks:
                            yield return PeriodUnits.Weeks;
                            break;
                        case PeriodUnits.Days:
                            yield return PeriodUnits.Days;
                            break;
                        case PeriodUnits.Hours:
                            yield return PeriodUnits.Hours;
                            break;
                        case PeriodUnits.Minutes:
                            yield return PeriodUnits.Minutes;
                            break;
                        case PeriodUnits.Seconds:
                            yield return PeriodUnits.Seconds;
                            break;
                        case PeriodUnits.Milliseconds:
                            yield return PeriodUnits.Milliseconds;
                            break;
                    }
                }
            }
        }

        private static String GetTextForUnit(PeriodUnits unit, String textValue)
        {
            //Depending on singular or plural, fetch different properties
            if (textValue == "1")
            {
                return GetTextForUnitSingular(unit);
            }
            else
            {
                return String.Format(GetTextForUnitPlural(unit), textValue);
            }
        }

        private static String GetTextForUnitSingular(PeriodUnits unit)
        {
            switch (unit)
            {
                case PeriodUnits.Days:
                    return Properties.Resources.OneDay;
                case PeriodUnits.Hours:
                    return Properties.Resources.OneHour;
                case PeriodUnits.Milliseconds:
                    return Properties.Resources.OneMillisecond;
                case PeriodUnits.Minutes:
                    return Properties.Resources.OneMinute;
                case PeriodUnits.Months:
                    return Properties.Resources.OneMonth;
                case PeriodUnits.Seconds:
                    return Properties.Resources.OneSecond;
                case PeriodUnits.Ticks:
                    return Properties.Resources.OneTick;
                case PeriodUnits.Weeks:
                    return Properties.Resources.OneWeek;
                case PeriodUnits.Years:
                    return Properties.Resources.OneYear;
                default:
                    //Shouldnt land in here...
                    return String.Empty;
            }
        }

        private static String GetTextForUnitPlural(PeriodUnits unit)
        {
            switch (unit)
            {
                case PeriodUnits.Days:
                    return Properties.Resources.ManyDays;
                case PeriodUnits.Hours:
                    return Properties.Resources.ManyHours;
                case PeriodUnits.Milliseconds:
                    return Properties.Resources.ManyMilliseconds;
                case PeriodUnits.Minutes:
                    return Properties.Resources.ManyMinutes;
                case PeriodUnits.Months:
                    return Properties.Resources.ManyMonths;
                case PeriodUnits.Seconds:
                    return Properties.Resources.ManySeconds;
                case PeriodUnits.Ticks:
                    return Properties.Resources.ManyTicks;
                case PeriodUnits.Weeks:
                    return Properties.Resources.ManyWeeks;
                case PeriodUnits.Years:
                    return Properties.Resources.ManyYears;
                default:
                    //Shouldnt land in here...
                    return String.Empty;
            }
        }
    }
}
