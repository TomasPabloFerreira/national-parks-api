using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static national_parks_api.Models.Trail;

namespace national_parks_api.Models.Dtos
{
	public class TrailUpdateDto
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public double Distance { get; set; }
		public DifficultyType Difficulty { get; set; }
		[Required]
		public int NationalParkId { get; set; }
	}
}
