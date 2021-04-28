using Core.WebServices.Interface;

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Identity.Database.Entities;
using _2BSite.Service.DTO.Identity;

namespace _2BSite.Service.Interface.Identity
{
    public interface IUserSystemService : IBase<UserSystem, UserSystemDTO, int>, IDatatable
    {

    }
}
