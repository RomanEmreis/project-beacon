using Beacon.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Beacon_API>("beacon.api");

builder.AddNpmApp("frontend", "../../../../frontend/BeaconUI", "dev")
    .WithReference(api)
    .WithServiceBinding(scheme: "http", hostPort: 5173);

builder.Build().Run();
