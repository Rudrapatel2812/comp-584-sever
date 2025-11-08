using CsvHelper;
using comp_584_sever.Data;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using WorldModel;
using Microsoft.AspNetCore.Identity;

namespace comp_584_sever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(Comp584DataContext context,IHostEnvironment environment, 
        RoleManager<IdentityRole> roleManager, UserManager<WorldModelUser> userManager,
        IConfiguration configuration
        ) : ControllerBase
    {
        string _pathName = Path.Combine(environment.ContentRootPath, "Data/worldcities.csv");
        [HttpPost("Counties")]
        public async Task<ActionResult> PostCountry()
        {
            Dictionary<string, Country> countries = await context.Countries.AsNoTracking().
                ToDictionaryAsync(c=>c.Name,StringComparer.OrdinalIgnoreCase);
            await context.SaveChangesAsync();

            CsvConfiguration config = new(CultureInfo.InvariantCulture) { HasHeaderRecord = true, HeaderValidated = null };
            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<comp854Data> records = csv.GetRecords<comp854Data>().ToList();

            foreach (comp854Data record in records)
            {
                if (!countries.ContainsKey(record.country))
                {
                    Country country = new()
                    {
                        Name = record.country,
                        Iso2 = record.iso2,
                        Iso3 = record.iso3,
         
                    };
                    countries.Add(country.Name, country);
                    await context.Countries.AddAsync(country);
                }
            }
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Cities")]
        public async Task<ActionResult> PostCities()
        {
            Dictionary<string, Country> counties = await context.Countries.AsNoTracking().
    ToDictionaryAsync(c => c.Name, StringComparer.OrdinalIgnoreCase);
            await context.SaveChangesAsync();

            CsvConfiguration config = new(CultureInfo.InvariantCulture) { HasHeaderRecord = true, HeaderValidated = null };
            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<comp854Data> records = csv.GetRecords<comp854Data>().ToList();

            int city_count=0;
            foreach (comp854Data record in records)
            {
                if(record.population.HasValue && record.population.Value > 0)
                {
                    City city = new()
                    {
                        Name=record.city,
                        Lat = (int)record.lat,
                        Lng = (int)record.lng,
                        Population= (int)record.population.Value,
                        CountryId=counties[record.country].Id
                        };

                    city_count++;
                    await context.Cities.AddAsync(city);
                }
                

            } 
         
            await context.SaveChangesAsync();

            return new JsonResult(city_count);
        }

        [HttpPost("Users")]
        public async Task<ActionResult> PostUsers()
        {
            string administrator= "administrator";
            string registertedUser= "registeredUser";

            if (!await roleManager.RoleExistsAsync(administrator))
            {
                await roleManager.CreateAsync(new IdentityRole(administrator));
            }

            if (!await roleManager.RoleExistsAsync(registertedUser))
            {
                await roleManager.CreateAsync(new IdentityRole(registertedUser));
            }
            WorldModelUser adminUser = new()
            {
                UserName = "admin",
                Email = "a@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await userManager.CreateAsync(adminUser, configuration["DefaultPassword:admin"]!);
            await userManager.AddToRoleAsync(adminUser, administrator);

            WorldModelUser registerUser = new()
            {
                UserName = "user",
                Email = "a@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await userManager.CreateAsync(registerUser, configuration["DefaultPassword:user"]!);
            await userManager.AddToRoleAsync(registerUser, registertedUser);

            return Ok();
        }
    }
}
