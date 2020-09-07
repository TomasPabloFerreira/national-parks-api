using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using national_parks_api.Repository.IRepository;

namespace national_parks_api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NationalParksController : Controller
	{
		private INationalParkRepository _npRepository;
		private readonly IMapper _mapper;

		public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
		{
			_npRepository = npRepo;
			_mapper = mapper;
		}
	}
}
