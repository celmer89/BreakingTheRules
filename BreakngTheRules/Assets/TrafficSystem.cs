using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    // Start is called before the first frame update
    List<CarSpawner> CarSpawners = new List<CarSpawner>();
    List<CarController> Cars = new List<CarController>();
    public float RoadhogDuration = 10;
    public float RoadhogInterval = 10;

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
        Cars[Random.Range(0, Cars.Count)].SetIsRoadhog(true, RoadhogDuration);
    }

}
