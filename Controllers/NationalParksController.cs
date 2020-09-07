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
	[ProducesResponseType(400)]
	public class NationalParksController : Controller
	{
		private INationalParkRepository _npRepository;
		private readonly IMapper _mapper;

		public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
		{
			_npRepository = npRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Get list of national parks.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
		public IActionResult GetNationalParks ()
		{
			var npDto = new List<NationalParkDto>();

			foreach(var np in _npRepository.GetNationalParks()){
				npDto.Add(_mapper.Map<NationalParkDto>(np));
			}

			return Ok(npDto);
		}

		/// <summary>
		/// Get individual national park
		/// </summary>
		/// <param name="nationalParkId"> The Id of the national park </param>
		/// <returns></returns>
		[HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
		[ProducesResponseType(200, Type = typeof(NationalParkDto))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetNationalPark (int nationalParkId)
		{
			var np = _npRepository.GetNationalPark(nationalParkId);
			if(np == null) return NotFound();

			var npDto = _mapper.Map<NationalParkDto>(np);
			return Ok(npDto);
		}

		[HttpPost]
		[ProducesResponseType(201, Type = typeof(NationalParkDto))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
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
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
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
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(409)]
		[ProducesResponseType(500)]
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
