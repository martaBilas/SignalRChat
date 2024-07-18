using Bogus;
using Domain;

namespace ChatIntegrationTests.Fixtures;

public class ChatFixture
{
    public static List<Chat> GetChats(int count, bool useNewSeed = false)
    {
        return GetChatFaker(useNewSeed).Generate(count);
    }

    public static Chat GetChat(bool useNewSeed = false) => GetChats(1, useNewSeed)[0];

    private static Faker<Chat> GetChatFaker(bool useNewSeed)
    {
        var seed = 0;
        if (useNewSeed)
        {
            seed = Random.Shared.Next(10, int.MaxValue);
        }
        return new Faker<Chat>()
            .RuleFor(t => t.Id, _ => 0)
            .RuleFor(t => t.Name, (faker) => faker.Name.FirstName())
            .RuleFor(t => t.UserCreatorId, _ => 1)
            .UseSeed(seed);
    }
}