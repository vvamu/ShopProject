using ShopProject.Data;

namespace ShopProject.ViewModels
{
    public class CollectionViewModel
    {
        
        public Collection? collection { get; set; }
        public IFormFile? imageUri { get; set; }

        public string[]? propertyName { get; set; }
        public int[]? propertyType { get; set; }

        public CollectionViewModel() { }

        public CollectionViewModel(Collection collection) 
        { 
            this.collection = collection; 
            
        }

    }
}
