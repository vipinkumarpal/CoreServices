using System.ComponentModel.DataAnnotations;

namespace CoreServices.Infrastructure.Repositories.Entities
{
    public partial class Category
    {
        [Key]
        public int    ID { get; set; }
        public string NAME {get; set;}
        public string SLUG { get; set; }

    }
}
