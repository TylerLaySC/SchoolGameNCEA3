using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //components
    TopDownCarController topDownCarController;

    //awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");     // stores the inputs
        inputVector.y = Input.GetAxis("Vertical");

        topDownCarController.SetInputVector(inputVector);  //updates input vectors
    }
}
