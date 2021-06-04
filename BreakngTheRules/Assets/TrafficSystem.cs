using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    // Start is called before the first frame update
    List<CarSpawner> CarSpawners = new List<CarSpawner>();
    List<CarController> Cars = new List<CarController>();
    public float RoadhogDuration = 10;
    public float RoadhogInterval = 1;

    void Start()
    {
        StartCoroutine(RedhogCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator RedhogCoroutine()
    {
        yield return new WaitForSeconds(RoadhogInterval);
        PickRedhog();
        StartCoroutine(RedhogCoroutine());
    }

    public void SetChallengeLevel(int level)
    {
        m_ChallengeLevel = level;

        switch(m_ChallengeLevel)
        {
            case 0:
                RoadhogDuration = 5;
                RoadhogInterval = 5;
                SetSpawnersFreq(10);
                break;
            case 1:
                RoadhogDuration = 7;
                RoadhogInterval = 4;
                SetSpawnersFreq(8);
                break;
            case 2:
                RoadhogDuration = 10;
                RoadhogInterval = 3;
                SetSpawnersFreq(6);
                break;
            case 3:
                RoadhogDuration = 12;
                RoadhogInterval = 2;
                SetSpawnersFreq(4);
                break;
            case 4:
                RoadhogDuration = 15;
                RoadhogInterval = 2;
                SetSpawnersFreq(4);
                break;
            case 5:
                RoadhogDuration = 17;
                RoadhogInterval = 2;
                SetSpawnersFreq(3);
                break;
            default:
                break;
        }
    }

    public void RegisterCar(CarController car)
    {
        Cars.Add(car);
    }

    public void UnregisterCar(CarController car)
    {
        Cars.Remove(car);
    }

    public void PickRedhog()
    {
        int maxRetries = 50;
        int retry = 0;
        bool succ = false;

        while (!succ && retry++ < maxRetries)
        {
            var car = Cars[Random.Range(0, Cars.Count)];
            var renderer = car.GetComponentInChildren<Renderer>();
            succ = renderer && renderer.isVisible && car.SetIsRoadhog(true, RoadhogDuration);
        }
    }

    private void SetSpawnersFreq(float freq)
    {
        foreach (CarSpawner spawner in CarSpawners)
        {
            spawner.CarSpawnFreq = freq;
        }
    }

    private int m_ChallengeLevel = 0;

}
