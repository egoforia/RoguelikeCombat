using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace RoguelikeCombat.UI
{
    public class ChainMeterUI : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private float segmentSize = 30f;
        [SerializeField] private float segmentSpacing = 10f;
        [SerializeField] private Color activeColor = Color.yellow;
        [SerializeField] private Color inactiveColor = Color.gray;
        [SerializeField] private float animationDuration = 0.2f;

        [Header("Chain Settings")]
        [SerializeField] private int maxSegments = 5;
        [SerializeField] private float chainBreakDelay = 1.5f;

        private List<RectTransform> segments = new List<RectTransform>();
        private List<Image> segmentImages = new List<Image>();
        private List<Tweener> segmentAnimations = new List<Tweener>();
        private RectTransform segmentContainer;
        private GameObject segmentPrefab;
        private int currentChain;
        private float lastHitTime;

        private void Awake()
        {
            // Create container for segments
            GameObject containerObj = new GameObject("SegmentContainer", typeof(RectTransform));
            containerObj.transform.SetParent(transform, false);
            segmentContainer = containerObj.GetComponent<RectTransform>();
            segmentContainer.anchorMin = new Vector2(0.5f, 0.5f);
            segmentContainer.anchorMax = new Vector2(0.5f, 0.5f);
            segmentContainer.pivot = new Vector2(0.5f, 0.5f);

            // Generate and load segment prefab
            var generator = gameObject.AddComponent<Utils.PrefabGenerator>();
            generator.GenerateChainSegmentPrefab();
            Destroy(generator);

            segmentPrefab = Resources.Load<GameObject>("UI/ChainSegment");
            if (segmentPrefab == null)
            {
                Debug.LogError("Failed to load ChainSegment prefab!");
                return;
            }

            // Initialize segments
            Initialize(maxSegments);
        }

        private void Update()
        {
            // Check for chain break
            if (currentChain > 0 && Time.time - lastHitTime > chainBreakDelay)
            {
                BreakChain();
            }
        }

        public void Initialize(int maxChainLength)
        {
            maxSegments = maxChainLength;
            ClearSegments();

            // Calculate total width
            float totalWidth = maxSegments * (segmentSize + segmentSpacing) - segmentSpacing;
            segmentContainer.sizeDelta = new Vector2(totalWidth, segmentSize);

            // Create segments
            for (int i = 0; i < maxSegments; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, segmentContainer);
                RectTransform segmentTransform = segment.GetComponent<RectTransform>();
                Image segmentImage = segment.GetComponent<Image>();

                segmentTransform.sizeDelta = new Vector2(segmentSize, segmentSize);
                segmentTransform.anchoredPosition = new Vector2(i * (segmentSize + segmentSpacing), 0);
                segmentImage.color = inactiveColor;

                segments.Add(segmentTransform);
                segmentImages.Add(segmentImage);
                segmentAnimations.Add(null);
            }
        }

        public void AddHit(float hitQuality)
        {
            // Reset chain if too much time has passed
            if (Time.time - lastHitTime > chainBreakDelay && currentChain > 0)
            {
                BreakChain();
            }

            // Add to chain if hit was good enough
            if (hitQuality <= 0.4f) // Good or Perfect hit
            {
                currentChain = Mathf.Min(currentChain + 1, maxSegments);
                UpdateVisuals();
            }
            else // Miss
            {
                BreakChain();
            }

            lastHitTime = Time.time;
        }

        private void UpdateVisuals()
        {
            for (int i = 0; i < segments.Count; i++)
            {
                bool isActive = i < currentChain;
                Color targetColor = isActive ? activeColor : inactiveColor;

                // Kill existing animation
                segmentAnimations[i]?.Kill();

                // Animate color change
                segmentAnimations[i] = segmentImages[i].DOColor(targetColor, animationDuration)
                    .SetEase(Ease.OutQuad);

                // Animate scale
                segments[i].DOScale(isActive ? 1.2f : 1f, animationDuration)
                    .SetEase(isActive ? Ease.OutBack : Ease.OutQuad);
            }
        }

        private void BreakChain()
        {
            if (currentChain > 0)
            {
                currentChain = 0;
                UpdateVisuals();
            }
        }

        private void ClearSegments()
        {
            foreach (var segment in segments)
            {
                if (segment != null)
                {
                    Destroy(segment.gameObject);
                }
            }

            foreach (var anim in segmentAnimations)
            {
                anim?.Kill();
            }

            segments.Clear();
            segmentImages.Clear();
            segmentAnimations.Clear();
            currentChain = 0;
        }

        private void OnDestroy()
        {
            foreach (var anim in segmentAnimations)
            {
                anim?.Kill();
            }
        }
    }
}
