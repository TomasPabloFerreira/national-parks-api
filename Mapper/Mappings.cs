using AutoMapper;
using national_parks_api.Models;
using national_parks_api.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace national_parks_api.Mapper
{
	public class Mappings : Profile
	{
		public Mappings()
		{
			CreateMap<NationalPark, NationalParkDto>().ReverseMap();
		}
	}
}
