using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{

    //components
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

    //awake is called when the script instance is being loaded
    void Awake()
    {
        //gets the car controller
        topDownCarController = GetComponentInParent<TopDownCarController>();

        //gets the trail renderer component
        trailRenderer = GetComponent<TrailRenderer>();

        //set the trail renderer to not appear/emit at the start
        trailRenderer.emitting = false;
    }

    // Start is called before the first frame update
    public void Start()
    {
        var tr = GetComponent<TrailRenderer>();
        tr.sortingLayerName = "Character";
    }

    // Update is called once per frame
    void Update()
    {
        //if IsTireScreech = true then emit the trail
        if (topDownCarController.IsTireScreeching(out float lateralVelocty, out bool isBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false;

       

        //when using handbrake emit trails
        if (Input.GetKey(KeyCode.Space))
            trailRenderer.emitting = true;
    }
}
