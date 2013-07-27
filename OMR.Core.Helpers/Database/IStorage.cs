namespace OMR.Core.Helpers.Database
{
    using System;
    using System.Collections.Generic;

    public interface IStorage
    {
        bool Create(EntityBase entity);
        bool Delete(EntityBase entity);
        IEnumerable<EntityBase> GetAllEntities();
        object Read(Guid id, Type objectType);
        bool Update(EntityBase entity);
        void RegisterTypes(params Type[] types);
    }
}
