
using UnityEngine;
using UnityEngine.UI;

public class SceneBuilder : MonoBehaviour
{
    public GameObject chessManagerPrefab;
    public GameObject boardViewPrefab;

    void Start()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Create UI Panel
        GameObject panel = new GameObject("UI_Panel", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        panel.transform.SetParent(canvasGO.transform, false);
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(1, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(-20, -20);
        rt.sizeDelta = new Vector2(200, 300);

        VerticalLayoutGroup vlg = panel.GetComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.padding = new RectOffset(10, 10, 10, 10);
        vlg.spacing = 10;

        ContentSizeFitter csf = panel.GetComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        AddText(panel.transform, "Tour : Blanc", "TourLabel");
        AddButton(panel.transform, "Lancer dés", "RollDiceButton");
        AddText(panel.transform, "Dé 1 : -", "DiceResult1");
        AddText(panel.transform, "Dé 2 : -", "DiceResult2");
        AddText(panel.transform, "Dé 3 : -", "DiceResult3");
        AddButton(panel.transform, "Rejouer", "RestartButton");

        // Create empty GameObjects
        new GameObject("Board");
        new GameObject("Prefabs");
        new GameObject("Sprites");

        // Instantiate logic if prefabs provided
        if (chessManagerPrefab) Instantiate(chessManagerPrefab).name = "ChessGameManager";
        if (boardViewPrefab) Instantiate(boardViewPrefab).name = "BoardView";

        Debug.Log("✅ Scene auto-setup complete.");
    }

    void AddText(Transform parent, string content, string name)
    {
        GameObject go = new GameObject(name, typeof(Text));
        go.transform.SetParent(parent, false);
        Text text = go.GetComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
    }

    void AddButton(Transform parent, string label, string name)
    {
        GameObject go = new GameObject(name, typeof(Button), typeof(Image));
        go.transform.SetParent(parent, false);
        Button btn = go.GetComponent<Button>();
        Image img = go.GetComponent<Image>();
        img.color = new Color(0.8f, 0.8f, 0.8f);

        GameObject txtGO = new GameObject("Text", typeof(Text));
        txtGO.transform.SetParent(go.transform, false);
        Text txt = txtGO.GetComponent<Text>();
        txt.text = label;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 20;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.black;

        RectTransform rt = txt.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
