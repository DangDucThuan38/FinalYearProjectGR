using System.Security.Claims;

namespace DangDucThuanFinalYear.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
