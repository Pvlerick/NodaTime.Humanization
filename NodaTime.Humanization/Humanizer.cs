using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    public sealed class Humanizer
    {
        const PeriodUnits DefaultUnitsToDisplay = PeriodUnits.Years | PeriodUnits.Months | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes | PeriodUnits.Seconds;
        const int DefaultMaximumNumberOfUnitsToDisplay = 2;

        /// <summary>
        /// The <see cref="NodaTime.PeriodUnits"/> that have to be displayed in the final result.
        /// </summary>
        public PeriodUnits UnitsToDisplay { get; private set; }

        /// <summary>
        /// The the limit of number of units that will be returned in the resulting string.
        /// </summary>
        public int MaxiumumNumberOfUnitsToDisplay { get; private set; }

        public HumanizerParameters Parameters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NodaTime.Humanization.Humanizer" /> class.
        /// </summary>
        public Humanizer() : this(DefaultUnitsToDisplay, DefaultMaximumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().Build()) { }

        public Humanizer(PeriodUnits unitsToDisplay) : this(unitsToDisplay, DefaultMaximumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().Build()) { }

        public Humanizer(int maxiumumNumberOfUnitsToDisplay) : this(DefaultUnitsToDisplay, maxiumumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().Build()) { }

        public Humanizer(HumanizerParameters parameters) : this(DefaultUnitsToDisplay, DefaultMaximumNumberOfUnitsToDisplay, parameters) { }

        public Humanizer(PeriodUnits unitsToDisplay, int maxiumumNumberOfUnitsToDisplay) : this(unitsToDisplay, maxiumumNumberOfUnitsToDisplay, new HumanizerParameters.Builder().Build()) { }

        public Humanizer(PeriodUnits unitsToDisplay, HumanizerParameters parameters) : this(unitsToDisplay, DefaultMaximumNumberOfUnitsToDisplay, parameters) { }

        public Humanizer(int maxiumumNumberOfUnitsToDisplay, HumanizerParameters parameters) : this(DefaultUnitsToDisplay, maxiumumNumberOfUnitsToDisplay, parameters) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NodaTime.Humanization.Humanizer" /> class.
        /// </summary>
        /// <param name="unitsToDisplay">The units to display in the result</param>
        public Humanizer(PeriodUnits unitsToDisplay, int maxiumumNumberOfUnitsToDisplay, HumanizerParameters parameters)
        {
            this.UnitsToDisplay = unitsToDisplay;
            this.MaxiumumNumberOfUnitsToDisplay = maxiumumNumberOfUnitsToDisplay;
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

            var unitsForRounding = this.GetRequiredUnitsForRounding();

            //The period has to use all the units to display plus the unit just lower than the last one to allow rounding
            var period = Period.Between(start, end, unitsForRounding);
            var periodBuilder = period.ToBuilder();

            var significantUnits = this.GetSignificantUnitsForPeriod(period);
            var lastUnitToDisplay = this.GetLastUnitToDisplay(significantUnits);

            var sb = new StringBuilder();

            //Only iterrate over the X first units to display as we will use the lowest limit (unitsToDisplay or maxNumberofUnitsToDisplay)
            foreach (var unit in this.UnitsToDisplay.GetDistinctUnits())
            {
                //if the unit is both to be displayed and significant in the period, we have to add it
                if ((this.UnitsToDisplay & significantUnits & unit) == unit)
                {
                    var value = (decimal)periodBuilder[unit];

                    if (sb.Length > 0)
                    {
                        sb.Replace(String.Format(" {0} ", Properties.Resources.And), ", ");
                        sb.AppendFormat(" {0} ", Properties.Resources.And);
                    }

                    //In case it's the last unit that we need to display, we need the fractional value of the unit just below,
                    // rounded according to rules found in the parameter object
                    if (unit == lastUnitToDisplay)
                    {
                        value += this.GetFractionalValueForLastUnit(period, end, lastUnitToDisplay);
                    }
                    else
                    {
                        //Nothing to do, use the raw value
                    }

                    var textValue = value.ToString("0.#");

                    sb.Append(this.GetTextForUnit(unit, textValue));
                }
            }

            return sb.ToString();
        }

        internal PeriodUnits GetSignificantUnitsForPeriod(Period period)
        {
            PeriodUnits significantUnits = PeriodUnits.None;
            int count = 0;

            if (period.Years > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Years;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Months > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Months;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Weeks > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Weeks;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Days > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Days;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Hours > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Hours;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Minutes > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Minutes;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Seconds > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Seconds;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            if (period.Milliseconds > 0)
            {
                significantUnits = significantUnits | PeriodUnits.Milliseconds;
                if (++count == this.MaxiumumNumberOfUnitsToDisplay) return significantUnits;
            }

            return significantUnits;
        }

        //TODO A few unit tests to be added here, just to make a point :-)
        internal PeriodUnits GetLastUnitToDisplay(PeriodUnits significantUnits)
        {
            if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Milliseconds) == PeriodUnits.Milliseconds) return PeriodUnits.Milliseconds;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Seconds) == PeriodUnits.Seconds) return PeriodUnits.Seconds;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Minutes) == PeriodUnits.Minutes) return PeriodUnits.Minutes;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Hours) == PeriodUnits.Hours) return PeriodUnits.Hours;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Days) == PeriodUnits.Days) return PeriodUnits.Days;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Weeks) == PeriodUnits.Weeks) return PeriodUnits.Weeks;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Months) == PeriodUnits.Months) return PeriodUnits.Months;
            else if ((this.UnitsToDisplay & significantUnits & PeriodUnits.Years) == PeriodUnits.Years) return PeriodUnits.Years;
            
            //If nothing was found, there is no common unit between UnitsToDisplay and significantsUnits... Let's simply return the smallest unit in UnitsToDisplay.
            return this.UnitsToDisplay.GetSmallestSingleUnit();
        }

        //TODO This should be unit tested, although it looks correct - just to have a safety net in case it is changed later
        internal decimal GetFractionalValueForLastUnit(Period period, LocalDateTime end, PeriodUnits lastUnit)
        {
            switch (lastUnit)
            {
                case PeriodUnits.Years:
                    return period.Months / 12M;

                case PeriodUnits.Months:
                    // when counting by months, we give fractions in terms of the month we didn't complete.
                    var daysInEndMonth = end.Calendar.GetDaysInMonth(end.Year, end.Month);
                    return period.Days / (decimal)daysInEndMonth;

                case PeriodUnits.Weeks:
                    // when counting weeks, we need to take partial days into account to get values like "1.5 weeks"
                    return (period.Days + (period.Hours / 24M)) / 7M;

                case PeriodUnits.Days:
                    return period.Hours / 24M;

                case PeriodUnits.Hours:
                    return period.Minutes / 60M;

                case PeriodUnits.Minutes:
                    return period.Seconds / 60M;

                case PeriodUnits.Seconds:
                    // no fractional seconds in results
                    return period.Milliseconds < 500 ? 0 : 1;
                default:
                    return 0M;
            }
        }

        /// <summary>
        /// Adds to the given PeriodUnits the single unit that is smaller than the smallest present single unit, e.g.: Days | Hours will return Days | Hours | Minutes.
        /// </summary>
        /// <returns>The PeriodUnits augmented with a single unit.</returns>
        internal PeriodUnits GetRequiredUnitsForRounding()
        {
            if (this.UnitsToDisplay.Contains(PeriodUnits.Milliseconds)) return this.UnitsToDisplay; //Already have the lowest unit
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Seconds)) return this.UnitsToDisplay | PeriodUnits.Milliseconds;
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Minutes)) return this.UnitsToDisplay | PeriodUnits.Seconds;
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Hours)) return this.UnitsToDisplay | PeriodUnits.Minutes;
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Days)) return this.UnitsToDisplay | PeriodUnits.Hours;
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Weeks)) return this.UnitsToDisplay | PeriodUnits.Days | PeriodUnits.Hours; //Round weeks with days and hours
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Months)) return this.UnitsToDisplay | PeriodUnits.Days;
            else if (this.UnitsToDisplay.Contains(PeriodUnits.Years)) return this.UnitsToDisplay | PeriodUnits.Months;
            else return this.UnitsToDisplay;
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
                    throw new ArgumentOutOfRangeException("unit", "GetTextForUnitSingular only takes a single unit");
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
                    throw new ArgumentOutOfRangeException("unit", "GetTextForUnitPlural only takes a single unit");
            }
        }
    }
}
