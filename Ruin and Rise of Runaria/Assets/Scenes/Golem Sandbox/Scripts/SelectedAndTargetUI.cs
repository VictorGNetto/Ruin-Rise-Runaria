using UnityEngine;

public class SelectedAndTargetUI : MonoBehaviour
{
    public void PlayAutoTarget()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("AutoTarget");
    }

    public void PlayFriendTarget()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("FriendTarget");
    }

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
        gameObject.GetComponent<Animator>().SetTrigger("Empty");
        gameObject.SetActive(false);
    }
}
