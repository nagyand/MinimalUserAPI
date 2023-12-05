using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.UnitTest.TestData;
internal static class UserTestData
{
    internal static User GetUser() => new User
    {
        Id = 3,
        Name = "Kurtis Test",
        UserName = "Elwyn Forrest",
        Email = "Telly.Hoeger@gmail.com",
        Address = new Address
        {
            Street = "Raptor Trail",
            Suite = "Suite 244",
            City = "Budapest",
            ZipCode = "10599",
            Geo = new Geo
            {
                Lat = 24.8918,
                Lng = 21.8984
            }
        },
        Phone = "210.067.4543",
        Website = "elvis.com",
        Company = new Company
        {
            Name = "Johns and Johns group",
            CatchPhrase = "Configurable multimedia task-force back",
            Bs = "something"
        }
    };
    internal static IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Kurtis Weissnat",
                UserName = "Elwyn.Skiles",
                Email = "Telly.Hoeger@billy.biz",
                Address = new Address
                {
                    Street =  "Rex Trail",
                    Suite = "Suite 280",
                    City = "Howemouth",
                    ZipCode = "58804-1099",
                    Geo = new Geo
                    {
                        Lat = 24.8918,
                        Lng = 21.8984
                    }
                },
                Phone = "210.067.6132",
                Website = "elvis.io",
                Company = new Company
                {
                    Name = "Johns Group",
                    CatchPhrase = "Configurable multimedia task-force",
                    Bs =  "generate enterprise e-tailers"
                }
            },
                        new User
            {
                Id = 2,
                Name = "Glenna Reichert",
                UserName = "Delphine",
                Email = "Chaim_McDermott@dana.io",
                Address = new Address
                {
                    Street =  "Dayna Park",
                    Suite = "Suite 449",
                    City = "Bartholomebury",
                    ZipCode = "76495-3109",
                    Geo = new Geo
                    {
                        Lat = 24.6463,
                        Lng = -168.8889
                    }
                },
                Phone = "(775)976-6794 x41206",
                Website = "conrad.com",
                Company = new Company
                {
                    Name = "Yost and Sons",
                    CatchPhrase = "Switchable contextually-based project",
                    Bs =  "aggregate real-time technologies"
                }
            }
        };
    }
}
