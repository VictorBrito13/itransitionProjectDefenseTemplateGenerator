namespace ItransitionTemplates.Utils
{
    public class Auth {
        public static Models.User? ValidateUserSession(HttpContext httpContext) {
            Models.User? user = Session.GetObject<Models.User>(httpContext, "userSession");

            if(user == null || user.Email == null || user.UserId == 0 || user.Username == null) {
                return null;
            }

            return user;
        }

        public static Models.User? ValidateSession(HttpContext httpContext) {
            Models.User? user = Session.GetObject<Models.User>(httpContext, "userSession");

            if(user == null || user.Email == null || user.UserId == 0 || user.Username == null) {
                return null;
            }

            return user;
        }
    }
}