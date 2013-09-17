﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodaTime.Humanization
{
    public sealed class HumanizerParameters
    {
        //Default values for the parameters
        const int DefaultMaximumNumberOfUnitsToDisplay = 10;
        const bool DefaultDisplaySignificantsZeroValueUnits = false;

        /// <summary>
        /// This is the limit of number of units that will be returned in the resulting string.
        /// </summary>
        public int MaxiumumNumberOfUnitsToDisplay { get; private set; }

        /// <summary>
        /// Indicates if significant zero value units are to be included in the result or not. For example for a period of 1 months, 0 days and 2 hours, is the 0 day included (and counted in the <see cref=" MaxiumumNumberOfUnitsToDisplay"/>).
        /// </summary>
        public bool DisplaySignificantZeroValueUnits { get; private set; }

        private HumanizerParameters() { }

        public class Builder
        {
            private int maxiumumNumberOfUnitsToDisplay = DefaultMaximumNumberOfUnitsToDisplay;
            private bool displaySignificantZeroValueUnits = DefaultDisplaySignificantsZeroValueUnits;

            public Builder MaxiumumNumberOfUnitsToDisplay(int maxiumumNumberOfUnitsToDisplay)
            {
                this.maxiumumNumberOfUnitsToDisplay = maxiumumNumberOfUnitsToDisplay;
                return this;
            }

            public Builder DisplaySignificantZeroValueUnits(bool displaySignificantZeroValueUnits)
            {
                this.displaySignificantZeroValueUnits = displaySignificantZeroValueUnits;
                return this;
            }

            public HumanizerParameters Build()
            {
                return new HumanizerParameters()
                {
                    MaxiumumNumberOfUnitsToDisplay = this.maxiumumNumberOfUnitsToDisplay,
                    DisplaySignificantZeroValueUnits = this.displaySignificantZeroValueUnits
                };
            }
        }
    }
}
