using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers;

public class UserController : Controller {
    
    [HttpGet("/user/log-in")]
    public IActionResult LogIn() {
        return View();
    }

    [HttpGet("/user/sign-up")]
    public IActionResult SignUp() {
        return View();
    }
}