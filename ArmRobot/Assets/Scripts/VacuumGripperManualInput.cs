using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumGripperManualInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Base");
        float verticalInput = Input.GetAxis("Shoulder");

        if (horizontalInput != 0)
        {
            Debug.Log("moving horizontally!");
        }

        if (verticalInput != 0)
        {
            Debug.Log("moving vertically!");
        }
    }
}
