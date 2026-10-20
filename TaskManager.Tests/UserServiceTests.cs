using TaskManager.Business.Services;

public class UserServiceTests
{
    private static (UserService svc, FakeUserRepository repo) Create()
    {
        var repo = new FakeUserRepository();
        return (new UserService(repo), repo);
    }

    [Fact]
    public async Task CreateAsync_HashesThePassword()
    {
        var (svc, _) = Create();
        var user = await svc.CreateAsync("Ana", "ana@x.com", "password123");
        Assert.NotEqual("password123", user.PasswordHash);
        Assert.StartsWith("$2", user.PasswordHash);
    }

    [Fact]
    public async Task CreateAsync_DuplicateEmail_Throws()
    {
        var (svc, _) = Create();
        await svc.CreateAsync("Ana", "ana@x.com", "password123");
        await Assert.ThrowsAsync<ArgumentException>(() => svc.CreateAsync("Ana2", "ANA@x.com", "password123"));
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsNull()
    {
        var (svc, _) = Create();
        await svc.CreateAsync("Ana", "ana@x.com", "password123");
        Assert.Null(await svc.LoginAsync("ana@x.com", "wrongpass1"));
    }

    [Fact]
    public async Task LoginAsync_CorrectPassword_ReturnsUser()
    {
        var (svc, _) = Create();
        await svc.CreateAsync("Ana", "ana@x.com", "password123");
        Assert.NotNull(await svc.LoginAsync("ana@x.com", "password123"));
    }
}