using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private void Update()
    {
        ViewInteractionPopup();
    }

    private void ViewInteractionPopup()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10, layerMask))
        {
            var _viewingObject = hit.transform.gameObject;

            var itemComponent = _viewingObject.GetComponent<DroppedItemComponent>();
            if (itemComponent)
            {
                // 아이템 보여주기
            }
        }
        else
        {
            // 아이템 정보 창 끄기
        }
    }
}
