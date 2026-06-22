using UnityEngine;
using Game.Util;
using Game.Simulation;
using System.Text;
using Game.Render;
namespace Game.Core
{
    public class PlayerController : SimMonobehaviour
    {
        override public int tickOrder { get => TickOrder.ControlOrder; }
        [SerializeField] private MoveModule moveModule;
        [SerializeField] private AttackModule attackModule;
        [SerializeField] private PlayerRenderer playerRenderer;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private bool continuousFire = false;
        private bool isDashCoolingDown = false;
        private float dashCooldownTimer;
        private bool skipCoastDampThisTick = false;
        int playerId;
        override public void Init()
        {   
            isDashCoolingDown = false;
        }
        override public void Tick(TickContext tickCtx)
        {
            BeforeTick(tickCtx);
            HandleInput(tickCtx.deltaTime, tickCtx.inputDatas[playerId], tickCtx.gameSettings[playerId].playerKeyBindSystem);
            AfterTick(tickCtx);
        }
        private void BeforeTick(TickContext tickCtx)
        {
            if (attackModule != null) attackModule.UpdateFireCooldown(tickCtx.deltaTime);
            UpdateDashCooldown(tickCtx.deltaTime);
            skipCoastDampThisTick = false;
        }
        private void UpdateDashCooldown(float deltaTime)
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
            bool isFireKeyHolding = inputData.GetKey[keyBindSystem.GetKeyCode(PlayerKey.Fire)];
            HandleAttackInput(inputData.mouseWorldPosition, isFireKeyDown, isFireKeyHolding);
        }
        private void HandleMoveInput(float deltaTime, Vector2 inputDir, bool isDashKeyDown)
        {
            if (moveModule == null) {
                this.LogError("MoveModule is not assigned");
                return;
            }
            if (inputDir.sqrMagnitude != 0) {
                skipCoastDampThisTick = true;
                moveModule.ApplyMoveDir(inputDir, deltaTime);
            }
            if (isDashKeyDown) {
                if (isDashCoolingDown) return;
                moveModule.ApplyDash(inputDir);
                isDashCoolingDown = true;
                dashCooldownTimer = 0f;
            }
        }

        private void HandleAttackInput(Vector2 mouseWorldPosition, bool isFireKeyDown, bool isFireKeyHolding)
        {
            Vector2 aimDir = mouseWorldPosition - (Vector2)transform.position;
            if (attackModule != null) attackModule.UpdateDir(aimDir);
            if (isFireKeyDown || (continuousFire && isFireKeyHolding)) {
                if (attackModule == null) {
                    this.LogError("AttackModule is not assigned", true);
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
        private void AfterTick(TickContext tickCtx)
        {
            if (moveModule != null) moveModule.UpdateVelocity(tickCtx.deltaTime, skipCoastDampThisTick);
        }
        public override void SerializeState(StateWriter writer)
        {
            writer.WriteBool(isDashCoolingDown);
            writer.WriteFloat(dashCooldownTimer);
        }
        public override void DeserializeState(StateReader reader)
        {
            isDashCoolingDown = reader.ReadBool();
            dashCooldownTimer = reader.ReadFloat();
        }

        public override void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendBool(sb, "isDashCoolingDown", isDashCoolingDown);
            StateSnapshotFormat.AppendFloat(sb, "dashCooldownTimer", dashCooldownTimer);
        }
        public override void Render(float deltaTime)
        {
            if (playerRenderer != null) playerRenderer.Render(new PlayerRendererData(transform, attackModule.aimAngle, transform.localScale), deltaTime);
        }
    }
}
