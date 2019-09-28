using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace SafeSpace.Models
{
    public class UserHaveFriends
    {
        [Key]
        public int RelationshipId {get;set;}
        public int RequestedId {get;set;}
        public int Accepted {get;set;} = 0;
        public Users Requested { get; set; }
        public int SentById {get;set;}
        public Users SentBy {get;set;}

    }

    public class Friends 
    {
        [Key]
        public int FriendId {get;set;}
        public int user1Id {get;set;}
        public int user2Id {get;set;}
    }
}