﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public DateTime CreateDate { get; set; }

        public AddressDTO Address { get; set; }

        public SettingsDTO Settings { get; set; }
    }
}
