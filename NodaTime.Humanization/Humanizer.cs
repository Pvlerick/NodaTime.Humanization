using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    public sealed class Humanizer
    {
        const PeriodUnits DefaultUnitsToDisplay = PeriodUnits.DateAndTime;
        const int DefaultMaximumNumberOfUnitsToDisplay = 10;

        public PeriodUnits UnitsToDisplay { get; private set; }
        public int MaxiumumNumberOfUnitsToDisplay { get; private set; }

        public Humanizer() : this(DefaultUnitsToDisplay, DefaultMaximumNumberOfUnitsToDisplay) { }

        public Humanizer(PeriodUnits unitsToDisplay) : this(unitsToDisplay, DefaultMaximumNumberOfUnitsToDisplay) { }

        public Humanizer(int maximumNumberOfUnitsToDisplay) : this(DefaultUnitsToDisplay, maximumNumberOfUnitsToDisplay) { }

        public Humanizer(PeriodUnits unitsToDisplay, int maximumNumberOfUnitsToDisplay)
        {
            if (maximumNumberOfUnitsToDisplay < 1)
            {
                throw new ArgumentOutOfRangeException("maximumNumberOfUnitsToDisplay", "maximumNumberOfUnitsToDisplay must be positive.");
            }

            this.UnitsToDisplay = unitsToDisplay;
            this.MaxiumumNumberOfUnitsToDisplay = maximumNumberOfUnitsToDisplay;
        }

        public string GetRelativeTime(Instant instant)
        {
            return GetRelativeTime(instant, SystemClock.Instance);
        }

        public string GetRelativeTime(Instant instant, IClock clock)
        {
            var now = clock.Now;

            if (instant == now)
                return Properties.Resources.RightNow;

            var periodText = GetRelativeTime(instant, now);

            return instant < now
                ? String.Format(Properties.Resources.PeriodPast, periodText)
                : String.Format(Properties.Resources.PeriodFuture, periodText);
        }

        public string GetRelativeTime(Instant start, Instant end)
        {
            return GetRelativeTime(start.InUtc().LocalDateTime, end.InUtc().LocalDateTime);
        }

        public string GetRelativeTime(OffsetDateTime start, OffsetDateTime end)
        {
            return GetRelativeTime(start.LocalDateTime, end.SwitchOffset(start.Offset).LocalDateTime);
        }

        public string GetRelativeTime(ZonedDateTime start, ZonedDateTime end)
        {
            return GetRelativeTime(start.LocalDateTime, end.ToOffsetDateTime().SwitchOffset(start.Offset).LocalDateTime);
        }

        public string GetRelativeTime(LocalDate start, LocalDate end)
        {
            return GetRelativeTime(start.AtMidnight(), end.AtMidnight());
        }

        public string GetRelativeTime(LocalTime start, LocalTime end)
        {
            return GetRelativeTime(start.LocalDateTime, end.LocalDateTime);
        }

        public string GetRelativeTime(LocalDateTime start, LocalDateTime end)
        {
            if (start > end)
            {
                var t = end;
                end = start;
                start = t;
            }

            var period = Period.Between(start, end, this.UnitsToDisplay);
            var periodBuilder = period.ToBuilder();

            int terms = 0;
            var sb = new StringBuilder();
            foreach (var unit in this.GetIndividualUnitsToDisplay())
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
                if (terms == this.MaxiumumNumberOfUnitsToDisplay)
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

                sb.Append(this.GetTextForUnit(unit, textValue));

                if (terms == this.MaxiumumNumberOfUnitsToDisplay) break;
            }

            return sb.ToString();
        }

        private IEnumerable<PeriodUnits> GetIndividualUnitsToDisplay()
        {
            foreach (PeriodUnits unit in Enum.GetValues(typeof(PeriodUnits)))
            {
                if ((this.UnitsToDisplay & unit) == unit) //Logical AND to find the present units
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

        private String GetTextForUnit(PeriodUnits unit, String textValue)
        {
            //Depending on singular or plural, fetch different properties
            if (textValue == "1")
            {
                return this.GetTextForUnitSingular(unit);
            }
            else
            {
                return String.Format(this.GetTextForUnitPlural(unit), textValue);
            }
        }

        private String GetTextForUnitSingular(PeriodUnits unit)
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

        private String GetTextForUnitPlural(PeriodUnits unit)
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
