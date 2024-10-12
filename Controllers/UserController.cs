using ItransitionTemplates.Models;
using ItransitionTemplates.Services.User;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ItransitionTemplates.Controllers;

public class UserController : Controller {

    private Services.User.IUserService _UserService;
    public UserController(Services.User.IUserService _userService) {
        _UserService = _userService;
    }
    
    [HttpGet("/user/log-in")]
    public IActionResult LogInView() {
        return View();
    }

    [HttpPost("/user/log-in")]
    public async Task<IActionResult> LogIn([FromBody] Models.User user) {
        Models.User userFound = await _UserService.Login(user);
        if(userFound != null) {
            Console.WriteLine(userFound.Email);
            return RedirectToAction("Index", "Home");
        } else {
            ViewData["errorMsg"] = "The user was not found, ensure email and password are correct";
            return View("LogInView");
        }
    }

    [HttpGet("/user/sign-up")]
    public IActionResult SignUpView() {
        return View();
    }

    [HttpPost("/user/sign-up")]
    public async Task<IActionResult> SignUp([FromBody] Models.User user) {
        //Store the user in the database
        try {
            string stored = await _UserService.AddUser(user);
            //This is temporary, this action must redirect to the templates view
            return RedirectToAction("LoginView");
        } catch (DBException err) {
            ViewData["ErrorMsg"] = err.Msg;
            Console.WriteLine(err.Msg);
            return View("SignUpView");
        } catch (Exception err) {
            Console.WriteLine(err);
            return View("SignUpView");
        }
    }
}