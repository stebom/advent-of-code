
async Task<int> ProduceAsync()
{
    Console.WriteLine("Starting to produce something");
    await Task.Delay(1000);
    Console.WriteLine("Starting to produce something");
    return 33;
}

async Task DoSomething()
{
    var t = ProduceAsync();

    Console.WriteLine("Continuing processing...");

    var val = await t;

    Console.WriteLine($"Produced {val}");
}

Task.WaitAll(DoSomething());
