using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Identity.Database.Entities;

namespace _2BSite.Service.Interface.Identity
{
    public interface ISystemService : IBase<Systems, DTO.Identity.SystemDTO, int>, IDatatable
    {

    }
}
