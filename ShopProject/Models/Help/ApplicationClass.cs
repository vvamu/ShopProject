using System.ComponentModel.DataAnnotations;
using ShopProject.Services;

namespace ShopProject.Data.Help
{
    public abstract class ApplicationClass
    {
        [Key]
        public virtual long Id { get; set; }
        public virtual string Name { get; set; } 
        public virtual string? ImagePath { get; set; } //ImageUri

        public virtual Uri? ImageUri { get; set; }

        /* --- */
        public virtual string? Description { get; set; }

        public virtual bool? IsPrivate { get; set; }

    }
}
