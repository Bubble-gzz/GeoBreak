using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
namespace Game.Core.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MoveModule moveModule;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private bool isDashCoolingDown = false;
        private IEnumerator DashCooldownCoroutine()
        {
            yield return new WaitForSeconds(dashCooldown);
            isDashCoolingDown = false;
        }
        KeyBindSystem<PlayerKey> keyBindSystem => GameSettings.playerKeyBindSystem;
        KeyCode dashKey => keyBindSystem.GetKeyCode(PlayerKey.Dash);
        void Awake()
        {
            if (moveModule == null) LogError("MoveModule is not assigned");
            isDashCoolingDown = false;
        }

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
                if (isDashCoolingDown) return;
                isDashCoolingDown = true;
                moveModule.ApplyDash(inputDir);
                StartCoroutine(DashCooldownCoroutine());
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
        private void LogError(string message)
        {
            Debug.LogError($"[{name}.PlayerController] " + message);
        }
    }
}
