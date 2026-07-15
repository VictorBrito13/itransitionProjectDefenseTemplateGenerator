namespace ItransitionTemplates.Utils
{
    public class Auth {
        /// <summary>
        /// Validates session for view-based actions. Redirects to login on failure.
        /// </summary>
        public static Models.User ValidateUserSession(HttpContext httpContext) {
            Models.User user = Session.GetObject<Models.User>(httpContext, "userSession");

            if(user == null) {
                httpContext.Response.Redirect("/user/log-in", true);
                return null;
            }

            if(user.Email == null || user.UserId == 0 || user.Username == null) {
                httpContext.Response.Redirect("/user/log-in", true);
                return null;
            }

            return user;
        }

        /// <summary>
        /// Validates session for JSON/API actions. Returns null if invalid WITHOUT redirecting.
        /// </summary>
        public static Models.User? ValidateSession(HttpContext httpContext) {
            Models.User? user = Session.GetObject<Models.User>(httpContext, "userSession");

            if(user == null || user.Email == null || user.UserId == 0 || user.Username == null) {
                return null;
            }

            return user;
        }
    }
}