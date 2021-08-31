using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera cityViewCamera;
    public Material portalCityViewMaterial;


    // Start is called before the first frame update
    void Start()
    {
        if(cityViewCamera.targetTexture != null)
        {
            cityViewCamera.targetTexture.Release();
        }
        cityViewCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        portalCityViewMaterial.mainTexture = cityViewCamera.targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
