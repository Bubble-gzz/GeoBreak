using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Simulation;
using Game.Util;

namespace Game.Core
{
    public class PrimaryWeapon : SimMonobehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireCooldown = 0.1f;
        private float fireCooldownTimer;
        private Vector2 aimDir;
        public void UpdateFireCooldown(float deltaTime)
        {
            if (fireCooldownTimer > 0f)
            {
                fireCooldownTimer = Mathf.Max(0f, fireCooldownTimer - deltaTime);
            }
        }
        public void UpdateAimDirection(Vector2 aimDir)
        {
            if (aimDir.sqrMagnitude == 0f) return;
            this.aimDir = aimDir;
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        public void Fire()
        {
            if (fireCooldownTimer > 0f) return;
            if (bulletPrefab == null) {
                this.LogError("Bullet prefab is not assigned", true);
                return;
            }
            if (simWorld == null) {
                this.LogError("SimWorld is not assigned", true);
                return;
            }
            Transform spawnPoint = firePoint == null ? transform : firePoint;
            GameObject bullet = SimInstantiate(simWorld, bulletPrefab, spawnPoint.position, spawnPoint.rotation, null);
            fireCooldownTimer = fireCooldown;
        }

        override public void SerializeState(StateWriter writer)
        {
            writer.WriteFloat(fireCooldownTimer);
            writer.WriteVector2(aimDir);
        }

        override public void DeserializeState(StateReader reader)
        {
            fireCooldownTimer = reader.ReadFloat();
            UpdateAimDirection(reader.ReadVector2());
        }
    }
}