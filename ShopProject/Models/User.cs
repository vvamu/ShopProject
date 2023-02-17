using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace ShopProject.Data;
public class User : IdentityUser
{
    

    [DisplayName("Date Registered")]
    public DateTime DateRegistered { get; set; } = DateTime.Now;

    [DisplayName("Is Blocked")]
    public bool IsBlocked { get; set; } = false;

    public string? Password { get; set; }


    //Need to do
    //public List<System.Security.Principal.IIdentity>? info;


    public List<Collection>? Collections { get ; set ; }
    public List<Like>? Likes {get;set; }
    public List<Comment>? Comments { get; set; }



    public User() { }
    public User(string name) : base(name) { UserName = name; }



}




