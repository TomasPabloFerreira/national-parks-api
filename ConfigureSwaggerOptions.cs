using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace national_parks_api
{
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		public void Configure(SwaggerGenOptions options)
		{
			options.SwaggerDoc(
				"NationalParksOpenAPISpec", 
				new Microsoft.OpenApi.Models.OpenApiInfo() {
					Title = "National Parks API",
					Version = "1",
					Description = "National Parks API - Open Source Project",
					Contact = new Microsoft.OpenApi.Models.OpenApiContact()
					{
						Email = "tomasferreira373@gmail.com",
						Name = "Tomas Pablo Ferreira",
						Url = new Uri("https://tomasferreira.com")
					},
					License = new Microsoft.OpenApi.Models.OpenApiLicense()
					{
						Name = "MIT License",
						Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
					}
				}
			);
			options.IncludeXmlComments("national-parks-api.xml");
		}
	}
}
