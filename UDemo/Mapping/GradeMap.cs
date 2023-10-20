using FluentNHibernate.Mapping;
using UDemo.Model;
using UDemo;

public class GradeMap : ClassMap<Grades>
{
    public GradeMap()
    {
        Id(x => x.GradeID);
        Map(x => x.CourseId);
        Map(x => x.StudentId);
    }
}
