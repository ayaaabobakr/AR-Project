using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject button;
    public GameObject thisCanvas;
    public GameObject panel;
    private RectTransform panelDimensions;
    Rect buttonDimensions;
    FirebaseFirestore db;
    private Vector2 newScale;
    GameObject item;
    public GameObject ObjectGenerator;
    public GameObject closePanel;

    void Start()
    {
        panelDimensions = panel.GetComponent<RectTransform>();
        buttonDimensions = button.GetComponent<RectTransform>().rect;
        db = FirebaseFirestore.DefaultInstance;

    }

    void SetUpGrid(GameObject panel, int item_num)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 1;
        grid.spacing = new Vector2(100, 100);
        grid.padding = new RectOffset(40, 40, 20, 0);

        grid.cellSize = new Vector2(203, 158);
        grid.childAlignment = TextAnchor.UpperLeft;
        newScale = panelDimensions.sizeDelta;
        newScale.x = (grid.spacing.x + grid.cellSize.x) * item_num + grid.padding.left + grid.padding.right;
        panelDimensions.sizeDelta = newScale;
    }

    public void loadButton()
    {
        Query products = db.Collection("Products");
        // StartCoroutine(GetAssetBundle());

        products.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            QuerySnapshot allcategoriesQuerySnapshot = task.Result;
            SetUpGrid(panel, allcategoriesQuerySnapshot.Count + 1);
            foreach (DocumentSnapshot documentSnapshot in allcategoriesQuerySnapshot.Documents)
            {

                Dictionary<string, object> product = documentSnapshot.ToDictionary();
                StartCoroutine(LoadFromWeb(product));
            }
        });
    }

    IEnumerator LoadFromWeb(Dictionary<string, object> product)
    {
        var name = product["name"].ToString();
        var imgURL = product["imgURL"].ToString();
        var prefabURL = product["prefabURL"].ToString();


        GameObject icon = Instantiate(button) as GameObject;
        icon.transform.SetParent(thisCanvas.transform, false);
        icon.transform.SetParent(panel.transform);
        icon.name = name.ToString();
        icon.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name.ToString();
        Sprite img = Resources.Load<Sprite>("img1") as Sprite;
        icon.GetComponent<Image>().sprite = img;

        AddObject addObj = icon.AddComponent<AddObject>();
        addObj.closePanel = closePanel;
        addObj.ObjectGenerator = ObjectGenerator;
        icon.GetComponent<Button>().onClick.AddListener(addObj.viewInSpace);


        yield return StartCoroutine(GetImage(imgURL, icon));
        yield return StartCoroutine(GetAssetBundle(prefabURL, name, addObj));
    }

    IEnumerator GetImage(string url, GameObject icon)
    {

        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (!(wr.isNetworkError || wr.isHttpError))
        {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);

            icon.GetComponent<Image>().sprite = s;
        }
    }
    IEnumerator GetAssetBundle(string url, string prefabName, AddObject addObj)
    {

        WWW www = new WWW(url);
        Debug.Log("I'm here");
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            Debug.Log("Prefab is loaded");
            item = (GameObject)bundle.LoadAsset(prefabName);
            item.AddComponent<BoxCollider>();
            addObj.prefab = item;
        }
        else
        {
            Debug.Log("www.error");
        }
    }
}