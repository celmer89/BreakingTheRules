using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    // Start is called before the first frame update
    public Light RedLight;
    public Light YellowLight;
    public Light GreenLight;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetColor(LightColor color)
    {
        m_lightColor = color;
        LightTheLights();
        NotifyCars();
    }

    private void LightTheLights()
    {
        switch (m_lightColor)
        {
            case LightColor.Red:
                RedLight.enabled = true;
                YellowLight.enabled = false;
                GreenLight.enabled = false;
                break;
            case LightColor.RedYellow:
                RedLight.enabled = true;
                YellowLight.enabled = true;
                GreenLight.enabled = false;
                break;
            case LightColor.Green:
                RedLight.enabled = false;
                YellowLight.enabled = false;
                GreenLight.enabled = true;
                break;
            case LightColor.Yellow:
                RedLight.enabled = false;
                YellowLight.enabled = true;
                GreenLight.enabled = false;
                break;
            default:
                break;

        }
    }

    private void NotifyCars()
    {
        foreach (CarController car in m_cars)
        {
            car.SetLightColor(m_lightColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            CarController car = other.gameObject.GetComponent<CarController>();
            car.SetLightColor(m_lightColor);
            m_cars.Add(car);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            CarController car = other.gameObject.GetComponent<CarController>();
            car.SetLightColor(LightColor.None);
            m_cars.Remove(car);
        }
    }

    private LightColor m_lightColor = LightColor.None;
    private List<CarController> m_cars = new List<CarController>();
}
