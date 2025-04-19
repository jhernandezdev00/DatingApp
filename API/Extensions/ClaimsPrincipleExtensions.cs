using System.Globalization;
using System.Security.Claims;

namespace API.Extensions{
    public static class ClaimsPrincipleExtensions{
        
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new ArgumentException("Cannot get the userId from token"), CultureInfo.InvariantCulture);

            return userId;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {

            var username = user.FindFirstValue(ClaimTypes.Name)
                ?? throw new ArgumentException("No se puede obtener el nombre de usuario del token");

            return username;
        }
    }
}