using System.Net;
using FluentAssertions;

namespace SpaReservation.API.Tests;

public class DemoHostingTests : IClassFixture<DemoWebApplicationFactory>
{
    private readonly DemoWebApplicationFactory _factory;

    public DemoHostingTests(DemoWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task Root_ServesTheSpaIndexDocument()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync())
            .Should().Contain(DemoWebApplicationFactory.StubIndexMarker);
    }

    [Fact]
    public async Task ClientSideRoute_FallsBackToTheSpaIndexDocument()
    {
        var client = _factory.CreateClient();

        // No controller and no file match /admin/bookings. Without the SPA
        // fallback this 404s, and refreshing any admin page breaks.
        var response = await client.GetAsync("/admin/bookings");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync())
            .Should().Contain(DemoWebApplicationFactory.StubIndexMarker);
    }

    [Fact]
    public async Task UnknownApiRoute_Returns404AndNotTheSpaIndexDocument()
    {
        var client = _factory.CreateClient();

        // Regression guard: a naive MapFallbackToFile swallows unmatched /api
        // routes and answers them with 200 + index.html, which turns every
        // client bug into a silent HTML-instead-of-JSON parse failure.
        var response = await client.GetAsync("/api/this-endpoint-does-not-exist");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync())
            .Should().NotContain(DemoWebApplicationFactory.StubIndexMarker);
    }

    [Fact]
    public async Task ServicesEndpoint_ReturnsSeededDemoData()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/services");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // This endpoint returns a PagedResult envelope, not a bare array.
        var body = await response.Content.ReadAsStringAsync();
        body.Should().StartWith("{\"items\":[");
        body.Should().NotContain("\"items\":[]", "DbSeeder should have populated services on startup");

        // The SPA filters on category NAMES. Serialising the enum as an integer
        // would make every category tab silently match nothing.
        body.Should().Contain("\"category\":\"", "enums must serialise as strings");
    }

    [Fact]
    public async Task StaffEndpoint_ReturnsSeededDemoData()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/staff");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // This endpoint returns a PagedResult envelope, not a bare array.
        var body = await response.Content.ReadAsStringAsync();
        body.Should().StartWith("{\"items\":[");
        body.Should().NotContain("\"items\":[]", "DbSeeder should have populated staff on startup");
    }
}
