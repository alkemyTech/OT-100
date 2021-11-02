﻿using System;

namespace OngProject.Domain.Entities
{
    public class User:BaseEntity
    {
        public Guid IdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
