using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetClub
{
    public class CustomAuthorization
    {
        public static bool ValidateClaimsUser(HttpContext context, string claimName, string claimValue)
        {
            var teste = context.User.Claims;
            var values = claimValue.Split(',');
            bool validate = false;

            foreach (var value in values)
            {
                validate = context.User.Identity.IsAuthenticated &&
                    context.User.Claims.Any(c => c.Type == claimName && c.Value.Split(',').Contains(value));

                if (validate)
                {
                    return validate;
                }
            }

            return validate;
        }
    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!CustomAuthorization.ValidateClaimsUser(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
