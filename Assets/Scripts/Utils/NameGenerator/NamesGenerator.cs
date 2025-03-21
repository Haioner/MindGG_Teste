public static class NamesGenerator
{
    private static readonly string Vowels = "aeiou";
    private static readonly string Consonants = "bcdfghjklmnpqrstvwxyz";

    /// <summary>
    /// Generates a random name that may consist of one or two words.
    /// </summary>
    /// <returns>A randomly generated name.</returns>
    public static string GenerateRandomName()
    {
        string firstWord = GenerateRandomWord();
        bool hasSecondWord = UnityEngine.Random.value < 0.5f;

        if (hasSecondWord)
        {
            string secondWord = GenerateRandomWord();
            return $"{firstWord} {secondWord}";
        }

        return firstWord;
    }

    /// <summary>
    /// Generates a random word based on vowels and consonants.
    /// </summary>
    /// <param name="minLength">Minimum length of the word.</param>
    /// <param name="maxLength">Maximum length of the word.</param>
    /// <returns>A randomly generated word with the first letter capitalized.</returns>
    public static string GenerateRandomWord(int minLength = 3, int maxLength = 8)
    {
        string word = "";
        int length = UnityEngine.Random.Range(minLength, maxLength + 1);

        for (int i = 0; i < length; i++)
        {
            if (i % 2 == 0)
                word += Consonants[UnityEngine.Random.Range(0, Consonants.Length)];
            else
                word += Vowels[UnityEngine.Random.Range(0, Vowels.Length)];
        }

        return Capitalize(word);
    }

    /// <summary>
    /// Capitalizes the first letter of a string.
    /// </summary>
    /// <param name="str">The string to capitalize.</param>
    /// <returns>The string with the first letter in uppercase.</returns>
    private static string Capitalize(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
}