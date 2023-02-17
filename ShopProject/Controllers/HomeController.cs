using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Data;
using System.Security.Principal;
using ShopProject.Data;

namespace ShopProject.Controllers;

[Authorize]
public class HomeController : Controller
{



    public readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public readonly UserManager<User> _userManager;
    public readonly SignInManager<User> _signInManager;


    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    [AllowAnonymous]
    public IActionResult Index()
    {

        return View();
    }

    [AllowAnonymous]
    public IActionResult SignIn()
    {
        ViewBag.externalProviders = _signInManager.GetExternalAuthenticationSchemesAsync().Result.ToList();
        return View();
    }

    [AllowAnonymous]
    public IActionResult ExternalSignIn(string provider,string returnUrl)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallBack),"Home", new {provider,returnUrl});
        AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //properties.AllowRefresh = true;
        //properties.ExpiresUtc = DateTimeOffset.UtcNow;
        
        return Challenge(properties,provider);
        //return Challenge(provider);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallBack(string provider,string returnUrl)
    {

        var info = await _signInManager.GetExternalLoginInfoAsync();

        //return Ok();
        if (info == null) return RedirectToAction("SignIn");

        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
        var res = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

        var b = _userManager.Users;
        var a = info.Principal.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault();

        var user = _userManager.FindByEmailAsync(a.Value).Result;
        if (user != null)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index");
        }


        if (result.Succeeded) { }//?


        return RedirectToAction("ConfirmExternalSignIn", "Home" , new User (info.Principal.FindFirstValue(ClaimTypes.Name)));
    }

    [AllowAnonymous]
    public async Task<IActionResult> ConfirmExternalSignIn(User user)
    {
        if (user == null) return RedirectToAction("SignIn");

        return View(user);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmedExternalSignIn(User user)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if(info == null) return RedirectToAction("SignIn");


        if (await _userManager.FindByNameAsync(user.UserName) != null)
        {
            ModelState.AddModelError("", "User there is in db");
            return View("ConfirmExternalSignIn",user);
        }

        //Need to do

        //var u =  _userManager.Users.ToList();

        //user.info = new List<IIdentity>();
        //user.info.Add(info.Principal.Identity);

        var r = await _userManager.CreateAsync(user);
        
        if (!r.Succeeded)
        {
            ModelState.AddModelError("", "Incorrect UserName");
            return View("ConfirmExternalSignIn", user );
        }

        await _signInManager.SignInAsync(user, false);

        //if (!res.Succeeded)
        //    return RedirectToAction("Registration", "Home");

        var resRole = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, _configuration["Role:user"]));

        return RedirectToAction("Index");

    }
    //Error with IsValid

    
    [AllowAnonymous]
    public IActionResult Registration()
    {
        return View();
    }


    public IActionResult Collection()
    {
        return RedirectToAction("Index","Collection");
    }

    public HomeController(ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;

    }



    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(User _user)
    {

        if (!ModelState.IsValid) return View("SignIn", _user);

        User? user = await _userManager.FindByNameAsync(_user.UserName);

        if (user == null) 
        { 
            ModelState.AddModelError("", "User was not found"); 
            return View("SignIn", _user); 
        }

        var res = await _signInManager.PasswordSignInAsync(_user.UserName, _user.Password, false, false);
        //if (!res.Succeeded)
        //    return RedirectToAction("SignIn","Home");

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }




    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration(User us)
    {

        if (!ModelState.IsValid) return RedirectToAction("Registration", "Home", new { us });

        if (await _userManager.FindByNameAsync(us.UserName) != null)
        {
            ModelState.AddModelError("", "User there is in db");
            return View(us);
        }

        //if(us.Password != us.ConfirmPassword)
        //    {
        //        ModelState.AddModelError("", "Incorrect password");
        //        return View(us);
        //    }

        var r = await _userManager.CreateAsync(us, us.Password.Trim());
        if (!r.Succeeded)
        {
            ModelState.AddModelError("All", "Hui sosy");
            return RedirectToAction("Registration", "Home");
        }

        var res = await _signInManager.PasswordSignInAsync(us.UserName, us.Password, false, false);

        //if (!res.Succeeded)
        //    return RedirectToAction("Registration", "Home");

        var resRole = await _userManager.AddClaimAsync(us, new Claim(ClaimTypes.Role, _configuration["Role:user"]));

        return RedirectToAction("Index");



    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SearchItem(long? collId)
    {
        var collection = await _db.Collections.Where(x => x.Id == collId).FirstOrDefaultAsync();
        var id = collId;

        return RedirectToAction("CollectionItems","Item", new { id = collId } );
    }


}

