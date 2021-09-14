using AutoMapper;
using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Models;
using BetterDaysContactBook.Models.DTOs;

namespace BetterDaysContactBook.API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<AppUser, UserDTO>()
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge())
                );

            //.ForMember used to do custom mapping where fields in both source and destination are not same
            // example: there is no Age and Name properties in AppUser
            // so we do custom mapping
        }
    }
}
