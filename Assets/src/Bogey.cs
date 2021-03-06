using UnityEngine;
using UnityEngine.UI;

public class Bogey : MonoBehaviour {

    public Sprite markerSprite;
    public Sprite trackedMarkerSprite;
    public float markerSize;
    public float blipHeight;
    public float blipWidth;
    public bool isActive = true;
    public BoxCollider2D blipHitbox;
    private Rigidbody2D rb;
    public FCR radar;
    public RectTransform TargetCursor;
    public Image MarkerImage
    {
        get
        {
            return markerImage;
        }
    }

    private Image markerImage;

    void Start () {
        //TargetCursor = GameObject.FindGameObjectWithTag("RadarCursor").GetComponent<RectTransform>();

        if (!markerSprite)
        {
            Debug.LogError(" Please, specify the marker sprite.");
        }

        GameObject markerImageObject = new GameObject("Marker");
        markerImageObject.AddComponent<Image>();

        blipHitbox = markerImageObject.AddComponent<BoxCollider2D>();
        rb = markerImageObject.AddComponent<Rigidbody2D>();
        blipHitbox.size = new Vector2(blipHeight,blipWidth);
        //blipHitbox.offset = new Vector2(blipHeight / 2,blipWidth / 2);
            blipHitbox.isTrigger = true;
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
        markerImageObject.transform.SetParent(controller.BogeyList.MarkerGroupRect);
        blipWidth = markerSprite.rect.width;
        blipHeight = markerSprite.rect.height;
        markerImageObject.tag = "Enemy";
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.sprite = markerSprite;
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);
    }


    void Update () {
        TargetCursor = radar.FCRCursor;
        CheckOverlap(TargetCursor,markerImage);
        RadarDisplay controller = RadarDisplay.Instance;
        if (!controller)
        {
            return;
        }
        RadarDisplay.Instance.checkIn(this);
        markerImage.rectTransform.rotation = Quaternion.Euler(0,0,360-gameObject.transform.rotation.eulerAngles.y);


    }

    void OnDestroy()
    {
        if (markerImage)
        {
            Destroy(markerImage.gameObject);
        }
    }

    public void show()
    {
        markerImage.gameObject.SetActive(true);
    }

    public void hide()
    {
        markerImage.gameObject.SetActive(false);
    }

    public bool isVisible()
    {
        return markerImage.gameObject.activeSelf;
    }

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    public void setLocalPos(Vector3 pos)
    {
        markerImage.rectTransform.localPosition = pos;

    }

    public void setOpacity(float opacity)
    {
        markerImage.color = new Color(1.0f, 1.0f, 1.0f, opacity);
    }

    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.anchoredPosition.x, rectTrans1.anchoredPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.anchoredPosition.x, rectTrans2.anchoredPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    bool CheckOverlap(RectTransform RadarCursor, Image marker){
        //Debug.Log("Bandit position:"+markerImage.rectTransform.anchoredPosition);
        //Debug.Log(RadarCursor.anchoredPosition);
        if(rectOverlaps(RadarCursor,markerImage.rectTransform)){
            Debug.Log("Bandit position:"+markerImage.rectTransform.anchoredPosition);
            Debug.Log(gameObject.transform.position);
            if (Input.GetKeyDown(KeyCode.Z)) {
                SelectTarget(gameObject);
                Debug.Log(gameObject.transform.position);
            }
            return true;
        }
        else
            return false;
    }

    void SelectTarget(GameObject target) {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(target.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //if (onScreen) {
            target.tag = "SelectedTarget";
            markerImage.sprite = trackedMarkerSprite;
        //}
    }

    void DeselectTarget(GameObject target){
        SetObjectTag(target, "SelectedTarget","Air");
    }

    void SwapTags(GameObject[] SelectedTargets, string oldtag, string newtag){
        SelectedTargets = GameObject.FindGameObjectsWithTag("SelectedTarget");
        if(SelectedTargets.Length > 1){
            SelectedTargets[SelectedTargets.Length-1].tag = "Air";
        }
    }

    void SetObjectTag(GameObject g, string previousTag, string newTag){
        if (previousTag == "SelectedTarget")
            g.tag = newTag;
    }
}
