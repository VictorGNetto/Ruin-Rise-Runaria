using UnityEngine;
using UnityEngine.InputSystem;

public class SelectHandler : MonoBehaviour
{
    public int selectedGolem = -1;

    private SelecGolem selecArea = null;
    private Camera mainCamera;
    private int golemLayer = 7;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider || rayHit.collider.gameObject.layer != golemLayer) {
            // selected = false;
            // numberOfClicks = 0;

            selectedGolem = -1;
            if (selecArea != null) {
                selecArea.UnselectGolem();
                selecArea = null;
            }

            return;
        }

        int oldGolemSelected = selectedGolem;
        selectedGolem = rayHit.collider.gameObject.GetComponent<SelecGolem>().golem.guid;

        if (oldGolemSelected != selectedGolem) {
            if (selecArea != null) {
                selecArea.UnselectGolem();
            }

            selecArea =  rayHit.collider.gameObject.GetComponent<SelecGolem>();
            selecArea.SelectGolem();
        } else {
            selecArea.OpenRuneSelectionUI();
        }
    }
}
