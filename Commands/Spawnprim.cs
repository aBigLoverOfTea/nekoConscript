namespace Buttplug.Commands {
    using System;
    using System.Security;
    using System.Windows.Input;
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using Primitive = Exiled.API.Features.Toys.Primitive;
    using CommandSystem;
    using UnityEngine;


    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawnprim : CommandSystem.ICommand, IUsageProvider
    {
        public string Command { get; } = "spawnprim";
        public string[] Aliases { get; } = new[] { "sp" };
        public string Description { get; } = "Spawns a primitive at a specified distance from the player.";
        public string[] Usage { get; } = new[] {"x", "y", "z", "r", "g", "b", "opacity"};
        public Config config = new Config();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);

            if (arguments.Count < 1)
            {
                response = "Usage:\n(x), (y), (z), (r), (g), (b), (opacity)";
                return false;
            }

            if (arguments.Count < 7)
            {
                response = "Error: insufficient arguments, please check your input";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float x))
            {
                response = $"Invalid value for x size: {arguments.At(0)}";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float y))
            {
                response = $"Invalid value for y size: {arguments.At(1)}";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float z))
            {
                response = $"Invalid value for z size: {arguments.At(2)}";
                return false;
            }

            if (!float.TryParse(arguments.At(3), out float r))
            {
                response = $"Invalid value for red: {arguments.At(3)}";
                return false;
            }

            if (!float.TryParse(arguments.At(4), out float g))
            {
                response = $"Invalid value for blue: {arguments.At(4)}";
                return false;
            }

            if (!float.TryParse(arguments.At(5), out float b))
            {
                response = $"Invalid value for green: {arguments.At(5)}";
                return false;
            }

            if (!float.TryParse(arguments.At(6), out float op))
            {
                response = $"Invalid value for opacity: {arguments.At(6)}";
                return false;
            }

            Vector3 size = new Vector3(x, y, z);

            // Converting user input (RGB, 0-255) into Color object-acceptable values (0f-1f)
            Color color = new Color(r / 255f, g / 255f, b / 255f, op);

            // Getting player's position and adding to the z-axis a configurable value
            Vector3 position = ( playerSender.Position + new Vector3(0f, 0f, config.PrimitiveDistance) );

            Primitive.Create(position, null, size, true, color); // Position, rotation, scale, spawn, color (for some reason it doesn't accept color=color, etc.)

            response = "Primitive successfuly created!";
            return true;

         }
    }
}