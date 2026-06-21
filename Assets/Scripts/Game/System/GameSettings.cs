using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
namespace Game.System
{
    public class GameSettings
    {
        private static GameSettings _localInstance;
        public static GameSettings LocalInstance {
            get {
                if (_localInstance == null)
                {
                    _localInstance = new GameSettings();
                }
                return _localInstance;
            }
            set {
                _localInstance = value;
            }
        }
        
        #region Player Key Bind System
        private KeyBindSystem<PlayerKey> _playerKeyBindSystem;
        private KeyBindSystem<PlayerKey> DefaultPlayerKeyBindSystem()
        {
            KeyBindSystem<PlayerKey> keyBindSystem = new KeyBindSystem<PlayerKey>();
            keyBindSystem.AddKeyBind(PlayerKey.Dash, KeyCode.Space);
            keyBindSystem.AddKeyBind(PlayerKey.Fire, KeyCode.Mouse1);
            return keyBindSystem;
        }
        public KeyBindSystem<PlayerKey> playerKeyBindSystem
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