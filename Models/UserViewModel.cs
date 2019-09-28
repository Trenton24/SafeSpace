using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SafeSpace.Models
{
    public class UserViewModel
    {
        public Users Person {get; set;}
        public List<Users> Pending { get; set; }
        public UserHaveFriends Accepted {get;set;}
    }
}
