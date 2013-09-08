using System;
using System.Text;

namespace NodaTime.Humanization
{
    public static class Humanizer
    {
        public static string GetRelativeTime(Instant instant, int maxTerms = 1, bool countWeeks = false)
        {
            return GetRelativeTime(instant, SystemClock.Instance, maxTerms, countWeeks);
        }

        public static string GetRelativeTime(Instant instant, IClock clock, int maxTerms = 1, bool countWeeks = false)
        {
            var now = clock.Now;

            if (instant == now)
                return "right now";

            var periodText = GetRelativeTime(instant, now, maxTerms, countWeeks);

            return instant < now
                ? periodText + " ago"
                : "in " + periodText;
        }

        public static string GetRelativeTime(Instant start, Instant end, int maxTerms = 1, bool countWeeks = false)
        {
            return GetRelativeTime(start.InUtc().LocalDateTime, end.InUtc().LocalDateTime, maxTerms, countWeeks);
        }

        public static string GetRelativeTime(OffsetDateTime start, OffsetDateTime end, int maxTerms = 1, bool countWeeks = false)
        {
            return GetRelativeTime(start.LocalDateTime, end.SwitchOffset(start.Offset).LocalDateTime, maxTerms, countWeeks);
        }

        public static string GetRelativeTime(ZonedDateTime start, ZonedDateTime end, int maxTerms = 1, bool countWeeks = false)
        {
            return GetRelativeTime(start.LocalDateTime, end.ToOffsetDateTime().SwitchOffset(start.Offset).LocalDateTime, maxTerms, countWeeks);
        }

        public static string GetRelativeTime(LocalDate start, LocalDate end, int maxTerms = 1, bool countWeeks = false)
        {
            if (maxTerms < 1 || maxTerms > 3)
                throw new ArgumentOutOfRangeException("maxTerms", "maxTerms must be between 1 and 3 when passing LocalDate values.");

            return GetRelativeTime(start.AtMidnight(), end.AtMidnight(), maxTerms, countWeeks);
        }

        public static string GetRelativeTime(LocalTime start, LocalTime end, int maxTerms = 1, bool countWeeks = false)
        {
            if (maxTerms < 1 || maxTerms > 3)
                throw new ArgumentOutOfRangeException("maxTerms", "maxTerms must be between 1 and 3 when passing LocalTime values.");

            return GetRelativeTime(start.LocalDateTime, end.LocalDateTime, maxTerms, countWeeks);
        }

        public static string GetRelativeTime(LocalDateTime start, LocalDateTime end, int maxTerms = 1, bool countWeeks = false)
        {
            if (maxTerms < 1 || maxTerms > (countWeeks ? 7 : 6))
                throw new ArgumentOutOfRangeException("maxTerms", "maxTerms must be between 1 and 6, or 7 if weeks are counted.");

            if (start > end)
            {
                var t = end;
                end = start;
                start = t;
            }

            var period = Period.Between(start, end, countWeeks ? PeriodUnits.AllUnits : PeriodUnits.DateAndTime);
            var periodBuilder = period.ToBuilder();

            var units = countWeeks
                ? new[]
                  {
                      PeriodUnits.Years, PeriodUnits.Months, PeriodUnits.Weeks, PeriodUnits.Days,
                      PeriodUnits.Hours, PeriodUnits.Minutes, PeriodUnits.Seconds
                  }
                : new[]
                  {
                      PeriodUnits.Years, PeriodUnits.Months, PeriodUnits.Days,
                      PeriodUnits.Hours, PeriodUnits.Minutes, PeriodUnits.Seconds
                  };

            int terms = 0;
            var sb = new StringBuilder();
            foreach (var unit in units)
            {
                var value = (decimal)periodBuilder[unit];
                if (value == 0)
                    continue;

                if (sb.Length > 0)
                {
                    sb.Replace(" and ", ", ");
                    sb.Append(" and ");
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
                            value += period.Days / (decimal) daysInEndMonth;
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

                var valueText = value.ToString("0.#");
                var unitText = unit.ToString().ToLower();

                if (valueText == "1")
                {
                    valueText = unit == PeriodUnits.Hours ? "an" : "a";
                    unitText = unitText.TrimEnd('s');
                }

                sb.AppendFormat("{0} {1}", valueText, unitText);

                if (terms == maxTerms) break;
            }

            return sb.ToString();
        }
    }
}
