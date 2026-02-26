using UnityEngine;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Enums;
using System.Collections.Generic;

namespace Buttplug
{
    public class PrimitiveController : MonoBehaviour
    {
        public static PrimitiveController Instance { get; set; }

        private Dictionary<Player, Primitive> players { get; set; } = new Dictionary<Player, Primitive>();

        public bool PlayerHasPrimitive(Player localPlayer)
        {
            return players.ContainsKey(localPlayer);
        }

        public bool AddPlayer(Player localPlayer, float opacity)
        {
            if (!playerChecks(localPlayer) || PlayerHasPrimitive(localPlayer))
            {
                return false;
            }

            spawnPrimitive(localPlayer, opacity);

            return true;
        }

        public bool RemovePlayer(Player localPlayer)
        {
            if (!PlayerHasPrimitive(localPlayer))
            {
                return false;
            }

            players[localPlayer].Destroy();

            players.Remove(localPlayer);

            return true;
        }

        private void Update()
        {
            if (!(players.Count < 1))
            {
                foreach (var pair in players)
                {
                    if (playerChecks(pair.Key))
                    {
                        spawnPrimitive(pair.Key);
                    }
                    else
                    {
                        RemovePlayer(pair.Key);
                    }
                }
            }
            
        }

        private void Awake()
        {
            Instance = this;
            Log.Warn("Awake");
        }

        private void OnDestroy()
        {
            Instance = null;
            Log.Warn("OnDestroy");
        }
        
        private bool playerChecks(Player localPlayer) {
            return localPlayer != null && localPlayer.IsAlive && localPlayer.IsConnected;
        }

        private void spawnPrimitive(Player localPlayer, float opacity)
        {
            Vector3 primRotation;
            Vector3 primPosition;
            Color primColor;

            var environmentMask = (int)(
                LayerMasks.Default |
                LayerMasks.Door |
                LayerMasks.OnlyWorldCollision
            );

            if (Physics.Raycast(localPlayer.CameraTransform.position,
                localPlayer.CameraTransform.forward,
                out RaycastHit hit,
                ButtplugPlugin.Instance.Config.PrimitiveDistance + ButtplugPlugin.Instance.Config.PrimitiveScale.x,
                environmentMask))
            {
                primPosition = hit.point;

                Vector3 upDirection = Vector3.ProjectOnPlane(Vector3.up, hit.normal).normalized;

                if (upDirection.sqrMagnitude < 0.01f) // false if object hits the a wall, true if ceiling or floor
                {
                    upDirection = Vector3.ProjectOnPlane(Vector3.forward, hit.normal).normalized;
                }

                primRotation = Quaternion.LookRotation(hit.normal, upDirection).eulerAngles;

                var stepSize = 0.05f;

                while (Physics.CheckSphere(primPosition, ButtplugPlugin.Instance.Config.PrimitiveScale.x * 0.5f, environmentMask))
                {
                    primPosition += hit.normal * stepSize;
                }

                primColor = ButtplugPlugin.Instance.Config.PrimitiveWallTouchColor;
            }
            else {
                primPosition = localPlayer.CameraTransform.position + (localPlayer.CameraTransform.forward * ButtplugPlugin.Instance.Config.PrimitiveDistance);

                primRotation = new Vector3();

                primColor = ButtplugPlugin.Instance.Config.PrimitiveDefaultColor;
            }

            if (!PlayerHasPrimitive(localPlayer))
            {
                var localPrimitive = Primitive.Create(PrimitiveType.Sphere,
                                                    primPosition,
                                                    primRotation,
                                                    ButtplugPlugin.Instance.Config.PrimitiveScale,
                                                    true,
                                                    new Color(primColor.r, primColor.g, primColor.b, opacity));
                
                localPrimitive.Collidable = false;

                players.Add(localPlayer, localPrimitive);
            }
            else {
                players[localPlayer].Position = primPosition;
                players[localPlayer].Rotation = Quaternion.Euler(primRotation.x, primRotation.y, primRotation.z);
                players[localPlayer].Color = primColor;
            }
            
        }

        private void spawnPrimitive(Player localPlayer)
        {
            float opacity;

            if (players.ContainsKey(localPlayer))
            {
                opacity = players[localPlayer].Color.a;
            }
            else
            {
                opacity = 1f;
            }

            spawnPrimitive(localPlayer, opacity);
        }
    }
}