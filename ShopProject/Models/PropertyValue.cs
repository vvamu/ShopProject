using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopProject.Data.Help;

namespace ShopProject.Data;

public class PropertyValue
{
    [Key]
    public long Id { get; set; }

    public string? DefaultValue { get; set; }

    public string Value { get; set; }

    public long PropertyId { get; set; }

    [ForeignKey("PropertyId")]
    public Property Property { get; set; } 

    public long ItemId { get; set; }
    
    [ForeignKey("ItemId")]
    public Item ApplicationItem { get; set; }



}

