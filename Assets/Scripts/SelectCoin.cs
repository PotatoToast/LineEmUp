using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCoin : MonoBehaviour
{
    void Update()
    {
        GameObject checkObjectClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                // RaycastHit hit = Physics.Raycast(mousePos, Vector2.zero);
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(r, out hit);
                if (hit.collider != null)
                {
                    return hit.collider.gameObject;
                }
                return null;
            }
            return null;
        }
    }

}
