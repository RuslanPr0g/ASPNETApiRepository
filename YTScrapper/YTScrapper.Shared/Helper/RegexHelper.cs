using System.Text.RegularExpressions;

namespace YTSearch.Shared.Helper
{
    public static class RegexHelper
    {
        public static string GetNMatchFromRegexPattern(string regex, string text, int match)
        {
            Regex r = new(regex, RegexOptions.IgnoreCase);
            Match m = r.Match(text);
            if (!m.Success || match > m.Groups.Count - 1)
            {
                return null;
            }
            else
            {
                return m.Groups[match].Value;
            }
        }
    }
}
