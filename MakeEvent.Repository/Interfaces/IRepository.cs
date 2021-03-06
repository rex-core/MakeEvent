﻿using System.Threading.Tasks;
using MakeEvent.Domain.Interfaces;

namespace MakeEvent.Repository.Interfaces
{
    public interface IRepository : IReadOnlyRepository
    {
        TEntity Create<TEntity>(TEntity entity, string createdBy = null)
            where TEntity : class, IEntity;

        TEntity Update<TEntity>(TEntity entity, string modifiedBy = null)
            where TEntity : class, IEntity;

        void Delete<TEntity>(object id)
            where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void Save();
    }
}
