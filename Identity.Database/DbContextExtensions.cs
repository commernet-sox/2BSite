using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Identity.Database
{
    public static class DbContextExtensions
    {
        public static DbContext GetDbContext<T>(this IQueryable<T> source)
        {
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = (QueryCompiler)compilerField.GetValue(source.Provider);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = (RelationalQueryContextFactory)queryContextFactoryField.GetValue(compiler);


            object stateManagerDynamic;

            var dependenciesProperty = typeof(RelationalQueryContextFactory).GetProperty("Dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dependenciesProperty != null)
            {
                // EFCore 2.x
                var dependencies = dependenciesProperty.GetValue(queryContextFactory);

                var stateManagerField = typeof(DbContext).Assembly.GetType("Microsoft.EntityFrameworkCore.Query.QueryContextDependencies").GetProperty("StateManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                stateManagerDynamic = stateManagerField.GetValue(dependencies);
            }
            else
            {
                // EFCore 1.x
                var stateManagerField = typeof(IQueryContextFactory).GetProperty("StateManager", BindingFlags.NonPublic | BindingFlags.Instance);
                stateManagerDynamic = stateManagerField.GetValue(queryContextFactory);
            }

            IStateManager stateManager = stateManagerDynamic as IStateManager;

            if (stateManager == null)
            {
                //Microsoft.EntityFrameworkCore.Internal.LazyLoader.LazyRef<IStateManager> lazyStateManager = stateManagerDynamic as Microsoft.EntityFrameworkCore.Internal.LazyRef<IStateManager>;
                var lazyStateManager = stateManagerDynamic as Lazy<IStateManager>;
                if (lazyStateManager != null)
                {
                    stateManager = lazyStateManager.Value;
                }
            }

            //if (stateManager == null)
            //{
            //    stateManager = ((dynamic) stateManagerDynamic).Value;
            //}


            return stateManager.Context;
        }
    }
}
