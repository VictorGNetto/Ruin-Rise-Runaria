using UnityEngine;
using UnityEngine.InputSystem;

public class SelectHandler : MonoBehaviour
{
    public bool canSelect = true;
    // public bool selected = false;
    // public int numberOfClicks = 0;
    public int selectedGolem = -1;

    private SelecArea selecArea = null;
    private Camera mainCamera;
    private int selectableLayer = 6;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!canSelect) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider || rayHit.collider.gameObject.layer != selectableLayer) {
            // selected = false;
            // numberOfClicks = 0;

            selectedGolem = -1;
            if (selecArea != null) {
                selecArea.UnselectGolem();
                selecArea = null;
            }

            return;
        }

        // selected = true;
        // numberOfClicks += 1;

        int oldGolemSelected = selectedGolem;
        selectedGolem = rayHit.collider.gameObject.GetComponent<SelecArea>().golem.guid;

        if (oldGolemSelected != selectedGolem) {
            if (selecArea != null) {
                selecArea.UnselectGolem();
            }

            selecArea =  rayHit.collider.gameObject.GetComponent<SelecArea>();
            selecArea.SelectGolem();
        } else {
            selecArea.OpenRuneSelectionUI();
        }
    }
}
