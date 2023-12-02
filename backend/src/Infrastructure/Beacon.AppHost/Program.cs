using Beacon.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Beacon_API>("beacon.api");

builder.AddNpmApp("frontend", "../../../../frontend/BeaconUI", "watch")
    .WithReference(api)
    .WithServiceBinding(scheme: "http");

builder.Build().Run();
