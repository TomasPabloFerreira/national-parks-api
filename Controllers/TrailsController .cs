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
	[Route("api/Trails")]
	[ApiController]
	[ProducesResponseType(400)]
	public class TrailsController : Controller
	{
		private ITrailRepository _trailRepository;
		private readonly IMapper _mapper;

		public TrailsController(ITrailRepository trailRepo, IMapper mapper)
		{
			_trailRepository = trailRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Get list of trails.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<TrailDto>))]
		public IActionResult GetTrails ()
		{
			var trailDto = new List<TrailDto>();

			foreach(var trail in _trailRepository.GetTrails()){
				trailDto.Add(_mapper.Map<TrailDto>(trail));
			}

			return Ok(trailDto);
		}

		/// <summary>
		/// Get individual trail
		/// </summary>
		/// <param name="TrailId"> The Id of the trail </param>
		/// <returns></returns>
		[HttpGet("{TrailId:int}", Name = "GetTrail")]
		[ProducesResponseType(200, Type = typeof(TrailDto))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetTrail (int TrailId)
		{
			var trail = _trailRepository.GetTrail(TrailId);
			if(trail == null) return NotFound();

			var trailDto = _mapper.Map<TrailDto>(trail);
			return Ok(trailDto);
		}

		/// <summary>
		/// Get trails in a national park
		/// </summary>
		/// <param name="nationalParkId"> The Id of the national park </param>
		/// <returns></returns>
		[HttpGet("InNationalPark/{nationalParkId:int}", Name = "GetTrailsInNationalPark")]
		[ProducesResponseType(200, Type = typeof(List<TrailDto>))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		public IActionResult GetTrailsInNationalPark (int nationalParkId)
		{
			var trailList = _trailRepository.GetTrailsInNationalPark(nationalParkId);
			if(trailList == null) return NotFound();

			var trails = new List<TrailDto>();
			foreach(var trail in trailList) {
				trails.Add(_mapper.Map<TrailDto>(trail));
			}

			return Ok(trails);
		}

		[HttpPost]
		[ProducesResponseType(201, Type = typeof(TrailDto))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
		{
			if(trailDto == null) return BadRequest(ModelState);

			if(_trailRepository.TrailExists(trailDto.Name)) {
				ModelState.AddModelError("", "trail Already Exists!");
				return StatusCode(404, ModelState);
			}

			if(!ModelState.IsValid) return BadRequest(ModelState);

			var trail = _mapper.Map<Trail>(trailDto);

			if(!_trailRepository.CreateTrail(trail)) {
				ModelState.AddModelError(
					"",
					$"Something went wrong when saving the record {trail.Name}"
				);
				return StatusCode(500, ModelState);
			}

			return CreatedAtRoute(
				"GetTrail",
				new { TrailId = trail.Id },
				trail
			);
		}

		[HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public IActionResult UpdateTrail(
			int TrailId,
			[FromBody] TrailUpdateDto trailDto
		)
		{
			if(trailDto == null || TrailId != trailDto.Id)
				return BadRequest(ModelState);

			var trail = _mapper.Map<Trail>(trailDto);

			if(!_trailRepository.UpdateTrail(trail)) {
				ModelState.AddModelError(
					"",
					$"Something went wrong when updating the record {trail.Name}"
				);
				return StatusCode(500, ModelState);
			}

			return NoContent();
		}

		[HttpDelete("{TrailId:int}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(409)]
		[ProducesResponseType(500)]
		public IActionResult DeleteTrail(int TrailId)
		{
			if(!_trailRepository.TrailExists(TrailId))
				return NotFound();

			var trail = _trailRepository.GetTrail(TrailId);

			if(!_trailRepository.DeleteTrail(trail)) {
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
