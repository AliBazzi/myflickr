namespace System
{
    internal static class StringExtensions
    {
        public static bool ToBoolean(this string str)
        {
            if (str == "1")
            {
                return true;
            }
            else if (str == "0")
            {
                return false;
            }
            throw new ArgumentException("str");
        }
    }
}
