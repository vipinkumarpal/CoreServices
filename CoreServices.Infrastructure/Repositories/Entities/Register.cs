using System.ComponentModel.DataAnnotations;

namespace CoreServices.Infrastructure.Repositories.Entities
{
    public partial class Register
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
