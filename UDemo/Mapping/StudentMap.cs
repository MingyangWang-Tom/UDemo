using FluentNHibernate.Mapping;
using UDemo.Model;

public class StudentMap : ClassMap<Students>
{
    public StudentMap()
    {
        Id(x => x.StudentId);
        Map(x => x.LastName);
        Map(x => x.FirstName);
    }
}

