﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMAPortal.Web.DTOs
{
    public class EmailSMTPConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
    }
}
