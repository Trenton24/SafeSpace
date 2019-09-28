using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SafeSpace.Models
{
    public class Users
    {
        [Key]
       public int UserId {get;set;}
       
       [Required]
       [MinLength(2)]
       public string FirstName {get;set;}

       [Required]
       [MinLength(2)]
       public string LastName {get;set;}

       [Required]
       [EmailAddress]
       public string Email {get;set;}

       [DataType(DataType.Password)]
       [Required]
       [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
       public string Password {get;set;}
       public DateTime CreatedAt {get;set;} = DateTime.Now;
       public DateTime UpdatedAt {get;set;} = DateTime.Now;
       
       [InverseProperty("Requested")]
       public List<UserHaveFriends> Pending {get;set;}

       [InverseProperty("SentBy")]
       public List<UserHaveFriends> Requested { get; set; }

       [NotMapped]
       [Compare("Password")]
       [DataType(DataType.Password)]
       public string Confirm {get;set;}
    }
}