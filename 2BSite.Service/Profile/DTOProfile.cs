using _2BSite.Database.Entities;
using _2BSite.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2BSite.Service.Profile
{
    public class DTOProfile : AutoMapper.Profile
    {
        public DTOProfile() : base()
        {
            CreateMap<CodeMasterDTO, CodeMaster>();
            CreateMap<CodeMaster, CodeMasterDTO>();
        }
    }
}
