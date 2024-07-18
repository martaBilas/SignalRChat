using Bogus;
using Domain;

namespace ChatIntegrationTests.Fixtures;

public class UserFixture
{
    public static List<User> GetUsers(int count, bool useNewSeed = false)
    {
        return GetUserFaker(useNewSeed).Generate(count);
    }

    public static User GetUser(bool useNewSeed = false) => GetUsers(1, useNewSeed)[0];

    private static Faker<User> GetUserFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }
        return new Faker<User>()
            .RuleFor(t => t.Id, _ => 0)
            .RuleFor(t => t.UserName, (faker) => faker.Name.FirstName())
            .UseSeed(seed);
    }
}