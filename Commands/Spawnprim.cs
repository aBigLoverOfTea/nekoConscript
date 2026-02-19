namespace Buttplug.Commands {
    using System;
    using System.Security;
    using System.Windows.Input;
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using Primitive = Exiled.API.Features.Toys.Primitive;
    using CommandSystem;
    using UnityEngine;
    using Color = UnityEngine.Color;
    using System.Drawing;

    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawnprim : CommandSystem.ICommand, IUsageProvider
    {
        public Config config = new Config();
        public string Command { get; } = "spawnprim";
        public string[] Aliases { get; } = new[] { "sp" };
        public string Description { get; } = "Spawns a primitive at a specific distance in huberts from the player.";
        public string[] Usage { get; } = new[] {"x", "y", "z", "r", "g", "b", "opacity"};

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
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

            int environmentMask = LayerMask.GetMask("Default");
            
            Color primColor;
            
            Player playerSender = Player.Get(sender);
            Transform camera = playerSender.CameraTransform;

            Vector3 primRotation = new Vector3();
            Vector3 primPosition = camera.position + (camera.forward * config.PrimitiveDistance);
            Vector3 primScale = new Vector3(x, y, z);

            if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, config.PrimitiveDistance, environmentMask))
            {
                primPosition = hit.point;

                Vector3 upDirection = Vector3.ProjectOnPlane(Vector3.up, hit.normal).normalized;
                if (upDirection.sqrMagnitude < 0.01f) // false if object hits the a wall, true if ceiling or floor
                {
                    upDirection = Vector3.ProjectOnPlane(Vector3.forward, hit.normal).normalized;
                }

                primRotation = Quaternion.LookRotation(hit.normal, upDirection).eulerAngles;

                float stepSize = 0.005f;

                while (Physics.CheckSphere(primPosition, x * 0.45f, environmentMask))
                {
                    primPosition += hit.normal * stepSize;
                }

                // Color inputs shift by one position forward if raycast hits a surface
                primColor = new Color(b / 255f, r / 255f, g / 255f, op);
            } else {
                // Converting user input (RGB, 0-255) into Color object-acceptable values (0f-1f)
                primColor = new Color(r / 255f, g / 255f, b / 255f, op);
            }

            Primitive.Create(PrimitiveType.Sphere, primPosition, primRotation, primScale, true, primColor);
            
            response = "Primitive successfuly created!";
            
            return true;

         }
    }
}