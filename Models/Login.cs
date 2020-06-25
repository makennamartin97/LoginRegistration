using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace loginregistration.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Required")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage="Required")]

        public string LoginPassword {get;set;}
    }
}