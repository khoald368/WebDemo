using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDemo.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public void Update(ViewModels.Customer customer)
        {
            Name = customer.Name;
            Address = customer.Address;
            PhoneNumber = customer.PhoneNumber;
            Email = customer.Email;
        }
    }
}
