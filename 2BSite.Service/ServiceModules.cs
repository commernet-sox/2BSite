using _2BSite.Database;
using _2BSite.Service.Interface;
using _2BSite.Service.Interface.Identity;
using _2BSite.Service.Interface.WX;
using _2BSite.Service.Profile;
using _2BSite.Service.Service;
using _2BSite.Service.Service.WX;
using _2BSite.Service.UnitOfWork;
using _2BSite.Service.UnitOfWork.VPD;
using Autofac;
using Core.Database.Repository;
using Identity.Database;
using System;
using System.Collections.Generic;
using System.Text;
using WXSite.Database;

namespace _2BSite.Service
{
    public class ServiceModules : Module
    {
        public ServiceModules()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MiniProgramVPD>().As<IVPD<WXContext>>().InstancePerLifetimeScope();

            builder.RegisterType<DemoDefaultVPD>().As<IVPD<BSiteContext>>().InstancePerLifetimeScope();

            builder.RegisterType<IdentityVPD>().As<IVPD<IdentityDataContext>>().InstancePerLifetimeScope();

            builder.RegisterType<DemoDatabseUnitofwork>().As<IUnitOfWork<BSiteContext>>().InstancePerLifetimeScope();

            builder.RegisterType<IdentityDatabaseUniofwork>().As<IUnitOfWork<IdentityDataContext>>().InstancePerLifetimeScope();

            builder.RegisterType<MiniProgramDatabaseUniofwork>().As<IUnitOfWork<WXContext>>().InstancePerLifetimeScope();

            builder.RegisterType<DTOProfile>().As<AutoMapper.Profile>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();

            #region Identity
            builder.RegisterType<Service.UserService>().As<Interface.Identity.IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<RolePermissionService>().As<IRolePermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<UserSystemService>().As<IUserSystemService>().InstancePerLifetimeScope();
            builder.RegisterType<SystemService>().As<ISystemService>().InstancePerLifetimeScope();
            #endregion

            #region wx
            builder.RegisterType<ErrorService>().As<IErrorService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedBackService>().As<IFeedBackService>().InstancePerLifetimeScope();
            builder.RegisterType<HistoryService>().As<IHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionMenuService>().As<IQuestionMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionsService>().As<IQuestionsService>().InstancePerLifetimeScope();
            builder.RegisterType<Service.WX.UserService>().As<Interface.WX.IUserService>().InstancePerLifetimeScope();
            #endregion

            builder.RegisterType<CodeMasterService>().As<ICodeMasterService>().InstancePerLifetimeScope();

        }
    }
}
