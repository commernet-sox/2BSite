﻿using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Model
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserPermission
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }
        /// <summary>
        /// 请求动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 请求控制器
        /// </summary>
        public string Control { get; set; }

        /// <summary>
        /// 请求区域
        /// </summary>
        public string Area { get; set; }

        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(Area))
                {
                    return $"/{Control}/{Action}";
                }
                else
                {

                    return $"/{Area}/{Control}/{Action}";
                }
            }
        }

        public string Url2
        {
            get
            {
                if (string.IsNullOrEmpty(Area))
                {
                    if (Action.ToLower() == "index")
                    {
                        return $"/{Control}";
                    }
                    else
                    {
                        return $"/{Control}/{Action}";
                    }
                }
                else
                {
                    if (Action.ToLower() == "index")
                    {
                        return $"/{Area}/{Control}";
                    }
                    else
                    {
                        return $"/{Area}/{Control}/{Action}";
                    }
                }
            }
        }

        public string Url3
        {
            get
            {
                if (string.IsNullOrEmpty(Area))
                {
                    if (Action.ToLower() == "index")
                    {
                        return $"/{Control}/";
                    }
                    else
                    {
                        return $"/{Control}/{Action}";
                    }
                }
                else
                {
                    if (Action.ToLower() == "index")
                    {
                        return $"/{Area}/{Control}/";
                    }
                    else
                    {
                        return $"/{Area}/{Control}/{Action}/";
                    }
                }
            }
        }
    }
}
