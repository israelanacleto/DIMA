namespace Dima.Api.Common.Api;

public interface IEndpoint
{
    static abstract RouteHandlerBuilder Map(IEndpointRouteBuilder app);
}