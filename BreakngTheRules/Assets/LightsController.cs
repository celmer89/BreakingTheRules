using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightColor
{
    None,
    Red,
    RedYellow,
    Yellow,
    Green
}

public class LightsController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lightsNorth;
    public GameObject lightsEast;
    public GameObject lightsSouth;
    public GameObject lightsWest;

    public float GreenLightInterval = 10;
    public float YellowLightInterval = 2;
    public float RedLightInterval = 10;

    private void Awake()
    {
        m_lightsNorth = lightsNorth.GetComponent<TrafficLight>();
        m_lightsEast = lightsEast.GetComponent<TrafficLight>();
        m_lightsSouth = lightsSouth.GetComponent<TrafficLight>();
        m_lightsWest = lightsWest.GetComponent<TrafficLight>();
    }

    void Start()
    {
        m_lightsNorth.SetColor(LightColor.Red);
        m_lightsEast.SetColor(LightColor.Green);
        m_lightsSouth.SetColor(LightColor.Red);
        m_lightsWest.SetColor(LightColor.Green);

        StartCoroutine(LightsCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator LightsCoroutine()
    {
        m_lightsNorth.SetColor(LightColor.Red);
        m_lightsEast.SetColor(LightColor.Green);
        m_lightsSouth.SetColor(LightColor.Red);
        m_lightsWest.SetColor(LightColor.Green);

        yield return new WaitForSeconds(GreenLightInterval);

        m_lightsNorth.SetColor(LightColor.RedYellow);
        m_lightsEast.SetColor(LightColor.Yellow);
        m_lightsSouth.SetColor(LightColor.RedYellow);
        m_lightsWest.SetColor(LightColor.Yellow);

        yield return new WaitForSeconds(YellowLightInterval);

        m_lightsNorth.SetColor(LightColor.Green);
        m_lightsEast.SetColor(LightColor.Red);
        m_lightsSouth.SetColor(LightColor.Green);
        m_lightsWest.SetColor(LightColor.Red);

        yield return new WaitForSeconds(RedLightInterval);

        m_lightsNorth.SetColor(LightColor.Yellow);
        m_lightsEast.SetColor(LightColor.RedYellow);
        m_lightsSouth.SetColor(LightColor.Yellow);
        m_lightsWest.SetColor(LightColor.RedYellow);

        yield return new WaitForSeconds(YellowLightInterval);

        StartCoroutine(LightsCoroutine());
    }

    private TrafficLight m_lightsNorth;
    private TrafficLight m_lightsEast;
    private TrafficLight m_lightsSouth;
    private TrafficLight m_lightsWest;

}
