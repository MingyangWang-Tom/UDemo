using FluentNHibernate.Mapping;
using UDemo;

public class CourseMap : ClassMap<UDemo.Model.Courses>
{
    public CourseMap()
    {
        Id(x => x.CourseId).GeneratedBy.Assigned();
        Map(x => x.CourseName);
        Map(x => x.ProfessorID);
    }
}
