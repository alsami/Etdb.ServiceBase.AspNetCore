﻿using System.Net;
using System.Threading.Tasks;
using Etdb.ServiceBase.EventSourcing.Abstractions.Response;
using Microsoft.AspNetCore.Mvc;

namespace Etdb.ServiceBase.EventSourcing.Abstractions.ActionResult
{
    public class EventSourcingResult : IActionResult
    {
        private readonly EventSourcedRepsonse eventSourcedRepsonse;

        public EventSourcingResult(EventSourcedRepsonse eventSourcedRepsonse)
        {
            this.eventSourcedRepsonse = eventSourcedRepsonse;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(this.eventSourcedRepsonse)
            {
                StatusCode = this.eventSourcedRepsonse.Success
                    ? (int) HttpStatusCode.OK
                    : (int) HttpStatusCode.BadRequest
            };

            await result.ExecuteResultAsync(context);
        }
    }
}