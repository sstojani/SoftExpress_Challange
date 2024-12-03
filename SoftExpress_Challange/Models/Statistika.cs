using System.ComponentModel.DataAnnotations;

namespace SoftExpress_Challange.Models
{
    public class Statistika
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Lloji_I_Pajisjes { get; set; }
        public string Rajoni { get; set; }
        public string Aplikacioni { get; set; }
        public DateTime DataOra { get; set; }

        //Relationship
        public User User { get; set; }

    }
}
