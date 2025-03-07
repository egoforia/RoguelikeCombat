using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace RoguelikeCombat.UI
{
    public class ChainMeterUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private RectTransform segmentContainer;

        [Header("Layout Settings")]
        [SerializeField] private float segmentSpacing = 10f;
        [SerializeField] private float segmentSize = 30f;

        [Header("Animation Settings")]
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private Ease pulseEase = Ease.OutBack;
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = Color.gray;

        private List<RectTransform> segments = new List<RectTransform>();
        private List<Image> segmentImages = new List<Image>();
        private List<Sequence> segmentAnimations = new List<Sequence>();
        private Vector3 originalScale;
        private Vector3 pulseScaleVector;
        private int maxSegments;

        private void Awake()
        {
            originalScale = Vector3.one;
            pulseScaleVector = Vector3.one * pulseScale;
        }

        public void Initialize(int maxChainLength)
        {
            maxSegments = maxChainLength;
            ClearSegments();

            // Create all segments in inactive state
            for (int i = 0; i < maxSegments; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, segmentContainer);
                RectTransform segmentTransform = segment.GetComponent<RectTransform>();
                Image segmentImage = segment.GetComponent<Image>();

                // Configure segment
                segmentTransform.sizeDelta = new Vector2(segmentSize, segmentSize);
                segmentTransform.anchoredPosition = new Vector2(i * (segmentSize + segmentSpacing), 0);
                segmentImage.color = inactiveColor;

                segments.Add(segmentTransform);
                segmentImages.Add(segmentImage);
                segmentAnimations.Add(null);
            }
        }

        public void AddSegment()
        {
            int nextIndex = segments.FindIndex(s => s.GetComponent<Image>().color == inactiveColor);
            if (nextIndex >= 0)
            {
                ActivateSegment(nextIndex);
            }
        }

        public void ResetChain()
        {
            foreach (var sequence in segmentAnimations)
            {
                sequence?.Kill();
            }
            segmentAnimations.Clear();

            for (int i = 0; i < segments.Count; i++)
            {
                segmentImages[i].color = inactiveColor;
                segments[i].localScale = originalScale;
                segmentAnimations.Add(null);
            }
        }

        private void ActivateSegment(int index)
        {
            // Kill any existing animation
            segmentAnimations[index]?.Kill();

            // Get references
            RectTransform segmentTransform = segments[index];
            Image segmentImage = segmentImages[index];

            // Create animation sequence
            segmentAnimations[index] = DOTween.Sequence();

            // Color transition
            segmentAnimations[index].Join(segmentImage.DOColor(activeColor, animationDuration));

            // Scale animation
            segmentAnimations[index].Append(segmentTransform.DOScale(pulseScaleVector, animationDuration / 2)
                .SetEase(pulseEase));
            segmentAnimations[index].Append(segmentTransform.DOScale(originalScale, animationDuration / 2)
                .SetEase(Ease.OutQuad));
        }

        private void ClearSegments()
        {
            // Kill all animations
            foreach (var sequence in segmentAnimations)
            {
                sequence?.Kill();
            }

            // Clear lists
            segments.Clear();
            segmentImages.Clear();
            segmentAnimations.Clear();

            // Destroy existing segments
            foreach (Transform child in segmentContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnDestroy()
        {
            foreach (var sequence in segmentAnimations)
            {
                sequence?.Kill();
            }
        }
    }
}
