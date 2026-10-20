using Microsoft.IdentityModel.JsonWebTokens;
using TaskManager.Business.Services;

public class TokenServiceTests
{
    private static TokenService Create() =>
        new TokenService("ThisIsATestKey_AtLeast32Characters_Long!", "TestIssuer", "TestAudience", 60);

    [Fact]
    public void GenerateToken_ReturnsThreePartToken()
    {
        var token = Create().GenerateToken(1, "Ana", "ana@x.com");
        Assert.Equal(3, token.Split('.').Length);
    }

    [Fact]
    public void GenerateToken_ContainsTheUserClaims()
    {
        var token = Create().GenerateToken(7, "Ana", "ana@x.com");
        var jwt = new JsonWebTokenHandler().ReadJsonWebToken(token);

        Assert.Equal("7", jwt.GetClaim("sub").Value);
        Assert.Equal("ana@x.com", jwt.GetClaim("email").Value);
        Assert.Equal("TestIssuer", jwt.Issuer);
    }
}