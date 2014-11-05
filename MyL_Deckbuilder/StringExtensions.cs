namespace StringExtensions
{
    public static class StringExtensionsClass
    {
        public static string Minimize(this string s)
        {
            string returnString = s.ToLower();

            returnString = returnString.Replace('á', 'a');
            returnString = returnString.Replace('é', 'e');
            returnString = returnString.Replace('í', 'i');
            returnString = returnString.Replace('ó', 'o');
            returnString = returnString.Replace('ú', 'u');

            return returnString;
        }

        public static bool ContainsAll(this string s, string[] strings)
        {
            foreach (string str in strings)
            {
                if (!s.Contains(str))
                {
                    return false;
                }
            }
            return true;
        }
    }
}