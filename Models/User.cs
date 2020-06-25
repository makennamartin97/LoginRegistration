using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    
public class User
{
    [Key]
    public int UserId {get;set;}
    [Required(ErrorMessage="Required")]
    [MinLength(2, ErrorMessage="First Name must be at least 2 characters")]
    public string FirstName {get;set;}
    [Required(ErrorMessage="Required")]
    [MinLength(2, ErrorMessage="Last Name must be at least 2 characters")]
    public string LastName {get;set;}
    [EmailAddress]
    [Required(ErrorMessage="Required")]
    public string Email {get;set;}
    [DataType(DataType.Password)]
    [Required(ErrorMessage="Required")]
    [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
    public string Password {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
    public decimal Balance {get;set;} = 0;

    public List<Transaction> Transactions {get;set;}
    
}    
