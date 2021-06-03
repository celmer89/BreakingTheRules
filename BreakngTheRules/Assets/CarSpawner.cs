using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public List<GameObject> Cars = new List<GameObject>();
    public List<Waypoint> Paths = new List<Waypoint>();


    public float CarSpawnFreq = 6;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine()
    {
        SpawnCar();
        yield return new WaitForSeconds(CarSpawnFreq + Random.Range(-1f,2f));
        StartCoroutine(SpawnCoroutine());
    }

    public void SpawnCar()
    {
        Waypoint path = Paths[Random.Range(0, Paths.Count)];

        Vector3 forward = path.transform.position - gameObject.transform.position;
        int i = Random.Range(0, Cars.Count);
        GameObject car = Instantiate(Cars[i], transform.position, Quaternion.identity);
        car.transform.forward = forward;
        WaypointNavigator navigator = car.GetComponent<WaypointNavigator>();
        navigator.currentWaypoint = path;
    }
}
