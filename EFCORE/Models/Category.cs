namespace EFCORE.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
