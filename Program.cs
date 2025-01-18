using CounterTestApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder().ConfigureServices(collection =>
{
    collection.AddMeasurementTransient<IWordCounter, WordCounter>();
}).Build();

//dotnet-counters monitor -n CounterTestApp CounterTestApp.WordCounter
var wordCounter = host.Services.GetRequiredService<IWordCounter>();

for (var i = 0; i < 100; i++)
{
    wordCounter.CountWords();
    Thread.Sleep(1000);
}

Console.ReadLine();

