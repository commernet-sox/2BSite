using _2BSite.Database;
using _2BSite.Service.Interface;
using _2BSite.Service.Profile;
using _2BSite.Service.Service;
using _2BSite.Service.UnitOfWork;
using Autofac;
using Core.Database.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service
{
    public class ServiceModules : Module
    {
        public ServiceModules()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DemoDefaultVPD>().As<IVPD<BSiteContext>>().InstancePerLifetimeScope();

            builder.RegisterType<DemoDatabseUnitofwork>().As<IUnitOfWork<BSiteContext>>().InstancePerLifetimeScope();

            builder.RegisterType<DTOProfile>().As<AutoMapper.Profile>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();

            

            builder.RegisterType<CodeMasterService>().As<ICodeMasterService>().InstancePerLifetimeScope();

        }
    }
}
