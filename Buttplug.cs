using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;

using ServerHandler = Exiled.Events.Handlers.Server;

namespace Buttplug
{
    public class ButtplugPlugin : Plugin<Config>
    {
        private EventHandlers eventHandlers;

        public override string Name => "Buttplug Plugin";
        public override string Author => "zaza";
        public override void OnEnabled()
        {
            if (!Config.IsEnabled)
                return;

            eventHandlers = new EventHandlers();
            ServerHandler.RoundStarted += eventHandlers.OnRoundStarted;
            Log.Warn("This shit works my nigga, but now cooler");
        }

        public override void OnDisabled()
        {
            Log.Warn("This shit shutdowns my nigga, but now cooler");
            ServerHandler.RoundStarted -= eventHandlers.OnRoundStarted;
            eventHandlers = null;
        }
    }
}