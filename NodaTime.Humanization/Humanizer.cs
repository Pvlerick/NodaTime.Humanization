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

        public PeriodUnits UnitsToDisplay { get; private set; }
        public HumanizerParameters Parameters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NodaTime.Humanization.Humanizer" /> class.
        /// </summary>
        public Humanizer() : this(DefaultUnitsToDisplay, new HumanizerParameters.Builder().Build()) { }

        public Humanizer(PeriodUnits unitsToDisplay) : this(unitsToDisplay, new HumanizerParameters.Builder().Build()) { }

        public Humanizer(HumanizerParameters parameters) : this(DefaultUnitsToDisplay, parameters) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NodaTime.Humanization.Humanizer" /> class.
        /// </summary>
        /// <param name="unitsToDisplay">The units to display in the result</param>
        public Humanizer(PeriodUnits unitsToDisplay, HumanizerParameters parameters)
        {
            this.UnitsToDisplay = unitsToDisplay;
            this.Parameters = parameters;
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

            //The units to display, in a list where we only take the first x
            var unitsToDisplay = this.GetUnitsToDisplay()
                                     //.Take(this.Parameters.MaxiumumNumberOfUnitsToDisplay)
                                     .ToList();

            //The last unit to display, important to know when to stop
            var lastUnitToDisplay = unitsToDisplay.Last();

            //The period has to use all the units to display plus the unit lower than the last one to allow rounding
            //TODO: this will also have to be changed if we want to round all the way up
            var period = Period.Between(start, end, this.UnitsToDisplay | this.GetLastUnitToCalculate(lastUnitToDisplay));
            var periodBuilder = period.ToBuilder();

            var sb = new StringBuilder();

            //Only iterrate over the X first units to display as we will use the lowest limit (unitsToDisplay or maxNumberofUnitsToDisplay)
            foreach (var unit in unitsToDisplay)
            {
                var value = (decimal)periodBuilder[unit];

                //If the value is zero and we  don't want to display zero values, move to the next unit
                if (value == 0 && !this.Parameters.DisplaySignificantZeroValueUnits)
                {
                    //May want to count that unit in the future - if we introduce a parameter for that purpose
                    continue;
                }

                if (sb.Length > 0)
                {
                    sb.Replace(String.Format(" {0} ", Properties.Resources.And), ", ");
                    sb.AppendFormat(" {0} ", Properties.Resources.And);
                }

                //If the current unit is the last unit that is display, we need to round what's left
                //TODO: currently the code only rounds the unit just after the last, but it might be nice to round all the way up from milliseconds
                if (unit == lastUnitToDisplay)
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
            }

            return sb.ToString();
        }

        private IEnumerable<PeriodUnits> GetUnitsToDisplay()
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

        private PeriodUnits GetLastUnitToCalculate(PeriodUnits lastUnitToDisplay)
        {
            //There might be a nicer way to write this...
            switch (lastUnitToDisplay)
            {
                case PeriodUnits.Years:
                    return PeriodUnits.Months;
                case PeriodUnits.Months:
                    //If UnitsToDisplay includes Weeks, the unit below Months is Weeks, otherwise it is Days
                    return (this.UnitsToDisplay & PeriodUnits.Weeks) == PeriodUnits.Weeks ? PeriodUnits.Weeks : PeriodUnits.Days;
                case PeriodUnits.Weeks:
                    return PeriodUnits.Days;
                case PeriodUnits.Days:
                    return PeriodUnits.Hours;
                case PeriodUnits.Hours:
                    return PeriodUnits.Minutes;
                case PeriodUnits.Minutes:
                    return PeriodUnits.Seconds;
                case PeriodUnits.Seconds:
                    return PeriodUnits.Milliseconds;
                case PeriodUnits.Milliseconds:
                    return PeriodUnits.Milliseconds;
                default:
                    throw new ArgumentOutOfRangeException("lastUnitToDisplay");
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
