using UnityEngine;
using UnityEngine.InputSystem;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.Tests
{
    public class TimingSystemTester : MonoBehaviour
    {
        [SerializeField] private CombatUIManager combatUI;
        [SerializeField] private KeyCode attackKey = KeyCode.Space;

        private void Start()
        {
            // Start first timing window
            combatUI.StartTimingWindow();
        }

        private void Update()
        {
            if (Input.GetKeyDown(attackKey))
            {
                combatUI.HandleTimingInput();
                combatUI.StartTimingWindow(); // Start next window immediately
            }
        }
    }
}
