using Hal.Applications.Model;
using AutoMapper;
using Hal.Services.Model;
using System;

namespace Hal.Applications
{
    public class AppsProfile : Profile
    {
        /// <summary>
        ///  Add mapper types to Container
        /// </summary>
        public AppsProfile()
        {
            CreateMap<MyModelX, MyModelXDto>();
        }
    }
}
