using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Monday.WebApi.Exceptions;
using Monday.WebApi.Handlers;
using NSubstitute;

namespace Monday.WebApi.UnitTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task Exception_Handler_Should_Write_Bad_Request_To_Response()
    {
        // Arrange
        var problemDetailsservice = Substitute.For<IProblemDetailsService>();
        var sut = new ExceptionsHandler(problemDetailsservice);
        var httpContext = Substitute.For<HttpContext>();
        var failures = new List<string>{"failure1"};
        var exception = new ValidationException("tmp", failures);
        
        // Act
        var result = await sut.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.IsFalse(result);

        httpContext.Response.StatusCode
            .Should()
            .Be(400)
            ;
    }
}
