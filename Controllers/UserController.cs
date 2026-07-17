using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTemplates.Controllers;

public class UserController : Controller {

    private Services.User.IUserService _UserService;
    private readonly ILogger<UserController> _logger;
    public UserController(Services.User.IUserService _userService, ILogger<UserController> logger) {
        _UserService = _userService;
        _logger = logger;
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
            var sessionPayload = new { UserId=userFound.UserId, Username=userFound.Username, Email=userFound.Email};
            Session.Store(HttpContext, "userSession", sessionPayload);

            // --- DEBUG: Log session state after Store ---
            string? rawJson = HttpContext.Session.GetString("userSession");
            bool keyExists = rawJson != null;
            Models.User? deserialized = null;
            try {
                if (rawJson != null) {
                    deserialized = System.Text.Json.JsonSerializer.Deserialize<Models.User>(rawJson);
                }
            } catch (Exception deserErr) {
                _logger.LogWarning("DEBUG [LogIn] Deserialization failed: {Error}", deserErr.Message);
            }
            _logger.LogInformation(
                "DEBUG [LogIn] Session stored. KeyExists={KeyExists}, RawJson={RawJson}, " +
                "DeserializedUserId={UserId}, DeserializedUsername={Username}, DeserializedEmail={Email}",
                keyExists,
                rawJson ?? "(null)",
                deserialized?.UserId.ToString() ?? "(null)",
                deserialized?.Username ?? "(null)",
                deserialized?.Email ?? "(null)"
            );
            // --- END DEBUG ---

            return RedirectToAction("Index", "Home");
        } else {
            TempData["errorMsg"] = "Invalid email or password, please check your credentials and try again";
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
            Models.User createdUser = await _UserService.AddUser(user);
            var sessionPayload = new { UserId=createdUser.UserId, Username=createdUser.Username, Email=createdUser.Email};
            Session.Store(HttpContext, "userSession", sessionPayload);

            // --- DEBUG: Log session state after Store ---
            string? rawJson = HttpContext.Session.GetString("userSession");
            bool keyExists = rawJson != null;
            Models.User? deserialized = null;
            try {
                if (rawJson != null) {
                    deserialized = System.Text.Json.JsonSerializer.Deserialize<Models.User>(rawJson);
                }
            } catch (Exception deserErr) {
                _logger.LogWarning("DEBUG [SignUp] Deserialization failed: {Error}", deserErr.Message);
            }
            _logger.LogInformation(
                "DEBUG [SignUp] Session stored. KeyExists={KeyExists}, RawJson={RawJson}, " +
                "DeserializedUserId={UserId}, DeserializedUsername={Username}, DeserializedEmail={Email}",
                keyExists,
                rawJson ?? "(null)",
                deserialized?.UserId.ToString() ?? "(null)",
                deserialized?.Username ?? "(null)",
                deserialized?.Email ?? "(null)"
            );
            // --- END DEBUG ---

            return RedirectToAction("Index", "Home");
        } catch (DBException err) {
            TempData["errorMsg"] = err.Msg;
            return View("SignUpView");
        } catch (Exception err) {
            _logger.LogError(err, "Unknown error during user sign-up");
            TempData["errorMsg"] = "Failed to create account, please try again";
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
            _logger.LogError(err, "Error getting user by username: {Username}", username);
            return JsonResponse.NotFound("User not found");
        }
    }

    [HttpGet("/user/log-out")]
    public IActionResult LogOut() {
        Session.Clear(HttpContext);
        return RedirectToAction("LogInView");
    }
}
