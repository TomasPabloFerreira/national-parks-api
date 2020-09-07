using Microsoft.EntityFrameworkCore;
using national_parks_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace national_parks_api.Data
{
	public class ApplicationDbContext : DbContext
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			
		}

		public DbSet<NationalPark> NationalParks { get; set; }
		public DbSet<Trail> Trails { get; set; }
	}
}
