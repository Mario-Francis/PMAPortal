﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMAPortal.Web.DTOs
{
    public class Recipient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
