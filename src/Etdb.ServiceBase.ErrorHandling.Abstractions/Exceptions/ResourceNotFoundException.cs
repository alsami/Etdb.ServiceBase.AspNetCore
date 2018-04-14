﻿using System;

namespace Etdb.ServiceBase.ErrorHandling.Abstractions.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
            
        }
    }
}