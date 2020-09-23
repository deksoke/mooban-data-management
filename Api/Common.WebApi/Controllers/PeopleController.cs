using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.DataAccess;
using Common.DataAccess.Repositories.Standard;
using Common.Entities.Master;
using Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Utils.Extensions;

namespace Common.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ApiControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public PeopleController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PeopleSearchCriteria criteria, [FromQuery] Pager pager)
        {
            var peoples = await this.uow.STD_PeopleRepository.GetPagedList(criteria, pager);
            var pages = peoples.ToPagedDTO<STD_People, STD_People>(mapper);
            return Ok(pages);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            STD_People people = await this.uow.STD_PeopleRepository.GetById(id);
            if (people == null)
            {
                return NotFound();
            }

            return Ok(people);
        }

        [HttpPost]
        public async Task<IActionResult> Create(STD_People data)
        {
            int lastId = await this.uow.STD_PeopleRepository.Insert(data);
            data.Id = lastId;
            return Created("", data);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, STD_People data)
        {
            var foo = await this.uow.STD_PeopleRepository.GetById(id);
            if (foo == null)
            {
                return NotFound();
            }

            var user = data.MapTo<STD_People>(mapper);
            mapper.Map<STD_People, STD_People>(user, foo);
            await this.uow.STD_PeopleRepository.Update(foo);
            return Ok(foo);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var foo = await this.uow.STD_PeopleRepository.GetById(id);
            if (foo == null)
            {
                return NotFound();
            }

            await this.uow.STD_PeopleRepository.Delete(foo);
            return Ok();
        }

    }
}
