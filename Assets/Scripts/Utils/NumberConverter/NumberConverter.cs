public class NumberConverter
{
    /// <summary>
    /// Converts a given number into a human-readable string format with an appropriate suffix (e.g., K for thousands, M for millions).
    /// The method supports both integer and floating-point formatting based on the input parameters.
    /// </summary>
    /// <param name="number">The number to be converted into a formatted string.</param>
    /// <param name="canFloatNumber">
    /// Determines whether the number can have decimal places. 
    /// If true, the number may include decimals; if false, it will always be rounded to an integer.
    /// Default is true.
    /// </param>
    /// <param name="BigDigitsFormatter">
    /// Specifies the numeric format string used for large numbers (greater than or equal to 1000). 
    /// Default is "F3", which formats the number to three decimal places.
    /// </param>
    /// <returns>
    /// A formatted string representing the number with a suffix (e.g., "1.5K" for 1500, "2M" for 2,000,000).
    /// If the number is less than 1000, it may include decimals depending on the value of <paramref name="canFloatNumber"/>.
    /// </returns>
    public static string ConvertNumberToString(double number, bool canFloatNumber = true, string BigDigitsFormatter = "F3")
    {
        string[] suffixes = { "", "K", "M", "B", "T", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q",
                              "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Aa", "Bb", "Cc", "Dd", "Ee", "Ff", "Gg", "Hh", "Ii", "Jj", "Kk", "Ll",
                              "Mm", "Nn", "Oo", "Pp", "Qq", "Rr", "Ss", "Tt", "Uu", "Vv", "Ww", "Xx", "Yy", "Zz" };

        int suffixIndex = 0;
        double convertedNumber = number;

        while (convertedNumber >= 1000.0 && suffixIndex < suffixes.Length - 1)
        {
            convertedNumber /= 1000.0;
            suffixIndex++;
        }

        string format = canFloatNumber ? ((number < 1000) ? ((convertedNumber % 1 == 0) ? "F0" : "F2") : BigDigitsFormatter) : "F0";
        return $"{convertedNumber.ToString(format)} {suffixes[suffixIndex]}";
    }

    /// <summary>
    /// Converts a given time in seconds into a human-readable string format with years, months, hours, minutes, and seconds.
    /// The method dynamically formats the output to include only relevant time units:
    /// - If the time is less than 1 minute, it displays only seconds (e.g., "40s").
    /// - If the time is between 1 minute and 1 hour, it displays minutes and seconds (e.g., "1min 20s").
    /// - If the time is 1 year or more, it displays years, months, and days (e.g., "2y 3mo 15d").
    /// </summary>
    /// <param name="time">The time duration in seconds to be converted into a formatted string.</param>
    /// <returns>
    /// A formatted string representing the time in a human-readable format with units (e.g., "2y 3mo 15d 3h 20min 37s").
    /// </returns>
    public static string GetTimePassed(float time)
    {
        int years = (int)(time / (365 * 24 * 3600));
        int months = (int)((time % (365 * 24 * 3600)) / (30 * 24 * 3600));
        int days = (int)((time % (30 * 24 * 3600)) / (24 * 3600));
        int hours = (int)((time % (24 * 3600)) / 3600);
        int minutes = (int)((time % 3600) / 60);
        int seconds = (int)(time % 60);

        string formattedTime = "";
        if (years > 0)
            formattedTime += $"{years}y ";
        if (months > 0 || years > 0)
            formattedTime += $"{months}mo ";
        if (days > 0 || months > 0 || years > 0)
            formattedTime += $"{days}d ";
        if (hours > 0 || days > 0 || months > 0 || years > 0)
            formattedTime += $"{hours}hr ";
        if (minutes > 0 || hours > 0 || days > 0 || months > 0 || years > 0)
            formattedTime += $"{minutes}min ";
        formattedTime += $"{seconds}s";

        return formattedTime.Trim();
    }
}
