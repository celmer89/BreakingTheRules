using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (ReachedDestination())
        //{
        //    return;
        //}

        // rotate
        Vector3 targetForward = m_destination - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, targetForward, Time.deltaTime * AngularVelocity);


        // velocity
        // TO DO LErp

        m_rigidBody.velocity = transform.forward * TargetVelocity;

       




    }

    public void SetDestination( Vector3 dst)
    {
        m_destination = dst;
    }

    public bool ReachedDestination()
    {
        return AlmostEqual(m_destination, transform.position, 0.5f);
    }

    public float Velocity = 1.0f;
    public float AngularVelocity = 1f;
    public float Acceleration = 1.0f;

    public float TargetVelocity = 5.0f;

    private static bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }


    private Vector3 m_destination;
    private Rigidbody m_rigidBody;


}
