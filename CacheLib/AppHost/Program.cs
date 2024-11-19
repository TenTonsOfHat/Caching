var builder = DistributedApplication.CreateBuilder(args);



var cache = builder
    .AddRedis("cache")
    .WithRedisCommander();

builder.AddProject<Projects.PrimaryAPI>(nameof(Projects.PrimaryAPI))
    .WithReference(cache);

builder.AddProject<Projects.SecondaryAPI>(nameof(Projects.SecondaryAPI))
    .WithReference(cache);



builder.Build().Run();