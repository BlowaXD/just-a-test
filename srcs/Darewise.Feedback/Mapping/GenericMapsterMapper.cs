using System;
using System.Collections.Generic;
using Mapster;

namespace Darewise.Feedback.Mapping
{
    public class GenericMapsterMapper<TEntity, TModel> : IMapper<TEntity, TModel>
    {
        public TModel Map(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return entity.Adapt<TEntity, TModel>();
        }

        public IEnumerable<TModel> Map(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            return entities.Adapt<IEnumerable<TEntity>, IEnumerable<TModel>>();
        }
    }
}