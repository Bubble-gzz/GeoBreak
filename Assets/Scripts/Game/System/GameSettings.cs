using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Control;
namespace Game.System
{
    public static class GameSettings
    {
        #region Player Key Bind System
        private static KeyBindSystem<PlayerKey> _playerKeyBindSystem;
        private static KeyBindSystem<PlayerKey> DefaultPlayerKeyBindSystem()
        {
            KeyBindSystem<PlayerKey> keyBindSystem = new KeyBindSystem<PlayerKey>();
            keyBindSystem.AddKeyBind(PlayerKey.Dash, KeyCode.Space);
            keyBindSystem.AddKeyBind(PlayerKey.Teleport, KeyCode.Mouse1);
            keyBindSystem.AddKeyBind(PlayerKey.Fire, KeyCode.Mouse0);
            return keyBindSystem;
        }
        public static KeyBindSystem<PlayerKey> playerKeyBindSystem
        {
            get
            {
                if (_playerKeyBindSystem == null)
                {
                    _playerKeyBindSystem = DefaultPlayerKeyBindSystem();
                }
                return _playerKeyBindSystem;
            }
        }
        #endregion
    }
}