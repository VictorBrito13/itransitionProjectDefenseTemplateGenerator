namespace ItransitionTemplates.Utils
{
    public class Auth() {
        public static Models.User ValidateUserSession(HttpContext httpContext) {
            Models.User user = Session.GetObject<Models.User>(httpContext, "userSession");

            if(user == null) {
                httpContext.Response.Redirect("/user/log-in", true);
                Console.WriteLine("Session invalida");
                return null;
            }

            if(user.Email == null || user.UserId == 0 || user.Username == null) {
                httpContext.Response.Redirect("/user/log-in", true);
                Console.WriteLine("Session invalida");
                return null;
            }

            return user;
        }
    }
}