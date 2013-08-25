using System;

namespace OMR.Core.Helpers.Database
{
    public class EntityBase
    {
        public Guid Id { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
