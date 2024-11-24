using AutoMapper;
using DemoDAL.Models;
using DemoPL.ViewModels;

namespace DemoPL.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
