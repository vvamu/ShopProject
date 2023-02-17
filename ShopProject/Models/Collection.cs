using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopProject.Data
{
    public class Collection : Help.ApplicationClass
    {

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User ApplicationUser { get; set; } 

        public List<Property>? Properties { get; set; }

        public List<Item>? Items { get; set; }


    }
}
