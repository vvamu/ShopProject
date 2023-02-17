using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Data;

namespace ShopProject.Controllers;


[Authorize]
public class AdminController : Controller
{

    const string admin = "Administrator";
    const string user = "User";


    private ApplicationDbContext _db;
    private UserManager<User> _userManager;


    public AdminController(ApplicationDbContext db,UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }


    [Authorize(Policy = admin)]
    public IActionResult Administrator(string returnUrl)
    {

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }


}
