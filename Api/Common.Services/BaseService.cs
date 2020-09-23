using Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IBaseService<TEntity, TUnitOfWork>
    {
    }

    public interface IStandardService<TEntity>
    {
        public void Create(TEntity entity);
        public void Edit(TEntity entity);
        public void Delete(TEntity entity);
    }

    public abstract class BaseService<TEntity, TUnitOfWork>: IBaseService<TEntity, TUnitOfWork>
        where TUnitOfWork: IUnitOfWork
    {
        protected internal TUnitOfWork uow;
        public BaseService(TUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}
