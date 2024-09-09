using System.Diagnostics.Contracts;
using UnityEngine;

public class FitWorkgroundToCamera : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] GameObject framePlane1, framePlane2;

    void Start()
    {
        mainCamera = Camera.main;
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);// * 100;
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height));// * 100;
        Vector3 screenSize = topRight - bottomLeft;
        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio > desiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height);

            framePlane2.transform.position = new Vector3( topRight.x, transform.position.y, 0);
            framePlane2.transform.localScale = new Vector3(.1f, height, 0);

            framePlane1.transform.position = new Vector3( bottomLeft.x, transform.position.y, 0);
            framePlane1.transform.localScale = new Vector3(.1f, height, 0);
            ExpandPlaneHorizontally();
        }
        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width / desiredRatio);

            framePlane1.transform.position = new Vector3(transform.position.x, topRight.y, 0);
            framePlane1.transform.localScale = new Vector3(width / desiredRatio, .1f, 0);

            framePlane2.transform.position = new Vector3(transform.position.x, bottomLeft.y, 0);
            framePlane2.transform.localScale = new Vector3(width / desiredRatio, .1f, 0);
            ExpandPlaneVertically();
        }
    }

    void ExpandPlaneHorizontally()
    {
            Debug.Log(1);
        Debug.Log("min x plane 2: " + framePlane2.GetComponent<SpriteRenderer>().bounds.min.x);
        Debug.Log("max sprite bound: " + gameObject.GetComponent<SpriteRenderer>().bounds.max.x);

        while(framePlane2.GetComponent<SpriteRenderer>().bounds.min.x > gameObject.GetComponent<SpriteRenderer>().bounds.max.x)
        {
            framePlane2.transform.localScale += new Vector3(.1f, 0, 0);
        }

            Debug.Log(2);
        while (framePlane1.GetComponent<SpriteRenderer>().bounds.max.x < gameObject.GetComponent<SpriteRenderer>().bounds.min.x)
        {
            framePlane1.transform.localScale += new Vector3(.1f, 0, 0);
        }
    }

    void ExpandPlaneVertically()
    {
        while (framePlane2.GetComponent<SpriteRenderer>().bounds.max.y < gameObject.GetComponent<SpriteRenderer>().bounds.min.y)
        {
            framePlane2.transform.localScale += new Vector3(0, .1f, 0);
        }

        while (framePlane1.GetComponent<SpriteRenderer>().bounds.min.y > gameObject.GetComponent<SpriteRenderer>().bounds.max.y)
        {
            framePlane1.transform.localScale += new Vector3(0, .1f, 0);
        }
    }
}