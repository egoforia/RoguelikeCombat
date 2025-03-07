using UnityEngine;
using UnityEngine.UI;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.Tests
{
    public class CombatUITester : MonoBehaviour
    {
        private CombatUIManager combatUI;
        private Button testButton;
        private float nextWindowDelay = 0.5f;
        private float nextWindowTime;

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

            // Create CombatUIManager
            GameObject combatUIObj = new GameObject("CombatUI");
            combatUIObj.transform.SetParent(canvas.transform, false);
            combatUI = combatUIObj.AddComponent<CombatUIManager>();

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

            testButton = buttonObj.GetComponent<Button>();
            testButton.onClick.AddListener(OnTestButtonClick);

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

            // Create feedback text
            GameObject feedbackObj = new GameObject("FeedbackText", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            feedbackObj.transform.SetParent(canvas.transform, false);

            RectTransform feedbackRect = feedbackObj.GetComponent<RectTransform>();
            feedbackRect.anchorMin = new Vector2(0.5f, 0.5f);
            feedbackRect.anchorMax = new Vector2(0.5f, 0.5f);
            feedbackRect.pivot = new Vector2(0.5f, 0.5f);
            feedbackRect.sizeDelta = new Vector2(200, 50);
            feedbackRect.anchoredPosition = new Vector2(0, 50);

            TMPro.TextMeshProUGUI feedbackText = feedbackObj.GetComponent<TMPro.TextMeshProUGUI>();
            feedbackText.alignment = TMPro.TextAlignmentOptions.Center;
            feedbackText.fontSize = 24;

            // Create chain text
            GameObject chainObj = new GameObject("ChainText", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            chainObj.transform.SetParent(canvas.transform, false);

            RectTransform chainRect = chainObj.GetComponent<RectTransform>();
            chainRect.anchorMin = new Vector2(0.5f, 0.5f);
            chainRect.anchorMax = new Vector2(0.5f, 0.5f);
            chainRect.pivot = new Vector2(0.5f, 0.5f);
            chainRect.sizeDelta = new Vector2(200, 50);
            chainRect.anchoredPosition = new Vector2(0, -50);

            TMPro.TextMeshProUGUI chainText = chainObj.GetComponent<TMPro.TextMeshProUGUI>();
            chainText.alignment = TMPro.TextAlignmentOptions.Center;
            chainText.fontSize = 20;
            chainText.text = "Chain: 0";

            // Start first timing window
            nextWindowTime = Time.time + nextWindowDelay;
        }

        private void Update()
        {
            if (Time.time >= nextWindowTime)
            {
                combatUI.StartTimingWindow();
                nextWindowTime = float.MaxValue;
            }
        }

        private void OnTestButtonClick()
        {
            combatUI.HandleTimingInput();
            nextWindowTime = Time.time + nextWindowDelay;
        }
    }
}
