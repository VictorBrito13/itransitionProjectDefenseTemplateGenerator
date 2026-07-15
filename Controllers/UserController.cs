using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> LogIn([FromForm] Models.User user) {
        Models.User userFound = await _UserService.Login(user);

        if(userFound != null) {
            //Store the user in the session
            Session.Store(HttpContext, "userSession", new { UserId=userFound.UserId, Username=userFound.Username, Email=userFound.Email});
            return RedirectToAction("Index", "Home");
        } else {
            TempData["errorMsg"] = "The user was not found, ensure email and password are correct";
            return View("LogInView");
        }
    }

    [HttpGet("/user/sign-up")]
    public IActionResult SignUpView() {
        return View();
    }

    [HttpPost("/user/sign-up")]
    public async Task<IActionResult> SignUp([FromForm] Models.User user) {
        //Store the user in the database
        try {
            string stored = await _UserService.AddUser(user);
            //This is temporary, this action must redirect to the templates view
            return RedirectToAction("LogInView");
        } catch (DBException err) {
            TempData["ErrorMsg"] = err.Msg;
            return View("SignUpView");
        } catch (Exception err) {
            TempData["errorMsg"] = "An unknown error has occurred";
            return View("SignUpView");
        }
    }

    [HttpGet("/user/get-by-username")]
    public async Task<IActionResult> GetUserByUsername([FromQuery] string username) {
        try {
            Models.User user = await _UserService.GetUserByUsername(username);

            if(user != null) {
                return JsonResponse.Ok(user);
            }

            return JsonResponse.NotFound("User not found");
        } catch (Exception err) {
            return JsonResponse.NotFound("User not found");
        }
    }

    [HttpGet("/user/log-out")]
    public IActionResult LogOut() {
        Session.Clear(HttpContext);
        return RedirectToAction("LogInView");
    }
}
