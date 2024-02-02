using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public HUD hud;

    private GameObject[] tips = new GameObject[4];
    private int currentTip = 0;

    private void Awake()
    {
        tips[0] = transform.GetChild(2).transform.GetChild(0).gameObject;
        tips[1] = transform.GetChild(2).transform.GetChild(1).gameObject;
        tips[2] = transform.GetChild(2).transform.GetChild(2).gameObject;
        tips[3] = transform.GetChild(2).transform.GetChild(3).gameObject;
    }

    public void Next()
    {
        currentTip = (currentTip + 1) % 4;
        UpdateTip(1);
    }

    public void Previous()
    {
        currentTip = (4 + currentTip - 1) % 4;
        UpdateTip(-1);
    }

    public void Close()
    {
        currentTip = 0;
        tips[0].SetActive(true);
        tips[1].SetActive(false);
        tips[2].SetActive(false);
        tips[3].SetActive(false);
        gameObject.SetActive(false);
        hud.ResumeTicTac();
    }

    public void Open()
    {
        hud.StopTicTac();
        gameObject.SetActive(true);
    }

    private void UpdateTip(int inc)
    {
        int oldCurrentTip = (4 + currentTip - inc) % 4;
        tips[oldCurrentTip].SetActive(false);
        tips[currentTip].SetActive(true);
    }
}
