namespace DangDucThuanFinalYear.Extensions
{
    public static class StringExtensions
    {
        public static string Ellipsis(this string str, int maxLength) =>
            (string.IsNullOrWhiteSpace(str) || str.Length <= maxLength)
            ? str
            : $"{str[0..97]}";
    }
}
