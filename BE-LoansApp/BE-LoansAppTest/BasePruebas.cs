using AutoMapper;
using BE_LoansApp.DataAccess;
using BE_LoansApp.DTOs;
using BE_LoansApp.Entities;
using BE_LoansApp.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_LoansApp.Tests
{
    public class BasePruebas
    {
        protected ThingsContext ContruirContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<ThingsContext>()
                .UseInMemoryDatabase(nombreDB).Options;
            var dbContext = new ThingsContext(opciones);
            return dbContext;
        }

        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.AddProfile<AutoMapperProfiles>();
                cfg.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }
    }
}

