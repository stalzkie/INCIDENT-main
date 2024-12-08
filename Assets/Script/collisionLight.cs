using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionLight : MonoBehaviour
{
    public GameObject lightObject;
    Light lightSetting;

    public Material mat;
    
    void Start()
    {
        lightSetting = lightObject.GetComponent<Light>();
        
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            lightSetting.color = Color.red;
            RenderSettings.skybox = mat;
        }
    }
}

