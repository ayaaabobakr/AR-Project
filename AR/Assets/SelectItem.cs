using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject button;

    public GameObject thisCanvas;

    public GameObject panel;

    public int item_num = 7;
    private RectTransform panelDimensions;

    public Sprite img;

    Rect buttonDimensions;

    private Vector2 newScale;
    void Start()
    {
        panelDimensions = panel.GetComponent<RectTransform>();
        buttonDimensions = button.GetComponent<RectTransform>().rect;

        SetUpGrid(panel);
        loadButton();

    }

    // Update is called once per frame

    void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();

        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 1;
        grid.spacing = new Vector2(160, 180);
        grid.padding = new RectOffset(50, 0, 50, 0);

        grid.cellSize = new Vector2(140, 140);
        grid.childAlignment = TextAnchor.UpperLeft;
        newScale = panelDimensions.sizeDelta;
        newScale.x = (grid.spacing.x + grid.cellSize.x) * item_num + grid.padding.left + grid.padding.right;
        panelDimensions.sizeDelta = newScale;
        // panelDimensions.anchoredPosition;

    }

    void loadButton()
    {
        for (int i = 0; i < item_num; i++)
        {
            GameObject icon = Instantiate(button) as GameObject;
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(panel.transform);

            // icon.AddComponent<LayoutElement>().flexibleHeight = 20;
            var name = "item" + (i + 1);
            icon.name = name;
            // Debug.Log(name);
            icon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;
            Sprite img = Resources.Load<Sprite>("img1") as Sprite;
            // icon.image = img;

            icon.GetComponent<Image>().sprite = img;

        }
    }
    void Update()
    {

    }
}
