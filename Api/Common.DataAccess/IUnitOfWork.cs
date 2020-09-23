using Common.DataAccess.Repositories;
using Common.DataAccess.Repositories.Master;
using Common.DataAccess.Repositories.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataAccess
{
    public interface IUnitOfWork
    {
        public IUnitOfWork BeginTran();
        public void RollBackTran();
        public void CommitTran();

        #region "APPLICATION"
        RoleRepository RoleRepository { get; }
        UserRepository UserRepository { get; }
        UserRoleRepository UserRoleRepository { get; }
        UserClaimRepository UserClaimRepository { get; }
        #endregion

        #region "MASTER"
        MT_BloodGroupTypeRepository MT_PeopleRepository { get; }
        #endregion

        #region "STANDARD"
        STD_PeopleRepository STD_PeopleRepository { get; }
        #endregion
    }
}
