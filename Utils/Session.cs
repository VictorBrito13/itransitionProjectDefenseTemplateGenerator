using System.Text.Json;

namespace ItransitionTemplates.Utils
{
    public class Session {
        //Store in session
        public static void Store<T>(HttpContext context, string key, T o) {
            string s = JsonSerializer.Serialize(o);
            context.Session.SetString(key, s);
        }

        //Get an object in the session
        public static T GetObject<T>(HttpContext context, string key) {
            string? s = context.Session.GetString(key);

            T? o = JsonSerializer.Deserialize<T>(s??"{}");
            return o;
        }

        public static void Clear(HttpContext context) {
            context.Session.Clear();
        }
    }
}