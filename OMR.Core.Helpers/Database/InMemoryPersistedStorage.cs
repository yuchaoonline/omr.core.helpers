using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OMR.Core.Helpers.Database
{
    public class InMemoryPersistedStorage : IStorage
    {
        private readonly string _dbDirectoryPath;

        private IList<Type> _registeredTypes;

        public InMemoryPersistedStorage(string dbDirectoryPath)
        {
            _dbDirectoryPath = dbDirectoryPath;
            _registeredTypes = new List<Type>();
        }

        private string GetEntityPath(Type entityType, Guid id)
        {
            return Path.Combine(_dbDirectoryPath, entityType.FullName + "$" + id.ToString() + ".xml"); // TODO: dir per type
        }

        private string GetEntityPath(EntityBase entity)
        {
            return Path.Combine(_dbDirectoryPath, entity.GetType().FullName + "$" + entity.Id.ToString() + ".xml"); // TODO: dir per type
        }

        public bool Create(EntityBase entity)
        {
            string path = GetEntityPath(entity);

            SerializeToXml(path, entity);

            return true;
        }

        public object Read(Guid id, Type objectType) // TODO
        {
            string path = GetEntityPath(objectType, id);

            return DeserializeFromXml(path, objectType);
        }

        public bool Update(EntityBase entity)
        {
            string path = GetEntityPath(entity);

            if (!File.Exists(path))
                throw new InvalidDataException(); // TODO: message

            if (Delete(entity))
            {
                if (Create(entity))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Delete(EntityBase entity)
        {
            string path = GetEntityPath(entity);
            File.Delete(path); // TODO: IOHelper

            return true;
        }

        public IEnumerable<EntityBase> GetAllEntities()
        {
            foreach (var item in Directory.GetFiles(_dbDirectoryPath)) // TODO: IOHelper
            {
                string typeName = Path.GetFileName(item).Split('$')[0];

                Type t = FindType(typeName);

                var obj = (EntityBase)DeserializeFromXml(item, t);

                yield return obj;
            }

        }

        public void RegisterTypes(params Type[] types)
        {
            foreach (var item in types)
            {
                _registeredTypes.Add(item);
            }
        }

        private Type FindType(string typeName)
        {
            Type t = Type.GetType(typeName);

            if (t == null)
            {
                t = _registeredTypes.Where(f => f.FullName == typeName).FirstOrDefault();
            }

            if (t == null)
            {
                throw new Exception(typeName + " can not be found"); // TODO: message
            }

            return t;
        }

        #region Serialization

        private void SerializeToXml(string path, object obj)
        {
            var s = new XmlSerializer(obj.GetType());

            using (var w = new StreamWriter(path, false, Encoding.UTF8))
            {
                s.Serialize(w, obj);
            }
        }

        private object DeserializeFromXml(string path, Type objectType)
        {
            var s = new XmlSerializer(objectType);

            using (var r = new StreamReader(path))
            {
                return s.Deserialize(r);
            }
        }

        #endregion
    }

}
