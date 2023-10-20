using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using UDemo.Model;

public class Repository<TEntity> where TEntity : class
{
    private readonly ISessionFactory _sessionFactory;

    public Repository(string connectionString)
    {
        _sessionFactory = _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<StudentMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
                .BuildSessionFactory(); 
    }

    public TEntity Create(TEntity entity)
    {
        using (var session = _sessionFactory.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(entity);
                transaction.Commit();
                return entity;
            }
        }
    }

    public TEntity Update(TEntity entity)
    {
        using (var session = _sessionFactory.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(entity);
                transaction.Commit();
                return entity;
            }
        }
    }

    public void Delete(TEntity entity)
    {
        using (var session = _sessionFactory.OpenSession())
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(entity);
                transaction.Commit();
            }
        }
    }

    public List<TEntity> GetAll()
    {
        using (var session = _sessionFactory.OpenSession())
        {
            return session.Query<TEntity>().ToList();
        }
    }

    public TEntity GetById(int id)
    {
        using (var session = _sessionFactory.OpenSession())
        {
            return session.Get<TEntity>(id);
        }
    }
}
