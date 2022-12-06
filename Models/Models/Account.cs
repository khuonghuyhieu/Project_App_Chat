﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Models.Models
{
    public partial class Account
    {
        public Account()
        {
            MessageUserReceive = new HashSet<MessageUser>();
            MessageUserSender = new HashSet<MessageUser>();
            Group = new HashSet<Group>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<MessageUser> MessageUserReceive { get; set; }
        public virtual ICollection<MessageUser> MessageUserSender { get; set; }

        public virtual ICollection<Group> Group { get; set; }
    }
}