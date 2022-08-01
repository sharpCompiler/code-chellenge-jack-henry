
namespace Code_Challenge.Infrastructure.DataGenerator;

public static class HashTagSource
{

    private static readonly string[] Tags =
    {
        // Large cats
        "Panther", "Wildcat", "Tiger", "Lion", "Cheetah", "Cougar", "Leopard",
        // Snakes
        "Viper", "Cottonmouth", "Python", "Boa", "Sidewinder", "Cobra",
        // Other predators
        "Grizzly", "Jackal", "Falcon",
        // Prey
        "Wildabeast", "Gazelle", "Zebra", "Elk", "Moose", "Deer", "Stag", "Pony",
        // HORSES!
        "Horse", "Stallion", "Foal", "Colt", "Mare", "Yearling", "Filly", "Gelding",
        // Occupations
        "Nomad", "Wizard", "Cleric", "Pilot",
        // Technology
        "Mainframe", "Device", "Motherboard", "Network", "Transistor", "Packet", "Robot", "Android", "Cyborg",
        // Sea life
        "Octopus", "Lobster", "Crab", "Barnacle", "Hammerhead", "Orca", "Piranha",
        // Weather
        "Storm", "Thunder", "Lightning", "Rain", "Hail", "Sun", "Drought", "Snow",
        // Other
        "Warning", "Presence", "Weapon"
    };

    public static string NextValue()
    {
        var random = new Random();
        var numberOfTags = random.Next(1, 5);
        var list = new List<string>();
        for (int i = 0; i < numberOfTags; i++)
        {
            var o = "#" + Tags[random.Next(0, Tags.Length)];
            list.Add(o);
        }

        return string.Join(' ', list);
    }
}