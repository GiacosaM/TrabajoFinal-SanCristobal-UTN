using BE_LoansApp.Controllers;
using BE_LoansApp.DTOs;
using BE_LoansApp.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_LoansAppTest
{
    [TestClass]
    public class PersonControllerTest : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerListaPersonas()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.People.Add(new BE_LoansApp.Entities.Person()
            {
                Name = "Martin",
                Lastname = "Giacosa",
                Email = "martin@gmail.com",
                Telefono = "154797587"
            });
            contexto.People.Add(new BE_LoansApp.Entities.Person()
            {
                Name = "Mario",
                Lastname = "Hernandez",
                Email = "Msurin@gmail.com",
                Telefono = "155474125"
            });

            await contexto.SaveChangesAsync();

            var contexto2 = ContruirContext(nombreDB);


            var controller = new PersonController(contexto2, mapper);
            var respuesta = await controller.GetAll();

            var personas = respuesta.ToList();
            Assert.AreEqual(2, personas.Count);

        }

        [TestMethod]
        public async Task ObtenerPersonaPorIdNoExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var controller = new PersonController(contexto, mapper);
            var respuesta = await controller.GetById(1);

            var resultado = respuesta.Result as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerPersonaPorIdExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.People.Add(new BE_LoansApp.Entities.Person()
            {
                Name = "Martin",
                Lastname = "Giacosa",
                Email = "martin@gmail.com",
                Telefono = "154797587"
            });
            contexto.People.Add(new BE_LoansApp.Entities.Person()
            {
                Name = "Mario",
                Lastname = "Hernandez",
                Email = "Msurin@gmail.com",
                Telefono = "155474125"
            });

            await contexto.SaveChangesAsync();

            var contexto2 = ContruirContext(nombreDB);
            var controller = new PersonController(contexto2, mapper);

            var id = 1;
            var respuesta = await controller.GetById(id);
            var resultado = respuesta.Value;
            Assert.AreEqual(id, resultado.Id);

        }

        [TestMethod]
        public async Task CrearPersona()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var nuevaPersona = new PersonCreationDTO() {
                Name = "Martin",
                Lastname = "Giacosa",
                Email = "giacosa@gmail.com",
                Telefono = "342154797584",
            };

            var controller = new PersonController(contexto, mapper);

            var respuesta = await controller.Post(nuevaPersona);
            var resultado = respuesta as CreatedAtRouteResult;
            Assert.IsNotNull(resultado);

            var contexto2 = ContruirContext(nombreDB);
            var cantidad = await contexto2.People.CountAsync();
            Assert.AreEqual(1, cantidad);

        }

        [TestMethod]
        public async Task ActualizarPersona()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.People.Add(new BE_LoansApp.Entities.Person()
            {
                Name = "Mario",
                Lastname = "Hernandez",
                Email = "Msurin@gmail.com",
                Telefono = "155474125"
            });
            await contexto.SaveChangesAsync();

            var contexto2 = ContruirContext(nombreDB);
            var controller = new PersonController(contexto2, mapper);

            var personaCreacionDTO = new PersonCreationDTO()
            {
                Name = "Nuevo Nombre",
                Lastname = "Nuevo Apellido",
                Email = "nuevo@gmail.com",
                Telefono = "111222333",
            };

            var id = 1;
            var respuesta = await controller.Put(personaCreacionDTO, id);

            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ContruirContext(nombreDB);
            var existe = await contexto3.People.AnyAsync(x => x.Name == "Nuevo Nombre");
            Assert.IsTrue(existe);

        }

        [TestMethod]
        public async Task BorrarPersonaNoExistente()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var controller = new PersonController(contexto, mapper);

            var respuesta = await controller.Delete(1);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);

        }

        [TestMethod]
        public async Task BorrarPersona()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ContruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.People.Add(new BE_LoansApp.Entities.Person()
            {
                Name = "Mario",
                Lastname = "Hernandez",
                Email = "Msurin@gmail.com",
                Telefono = "155474125"
            });
            await contexto.SaveChangesAsync();

            var contexto2 = ContruirContext(nombreDB);
            var controller = new PersonController(contexto2, mapper);

            var respuesta = await controller.Delete(1);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ContruirContext(nombreDB);
            var existe = await contexto3.People.AnyAsync();
            Assert.IsFalse(existe);


        }



 
    }
    


}

