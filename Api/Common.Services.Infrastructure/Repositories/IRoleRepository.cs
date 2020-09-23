using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Infrastructure.Repositories
{
    public interface IRoleRepository<TRole> where TRole : Role
    {
        Task Delete(int id);
        Task<TRole> Get(int id);
        Task<TRole> Get(string name);
        Task<TRole> Edit(TRole role);
    }
}
