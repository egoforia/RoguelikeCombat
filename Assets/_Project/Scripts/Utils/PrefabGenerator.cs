using UnityEngine;
using UnityEngine.UI;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.Utils
{
    public class PrefabGenerator : MonoBehaviour
    {
        private const string PREFAB_PATH = "Assets/_Project/Prefabs/UI/";
        private const string RESOURCES_PATH = "Assets/_Project/Resources/UI/";

        public void GenerateTimingBarPrefab()
        {
            // Create root object
            GameObject timingBarObj = new GameObject("TimingBar", typeof(RectTransform), typeof(TimingBarUI));
            RectTransform timingBarRect = timingBarObj.GetComponent<RectTransform>();
            TimingBarUI timingBarUI = timingBarObj.GetComponent<TimingBarUI>();

            // Set RectTransform properties
            timingBarRect.anchorMin = new Vector2(0.5f, 0.5f);
            timingBarRect.anchorMax = new Vector2(0.5f, 0.5f);
            timingBarRect.pivot = new Vector2(0.5f, 0.5f);
            timingBarRect.sizeDelta = new Vector2(300f, 30f);

            // Create background
            GameObject bgObj = new GameObject("Background", typeof(RectTransform), typeof(Image));
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            Image bgImage = bgObj.GetComponent<Image>();
            bgObj.transform.SetParent(timingBarObj.transform, false);
            
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Create fill bar
            GameObject fillObj = new GameObject("FillBar", typeof(RectTransform), typeof(Image));
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            Image fillImage = fillObj.GetComponent<Image>();
            fillObj.transform.SetParent(timingBarObj.transform, false);
            
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(0f, 1f);
            fillRect.pivot = new Vector2(0f, 0.5f);
            fillRect.sizeDelta = new Vector2(0f, 0f);
            fillImage.color = Color.white;

            // Create good zone
            GameObject goodObj = new GameObject("GoodZone", typeof(RectTransform), typeof(Image));
            RectTransform goodRect = goodObj.GetComponent<RectTransform>();
            Image goodImage = goodObj.GetComponent<Image>();
            goodObj.transform.SetParent(timingBarObj.transform, false);
            
            goodRect.anchorMin = new Vector2(0.3f, 0f);
            goodRect.anchorMax = new Vector2(0.7f, 1f);
            goodRect.sizeDelta = Vector2.zero;
            goodImage.color = new Color(0f, 1f, 0f, 0.3f);

            // Create perfect zone
            GameObject perfectObj = new GameObject("PerfectZone", typeof(RectTransform), typeof(Image));
            RectTransform perfectRect = perfectObj.GetComponent<RectTransform>();
            Image perfectImage = perfectObj.GetComponent<Image>();
            perfectObj.transform.SetParent(timingBarObj.transform, false);
            
            perfectRect.anchorMin = new Vector2(0.4f, 0f);
            perfectRect.anchorMax = new Vector2(0.6f, 1f);
            perfectRect.sizeDelta = Vector2.zero;
            perfectImage.color = new Color(1f, 1f, 0f, 0.5f);

            // Create marker
            GameObject markerObj = new GameObject("Marker", typeof(RectTransform), typeof(Image));
            RectTransform markerRect = markerObj.GetComponent<RectTransform>();
            Image markerImage = markerObj.GetComponent<Image>();
            markerObj.transform.SetParent(timingBarObj.transform, false);
            
            markerRect.anchorMin = new Vector2(0f, 0f);
            markerRect.anchorMax = new Vector2(0f, 1f);
            markerRect.pivot = new Vector2(0.5f, 0.5f);
            markerRect.sizeDelta = new Vector2(4f, 0f);
            markerImage.color = Color.red;

            // Assign references
            timingBarUI.fillBar = fillRect;
            timingBarUI.perfectZone = perfectRect;
            timingBarUI.goodZone = goodRect;
            timingBarUI.marker = markerRect;

            // Save prefab
            #if UNITY_EDITOR
            if (!System.IO.Directory.Exists(PREFAB_PATH))
            {
                System.IO.Directory.CreateDirectory(PREFAB_PATH);
            }
            if (!System.IO.Directory.Exists(RESOURCES_PATH))
            {
                System.IO.Directory.CreateDirectory(RESOURCES_PATH);
            }

            // Save to Prefabs folder
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(timingBarObj, PREFAB_PATH + "TimingBar.prefab");
            
            // Copy to Resources folder
            System.IO.File.Copy(PREFAB_PATH + "TimingBar.prefab", RESOURCES_PATH + "TimingBar.prefab", true);
            
            DestroyImmediate(timingBarObj);
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }

        public void GenerateChainSegmentPrefab()
        {
            // Create root object with RectTransform and Image
            GameObject segmentObj = new GameObject("ChainSegment", typeof(RectTransform), typeof(Image));
            RectTransform segmentRect = segmentObj.GetComponent<RectTransform>();
            Image segmentImage = segmentObj.GetComponent<Image>();

            // Set RectTransform properties
            segmentRect.anchorMin = new Vector2(0.5f, 0.5f);
            segmentRect.anchorMax = new Vector2(0.5f, 0.5f);
            segmentRect.pivot = new Vector2(0.5f, 0.5f);
            segmentRect.sizeDelta = new Vector2(30f, 30f);

            // Set Image properties
            segmentImage.sprite = GenerateCircleSprite();
            segmentImage.color = Color.white;

            // Save prefab
            #if UNITY_EDITOR
            if (!System.IO.Directory.Exists(PREFAB_PATH))
            {
                System.IO.Directory.CreateDirectory(PREFAB_PATH);
            }
            if (!System.IO.Directory.Exists(RESOURCES_PATH))
            {
                System.IO.Directory.CreateDirectory(RESOURCES_PATH);
            }

            // Save to Prefabs folder
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(segmentObj, PREFAB_PATH + "ChainSegment.prefab");
            
            // Copy to Resources folder
            System.IO.File.Copy(PREFAB_PATH + "ChainSegment.prefab", RESOURCES_PATH + "ChainSegment.prefab", true);
            
            DestroyImmediate(segmentObj);
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }

        private Sprite GenerateCircleSprite()
        {
            const int TEXTURE_SIZE = 32;
            const int CIRCLE_RADIUS = 14;
            
            Texture2D texture = new Texture2D(TEXTURE_SIZE, TEXTURE_SIZE);
            Color[] colors = new Color[TEXTURE_SIZE * TEXTURE_SIZE];
            Vector2 center = new Vector2(TEXTURE_SIZE / 2f, TEXTURE_SIZE / 2f);
            
            for (int x = 0; x < TEXTURE_SIZE; x++)
            {
                for (int y = 0; y < TEXTURE_SIZE; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    colors[y * TEXTURE_SIZE + x] = distance <= CIRCLE_RADIUS ? Color.white : Color.clear;
                }
            }
            
            texture.SetPixels(colors);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, TEXTURE_SIZE, TEXTURE_SIZE), new Vector2(0.5f, 0.5f), 100f);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("RoguelikeCombat/Generate UI Prefabs")]
        public static void GenerateUIPrefabs()
        {
            // Create a temporary GameObject to hold the PrefabGenerator
            GameObject tempObj = new GameObject("TempPrefabGenerator");
            PrefabGenerator generator = tempObj.AddComponent<PrefabGenerator>();
            
            // Generate prefabs
            generator.GenerateTimingBarPrefab();
            generator.GenerateChainSegmentPrefab();
            
            // Clean up
            DestroyImmediate(tempObj);
        }
#endif
    }
}
