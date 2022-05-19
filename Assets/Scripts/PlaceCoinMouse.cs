using UnityEngine;
using System.Collections;
 
 [RequireComponent(typeof(MeshCollider))]
 
 public class PlaceCoinMouse : MonoBehaviour {
 
 private Vector3 screenPoint;
 private Vector3 offset;
 
 void OnMouseDown() {
    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
 
 }
 
 void OnMouseDrag() {
    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    transform.position = curPosition;
 
 }

void Update() {
    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    Physics.Raycast(r, out hit);
    
    if (hit.collider != null) {
        transform.position = hit.collider.gameObject.transform.position;
        transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        
    }

}

 }