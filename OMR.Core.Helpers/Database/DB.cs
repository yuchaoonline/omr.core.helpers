using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMR.Core.Helpers.Database
{
    public class DB
    {
        private IList<EntityBase> _entities;
        private IStorage _storage;

        public DB(IStorage storage)
        {
            _storage = storage;
        }

        public void Initialize()
        {
            // Load from persistent
            _entities = _storage.GetAllEntities().ToList();
        }

        /// <summary>
        /// Checks on memory
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists(EntityBase entity)
        {
            var r = _entities.Where(f => f.Id == entity.Id && f.GetType() == entity.GetType()).FirstOrDefault();

            return r != null;
        }

        public bool Add(EntityBase entity)
        {
            if (!_storage.Create(entity))
                throw new IOException(); // TODO: message

            _entities.Add(entity);

            return true;
        }

        public bool Remove(EntityBase entity)
        {
            if (!_storage.Delete(entity))
                throw new IOException(); // TODO: message

            if (_entities.Remove(entity))
            {
                return true;
            }

            return false;
        }

        public bool Update(EntityBase entity)
        {
            if (!Exists(entity))
                throw new InvalidDataException("Entity does not exists"); // TODO: message

            if (!_storage.Update(entity))
                throw new IOException(); // TODO: message

            if (Remove(entity))
            {
                if (Add(entity))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddOrUpdate(EntityBase entity)
        {
            if (Exists(entity))
            {
                Update(entity);
            }
            else
            {
                Add(entity);
            }
        }

        public T FirstOrDefault<T>(Guid id) where T : EntityBase
        {
            return (T)_entities.Where(f => f.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="match"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Predicate<T> match) where T : EntityBase
        {
            var r = new List<T>();
            // TODO: linq?
            foreach (var item in _entities)
            {
                if (item is T && match.Invoke((T)item))
                {
                    yield return (T)item;
                    //r.Add((T)item);
                }
            }

            //return r;
        }

        public int Count<T>() where T : EntityBase
        {
            int r = 0;

            foreach (var item in _entities)
            {
                if (item is T)
                    ++r;
            }

            return r;
        }

    }
}
