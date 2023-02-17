using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShopProject.Data;
using ShopProject.Services;

namespace ShopProject.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        // GET: CollectionController
        const string admin = "Administrator";
        const string user = "User";


        private ApplicationDbContext _db;
        private  SignInManager<User> _manager;

        public ItemController(ApplicationDbContext db, SignInManager<User> userManager)
        {
            _db = db;
            _manager = userManager;
        }



        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> CollectionItems(long collectionId)
        {
            var coll = await _db.Collections.Where(x => x.Id == collectionId).FirstOrDefaultAsync();
            
            var Collection_Items = await _db.Items.Where(x => x.ApplicationCollection == coll).ToListAsync();
            if (Collection_Items.Count == 0) return RedirectToAction("Create",new {idd = collectionId});

            ViewBag.CollectionName = coll.Name;
            ViewBag.CollectionId = coll.Id;

            return View(Collection_Items);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string CollectionId,long idd = -1)
        {
            long id = idd;

            if(idd == -1)
             id = Convert.ToInt64(CollectionId);

            ViewBag.CollectionId = id;

            var properties = await _db.Properties.Where(x => x.CollectionId == id).ToListAsync();
            ViewBag.Properties = properties;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item , string[] propertyValues, string CollectionId)
        {
            //if (!ModelState.IsValid) RedirectToAction("Create");
            var id = Convert.ToInt64(CollectionId);
            //item.ApplicationCollection = await _db.Collections.FindAsync(id);
            item.CollectionId = id;
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();



            foreach (var value in propertyValues)
            {
                if (string.IsNullOrEmpty(value))
                {
                    ModelState.AddModelError("", "You need to fill all item properties");
                    return View(item);
                }
            }


            await CreateProperties(item, propertyValues);
          


            return RedirectToAction("CollectionItems", new {id = id});

        }

        public async Task<List<PropertyValue>> CreateProperties(Item item,string[] propertyValues)
        {


            var itemProperties = new List<PropertyValue>();
            var collectionProperties = await _db.Properties.Where(x => x.CollectionId == item.CollectionId).ToArrayAsync();

            for (int i = 0; i < propertyValues.Length; i++)
            {
                var property = new PropertyValue()
                {
                    ItemId = item.Id,
                    PropertyId = collectionProperties[i].Id,
                    Value = propertyValues[i]
                };
                
                
                itemProperties.Add(property);

            }
            _db.PropertyValues.AddRange(itemProperties);
            await _db.SaveChangesAsync();

            return itemProperties;
        }

        public async Task<List<PropertyValue>> UpdateProperties(Item item, string[] propertyValues)
        {


            var itemProperties = new List<PropertyValue>();
            var collectionProperties = await _db.Properties.Where(x => x.CollectionId == item.CollectionId).ToArrayAsync();

            for (int i = 0; i < propertyValues.Length; i++)
            {
                var property = new PropertyValue()
                {
                    ItemId = item.Id,
                    PropertyId = collectionProperties[i].Id,
                    Value = propertyValues[i]
                };
                _db.PropertyValues.Where(x => x.ItemId == property.ItemId && x.PropertyId == property.PropertyId).First();
                _db.PropertyValues.Update(property);

                itemProperties.Add(property);

            }
            
            _db.PropertyValues.AddRange(itemProperties);
            await _db.SaveChangesAsync();

            return itemProperties;
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            //var id = item.Id;
            var item = await _db.Items.FindAsync(id);
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("CollectionItems", new {id = item.CollectionId});
        }


        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var item = await _db.Items.FindAsync(id);
            var collection = await _db.Collections.FindAsync(item.CollectionId);

            var properties = await _db.Properties.Where(x => x.CollectionId == collection.Id).ToArrayAsync();
            var values = await _db.PropertyValues.Where(x => x.ItemId == item.Id).ToArrayAsync();
            var v = _db.PropertyValues.ToArrayAsync();


            var PropertiesWithValue = new Dictionary<Property, PropertyValue>();

            for(int i = 0; i<properties.Length;i++)
            {
                PropertiesWithValue.Add(properties[i], values[i]);
            }


            @ViewBag.PropertiesWithValue = PropertiesWithValue;
            return View(item);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Item item , string[] propertyValues)
        {
            
            var Editable_item = await _db.Items.FindAsync(item.Id);
            Editable_item = item;
            await CreateProperties(Editable_item, propertyValues);


             _db.Items.Update(Editable_item);
            await _db.SaveChangesAsync();


            var client = new System.Net.WebClient();
 
            var imageBinary = client.DownloadData("https://www.google.com/url?sa=i&url=https%3A%2F%2Ftech.segodnya.ua%2Ftech%2Fne-dorogie-a-mertvye-perevodchik-google-ispravlyaet-znamenituyu-frazu-o-rossiyanah-1613999.html&psig=AOvVaw26lu0i2vs3zku9kjRL6tjC&ust=1675192844985000&source=images&cd=vfe&ved=0CBAQjRxqFwoTCOiI6-CB8PwCFQAAAAAdAAAAABAE");
            File(imageBinary,"img/png");


            return View("CollectionItems");
        }


        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> ChangeLike(long _itemId)
        {

            var itemId = Convert.ToInt64(_itemId);
            var item = _db.Items.Find(itemId);
            var collectionID = _db.Collections.Where(x => x.Id == item.CollectionId).Select(x=>x.Id);

            var _userId = User.Identity.Name;
            

            var like =  _db.Likes.Where(x=>x.ItemId == itemId).Where(x => x.UserId == _userId).FirstOrDefault();
            if (like == null)
            {
                like = new Like()
                {
                    ItemId = itemId,
                    UserId = _userId,
                };

                await _db.Likes.AddAsync(like);
            }
            else
            {
                _db.Likes.Remove(like);
            }
            
            await _db.SaveChangesAsync();

            return RedirectToAction("CollectionItems", new { collectionID });
        }


        [HttpPost]
        [HttpGet]
        public FileContentResult GetImage(long itemId)
        {
            var item = _db.Items.Find(itemId);


           string contentType;
           new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider().TryGetContentType(item.ImageUri.OriginalString, out contentType);


            //return File(MegaService.GetModelImage(item).Result, );
            //return File(MegaService.GetModelImage(item).Result, System.Net.Mime.MediaTypeNames.Image.Jpeg);
            var a = MegaService.GetModelImage(item).Result;

            return File(MegaService.GetModelImage(item).Result, "image/png");
        }

    }
}
