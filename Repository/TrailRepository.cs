using Microsoft.EntityFrameworkCore;
using national_parks_api.Data;
using national_parks_api.Models;
using national_parks_api.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace national_parks_api.Repository
{
	public class TrailRepository : ITrailRepository
	{
		private readonly ApplicationDbContext _db;

		public TrailRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public bool CreateTrail(Trail Trail)
		{
			_db.Trails.Add(Trail);
			return Save();
		}

		public bool DeleteTrail(Trail Trail)
		{
			_db.Trails.Remove(Trail);
			return Save();
		}

		public Trail GetTrail(int TrailId)
		{
			return _db.Trails
				.Include(t => t.NationalPark)
				.FirstOrDefault(x => x.Id == TrailId);
		}

		public ICollection<Trail> GetTrails()
		{
			return _db.Trails
				.Include(t => t.NationalPark)
				.OrderBy(x => x.Name).ToList();
		}

		public bool TrailExists(string name)
		{
			return _db.Trails.Any(
				x => x.Name.ToLower().Trim() == name.ToLower().Trim()
			);
		}

		public bool TrailExists(int id)
		{
			return _db.Trails.Any(x => x.Id == id);
		}

		public bool Save()
		{
			return _db.SaveChanges() > 0;
		}

		public bool UpdateTrail(Trail Trail)
		{
			_db.Trails.Update(Trail);
			return Save();
		}

		public ICollection<Trail> GetTrailsInNationalPark(int npId)
		{
			return _db.Trails
				.Include(t => t.NationalPark)
				.Where(t => t.NationalParkId == npId)
				.ToList();
		}
	}
}
