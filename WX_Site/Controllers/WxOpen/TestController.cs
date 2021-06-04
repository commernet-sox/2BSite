﻿using _2BSite.Service.Interface;
using Core.Database.Repository;
using Core.Infrastructure;
using Core.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WX_Site.Controllers.WxOpen
{
    public class TestController : Controller
    {
        private Core.Redis.ICacheClient _cacheClient;
        private IServiceProvider _serviceProvider;
        private IUnitOfWork<_2BSite.Database.BSiteContext> _unitOfWork;

        public TestController(ICacheClient cacheClient, IServiceProvider serviceProvider, IUnitOfWork<_2BSite.Database.BSiteContext> unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _cacheClient = cacheClient;
            _unitOfWork = unitOfWork;
            //获取子连接
            var connect = _unitOfWork.DbContext.Database.GetDbConnection();

            if (Core.Infrastructure.Global.DBRWManager.IsMaterConnection(typeof(_2BSite.Database.BSiteContext).ToString(), connect.ConnectionString))
            {
                if (connect.State == System.Data.ConnectionState.Closed)
                {
                    _unitOfWork.DbContext.Database.GetDbConnection().ConnectionString = Global.DBRWManager.GetSlave(typeof(_2BSite.Database.BSiteContext).ToString());
                }
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCodeMaster()
        {
            var codeMasterService = _serviceProvider.GetService(typeof(ICodeMasterService)) as ICodeMasterService;
            var codeMasters = codeMasterService.GetAll().ToList();
            return Json(codeMasters);
        }
    }
}
