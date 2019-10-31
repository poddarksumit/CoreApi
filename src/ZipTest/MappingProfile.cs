using AutoMapper;
using ZipTest.Db.Model;
using ZipTest.Models.Response;

namespace ZipTest
{
    /// <summary>
    /// Configuration for profilers.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Users, User>().ForMember(d => d.Id, s => s.MapFrom(x => x.UserId)).ForMember(d => d.EmailAddress, s => s.MapFrom(x => x.Email));
        }
    }
}
