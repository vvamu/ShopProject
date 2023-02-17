using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ShopProject.Data;
using ShopProject.Data.Help;
using ShopProject.Repository;
using ShopProject.Repository.Interfaces;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;
using ShopProject.Services;
using ShopProject.ViewModels;

namespace ShopProject.Controllers;

[Authorize]
public class CollectionController : Controller
{
    // GET: CollectionController
    const string admin = "Administrator";
    const string user = "User";


    private ApplicationDbContext _db;
    private UserManager<User> _userManager;
    private IUnitOfWork _unitOfWork;
    private IUserRepository _userRepository;


    public CollectionController(ApplicationDbContext db, UserManager<User> userManager , IUnitOfWork unitOfWork , IUserRepository userRepository)
    {
        _db = db;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }


    public static int Sum(int a, int b) => a + b;

    [HttpGet]
    public async Task<IActionResult> Collection_IndexAsync()
    {
        var userId = _userRepository.GetUserIdAsync(User).Result;
        var UserCollections = _unitOfWork.CollectionRepository.GetAllByUserAsync(userId).Result.ToList();

        return View(UserCollections);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ViewModels.CollectionViewModel collectionVm)
    {
        //if (!ModelState.IsValid) return View(collection);

        var userId = _userManager.GetUserId(User);

        var coll = new Collection() { 
            Name= collectionVm.collection.Name , 
            IsPrivate = collectionVm.collection.IsPrivate, 
            UserId = userId,
            Description= collectionVm.collection.Description,
            ImagePath = collectionVm.collection.ImagePath
        };
        

        await _unitOfWork.CollectionRepository.CreateAsync(coll);


        await CreateProperties(coll, collectionVm.propertyName, collectionVm.propertyType);

        if (collectionVm.imageUri != null) await MegaService.UploadImage(coll, collectionVm.imageUri);
        await _db.SaveChangesAsync();




        return RedirectToAction("Collection_Index");

    }


    /// - - - - - - - - -
    public async Task<bool> CreateProperties(Collection collection, string[]? propertyName,
        int[]? propertyType)
    {
        if (propertyName.Length != propertyType.Length && propertyName.Length == 0) return false;
        for (int i = 0; i < propertyName.Length; i++)
        {
            var properType = Convert.ToInt64(propertyType[i]);
            Property property = new Property()
            { Name = propertyName[i], CollectionId = collection.Id, PropertyType = (EnumPropertyType)properType };
            await _db.Properties.AddAsync(property);
        }

        return true;
    }

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Delete(long collectionId)
    {
        var collection = await _unitOfWork.CollectionRepository.GetAsync(collectionId);

        await _unitOfWork.CollectionRepository.DeleteAsync(collection);
        _unitOfWork.Save();
        return RedirectToAction("Collection_Index");

    }

    [HttpGet]
    public async Task<IActionResult> Edit(long collectionId)
    {
        

        var collection = _unitOfWork.CollectionRepository.GetAsync(collectionId);

        var properties = _unitOfWork.PropertyRepository.GetByCollectionAsync(collectionId).Result.ToList();
        var owner = _userRepository.GetUserByIdAsync(collection.Result.UserId).Result.UserName ?? " ";

        ViewBag.Properties = properties;
        ViewBag.Owner = owner;

        //if (collection.Result.ImageUri != null)
        //  await MegaService.GetModelImage(collection.Result);

        var vm = new ViewModels.CollectionViewModel(collection.Result);




        return View(vm);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(CollectionViewModel collectionVm)
    {
        //var userId = _userManager.GetUserId(User);

        //var coll = new Collection()
        //{
        //    Name = collectionVm.collection.Name,
        //    IsPrivate = collectionVm.collection.IsPrivate,
        //    UserId = userId,
        //    Description = collectionVm.collection.Description,
        //    ImagePath = collectionVm.collection.ImagePath
        //};

        await _unitOfWork.CollectionRepository.UpdateAsync(collectionVm.collection);
        _unitOfWork.Save();

        var collection = _unitOfWork.CollectionRepository.GetAsync(collectionVm.collection.Id);
        
        
            var properties = _unitOfWork.PropertyRepository.GetByCollectionAsync(collection.Result.Id).Result.ToList();
        foreach (var i in collectionVm.collection.)
        {
            //var prop = _unitOfWork.PropertyRepository.GetAsync()
            //if (properties.Contains()

        }
        //_unitOfWork.PropertyRepository.UpdateAsync()
        var owner = _userRepository.GetUserByIdAsync(collection.Result.UserId).Result.UserName ?? " ";

        ViewBag.Properties = properties;
        ViewBag.Owner = owner;

        //if (collection.Result.ImageUri != null)
        //  await MegaService.GetModelImage(collection.Result);

        var vm = new ViewModels.CollectionViewModel(collection.Result);




        return View(vm);

    }

    [HttpPost]
    [HttpGet]
    public FileContentResult GetImage(long collectionId)
    {
        var collection = _unitOfWork.CollectionRepository.GetAsync(collectionId);

        return File(MegaService.GetModelImage(collection.Result).Result, "image/png"); 
    }

}
