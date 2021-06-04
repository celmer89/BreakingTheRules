using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{


    public float CurrentVelocity = 1.0f;
    public float AngularVelocity = 1f;
    public float RoadhogAngularVelocity = 5f;
    public float Acceleration = 1.0f;
    public float TargetVelocity = 5.0f;
    public float AllowedVelocity = 7.0f;
    public float RedhogVelocity = 15.0f;
    public float RedhogDuration = 5.0f;
    public List<AudioClip> Honks = new List<AudioClip>();
    public List<AudioClip> Destroys = new List<AudioClip>();
    public MeshRenderer meshRenderer;
    public GameObject HighLight;
    public GameObject RedhogHelper;
    public BoxCollider leftCollider;

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
        if (m_IsBusted) return;

        //chack redhog
        if(m_IsRoadhog)
        {
            m_RedhogeTimeRemaining -= Time.deltaTime;
            if(m_RedhogeTimeRemaining < 0 || IsBlocked())
            {
                m_IsRoadhog = false;
                m_RedhogeTimeRemaining = 0f;
                RedhogHelper.SetActive(false);
            }
        }

        //check if turning left
        Vector3 offset1 = m_destination - transform.position;
        float sqrLen = offset1.sqrMagnitude;
        Vector3 offset2 = m_nextDestination - transform.position;
        float sqrLen2 = offset2.sqrMagnitude;
        float dot = 1;
        float dotNext = 1;
        if (sqrLen < 64 )
        {
            dot = Vector3.Dot(transform.right, offset1.normalized);
            if (dot < -0.01)
            {
                leftCollider.enabled = true;
            }
            else if (sqrLen2 < 100)
            {
                dotNext = Vector3.Dot(transform.right, offset2.normalized);
                if (dotNext < -0.01)
                {
                    leftCollider.enabled = true;
                }
            }
        }

        if ((dot > -0.005 && dotNext > -0.0005) && !IsBlocked())
        {
            leftCollider.enabled = false;
        }

        // check lights
        bool respectLights = m_lightColor == LightColor.Red || m_lightColor == LightColor.Yellow || m_lightColor == LightColor.RedYellow;
        if(m_IsRoadhog)
        {
            respectLights = false;
        }

        // find target velocity
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
        if(m_IsRoadhog)
        {
            transform.forward = Vector3.Lerp(transform.forward, targetForward, Time.deltaTime * RoadhogAngularVelocity);
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, targetForward, Time.deltaTime * AngularVelocity);
        }
       
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

        //
        // Secondary movement (noise)
        //
        bool USE_NOISE = true;
        if (USE_NOISE)
        {
            float RoadhogNoiseSpeed = 8.0f;
            float RoadhogNoiseAplitude = 1.4f;

            float target_noise = 0.0f;

            var mesh_xform = GetComponentInChildren<Renderer>().gameObject.transform;
            if (m_IsRoadhog && CurrentVelocity > AllowedVelocity)
            {
                target_noise = RoadhogNoiseAplitude * Mathf.Sin(RoadhogNoiseSpeed * Time.time);
            }
            else
            {
                target_noise = 0.0f;
            }

            float delta = target_noise - m_CurrentNoiseOffset; 
            m_CurrentNoiseOffset += delta * Time.deltaTime;

            Vector3 local_pos = mesh_xform.localPosition;
            local_pos.x = m_CurrentNoiseOffset;
            mesh_xform.localPosition = local_pos;
        }

        foreach (Transform xform in transform)
        {
            if (xform.gameObject.CompareTag("Flame"))
            {
                xform.gameObject.SetActive(m_IsRoadhog);
            }
        }

        // anti blocking
        if (IsBlocked())
        {
            m_TimeInBlocked += Time.deltaTime;
        }
        else
        {
            m_TimeInBlocked = 0;
        }

        if (m_TimeInBlocked > 15)
        {
            m_blockingCars = 0;
        }

        m_TimeSinceLastWaypoint += Time.deltaTime;
        if(IsBlocked() && m_TimeSinceLastWaypoint > 20)
        {
            gameObject.transform.position = new Vector3(999f, 999f, 999f);
            Destroy(gameObject, 3f);
        }
    }

    public void SetDestination( Vector3 dst)
    {
        m_destination = dst;
        m_TimeSinceLastWaypoint = 0;
    }

    public void SetNextDestination(Vector3 dst)
    {
        m_nextDestination = dst;
    }

    public bool IsBlocked()
    {
        return m_blockingCars > 0;
    }

    public bool ReachedDestination()
    {
        return AlmostEqual(m_destination, transform.position, 2f);
    }

    public void SetLightColor(LightColor color)
    {
        m_lightColor = color;
    }
    public void SetAllowedVelocity(float velocity)
    {
        AllowedVelocity = velocity;
    }

    public bool SetIsRoadhog(bool isRoadhog, float duration)
    {
        if (IsBlocked())
        {
            return false;
        }
        else
        {
            m_IsRoadhog = isRoadhog;
            if(m_IsRoadhog)
            {
                RedhogDuration = duration;
                m_RedhogeTimeRemaining = RedhogDuration;
                RedhogHelper.SetActive(true);
            }

            return true;
        }
    }

    public void SetHighlight(bool highlight)
    {
        HighLight.SetActive(highlight);
    }

    public void Busted()
    {
        m_IsBusted = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddExplosionForce(700, transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f)), 30f);
        int it = Random.Range(0, Destroys.Count);
        m_AudioSource.PlayOneShot(Destroys[it]);

        Destroy(gameObject, 3);
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
            if (m_IsRoadhog)
            {
                if (GetComponentInChildren<Renderer>().isVisible)
                {
                    int it = Random.Range(0, Honks.Count);
                    m_AudioSource.PlayOneShot(Honks[it]);
                }
            }
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
    private Vector3 m_nextDestination;
    private Rigidbody m_rigidBody;
    private LightColor m_lightColor = LightColor.None;
    private bool m_IsRoadhog = false;
    private int m_blockingCars = 0;
    private AudioSource m_AudioSource;
    private float m_TimeSinceLastWaypoint = 0;
    private float m_TimeInBlocked = 0;
    private float m_RedhogeTimeRemaining = 0;
    TrafficSystem m_TrafficSystem;
    private bool m_IsBusted = false;
    private float m_CurrentNoiseOffset = 0.0f;

}
