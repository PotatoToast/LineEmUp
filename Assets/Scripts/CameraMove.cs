using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 player1Pos;
    [SerializeField] private Vector3 player2Pos;
    public float transitionSpeed;
    public float rotationSpeed;
    private int currPlayer = 1;
    private bool isMoving = false;

    public void SwitchPlayersWrapper(){
        if (!isMoving) StartCoroutine(SwitchPlayers());
    }

    public IEnumerator SwitchPlayers(){
        isMoving = true;
        Debug.Log("Moving camera");
        
        
        Vector3 currPos = gameObject.transform.position;
        Quaternion currRot = gameObject.transform.rotation;
        Vector3 nextPos;
        Quaternion nextRot;

        int xDir;
        float initZ = gameObject.transform.position.z;
        if (currPlayer == 1){
            nextPos = player2Pos;
            currPlayer = 2;
            nextRot = Quaternion.Euler(0, -90, 0);
            xDir = 1;
        }
        else{
            nextPos = player1Pos;
            currPlayer = 1;
            nextRot = Quaternion.Euler(0, 90, 0);
            xDir = -1;
        }

        while (currPos != nextPos || currRot != nextRot){
            Vector3 newPos = Vector3.MoveTowards(currPos, nextPos, transitionSpeed * Time.deltaTime);

            // Vector3 newPos = new Vector3(newX, currPos.y, currPos.z);
            Quaternion newRot = Quaternion.Slerp(currRot, nextRot, rotationSpeed * Time.deltaTime);


            currPos = newPos;
            currRot = newRot;
            gameObject.transform.position = newPos;
            gameObject.transform.rotation = newRot;
            yield return null;
        }
        Debug.Log("Camera reached destination");
        isMoving = false;
    }
}
