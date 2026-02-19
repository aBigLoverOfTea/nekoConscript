using System.ComponentModel;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace Buttplug
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public float PrimitiveDistance { get; set; } = 6f;
    }
}