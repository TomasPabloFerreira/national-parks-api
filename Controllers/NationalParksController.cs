using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using national_parks_api.Models;
using national_parks_api.Models.Dtos;
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

		[HttpGet]
		public IActionResult GetNationalParks ()
		{
			var npDto = new List<NationalParkDto>();

			foreach(var np in _npRepository.GetNationalParks()){
				npDto.Add(_mapper.Map<NationalParkDto>(np));
			}

			return Ok(npDto);
		}

		[HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
		public IActionResult GetNationalPark (int nationalParkId)
		{
			var np = _npRepository.GetNationalPark(nationalParkId);
			if(np == null) return NotFound();

			var npDto = _mapper.Map<NationalParkDto>(np);
			return Ok(npDto);
		}

		[HttpPost]
		public IActionResult CreateNationalPark([FromBody] NationalParkDto npDto)
		{
			if(npDto == null) return BadRequest(ModelState);

			if(_npRepository.NationalParkExists(npDto.Name)) {
				ModelState.AddModelError("", "National Park Already Exists!");
				return StatusCode(404, ModelState);
			}

			if(!ModelState.IsValid) return BadRequest(ModelState);

			var np = _mapper.Map<NationalPark>(npDto);

			if(!_npRepository.CreateNationalPark(np)) {
				ModelState.AddModelError(
					"",
					$"Something went wrong when saving the record {np.Name}"
				);
				return StatusCode(500, ModelState);
			}

			return CreatedAtRoute(
				"GetNationalPark",
				new { nationalParkId = np.Id },
				np
			);
		}

		[HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
		public IActionResult UpdateNationalPark(
			int nationalParkId,
			[FromBody] NationalParkDto npDto
		)
		{
			if(npDto == null || nationalParkId != npDto.Id)
				return BadRequest(ModelState);

			var np = _mapper.Map<NationalPark>(npDto);

			if(!_npRepository.UpdateNationalPark(np)) {
				ModelState.AddModelError(
					"",
					$"Something went wrong when updating the record {np.Name}"
				);
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}

		[HttpDelete("{nationalParkId:int}")]
		public IActionResult DeleteNationalPark(int nationalParkId)
		{
			if(!_npRepository.NationalParkExists(nationalParkId))
				return NotFound();

			var np = _npRepository.GetNationalPark(nationalParkId);

			if(!_npRepository.DeleteNationalPark(np)) {
				ModelState.AddModelError(
					"",
					"Something went wrong when deleting the record"
				);
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}
	}
}
