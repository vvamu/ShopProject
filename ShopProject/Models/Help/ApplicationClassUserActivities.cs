using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopProject.Data;

public abstract class ApplicationClassUserActivities
{
    [Key]
    public virtual long Id { get; set; }

    #region Keys

    
    public virtual string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User ApplicationUser { get; set; }

    public virtual long ItemId { get; set; }
    [ForeignKey("ItemId")]
    public virtual Item ApplicationItem { get; set; }

    #endregion
}
