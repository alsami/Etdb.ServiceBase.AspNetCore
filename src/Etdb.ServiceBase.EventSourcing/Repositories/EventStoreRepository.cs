﻿using System;
using System.Collections.Generic;
using System.Linq;
using Etdb.ServiceBase.EventSourcing.Abstractions.Base;
using Etdb.ServiceBase.EventSourcing.Abstractions.Events;
using Etdb.ServiceBase.EventSourcing.Abstractions.Repositories;

namespace Etdb.ServiceBase.EventSourcing.Repositories
{
    public class EventStoreRepository : IEventStoreRepository, IDisposable
    {
        private readonly EventStoreContextBase context;

        public EventStoreRepository(EventStoreContextBase context)
        {
            this.context = context;
        }

        public IList<StoredEvent> All(Guid aggregateId)
        {
            return this.context.Set<StoredEvent>()
                .AsQueryable()
                .Where(storedEvent => storedEvent.AggregateId == aggregateId)
                .ToList();
        }

        public void Store(StoredEvent theEvent)
        {
            context.Set<StoredEvent>().Add(theEvent);
            context.SaveChanges();
        }

        public void Dispose()
        {
            this.context?.Dispose();
        }
    }
}