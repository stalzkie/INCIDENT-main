using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScript : MonoBehaviour
{
    public GameObject lightObject;
    Light lightSetting;

    public Material mat1;
    public Material mat2;
    
    void Start()
    {
        lightSetting = lightObject.GetComponent<Light>();
        
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name == "Cube")
        {
            lightSetting.color = Color.blue;
        }
        if(collision.gameObject.name == "Sphere")
        {
            lightSetting.color = Color.white;
            RenderSettings.skybox = mat2;
            GetComponent<Renderer>().material.color = Color.white;
        }
        if(collision.gameObject.name == "skyboxchange")
        {
            RenderSettings.skybox = mat1;
            GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}
