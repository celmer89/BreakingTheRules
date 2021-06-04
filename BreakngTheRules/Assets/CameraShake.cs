//using UnityEngine;
//using System.Collections;

//public class CameraShake : MonoBehaviour
//{
//    // Transform of the camera to shake. Grabs the gameObject's transform
//    // if null.
//    public Transform camTransform;

//    // How long the object should shake for.
//    public float shakeDuration = 999999f;

//    // Amplitude of the shake. A larger value shakes the camera harder.
//    public float shakeAmount = 0.1f;
//    public float decreaseFactor = 0;//1.0f;

//    Vector3 originalPos;

//    void Awake()
//    {
//        if (camTransform == null)
//        {
//            camTransform = GetComponent(typeof(Transform)) as Transform;
//        }
//    }

//    void OnEnable()
//    {
//        originalPos = camTransform.localPosition;
//    }

//    void Update()
//    {
//        Vector3 localPos = camTransform.localPosition;
//        localPos.x = Random.insideUnitSphere.x * shakeAmount;
//        localPos.y = Random.insideUnitSphere.y * shakeAmount + 1.78f;
//        camTransform.localPosition = localPos;

//        //camTransform.localPosition.x = Random.insideUnitSphere.x * shakeAmount;
//        //camTransform.localPosition.y = Random.insideUnitSphere.y * shakeAmount + 1.78;


//        //if (shakeDuration > 0)
//        //{
//        //    camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

//        //    shakeDuration -= Time.deltaTime * decreaseFactor;
//        //}
//        //else
//        //{
//        //    shakeDuration = 0f;
//        //    camTransform.localPosition = originalPos;
//        //}
//    }
//}