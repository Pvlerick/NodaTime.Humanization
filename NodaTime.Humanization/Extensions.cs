namespace NodaTime.Humanization
{
    internal static class Extensions
    {
        /// <summary>
        /// Returns an OffsetDateTime that represents the same moment in time, but at a potentially different Offset.
        /// </summary>
        /// <param name="offsetDateTime">The OffsetDateTime value to start with.</param>
        /// <param name="offset">The Offset to switch to.</param>
        /// <returns>The adjusted OffsetDateTime.</returns>
        public static OffsetDateTime SwitchOffset(this OffsetDateTime offsetDateTime, Offset offset)
        {
            return offsetDateTime.Offset != offset
                ? offsetDateTime.ToInstant().InZone(DateTimeZone.ForOffset(offset)).ToOffsetDateTime()
                : offsetDateTime;
        }
    }
}
