using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Utils
{
    public class Session {
        //Sotre in session
        public static void Store<T>(HttpContext context, string key, T o) {
            string s = JsonSerializer.Serialize(o);
            context.Session.SetString(key, s);
        }

        //Get an object in the session
        public static T GetObject<T>(HttpContext context, string key, bool redirect) {
            string? s = context.Session.GetString(key);

            // if((s == null || s.Length == 0) && redirect) {
            //     context.Response.Redirect("/user/log-in", true);
            // }

            T? o = JsonSerializer.Deserialize<T>(s??"{}");
            Console.WriteLine(o);
            return o;
        }

        public static void Clear(HttpContext context) {
            context.Session.Clear();
        }
    }
}