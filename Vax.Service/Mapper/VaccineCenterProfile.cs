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
	public class VaccineCenterProfile : Profile
	{
        public VaccineCenterProfile()
        {
            CreateMap<VaccineCenter, VaccineCenterResponseDto>();
            CreateMap<VaccineCenterResponseDto, VaccineCenter>();
            CreateMap<VaccineCenter,VaccineCenterRequestDto>();
            CreateMap<VaccineCenterRequestDto, VaccineCenter>();
        }
    }
}
