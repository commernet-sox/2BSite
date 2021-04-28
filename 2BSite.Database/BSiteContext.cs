using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Core.Database.Extension;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using _2BSite.Database.Entities;
using _2BSite.Database.Interfaces;

namespace _2BSite.Database
{
    public class BSiteContext : DbContext
    {
        public BSiteContext(DbContextOptions<BSiteContext> options)
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
        /// 字典类
        /// </summary>
        public DbSet<CodeMaster> CodeMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasSequence<int>("SEQ_ORDER").HasMin(1).HasMax(999999).StartsAt(1).IncrementsBy(1);

            modelBuilder.Entity<AuditEntry>().ToTable("AuditEntry");
            modelBuilder.Entity<AuditEntryProperty>().ToTable("AuditEntryProperty");


            modelBuilder.Entity<CodeMaster>().ToTable("CodeMaster");
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
    public class DbContextFactory : IDesignTimeDbContextFactory<BSiteContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public BSiteContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BSiteContext>();
            optionsBuilder.UseSqlServer(@"Server=120.55.195.2;Database=BSite;User=sa;Pwd=123qwe!@#;");

            return new BSiteContext(optionsBuilder.Options);
        }
    }
}
