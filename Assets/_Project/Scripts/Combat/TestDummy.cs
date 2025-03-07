using UnityEngine;
using TMPro;

namespace RoguelikeCombat.Combat
{
    public class TestDummy : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private float textDisplayDuration = 1f;
        [SerializeField] private Vector3 textOffset = new Vector3(0, 2f, 0);
        
        private void OnEnable()
        {
            if (damageText != null)
            {
                damageText.gameObject.SetActive(false);
            }
        }

        public void TakeDamage(float damage, TimingResult timingResult)
        {
            if (damageText != null)
            {
                damageText.text = $"{damage:F1}";
                damageText.color = GetDamageColor(timingResult);
                damageText.transform.position = transform.position + textOffset;
                damageText.gameObject.SetActive(true);

                // Hide the text after duration
                Invoke(nameof(HideDamageText), textDisplayDuration);
            }
        }

        private void HideDamageText()
        {
            if (damageText != null)
            {
                damageText.gameObject.SetActive(false);
            }
        }

        private Color GetDamageColor(TimingResult result)
        {
            switch (result)
            {
                case TimingResult.Perfect:
                    return Color.green;
                case TimingResult.Good:
                    return Color.yellow;
                case TimingResult.Normal:
                    return Color.white;
                default:
                    return Color.gray;
            }
        }
    }
}
