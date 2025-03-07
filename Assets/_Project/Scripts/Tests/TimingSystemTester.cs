using UnityEngine;
using UnityEngine.UI;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.Tests
{
    public class TimingSystemTester : MonoBehaviour
    {
        private TimingBarUI timingBar;
        private float lastHitTime;
        private const float CHAIN_BREAK_DELAY = 1.5f;
        private const float WINDOW_DURATION = 2f;
        private int currentChain;

        private void Start()
        {
            // Create canvas if it doesn't exist
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // Generate timing bar prefab using a temporary GameObject
            GameObject tempObj = new GameObject("TempPrefabGenerator");
            var generator = tempObj.AddComponent<Utils.PrefabGenerator>();
            generator.GenerateTimingBarPrefab();
            DestroyImmediate(tempObj);

            // Load and instantiate timing bar
            GameObject timingBarPrefab = Resources.Load<GameObject>("UI/TimingBar");
            if (timingBarPrefab == null)
            {
                Debug.LogError("Failed to load TimingBar prefab!");
                return;
            }

            GameObject timingBarObj = Instantiate(timingBarPrefab, canvas.transform);
            timingBar = timingBarObj.GetComponent<TimingBarUI>();

            // Create test button
            GameObject buttonObj = new GameObject("TestButton", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObj.transform.SetParent(canvas.transform, false);

            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0f);
            buttonRect.anchorMax = new Vector2(0.5f, 0f);
            buttonRect.pivot = new Vector2(0.5f, 0f);
            buttonRect.sizeDelta = new Vector2(160, 30);
            buttonRect.anchoredPosition = new Vector2(0, 20);

            Image buttonImage = buttonObj.GetComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.2f);

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(OnTestButtonClick);

            // Add button text
            GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TMPro.TextMeshProUGUI text = textObj.GetComponent<TMPro.TextMeshProUGUI>();
            text.text = "Test Hit";
            text.color = Color.white;
            text.alignment = TMPro.TextAlignmentOptions.Center;
            text.fontSize = 16;

            // Start first timing window
            StartNewTimingWindow();
        }

        private void OnTestButtonClick()
        {
            float timeSinceLastHit = Time.time - lastHitTime;
            
            // Check for chain break
            if (timeSinceLastHit > CHAIN_BREAK_DELAY)
            {
                currentChain = 0;
                Debug.Log("Chain broken!");
            }

            // Get timing result
            float result = timingBar.GetTimingResult();
            
            // Evaluate timing
            if (result <= 0.2f) // Perfect hit
            {
                currentChain++;
                Debug.Log($"Perfect hit! Chain: {currentChain}");
            }
            else if (result <= 0.4f) // Good hit
            {
                currentChain++;
                Debug.Log($"Good hit! Chain: {currentChain}");
            }
            else // Miss
            {
                currentChain = 0;
                Debug.Log("Miss! Chain broken.");
            }

            // Start next window
            StartNewTimingWindow();
            lastHitTime = Time.time;
        }

        private void StartNewTimingWindow()
        {
            timingBar.StartTimingWindow(WINDOW_DURATION);
        }
    }
}
