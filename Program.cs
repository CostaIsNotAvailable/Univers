using System.Collections.ObjectModel;
using ConsoleApp1.Models;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using ConsoleApp1;

Stopwatch watch = new Stopwatch();

// Sync
watch.Start();
DeserializeUniverse();
watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

// Async
watch.Restart();
await DeserializeUniverseAsync();
watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

#region Sync

static List<SolarSystem> DeserializeUniverse()
{
    // Progress Viewer
    ProgressViewer viewer = new ProgressViewer(10020);

    // Get list of system folders
    string universeDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Universe";
    string[] directories = Directory.GetDirectories(universeDirectory);

    List<SolarSystem> systems = new List<SolarSystem>();

    foreach (string systemPath in directories)
    {
        systems.Add(new SolarSystem(DeserializeSystem(systemPath, viewer)));
    }

    return systems;
}

static List<Planet> DeserializeSystem(string systemPath, ProgressViewer viewer)
{
    // Get list of planet files
    string[] files = Directory.GetFiles(systemPath, "*.json");

    List<Planet> planets = new List<Planet>();

    foreach (string planetPath in files)
    {
        planets.Add(DeserializePlanet(planetPath, viewer));
    }

    return planets;
}

static Planet DeserializePlanet(string planetPath, ProgressViewer viewer)
{
    string file = File.ReadAllText(planetPath);

    Planet planet = JsonConvert.DeserializeObject<Planet>(file);

    planet.Deserialized += viewer.OnPlanetDeserialized;

    planet.OnDeserialized();

    return planet;
}
#endregion

#region Async
static async Task<List<SolarSystem>> DeserializeUniverseAsync()
{
    return await Task.Run(() =>
    {
        // Progress Viewer
        ProgressViewer viewer = new ProgressViewer(10020);

        // Get list of system folders
        string universeDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Universe";
        string[] directories = Directory.GetDirectories(universeDirectory);

        List<Task<List<Planet>>> tasks = new List<Task<List<Planet>>>();
        foreach (string systemPath in directories)
        {
            tasks.Add(DeserializeSystemAsync(systemPath, viewer));
        }

        Task.WhenAll(tasks);

        List<SolarSystem> systems = new List<SolarSystem>();
        foreach (Task<List<Planet>> task in tasks)
        {
            systems.Add(new SolarSystem(task.Result));
        }

        return systems;
    });
}

static Task<List<Planet>> DeserializeSystemAsync(string systemPath, ProgressViewer viewer)
{
    return Task.Run(() =>
    {
        // Get list of planet files
        string[] files = Directory.GetFiles(systemPath, "*.json");

        List<Task<Planet>> tasks = new List<Task<Planet>>();
        foreach (string planetPath in files)
        {
            tasks.Add(DeserializePlanetAsync(planetPath, viewer));
        }

        Task.WhenAll(tasks);

        List<Planet> planets = new List<Planet>();
        foreach (Task<Planet> task in tasks)
        {
            planets.Add(task.Result);
        }

        return planets;
    });
}

static Task<Planet> DeserializePlanetAsync(string planetPath, ProgressViewer viewer)
{
    return Task.Run(() =>
    {
        string file = File.ReadAllText(planetPath);

        Planet planet = JsonConvert.DeserializeObject<Planet>(file);

        planet.Deserialized += viewer.OnPlanetDeserialized;

        planet.OnDeserialized();

        return planet;
    });
}
#endregion