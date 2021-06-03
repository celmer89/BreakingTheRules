using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{


    public float CurrentVelocity = 1.0f;
    public float AngularVelocity = 1f;
    public float Acceleration = 1.0f;

    public float TargetVelocity = 5.0f;
    public float AllowedVelocity = 7.0f;
    public float RedhogVelocity = 15.0f;
    public float RedhogDuration = 5.0f;
    public List<AudioClip> Honks = new List<AudioClip>();
    public MeshRenderer meshRenderer;
    public GameObject RedhogHelper;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_TrafficSystem = GameObject.FindGameObjectsWithTag("TrafficSystem")[0].GetComponent<TrafficSystem>();
        m_TrafficSystem.RegisterCar(this);
    }

    void OnDestroy()
    {
        m_TrafficSystem.UnregisterCar(this);
        Debug.Log("car destroyed");
    }

    // Update is called once per frame
    void Update()
    {
        //chack redhog
        if(m_IsRoadhog)
        {
            m_RedhogeTimeRemaining -= Time.deltaTime;
            if(m_RedhogeTimeRemaining < 0)
            {
                m_IsRoadhog = false;
                m_RedhogeTimeRemaining = 0f;
                RedhogHelper.SetActive(false);
            }
        }

        bool respectLights = m_lightColor == LightColor.Red || m_lightColor == LightColor.Yellow || m_lightColor == LightColor.RedYellow;
        if(m_IsRoadhog)
        {
            respectLights = false;
        }


        bool stop = respectLights || (m_blockingCars > 0);
        if (stop)
        {
             TargetVelocity = 0;
        }
        else
        {
            if(m_IsRoadhog)
            {
                TargetVelocity = RedhogVelocity - Random.Range(0, 0.5f);
            }
            else
            {
                TargetVelocity = AllowedVelocity - Random.Range(0, 0.5f);
            }
        }

        // rotate
        Vector3 targetForward = m_destination - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, targetForward, Time.deltaTime * AngularVelocity);

        // set velocity
        CurrentVelocity = Mathf.Lerp(CurrentVelocity, TargetVelocity, Time.deltaTime * Acceleration);
        if (Mathf.Abs(CurrentVelocity) > 0.1) 
        {
            transform.position = transform.position + transform.forward * CurrentVelocity * Time.deltaTime;
        }
        else
        {
            m_rigidBody.velocity = Vector3.zero;
        }

        // anti blocking
        m_TimeSinceLastWaypoint += Time.deltaTime;
        if(m_blockingCars > 0 && m_TimeSinceLastWaypoint > 15)
        {
            gameObject.transform.position = new Vector3(9999f, 9999f, 99999f);
            Destroy(gameObject);
        }
    }

    public void SetDestination( Vector3 dst)
    {
        m_destination = dst;
        m_TimeSinceLastWaypoint = 0;
    }

    public bool ReachedDestination()
    {
        return AlmostEqual(m_destination, transform.position, 1f);
    }

    public void SetLightColor(LightColor color)
    {
        m_lightColor = color;
    }
    public void SetAllowedVelocity(float velocity)
    {
        AllowedVelocity = velocity;
    }

    public void SetIsRoadhog(bool isRoadhog, float duration)
    {
        m_IsRoadhog = isRoadhog;
        if(m_IsRoadhog)
        {
            RedhogDuration = duration;
            m_RedhogeTimeRemaining = RedhogDuration;
            RedhogHelper.SetActive(true);
            //meshRenderer.material.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void SetHighlight(bool highlight)
    {

    }

    public void Busted()
    {

    }

    public bool GetIsRoadhog()
    {
       return m_IsRoadhog;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject && other.gameObject.tag == "Car" && !other.isTrigger)
        {
            m_blockingCars++;
            int it = Random.Range(0, Honks.Count);
            m_AudioSource.PlayOneShot(Honks[it]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != gameObject  && other.gameObject.tag == "Car" && !other.isTrigger)
        {
            m_blockingCars--;

            if (m_blockingCars < 0)
            {
                m_blockingCars = 0;
            }
        }
    }

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
    private LightColor m_lightColor = LightColor.None;
    private bool m_IsRoadhog = false;
    private int m_blockingCars = 0;
    private AudioSource m_AudioSource;
    private float m_TimeSinceLastWaypoint = 0;
    private float m_RedhogeTimeRemaining = 0;
    TrafficSystem m_TrafficSystem;

}
