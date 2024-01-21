using UnityEngine;

public class SelectedAndTargetUI : MonoBehaviour
{
    public void PlaySelected()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("Selected");
    }

    public void PlayTarget()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("Target");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
