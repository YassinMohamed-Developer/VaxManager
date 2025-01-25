using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Service.DTOS.ResponseDto;

namespace Vax.Service.Mapper
{
	public class AdminProfile : Profile
	{
        public AdminProfile()
        {
            CreateMap<Admin, AdminResponseDto>();
            CreateMap<AdminResponseDto, Admin>();
            CreateMap<VaccineCenter, VaccineCenterResponseDto>();
            CreateMap<VaccineCenterResponseDto, VaccineCenter>();
        }
    }
}
