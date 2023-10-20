using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using UDemo.Model;

public class UDemoHelper
{
    private ISessionFactory _sessionFactory;

    public UDemoHelper(string connectionString)
    {
        CreateSessionFactory(connectionString);
    }

    private void CreateSessionFactory(string connectionString)
    {
        try
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Courses>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
                .BuildSessionFactory();
        }
        catch (Exception ex)
        {
            throw new Exception("Could not create NHibernate session factory", ex);
        }
    }

    public NHibernate.ISession OpenSession()
    {
        if (_sessionFactory == null)
        {
            throw new InvalidOperationException("Session factory has not been initialized. Ensure you have called CreateSessionFactory.");
        }
        return _sessionFactory.OpenSession();
    }

}
