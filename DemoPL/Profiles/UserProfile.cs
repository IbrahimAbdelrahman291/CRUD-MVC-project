using AutoMapper;
using DemoDAL.Models;
using DemoPL.ViewModels;

namespace DemoPL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
