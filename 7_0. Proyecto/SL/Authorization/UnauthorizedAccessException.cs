﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Authentication
{
    public class UnauthorizedAccessException:Exception
    {
        public UnauthorizedAccessException() : base("No autorizado.")
        {
        }

        public UnauthorizedAccessException(string message) : base(message)
        {
        }
    }
}
