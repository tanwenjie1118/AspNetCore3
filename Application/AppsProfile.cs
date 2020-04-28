using Applications.Model;
using AutoMapper;
using Services.Model;
using System;

namespace Applications
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
