using AutoMapper;

namespace cube360.vbs.application.Common.Mappings;

public interface IMapTo<T>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(GetType(), typeof(T)).ReverseMap();
    }
}