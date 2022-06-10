using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.forward = Camera.main.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
