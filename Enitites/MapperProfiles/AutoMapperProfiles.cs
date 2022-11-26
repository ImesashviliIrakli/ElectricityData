using AutoMapper;
using Enitites.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enitites.MapperProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AggregatedData, AggregatedDataViewModel>()
                .ForMember(
                    x => x.Month,
                    opt => opt.MapFrom(
                        src => src.Date.ToString("yyyy-MM")
                        )
                    );
        }
    }
}
