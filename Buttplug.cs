using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using UnityEngine;

namespace Buttplug
{

    using Buttplug.Commands;

    public class ButtplugPlugin : Plugin<Config>
    {
        public override string Name => "Buttplug Plugin";

        public override string Author => "zaza";

        public static ButtplugPlugin Instance { get; private set; }

        public override void OnEnabled()
        {
            if (!Config.IsEnabled)
                return;

            GameObject manager = new GameObject("UniversalManager");

            manager.AddComponent<PrimitiveController>();

            UnityEngine.Object.DontDestroyOnLoad(manager);

            Instance = this;

            Log.Warn("This shit works my nigga");
        }

        public override void OnDisabled()
        {
            Log.Warn("This shit shutdowns my nigga");

            GameObject.Destroy(GameObject.Find("UniversalManager"));

            Instance = null;
        }
    }
}