namespace Bezysoftware.Navigation.Sample.Dto
{
    using System;
    using System.Collections.Generic;

    public class Store : IStore
    {
        public IEnumerable<Group> GetGroups()
        {
            return new[]
            {
                new Group
                {
                    Name = "Group 1",
                    Items = new []
                    {
                        new Item
                                {
                                    Header = "Header 1",
                                    Content = "Content 1"
                                },
                                new Item
                                {
                                    Header = "Header 2",
                                    Content = "Content 2"
                                },
                                new Item
                                {
                                    Header = "Header 3",
                                    Content = "Content 3"
                                },
                                new Item
                                {
                                    Header = "Header 4",
                                    Content = "Content 4"
                                }
                            }
                },
                new Group
                {
                    Name = "Group 2",
                    Items = new []
                            {
                                new Item
                                {
                                    Header = "Header 5",
                                    Content = "Content 5"
                                },
                                new Item
                                {
                                    Header = "Header 6",
                                    Content = "Content 6"
                                }
                            }
                    
                }
            };
        }
    }
}
