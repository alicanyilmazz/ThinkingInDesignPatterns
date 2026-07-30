using Creational.Patterns.Builder;

var user = new UserBuilder()
        .WithName("Ali")
        .WithEmail("ali@test.com")
        .Build();
