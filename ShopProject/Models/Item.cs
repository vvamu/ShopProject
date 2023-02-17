using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopProject.Data
{
    public class Item : Help.ApplicationClass
    {
        //public bool IsPrivate { get; set; }
        public long CollectionId { get; set; }
        [ForeignKey("CollectionId")]
        public Collection ApplicationCollection { get; set; }


        public List<PropertyValue>? PropertyValues { get; set; }
        public List<Like>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }

    }
}
