using System.ComponentModel.DataAnnotations;

namespace SoftExpress_Challange.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Emri { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public DateTime Ditelindja { get; set;}
        public string Roli { get; set; }

        //Relationship
        public ICollection<Statistika> Statistika { get; set; }


    }

}
