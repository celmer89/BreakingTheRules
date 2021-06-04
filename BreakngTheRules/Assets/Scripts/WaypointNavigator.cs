using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    CarController controller = null;
    public Waypoint currentWaypoint = null;


    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CarController>();
    }

    void Start()
    {
        if (controller && currentWaypoint)
        {
            controller.SetDestination(currentWaypoint.GetPosition());
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.ReachedDestination())
        {
            if (currentWaypoint.nextWaypoint && currentWaypoint.nextWaypoint.nextWaypoint)
            {
                currentWaypoint = currentWaypoint.nextWaypoint;
                controller.SetDestination(currentWaypoint.GetPosition());
                controller.SetNextDestination(currentWaypoint.nextWaypoint.GetPosition());
            }
        }

    }
}
