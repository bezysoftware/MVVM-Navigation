namespace Bezysoftware.Navigation.Sample.Dto
{
    using System.Collections.Generic;

    public interface IStore
    {
        IEnumerable<Group> GetGroups();
    }
}
