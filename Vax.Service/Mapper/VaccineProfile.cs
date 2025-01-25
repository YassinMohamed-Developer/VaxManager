using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;

namespace Vax.Service.Mapper
{
	public class VaccineProfile : Profile
	{
        public VaccineProfile()
        {
            CreateMap<Vaccine, VaccineResponseDto>()
                .ForMember(dest => dest.VaccineCenterName, options => options.MapFrom(src => src.VaccineCenter.Name));

            CreateMap<VaccineRequestDto, Vaccine>();
        }
    }
}
