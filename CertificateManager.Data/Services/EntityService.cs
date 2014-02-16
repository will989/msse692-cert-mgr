using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CertificateManager.Data.Services
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using Entities;

    public abstract class EntityService<T> : IEntityService<T> where T : IMongoEntity
    {
        protected readonly MongoConnectionHandler<T> MongoConnectionHandler;

        public virtual void Create(T entity)
        {
            //// Save the entity with safe mode (WriteConcern.Acknowledged)
            var result = this.MongoConnectionHandler.MongoCollection.Save(
                entity,
                new MongoInsertOptions
                {
                    WriteConcern = WriteConcern.Acknowledged
                });

            if (!result.Ok)
            {
                //// Something went wrong
            }
        }

        public virtual void Delete(string id)
        {
            var result = this.MongoConnectionHandler.MongoCollection.Remove(
                Query<T>.EQ(e => e.Id,
                new ObjectId(id)),
                RemoveFlags.None,
                WriteConcern.Acknowledged);

            if (!result.Ok)
            {
                //// Something went wrong
            }
        }

        protected EntityService()
        {
            MongoConnectionHandler = new MongoConnectionHandler<T>();
        }

        
        public virtual T GetByName(string name)
        {
            string id = "1";
            //var entityQuery = Query<T>.EQ(c => c.SubjectName, name);
            //return this.MongoConnectionHandler.MongoCollection.FindOne
            //var entityQuery = Query<T>.Where(c => c.Name.Equals(name));
            var entityQuery = Query<T>.EQ(c => c.Id, new ObjectId(id));
            return this.MongoConnectionHandler.MongoCollection.FindOne(entityQuery);
        }
        
         

        public IEnumerable<Certificate> GetCertificateDetails()
        {
            var certificatesCursor = this.MongoConnectionHandler.MongoCollection.FindAllAs<Certificate>()
                .SetSortOrder(SortBy<Certificate>.Ascending(c => c.Name))
               .SetFields(Fields<Certificate>.Include(c => c.Id, c => c.Name, c => c.Thumbprint));
            return certificatesCursor;
        }

        public virtual T GetById(string id)
        {
            var entityQuery = Query<T>.EQ(c => c.Id, new ObjectId(id));
            return this.MongoConnectionHandler.MongoCollection.FindOne(entityQuery);
        }

        public abstract void Update(T entity);
        public void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
