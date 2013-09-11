using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    public sealed class HumanizerParameters
    {
        //Default values for the parameters
        const int DefaultMaximumNumberOfUnitsToDisplay = 10;
        const bool DefaultDisplayZeroValueUnits = false;

        /// <summary>
        /// This is the limit of number of units that will be returned in the resulting string.
        /// </summary>
        public int MaxiumumNumberOfUnitsToDisplay { get; private set; }

        /// <summary>
        /// Indicates if zero value units are to be included in the result or not. For example, if the period has 0 days, is it included (and counted in the <see cref=" MaxiumumNumberOfUnitsToDisplay"/>).
        /// </summary>
        public bool DisplayZeroValueUnits { get; private set; }

        private HumanizerParameters() { }

        public class Builder
        {
            private int maxiumumNumberOfUnitsToDisplay = DefaultMaximumNumberOfUnitsToDisplay;
            private bool displayZeroValueUnits = DefaultDisplayZeroValueUnits;

            public Builder WithMaxiumumNumberOfUnitsToDisplay(int maxiumumNumberOfUnitsToDisplay)
            {
                this.maxiumumNumberOfUnitsToDisplay = maxiumumNumberOfUnitsToDisplay;
                return this;
            }

            public Builder WithDisplayZeroValueUnits(bool displayZeroValueUnits)
            {
                this.displayZeroValueUnits = displayZeroValueUnits;
                return this;
            }

            public HumanizerParameters Build()
            {
                return new HumanizerParameters()
                {
                    MaxiumumNumberOfUnitsToDisplay = this.maxiumumNumberOfUnitsToDisplay,
                    DisplayZeroValueUnits = this.displayZeroValueUnits
                };
            }
        }
    }
}
