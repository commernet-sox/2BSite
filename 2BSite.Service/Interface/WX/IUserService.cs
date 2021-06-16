﻿
using _2BSite.Service.DTO.WX;
using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using WXSite.Database.Entities;

namespace _2BSite.Service.Interface.WX
{
    public interface IUserService : IBase<User, UserDTO, int>, IDatatable
    {
    }
}
