using AutoMapper;
using DemoDAL.Models;
using DemoPL.ViewModels;

namespace DemoPL.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() 
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
