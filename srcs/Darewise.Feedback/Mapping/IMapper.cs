using System.Collections.Generic;

namespace Darewise.Feedback.Mapping
{
    public interface IMapper<in TEntity, out TModel>
    {
        TModel Map(TEntity entity);
        IEnumerable<TModel> Map(IEnumerable<TEntity> entities);
    }
}