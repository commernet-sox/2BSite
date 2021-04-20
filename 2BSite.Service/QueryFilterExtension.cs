using System;

namespace _2BSite.Service
{
    /// <summary>
    /// 全局QUERY过滤启用/停用
    /// </summary>
    public static class QueryFilterExtension
    {

        ///// <summary>
        ///// 停用全局过滤
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="httpContext"></param>
        ///// <param name="context"></param>
        //public static void DisableQueryFilter<T>(HttpContext httpContext, DbContext context) where T : class
        //{
        //    UserDTO loginsession = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(httpContext.Session.GetString("User"));
        //    DisableQueryFilter<T>(loginsession, context);
        //}
        ///// <summary>
        ///// 停用全局过滤
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="loginsession">登录的SESSION 从REDIS获取</param>
        ///// <param name="context"></param>
        //public static void DisableQueryFilter<T>(UserDTO loginsession, DbContext context) where T : class
        //{
        //    if (!loginsession.IsAdmin)
        //    {
        //        List<int> list_department = new List<int>();
        //        if (loginsession.UserDepartment != null)
        //        {
        //            string name = typeof(T).Name;
        //            list_department = loginsession.UserDepartment.Select(s => s.DepartmentId).Distinct().ToList();
        //            if (list_department.Count == 0)
        //                list_department.Add(-100);
        //            string departmentkey = string.Join("_", list_department);
        //            var queryFilterContext = QueryFilterManager.AddOrGetFilterContext(context);
        //            var queryFilter = queryFilterContext.Filters.Where(p => p.Key.ToString() == departmentkey + "_D_" + name).FirstOrDefault();
        //            if (queryFilter.Key != null)
        //            {
        //                queryFilterContext.DisableFilter(queryFilter.Value, typeof(T));
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 停用全局过滤
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="loginsession">登录的SESSION 从REDIS获取</param>
        ///// <param name="context"></param>
        //public static void DisableQueryFilter<T>(UserDTO loginsession, DbContext context, string preString) where T : class
        //{
        //    if (!loginsession.IsAdmin)
        //    {

        //        if (loginsession.UserDepartment != null)
        //        {
        //            List<int> list_department = new List<int>();
        //            string name = typeof(T).Name;

        //            list_department = loginsession.UserDepartment.Select(s => s.DepartmentId).Distinct().ToList();
        //            if (list_department.Count == 0)
        //                list_department.Add(-100);
        //            string departmentkey = string.Join("_", list_department);
        //            var queryFilterContext = QueryFilterManager.AddOrGetFilterContext(context);
        //            var queryFilter = queryFilterContext.Filters.Where(p => p.Key.ToString() == preString + departmentkey + "_D_" + name).FirstOrDefault();
        //            if (queryFilter.Key != null)
        //            {
        //                queryFilterContext.DisableFilter(queryFilter.Value, typeof(T));
        //            }
        //        }
        //        else if (loginsession.UserSupplier != null)
        //        {
        //            List<int> list_sipplier = new List<int>();
        //            string name = typeof(T).Name;

        //            list_sipplier = loginsession.UserSupplier.Select(s => s.SupplierId).Distinct().ToList();
        //            if (list_sipplier.Count == 0)
        //                list_sipplier.Add(-100);
        //            string supplierkey = string.Join("_", list_sipplier);
        //            var queryFilterContext = QueryFilterManager.AddOrGetFilterContext(context);
        //            var queryFilter = queryFilterContext.Filters.Where(p => p.Key.ToString() == preString + supplierkey + "_S_" + name).FirstOrDefault();
        //            if (queryFilter.Key != null)
        //            {
        //                queryFilterContext.DisableFilter(queryFilter.Value, typeof(T));
        //            }
        //        }
        //    }
        //}


        ///// <summary>
        ///// 启用全局过滤(默认是开启)
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="httpContext"></param>
        ///// <param name="context"></param>
        //public static void EnableQueryFilter<T>(HttpContext httpContext, DbContext context) where T : class
        //{
        //    UserDTO loginsession = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(httpContext.Session.GetString("User"));
        //    EnableQueryFilter<T>(loginsession, context);
        //}
        ///// <summary>
        ///// 启用全局过滤(默认是开启)
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="loginsession">登录的SESSION 从REDIS获取</param>
        ///// <param name="context"></param>
        //public static void EnableQueryFilter<T>(UserDTO loginsession, DbContext context) where T : class
        //{
        //    if (!loginsession.IsAdmin)
        //    {
        //        if (loginsession.UserDepartment != null)
        //        {
        //            List<int> list_department = new List<int>();

        //            string name = typeof(T).Name;
        //            list_department = loginsession.UserDepartment.Select(s => s.DepartmentId).Distinct().ToList();
        //            if (list_department.Count == 0)
        //                list_department.Add(-100);
        //            string departmentkey = string.Join("_", list_department);
        //            var queryFilterContext = QueryFilterManager.AddOrGetFilterContext(context);
        //            var queryFilter = queryFilterContext.Filters.Where(p => p.Key.ToString() == departmentkey + "_D_" + name).FirstOrDefault();
        //            if (queryFilter.Key != null)
        //            {
        //                queryFilterContext.EnableFilter(queryFilter.Value, typeof(T));
        //            }
        //        }
        //        else if (loginsession.UserSupplier != null)
        //        {
        //            List<int> list_supplier = new List<int>();

        //            string name = typeof(T).Name;
        //            list_supplier = loginsession.UserSupplier.Select(s => s.SupplierId).Distinct().ToList();
        //            if (list_supplier.Count == 0)
        //                list_supplier.Add(-100);
        //            string supplierkey = string.Join("_", list_supplier);
        //            var queryFilterContext = QueryFilterManager.AddOrGetFilterContext(context);
        //            var queryFilter = queryFilterContext.Filters.Where(p => p.Key.ToString() == supplierkey + "_S_" + name).FirstOrDefault();
        //            if (queryFilter.Key != null)
        //            {
        //                queryFilterContext.EnableFilter(queryFilter.Value, typeof(T));
        //            }
        //        }
        //    }
        //}

    }
}
