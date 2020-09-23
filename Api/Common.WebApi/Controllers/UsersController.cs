using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTO;
using Common.Entities;
using Dapper.Contrib.Extensions;
using Common.WebApi.ConnectionsDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Common.DataAccess.Repositories;
using Common.Utils;
using Common.Utils.Extensions;

namespace Common.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController: ApiControllerBase
    {
        private IMapper _mapper;
        private IUserRepository userRepository;

        public UsersController(IMapper mapper, IUserRepository _userRepository)
        {
            _mapper = mapper;
            userRepository = _userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserSearchCriteria criteria, [FromQuery] Pager pager)
        {
            var users = await userRepository.GetPagedList(criteria, pager);
            var pages = users.ToPagedDTO<User, UserDTO>(_mapper);
            return Ok(pages);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            User user = await userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO data)
        {
            User user = data.MapTo<User>(_mapper);
            int lastId = await userRepository.Insert(user);
            data.Id = lastId;
            return Created("", data);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, UserDTO data)
        {
            var foo = await userRepository.GetById(id);
            if (foo == null)
            {
                return NotFound();
            }

            var user = data.MapTo<User>(_mapper);
            _mapper.Map<User, User>(user, foo);
            await userRepository.Update(foo);
            return Ok(foo);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var foo = await userRepository.GetById(id);
            if (foo == null)
            {
                return NotFound();
            }

            await userRepository.Delete(foo);
            return Ok();
        }
    }
}