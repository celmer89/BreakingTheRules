using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class IntroController : MonoBehaviour
{
    public GameObject title;
    public GameObject description1;
    public GameObject description2;
    public GameObject space;
    public GameObject authors;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UICoroutine());
    }

    // Update is called once per frame

    IEnumerator UICoroutine()
    {
        yield return new WaitForSeconds(1);
        description1.SetActive(true);
        yield return new WaitForSeconds(3);
        description2.SetActive(true);
        yield return new WaitForSeconds(3);
        title.SetActive(true);
        yield return new WaitForSeconds(3);
        authors.SetActive(true);
        yield return new WaitForSeconds(3);
        space.SetActive(true);
    }

        void Update()
    {
       if (space.active && Input.GetKey("space"))
       {
            SceneManager.LoadScene(1);
        }
    }
}
