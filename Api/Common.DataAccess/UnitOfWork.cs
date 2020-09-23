using Common.DataAccess.Repositories;
using Common.DataAccess.Repositories.Master;
using Common.DataAccess.Repositories.Standard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.DataAccess
{
    public class UnitOfWork: IUnitOfWork
    {
        private IDbConnection db;
        private IDbTransaction tran;

        public UnitOfWork(
            IDbConnection db
            )
        {
            this.db = db;
            //this.BeginTran();
        }

        #region "APPLICATION"
        private RoleRepository roleRepository;
        public RoleRepository RoleRepository
        {
            get { return roleRepository ?? (roleRepository = new RoleRepository(db)); }
        }
        private UserClaimRepository userClaimRepository;
        public UserClaimRepository UserClaimRepository
        {
            get { return userClaimRepository ?? (userClaimRepository = new UserClaimRepository(db)); }
        }
        private UserRepository userRepository;
        public UserRepository UserRepository
        {
            get { return userRepository ?? (userRepository = new UserRepository(db)); }
        }
        private UserRoleRepository userRoleRepository;
        public UserRoleRepository UserRoleRepository
        {
            get { return userRoleRepository ?? (userRoleRepository = new UserRoleRepository(db)); }
        }
        #endregion

        #region "MASTER"
        private MT_BloodGroupTypeRepository mtPeopleRepository;
        public MT_BloodGroupTypeRepository MT_PeopleRepository
        {
            get { return mtPeopleRepository ?? (mtPeopleRepository = new MT_BloodGroupTypeRepository(db)); }
        }
        #endregion

        #region "STANDARD"
        private STD_PeopleRepository stdPeopleRepository;
        public STD_PeopleRepository STD_PeopleRepository
        {
            get { return stdPeopleRepository ?? (stdPeopleRepository = new STD_PeopleRepository(db)); }
        }
        #endregion

        public IUnitOfWork BeginTran()
        {
            if (this.tran == null)
            {
                this.tran = db.BeginTransaction();
            }
            return this;
        }

        public void CommitTran()
        {
            if (this.tran != null)
            {
                this.tran.Commit();
            }
            this.tran = null;
        }

        public void RollBackTran()
        {
            if (this.tran != null)
            {
                this.tran.Rollback();
            }
            this.tran = null;
        }
    }
}
