using UnityEngine;
using UnityEngine.InputSystem;

namespace RoguelikeCombat.Combat
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("Combat References")]
        [SerializeField] private CombatManager combatManager;
        [SerializeField] private TimingSystem timingSystem;

        [Header("Attack Settings")]
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private float attackRange = 2f;

        private bool canAttack = true;
        private float lastAttackTime;

        private void Awake()
        {
            if (combatManager == null)
                combatManager = FindObjectOfType<CombatManager>();
            if (timingSystem == null)
                timingSystem = FindObjectOfType<TimingSystem>();
        }

        public void OnAttack(InputValue value)
        {
            if (value.isPressed)
            {
                TryStartAttack();
            }
        }

        public void OnChain(InputValue value)
        {
            if (value.isPressed && timingSystem.IsWindowActive())
            {
                timingSystem.AttemptTiming();
            }
        }

        private void TryStartAttack()
        {
            if (!canAttack || Time.time - lastAttackTime < attackCooldown)
                return;

            if (IsTargetInRange())
            {
                StartAttack();
            }
        }

        private bool IsTargetInRange()
        {
            // Simple forward raycast check
            Ray ray = new Ray(transform.position, transform.forward);
            return Physics.Raycast(ray, attackRange, targetLayers);
        }

        private void StartAttack()
        {
            lastAttackTime = Time.time;
            canAttack = false;
            timingSystem.StartTimingWindow();
            
            // Reset attack availability after cooldown
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void ResetAttack()
        {
            canAttack = true;
        }

        private void OnDrawGizmosSelected()
        {
            // Visualize attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
