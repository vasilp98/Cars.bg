﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public static class Helper
    {
        public static string CnnString(string name)
        {
            return "Server=localhost;Database=Cars.bg;Trusted_Connection=True;";
        }
    }
}
