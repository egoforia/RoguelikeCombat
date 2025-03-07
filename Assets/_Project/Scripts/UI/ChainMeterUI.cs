using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RoguelikeCombat.Combat;

namespace RoguelikeCombat.UI
{
    public class ChainMeterUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform segmentsContainer;
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private int maxSegments = 4;

        [Header("Visual Settings")]
        [SerializeField] private Color perfectColor = Color.green;
        [SerializeField] private Color goodColor = Color.yellow;
        [SerializeField] private Color failedColor = Color.red;
        [SerializeField] private float segmentScale = 1f;
        [SerializeField] private float rotationOffset = 90f;
        [SerializeField] private float pulseDuration = 0.2f;
        
        private Image[] segments;
        private int currentChain;

        private void Awake()
        {
            InitializeSegments();
        }

        private void InitializeSegments()
        {
            segments = new Image[maxSegments];
            float angleStep = 360f / maxSegments;

            for (int i = 0; i < maxSegments; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, segmentsContainer);
                segment.transform.localPosition = Vector3.zero;
                
                // Position segments in a circle
                float angle = i * angleStep + rotationOffset;
                segment.transform.localRotation = Quaternion.Euler(0, 0, angle);
                
                segments[i] = segment.GetComponent<Image>();
                segments[i].color = Color.gray;
                segment.transform.localScale = Vector3.one * segmentScale;
            }
        }

        public void UpdateChainVisual(int chainCount, TimingResult result)
        {
            currentChain = Mathf.Min(chainCount, maxSegments);

            // Update active segments
            for (int i = 0; i < maxSegments; i++)
            {
                if (i < currentChain)
                {
                    segments[i].color = GetColorForResult(result);
                    StartCoroutine(PulseSegment(segments[i].gameObject));
                }
                else
                {
                    segments[i].color = Color.gray;
                }
            }
        }

        private Color GetColorForResult(TimingResult result)
        {
            switch (result)
            {
                case TimingResult.Perfect:
                    return perfectColor;
                case TimingResult.Good:
                    return goodColor;
                default:
                    return failedColor;
            }
        }

        private IEnumerator PulseSegment(GameObject segment)
        {
            Vector3 originalScale = segment.transform.localScale;
            Vector3 pulseScale = originalScale * 1.2f;
            float elapsedTime = 0f;

            // Scale up
            while (elapsedTime < pulseDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (pulseDuration / 2);
                segment.transform.localScale = Vector3.Lerp(originalScale, pulseScale, t);
                yield return null;
            }

            elapsedTime = 0f;

            // Scale down
            while (elapsedTime < pulseDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (pulseDuration / 2);
                segment.transform.localScale = Vector3.Lerp(pulseScale, originalScale, t);
                yield return null;
            }

            segment.transform.localScale = originalScale;
        }

        public void ResetChain()
        {
            currentChain = 0;
            foreach (var segment in segments)
            {
                segment.color = Color.gray;
            }
        }
    }
}
