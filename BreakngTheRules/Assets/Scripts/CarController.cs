using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( m_lightColor == LightColor.Red || m_lightColor == LightColor.Yellow || m_lightColor == LightColor.RedYellow || m_IsBlocked)
        {
             TargetVelocity = 0;
        }
        else
        {
            TargetVelocity = AllowedVelocity - Random.Range(0,0.5f);
        }

        // rotate
        Vector3 targetForward = m_destination - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, targetForward, Time.deltaTime * AngularVelocity);

        // set velocity
        CurrentVelocity = Mathf.Lerp(CurrentVelocity, TargetVelocity, Time.deltaTime * Acceleration);
        m_rigidBody.velocity = transform.forward * CurrentVelocity;
    }

    public void SetDestination( Vector3 dst)
    {
        m_destination = dst;
    }

    public bool ReachedDestination()
    {
        return AlmostEqual(m_destination, transform.position, 0.5f);
    }

    public void SetLightColor(LightColor color)
    {
        m_lightColor = color;
    }
    public void SetAllowedVelocity(float velocity)
    {
        AllowedVelocity = velocity;
    }

    public void SetIsRoadhog(bool isRoadhog)
    {
        m_IsRoadhog = isRoadhog;
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
            m_IsBlocked = true;
            int it = Random.Range(0, Honks.Count);
            m_AudioSource.PlayOneShot(Honks[it]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            m_IsBlocked = false;
        }
    }


    public float CurrentVelocity = 1.0f;
    public float AngularVelocity = 1f;
    public float Acceleration = 1.0f;

    public float TargetVelocity = 5.0f;
    public float AllowedVelocity = 5.0f;
    public List<AudioClip> Honks = new List<AudioClip>();

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
    private bool m_IsBlocked = false;
    private AudioSource m_AudioSource;

}
