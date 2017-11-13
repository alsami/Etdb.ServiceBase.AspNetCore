﻿using System;
using EntertainmentDatabase.REST.API.WebService.Domain.Base;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Entities
{
    public class ErrorLog : ILogInfo
    {
        public Guid Id
        {
            get;
            set;
        }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public string TraceId
        {
            get;
            set;
        }

        public string HttpMethod
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public DateTime Occurrence
        {
            get;
            set;
        }
    }
}
