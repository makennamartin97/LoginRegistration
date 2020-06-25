using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

    
public class Transaction
{
    [Key]
    public int TransactionId {get;set;}

    [Required]
    [Display(Name = "Amount:")]
    [DataType(DataType.Currency)]

    public decimal Amount {get;set;}

    public int UserId {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;


    public User accountholder {get;set;}
}