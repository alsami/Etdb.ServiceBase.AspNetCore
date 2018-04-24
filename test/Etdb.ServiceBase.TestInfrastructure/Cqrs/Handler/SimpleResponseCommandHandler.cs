﻿using System.Threading;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cqrs.Abstractions.Handler;
using Etdb.ServiceBase.TestInfrastructure.Cqrs.Commands;

namespace Etdb.ServiceBase.TestInfrastructure.Cqrs.Handler
{
    public class SimpleResponseCommandHandler : IResponseCommandHandler<SimpleResponseCommand, int>
    {
        public Task<int> Handle(SimpleResponseCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}