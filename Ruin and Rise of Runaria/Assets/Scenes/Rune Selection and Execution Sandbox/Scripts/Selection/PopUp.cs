using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Text text;
    public float timer;

    private void Awake()
    {
        text = transform.GetChild(1).gameObject.GetComponent<Text>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.unscaledDeltaTime;
        if (timer < 0) {
            gameObject.SetActive(false);
        }
    }

    public void SetText(string str)
    {
        text.text = str;
    }

    public void Open(float x, float y, float w, float h, float timer)
    {
        this.timer = timer;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        this.timer = 0;
        gameObject.SetActive(false);
    }
}
