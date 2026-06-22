using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Simulation;
namespace Game.Core
{
    public class AttackModule : SimMonobehaviour
    {
        [SerializeField] private PrimaryWeapon primaryWeapon;
        Vector2 dir;

        override public void Init()
        {
            AutoFillSimObjectField(ref primaryWeapon, autoAdd: false);
        }

        public void UpdateFireCooldown(float deltaTime)
        {
            if (primaryWeapon != null) primaryWeapon.UpdateFireCooldown(deltaTime);
        }
        public void UpdateDir(Vector2 newDir)
        {
            dir = newDir;
            if (primaryWeapon != null) primaryWeapon.UpdateAimDirection(dir);
        }
        public void Fire()
        {
            if (primaryWeapon == null) return;
            primaryWeapon.Fire();
        }

        override public void SerializeState(StateWriter writer)
        {
            writer.WriteVector2(dir);
        }

        override public void DeserializeState(StateReader reader)
        {
            dir = reader.ReadVector2();
        }
        override public void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendVector2(sb, "dir", dir);
        }
    }
}