using System.ComponentModel;
using UnityEngine;
using Color = UnityEngine.Color;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace Buttplug
{
    public class Config : IConfig
    {
        [Description("Включён ли плагин")]
        public bool IsEnabled { get; set; } = true;

        [Description("Включён ли режим отладки")]
        public bool Debug { get; set; } = false;

        [Description("Дистанция от игрока до примитива")]
        public float PrimitiveDistance { get; set; } = 6f;

        [Description("Множитель размера примитива по всем осям (размер)")]
        public Vector3 PrimitiveScale { get; set; } = new Vector3(1f, 1f, 1f);

        [Description("Цвет примитива если он касается стены")]
        public Color PrimitiveWallTouchColor = new Color(1f, 0f, 0f); // RGB 0-1

        [Description("Цвет примитива если он НЕ касается стены")]
        public Color PrimitiveDefaultColor = new Color(0f, 1f, 0f);  // RGB 0-1
     }
}