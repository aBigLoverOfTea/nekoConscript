namespace Buttplug.Commands {
    using System;
    using System.Security;
    using System.Windows.Input;
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using CommandSystem;
    using UnityEngine;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawnprim : CommandSystem.ICommand, IUsageProvider
    {
        public string Command { get; } = "spawnprim";

        public string[] Aliases { get; } = new[] { "sp" };

        public string Description { get; } = "Spawns a primitive at a specific distance in huberts from the player.";

        public string[] Usage { get; } = new[] {"opacity"};

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var playerSender = Player.Get(sender);

            if (playerSender == null)
            {
                response = "Error: could not find the command sender.";
                return false;
            }

            var playerSenderHasPrimitive = PrimitiveController.Instance.PlayerHasPrimitive(playerSender);
            
            if (playerSenderHasPrimitive)
            {
                if (!PrimitiveController.Instance.RemovePlayer(playerSender))
                {
                    response = "You don't meet the requirements for destroying the primitive";
                    return false;
                };

                response = "Your primitive was destroyed successfully!";
                return true;
            }
            else
            {
                if (arguments.Count < 1)
                {
                    response = "Usage:\n(opacity)";
                    return false;
                }

                if (!float.TryParse(arguments.At(0), out float op))
                {
                    response = $"Invalid value for opacity: {arguments.At(0)}";
                    return false;
                }

                if (!PrimitiveController.Instance.AddPlayer(playerSender, op))
                {
                    response = "You don't meet the requirements for creating the primitive";
                    return false;
                };

                response = "Your primitive was created successfuly!";
                return true;
            }
         }
    }
}