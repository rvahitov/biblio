using Aspire.Hosting;
using Biblio.Aspire.Resources.KurrentDb;

var builder = DistributedApplication.CreateBuilder(args);
var kurrentDb = builder
    .AddKurrentDb("KurrentDb")
    .EnableInsecureMode()
    .RunAllProjections()
    .EnableAtomPubOverHttp();

builder.AddProject<Projects.Biblio_Citations_WebApi>("WebApi")
    .WithHttpHealthCheck("/health")
    .WithReference(kurrentDb)
    .WaitFor(kurrentDb);

builder.Build().Run();
