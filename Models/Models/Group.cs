﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Models.Models
{
    public partial class Group
    {
        public Group()
        {
            MessageGroup = new HashSet<MessageGroup>();
            Account = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MessageGroup> MessageGroup { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}