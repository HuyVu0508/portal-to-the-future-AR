using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAR : MonoBehaviour
{
    public GameObject scalingAR;

    // Start is called before the first frame update
    void Start()
    {
        scalingAR.transform.localScale = 8.0f * scalingAR.transform.localScale;        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
