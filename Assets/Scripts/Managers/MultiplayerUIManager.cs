using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class MultiplayerUIManager : MonoBehaviour
{
    public Button startServerButton;

    public Button startClientButton;

    // Start is called before the first frame update
    void Start()
    {
        startServerButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartHost())
            {
                
            }
            else
            {

            }
        });

        startClientButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartClient())
            {

            }
            else
            {

            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
