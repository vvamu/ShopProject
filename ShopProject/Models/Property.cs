using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopProject.Data.Help;

namespace ShopProject.Data;

public enum EnumPropertyType : byte
{
    Int = 1 ,
    Double = 2,
    String = 3,
    Bool = 4,
    Date = 5 
}
public class Property 
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }



    public EnumPropertyType PropertyType { get; set; }
    public long CollectionId { get; set; }
    [ForeignKey("CollectionId")]
    public Collection ApplicationCollection { get; set; }

    public List<PropertyValue>? PropertyValues { get; set; } 



}

