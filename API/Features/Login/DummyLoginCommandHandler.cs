using System.Security.Claims;
using BuildingBlocks.API.Models.CQRS;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Models.Responses;

namespace API.Features.Login;

public class DummyLoginCommandHandler : ICommandHandler<DummyLoginCommand, Response<DummyLoginResponse>>
{
    private readonly IJwtManager _jwtManager;
    
    public DummyLoginCommandHandler(IJwtManager jwtManager)
    {
        _jwtManager = jwtManager;
    }
    
    public Task<Response<DummyLoginResponse>> Handle(DummyLoginCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "Suka Tani"),
            new Claim(JwtRegisteredClaimNames.Email, "bayarpolisi@gmail.com"),
            new Claim(ClaimTypes.Role, "rakyatkecil"),
        };

        var accessToken = _jwtManager.CreateAccessToken(claims);
        var refreshToken = _jwtManager.CreateRefreshToken();

        var responseDto = new DummyLoginResponse(accessToken, refreshToken);
        return Task.FromResult(Response.Build(responseDto));
    }
}