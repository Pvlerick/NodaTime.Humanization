# NodaTime.Humanization

Humanization add-on for [Noda Time](http://nodatime.org). Provides human readable strings such as "an hour ago".

### Quick Start

```csharp
var h = new Humanizer();

var start = new LocalDateTime(2013, 1, 1, 0, 0);
var end = new LocalDateTime(2014, 2, 1, 0, 0);

String relativeTime = h.GetRelativeTime(start, end); // "a year and a month"
```

#### Parameters

At the moment, the Humanizer class supports four parameters:

- unitsToDisplay: the [PeriodUnits](https://nodatime.org/1.1.x/api/NodaTime.PeriodUnits.html) that are to be included in the resulting string
- maxiumumNumberOfUnitsToDisplay: the number of units to display (default is 2)
- digitsAfterDecimalPoint: the number of digits, after decimal point, to display for the last unit in the resulting string
- displaySignificantZeroValueUnits: displays units that have a zero value but that are between units that have non-zero values

### Status

- In development. Version 0.1.0
- Need to add _many_ more unit tests.
- Localized with English (default) and French.
- No PCL version or NuGet package yet.

### Known issues

Refer to the [Issue Tracker](https://github.com/mj1856/NodaTime.Humanization/issues) for further details

### Contributing

- Feel free to pick up any of the items on the issue tracker or add your own.
- Fork the project and send a pull request with any changes.
- If you have questions, please use the issue tracker so everyone can participate.
- By contributing, you agree that your contributions are released to the public for inclusion in this library,
  which is released under the MIT license.
- If you need to contact the project owner, send email to Matt Johnson (mj1856 _at_ hotmail).
