using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopProject.Data.Help;

namespace ShopProject.Data;

public class Comment : ApplicationClassUserActivities
{
    public virtual string Value { get; set; }

}

