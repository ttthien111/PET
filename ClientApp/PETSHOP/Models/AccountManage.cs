﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models
{
    public class AccountManage
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActivated { get; set; }
        public int AccountRoleId { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }


    }
}
