using Imagine.Core.Entities;

namespace Imagine.Core.Specifications;

public class ArtsWithUserAndSettingSpecification : BaseSpecification<Art>
{
    public ArtsWithUserAndSettingSpecification()
    {
        AddInclude(x => x.User);
        AddInclude(x => x.ArtSettings);
    }

    public ArtsWithUserAndSettingSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.User);
        AddInclude(x => x.ArtSettings);
    }
}