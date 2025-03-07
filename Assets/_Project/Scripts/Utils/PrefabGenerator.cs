using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RoguelikeCombat.Utils
{
    public class PrefabGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("RoguelikeCombat/Generate UI Prefabs")]
        public static void GenerateUIPrefabs()
        {
            CreateChainSegmentPrefab();
            CreateTimingBarPrefab();
        }

        private static void CreateChainSegmentPrefab()
        {
            // Create the segment GameObject
            GameObject segment = new GameObject("ChainSegment");
            RectTransform rectTransform = segment.AddComponent<RectTransform>();
            Image image = segment.AddComponent<Image>();

            // Configure the RectTransform
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(30f, 30f); // Size of the segment

            // Configure the Image component
            image.sprite = CreateCircleSprite();
            image.color = Color.gray;
            image.type = Image.Type.Simple;

            // Create the prefab
            string prefabPath = "Assets/_Project/Prefabs/UI/ChainSegment.prefab";
            
            // Ensure the directory exists
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            // Create the prefab asset
            PrefabUtility.SaveAsPrefabAsset(segment, prefabPath);
            DestroyImmediate(segment);

            Debug.Log("Chain segment prefab created at: " + prefabPath);
        }

        private static void CreateTimingBarPrefab()
        {
            // Create main container
            GameObject timingBar = new GameObject("TimingBar");
            RectTransform timingBarRect = timingBar.AddComponent<RectTransform>();
            timingBar.AddComponent<TimingBarUI>();

            // Configure main container
            timingBarRect.anchorMin = new Vector2(0.5f, 0f);
            timingBarRect.anchorMax = new Vector2(0.5f, 0f);
            timingBarRect.pivot = new Vector2(0.5f, 0f);
            timingBarRect.sizeDelta = new Vector2(400f, 40f);
            timingBarRect.anchoredPosition = new Vector2(0f, 100f);

            // Create background
            GameObject background = CreateUIElement("Background", timingBar, new Color(0.2f, 0.2f, 0.2f));
            background.GetComponent<RectTransform>().sizeDelta = timingBarRect.sizeDelta;

            // Create good zone
            GameObject goodZone = CreateUIElement("GoodZone", timingBar, new Color(1f, 0.8f, 0f, 0.5f));
            RectTransform goodZoneRect = goodZone.GetComponent<RectTransform>();
            goodZoneRect.sizeDelta = new Vector2(160f, 40f);

            // Create perfect zone
            GameObject perfectZone = CreateUIElement("PerfectZone", timingBar, new Color(0f, 1f, 0f, 0.5f));
            RectTransform perfectZoneRect = perfectZone.GetComponent<RectTransform>();
            perfectZoneRect.sizeDelta = new Vector2(80f, 40f);

            // Create marker
            GameObject marker = CreateUIElement("Marker", timingBar, Color.white);
            RectTransform markerRect = marker.GetComponent<RectTransform>();
            markerRect.sizeDelta = new Vector2(4f, 40f);

            // Create fill bar
            GameObject fillBar = CreateUIElement("FillBar", timingBar, Color.white);
            RectTransform fillBarRect = fillBar.GetComponent<RectTransform>();
            fillBarRect.sizeDelta = new Vector2(0f, 40f);
            fillBarRect.anchorMin = new Vector2(0f, 0f);
            fillBarRect.anchorMax = new Vector2(0f, 1f);
            fillBarRect.pivot = new Vector2(0f, 0.5f);

            // Assign references in TimingBarUI component
            TimingBarUI timingBarUI = timingBar.GetComponent<TimingBarUI>();
            timingBarUI.GetType().GetField("fillBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(timingBarUI, fillBarRect);
            timingBarUI.GetType().GetField("perfectZone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(timingBarUI, perfectZoneRect);
            timingBarUI.GetType().GetField("goodZone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(timingBarUI, goodZoneRect);
            timingBarUI.GetType().GetField("marker", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(timingBarUI, markerRect);

            // Create the prefab
            string prefabPath = "Assets/_Project/Prefabs/UI/TimingBar.prefab";
            
            // Ensure the directory exists
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            // Create the prefab asset
            PrefabUtility.SaveAsPrefabAsset(timingBar, prefabPath);
            DestroyImmediate(timingBar);

            Debug.Log("Timing bar prefab created at: " + prefabPath);
            AssetDatabase.Refresh();
        }

        private static GameObject CreateUIElement(string name, GameObject parent, Color color)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent.transform);
            
            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            
            Image image = obj.AddComponent<Image>();
            image.color = color;
            
            return obj;
        }

        private static Sprite CreateCircleSprite()
        {
            // Create a circular texture
            int size = 128;
            Texture2D texture = new Texture2D(size, size);
            Color[] colors = new Color[size * size];
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float radius = size / 2f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    float alpha = distance <= radius ? 1f : 0f;
                    
                    // Add a slight gradient for better visual
                    if (alpha > 0f)
                    {
                        alpha = Mathf.Lerp(1f, 0.8f, distance / radius);
                    }
                    
                    colors[y * size + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(colors);
            texture.Apply();

            // Save the texture as an asset
            string texturePath = "Assets/_Project/Textures/UI/CircleSegment.png";
            
            // Ensure the directory exists
            string directory = System.IO.Path.GetDirectoryName(texturePath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            // Save the texture asset
            byte[] pngData = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(texturePath, pngData);
            AssetDatabase.Refresh();

            // Load and configure the texture
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 100;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Compressed;
                importer.SaveAndReimport();
            }

            // Create and return the sprite
            return AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        }
#endif
    }
}
