﻿using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using ETDB.API.ServiceBase.Abstractions.EventSourcing;
using ETDB.API.ServiceBase.Abstractions.Repositories;
using ETDB.API.ServiceBase.Bus;
using ETDB.API.ServiceBase.ContextBase;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using ETDB.API.ServiceBase.Entities;
using ETDB.API.ServiceBase.EventSourcing;
using ETDB.API.ServiceBase.Repositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remotion.Linq.Clauses;

namespace ETDB.API.ServiceBase.Builder
{
    public class ServiceContainerBuilder
    {
        private readonly ContainerBuilder containerBuilder;

        public ServiceContainerBuilder(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }

        public ServiceContainerBuilder UseConfiguration(IConfigurationRoot configurationRoot)
        {
            this.containerBuilder
                .RegisterInstance(configurationRoot)
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder UseEnvironment(IHostingEnvironment hostingEnvironment)
        {
            this.containerBuilder
                .RegisterInstance(hostingEnvironment)
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder UseGenericRepositoryPattern<TDbContext>() where TDbContext : AppContextBase
        {
            this.containerBuilder.RegisterGeneric(typeof(EntityRepository<>))
                .As(typeof(IEntityRepository<>))
                .InstancePerRequest()
                .WithParameter(new ResolvedParameter(
                    (parameterInfo, componentContext) => parameterInfo.ParameterType == typeof(AppContextBase),
                    (parameterInfo, componentContext) => componentContext.Resolve<TDbContext>()))
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder UseEventSourcing<TDbContext>() where TDbContext : AppContextBase
        {
            this.containerBuilder.RegisterGeneric(typeof(DomainNotificationHandler<>))
                .AsSelf()
                .As(typeof(INotificationHandler<>))
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<InMemoryBus>()
                .As<IMediatorHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .WithParameter(new ResolvedParameter(
                    (parameterInfo, componentContext) => parameterInfo.ParameterType == typeof(AppContextBase),
                    (parameterInfo, componentContext) => componentContext.Resolve<TDbContext>()))
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder UseEventStore<TDbContext>() where TDbContext : EventStoreContextBase
        {
            this.containerBuilder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .AsSelf()
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<EventStoreRepository>()
                .As<IEventStoreRepository>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .WithParameter(new ResolvedParameter(
                    (parameterInfo, componentContext) => parameterInfo.ParameterType == typeof(EventStoreContextBase),
                    (parameterInfo, componentContext) => componentContext.Resolve<TDbContext>()))
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<EventUser>()
                .As<IEventUser>()
                .AsSelf()
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<EventStore>()
                .As<IEventStore>()
                .AsSelf()
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder RegisterTypeAsSingleton<TImplementation>()
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder RegisterTypeAsSingleton<TImplementation, TInterface>() 
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .AsSelf()
                .SingleInstance();

            return this;
        }

        public ServiceContainerBuilder RegisterTypePerRequest<TImplementation, TInterface>()
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerRequest();

            return this;
        }

        public ServiceContainerBuilder RegisterTypePerDependency<TImplementation, TInterface>()
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerDependency();

            return this;
        }

        public ServiceContainerBuilder RegisterTypePerLifetimeScope<TImplementation, TInterface>()
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerLifetimeScope();

            return this;
        }

        public IContainer Build(IServiceCollection serviceCollection)
        {
            this.containerBuilder.Populate(serviceCollection);
            return this.containerBuilder
                .Build();
        }
    }
}
