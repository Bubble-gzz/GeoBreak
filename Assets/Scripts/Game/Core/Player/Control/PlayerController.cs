using UnityEngine;
using Game.Simulation;
namespace Game.Core.Control
{
    [TickOrder(TickOrder.ControlOrder)]
    public class PlayerController : SimulatedMonobehaviour
    {
        [SerializeField] private MoveModule moveModule;
        [SerializeField] private AttackModule attackModule;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private bool isDashCoolingDown = false;
        [SerializeField] private float dashCooldownTimer;
        int playerId;
        override public void Init()
        {   
            isDashCoolingDown = false;
        }
        override public void Tick(TickContext tickCtx)
        {
            HandleInput(tickCtx.deltaTime, tickCtx.inputDatas[playerId], tickCtx.gameSettings[playerId].playerKeyBindSystem);
            DashCooldownTick(tickCtx.deltaTime);
        }
        private void DashCooldownTick(float deltaTime)
        {
            if (!isDashCoolingDown) return;
            dashCooldownTimer += deltaTime;
            if (dashCooldownTimer >= dashCooldown) {
                isDashCoolingDown = false;
                dashCooldownTimer = 0f;
            }
        }
        private void HandleInput(float deltaTime, InputData inputData, KeyBindSystem<PlayerKey> keyBindSystem)
        {
            bool isDashKeyDown = inputData.GetKeyDown[keyBindSystem.GetKeyCode(PlayerKey.Dash)];
            HandleMoveInput(deltaTime, GetInputDir(inputData), isDashKeyDown);
            bool isFireKeyDown = inputData.GetKeyDown[keyBindSystem.GetKeyCode(PlayerKey.Fire)];
            //HandleAttackInput(isFireKeyDown);
        }
        private void HandleMoveInput(float deltaTime, Vector2 inputDir, bool isDashKeyDown)
        {
            if (moveModule == null) {
                LogError("MoveModule is not assigned");
                return;
            }
            moveModule.ApplyMoveDir(inputDir, deltaTime);
            if (isDashKeyDown) {
                if (isDashCoolingDown) return;
                moveModule.ApplyDash(inputDir);
                isDashCoolingDown = true;
                dashCooldownTimer = 0f;
            }
        }

        private void HandleAttackInput(bool isFireKeyDown)
        {
            if (isFireKeyDown) {
                if (attackModule == null) {
                    LogError("AttackModule is not assigned");
                    return;
                }
                attackModule.Fire();
            }
        }
        Vector2 GetInputDir(InputData inputData)
        {
            float moveHorizontal = inputData.GetAxisRaw["Horizontal"];
            float moveVertical = inputData.GetAxisRaw["Vertical"];
            return new Vector2(moveHorizontal, moveVertical).normalized;
        }
        private void LogError(string message)
        {
            Debug.LogError($"[{name}.PlayerController] " + message);
        }
    }
}
