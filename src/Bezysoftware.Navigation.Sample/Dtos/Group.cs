namespace Bezysoftware.Navigation.Sample.Dto
{
    using System.Collections.Generic;

    public class Group
    {
        public string Name { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }
}
