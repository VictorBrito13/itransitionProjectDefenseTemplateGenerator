using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Utils
{
    public class Session {
        //Sotre in session
        public static void Store<T>(HttpContext context, string key, T o) {
            string s = JsonSerializer.Serialize(o);
            context.Session.SetString(key, s);
            Console.WriteLine("Object stored: ");
            Console.WriteLine(s);
        }

        //Get an object in the session
        public static T GetObject<T>(HttpContext context, string key) {
            Console.WriteLine("exec get object session");
            string? s = context.Session.GetString(key);

            if(s == null) {
                context.Response.Redirect("/user/log-in", true);
            }

            T? o = JsonSerializer.Deserialize<T>(s??"");
            return o;
        }
    }
}