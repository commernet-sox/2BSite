﻿using Core.Database.Extension;
using Identity.Database.Entities;
using Identity.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Database
{
    public class IdentityDataContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 审计的创建者，默认为NULL SYSTEM 外部传入LOGIN SESION对应的USERDTO
        /// </summary>
        public string AuditCreateBy { get; set; }


        /// <summary>
        /// 审计数据实例的修改
        /// </summary>
        public DbSet<AuditEntry> AuditEntries { get; set; }
        /// <summary>
        /// 审计数据实例属性的修改
        /// </summary>
        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }


        /// <summary>
        /// 系统
        /// </summary>
        public DbSet<Identity.Database.Entities.Systems> Systems { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 用户系统
        /// </summary>
        public DbSet<UserSystem> UserSystems { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }
        /// <summary>
        /// 角色权限
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuditEntry>().ToTable("AuditEntry");
            modelBuilder.Entity<AuditEntryProperty>().ToTable("AuditEntryProperty");
            modelBuilder.Entity<Identity.Database.Entities.Systems>().ToTable("System");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermission");
            modelBuilder.Entity<UserSystem>().ToTable("UserSystem");

        }


        /// <summary>
        /// 重写保存方法，扩展全局审计
        /// 存在一个问题：修改状态下添加审计，因为是DTO AUTOMAPPER到ENTITY。存在OLD VALUE与NEW VALUE相等问题
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var conn = this.Database.GetDbConnection();
            if (!Core.Infrastructure.Global.DBRWManager.IsMaterConnection(this.GetType().ToString(), conn.ConnectionString))
            {
                DbTransaction transaction = null;
                if (this.Database.CurrentTransaction != null)
                {
                    transaction = this.Database.CurrentTransaction.GetDbTransaction();
                }
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }

                conn.ConnectionString = Core.Infrastructure.Global.DBRWManager.GetMaster(this.GetType().ToString());
                this.Database.UseTransaction(transaction);
            }

            var audit = new Audit();
            audit.Configuration.Exclude(x => true); // 移除所有
            audit.Configuration.Include<IAuditable>();//当实体继承IAuditable时候才添加审计
            audit.Configuration.IgnorePropertyUnchanged = true;
            audit.Configuration.IgnoreRelationshipAdded = true;
            audit.Configuration.IgnoreRelationshipDeleted = true;
            audit.Configuration.IgnoreEntityAdded = true;

            audit.CreatedBy = this.AuditCreateBy;

            audit.PreSaveChanges(this);
            var rowAffecteds = base.SaveChanges();
            audit.PostSaveChanges();
            if (audit.Entries.Count > 0)
            {
                this.AuditEntries.AddRange(audit.Entries);
                base.SaveChanges();
            }

            return rowAffecteds;
        }

        /// <summary>
        /// 重写异步保存方法，扩展全局审计
        ///  存在一个问题：修改状态下添加审计，因为是DTO AUTOMAPPER到ENTITY。存在OLD VALUE与NEW VALUE相等问题
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var conn = this.Database.GetDbConnection();
            if (!Core.Infrastructure.Global.DBRWManager.IsMaterConnection(this.GetType().ToString(), conn.ConnectionString))
            {
                DbTransaction transaction = null;
                if (this.Database.CurrentTransaction != null)
                {
                    transaction = this.Database.CurrentTransaction.GetDbTransaction();
                }
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }

                conn.ConnectionString = Core.Infrastructure.Global.DBRWManager.GetMaster(this.GetType().ToString());
                this.Database.UseTransaction(transaction);
            }
            var audit = new Audit();
            audit.Configuration.Exclude(x => true); // 移除所有
            audit.Configuration.Include<IAuditable>();//当实体继承IAuditable时候才添加审计
            audit.Configuration.IgnorePropertyUnchanged = true;
            audit.Configuration.IgnoreRelationshipAdded = true;
            audit.Configuration.IgnoreRelationshipDeleted = true;
            audit.Configuration.IgnoreEntityAdded = true;

            audit.CreatedBy = this.AuditCreateBy;

            audit.PreSaveChanges(this);
            var rowAffecteds = base.SaveChanges();
            audit.PostSaveChanges();
            if (audit.Entries.Count > 0)
            {
                this.AuditEntries.AddRange(audit.Entries);
                await base.SaveChangesAsync();
            }

            return rowAffecteds;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class DbContextFactory : IDesignTimeDbContextFactory<IdentityDataContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IdentityDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDataContext>();
            optionsBuilder.UseSqlServer(@"Server=120.55.195.2;Database=Identity;User=sa;Pwd=123qwe!@#;");

            return new IdentityDataContext(optionsBuilder.Options);
        }
    }
}
