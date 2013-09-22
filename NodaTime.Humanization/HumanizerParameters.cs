using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    public sealed class HumanizerParameters
    {
        //Default values for the parameters
        const bool DefaultDisplaySignificantsZeroValueUnits = false;
        const int DefaultDigitsAfterDecimalPoint = 0;

        /// <summary>
        /// Indicates if significant zero value units are to be included in the result or not. For example for a period of 1 months, 0 days and 2 hours, is the 0 day included (and counted in the <see cref=" MaxiumumNumberOfUnitsToDisplay"/>).
        /// </summary>
        public bool DisplaySignificantZeroValueUnits { get; private set; }

        /// <summary>
        /// Set the number of digits that have to be displayed after the decimal point of the last unit to display, e.g. 1.4 days.
        /// </summary>
        public int DigitsAfterDecimalPoint { get; private set; }

        private HumanizerParameters() { }

        public class Builder
        {
            private bool displaySignificantZeroValueUnits = DefaultDisplaySignificantsZeroValueUnits;
            private int digitsAfterDecimalPoint = DefaultDigitsAfterDecimalPoint;

            public Builder DisplaySignificantZeroValueUnits(bool displaySignificantZeroValueUnits)
            {
                this.displaySignificantZeroValueUnits = displaySignificantZeroValueUnits;
                return this;
            }

            public Builder DigitsAfterDecimalPoint(int digitsAfterDecimalPoint)
            {
                this.digitsAfterDecimalPoint = digitsAfterDecimalPoint;
                return this;
            }

            public HumanizerParameters Build()
            {
                return new HumanizerParameters()
                {
                    DisplaySignificantZeroValueUnits = this.displaySignificantZeroValueUnits,
                    DigitsAfterDecimalPoint = this.digitsAfterDecimalPoint
                };
            }
        }
    }
}
