using Microsoft.AspNetCore.Identity;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Profile;

public class AutoMapperProfile : AutoMapper.Profile
{
    public AutoMapperProfile()
    {
        #region Obstacle mapping profile
        
            CreateMap<ObstacleDto, ObstacleCompleteModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GeometryGeoJson, opt => opt.MapFrom(src => src.GeometryGeoJson))
                .ForMember(dest => dest.Illuminated, opt => opt.MapFrom(src => src.Illuminated));
            
            CreateMap<ObstacleCompleteModel, ObstacleDto>();
            
            CreateMap<ObstacleStep1Model, ObstacleDto>();
            
            CreateMap<ObstacleStep2Model, ObstacleDto>();
            
            CreateMap<ObstacleStep3Model, ObstacleDto>();
            
        #endregion

        #region User mapping profile

            CreateMap<IdentityUser, UserViewModel>();
        
        #endregion

        #region Registrar report mapping profile

            CreateMap<ViewObstacleUserDto, ObstacleUserModel>();

        #endregion

        #region admin mapping profile

        CreateMap<ViewUserRoleDto, UserViewModel>();
        
        #endregion
    }
}