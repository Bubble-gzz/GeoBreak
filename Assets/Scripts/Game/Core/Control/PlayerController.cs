using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
namespace Game.Core.Control
{
    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private MoveModule moveModule;
        KeyBindSystem<PlayerKey> keyBindSystem => GameSettings.playerKeyBindSystem;
        KeyCode dashKey => keyBindSystem.GetKeyCode(PlayerKey.Dash);
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }
        private void HandleInput()
        {
            HandleMoveInput();
            HandleAttackInput();
        }
        private void HandleMoveInput()
        {
            Vector2 inputDir = GetInputDir();
            moveModule.ApplyMoveDir(inputDir);
            if (Input.GetKeyDown(dashKey)) {
                moveModule.ApplyDash(inputDir);
            }
        }
        private void HandleAttackInput()
        {

        }
        Vector2 GetInputDir()
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            return new Vector2(moveHorizontal, moveVertical).normalized;
        }
    }
}
