using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    internal static class PeriodUnitsExtensions
    {
        /// <summary>
        /// Returns the list of distinct units that are interesting to display, e.g.: Years, Months, Week, Days, Hours, Minutes, Seconds and Milliseconds.
        /// </summary>
        /// <param name="units">The units enumeration value to get the distinct units from</param>
        /// <returns></returns>
        public static IEnumerable<PeriodUnits> GetDistinctUnits(this PeriodUnits units)
        {
            foreach (var unit in Enum.GetValues(typeof(PeriodUnits))
                                     .Cast<PeriodUnits>()
                                     .Where(u => units.Contains(u)))
            {
                //Only return single units, compound units are ignored
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
                    case PeriodUnits.Ticks:
                        yield return PeriodUnits.Ticks;
                        break;
                }
            }
        }

        public static PeriodUnits GetSmallestSingleUnit(this PeriodUnits units)
        {
            if (units.Contains(PeriodUnits.Milliseconds)) return PeriodUnits.Milliseconds;
            else if (units.Contains(PeriodUnits.Seconds)) return PeriodUnits.Seconds;
            else if (units.Contains(PeriodUnits.Minutes)) return PeriodUnits.Minutes;
            else if (units.Contains(PeriodUnits.Hours)) return PeriodUnits.Hours;
            else if (units.Contains(PeriodUnits.Days)) return PeriodUnits.Days;
            else if (units.Contains(PeriodUnits.Weeks)) return PeriodUnits.Weeks;
            else if (units.Contains(PeriodUnits.Months)) return PeriodUnits.Months;
            else if (units.Contains(PeriodUnits.Years)) return PeriodUnits.Years;
            else throw new ArgumentOutOfRangeException("unit");
        }

        public static bool Contains(this PeriodUnits units, PeriodUnits otherUnit)
        {
            return (units & otherUnit) == otherUnit;
        }
    }
}
