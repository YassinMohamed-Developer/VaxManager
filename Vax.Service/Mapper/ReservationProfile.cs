﻿using AutoMapper;
using Microsoft.EntityFrameworkCore.Update.Internal;
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
	public class ReservationProfile : Profile
	{
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationRequestDto>();

            CreateMap<ReservationRequestDto, Reservation>();

            CreateMap<Reservation, ReservationResponseDto>()
                .ForMember(dest => dest.PatientName, option => option.MapFrom(src => src.Patient.FirstName + ' ' + src.Patient.LastName))
                .ForMember(dest => dest.VaccineName, option => option.MapFrom(src => src.Vaccine.Name))
                .ForMember(dest => dest.VaccineCenterName, option => option.MapFrom(src => src.VaccineCenter.Name))
                .ForMember(dest => dest.DoseNumber,option => option.MapFrom(src => src.DoseNumber.ToString() + " Dose"))
                .ForMember(dest => dest.ReservationStatus,option => option.MapFrom(src => src.ReservationStatus.ToString()));

            CreateMap<Reservation, PatientsWithVaccines>()
            .ForMember(dest => dest.VaccineId, option => option.MapFrom(src => src.Vaccine.Id))
            .ForMember(dest => dest.VaccineName, option => option.MapFrom(src => src.Vaccine.Name))
            .ForMember(dest => dest.PatientId, option => option.MapFrom(src => src.Patient.Id))
            .ForMember(dest => dest.PatientName, option => option.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
            .ForMember(dest => dest.City, option => option.MapFrom(src => src.Patient.City))
            .ForMember(dest => dest.Location, option => option.MapFrom(src => src.Patient.Location));
        }
    }
}
