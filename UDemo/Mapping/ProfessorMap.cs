using FluentNHibernate.Mapping;
using UDemo.Model;
public class ProfessorMap : ClassMap<Professors>
{
    public ProfessorMap()
    {
        Id(x => x.ProfessorID);
        Map(x => x.LastName);
        Map(x => x.FirstName);
    }
}