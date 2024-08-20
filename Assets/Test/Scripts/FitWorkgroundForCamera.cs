using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWorkgroundForCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height, 0)) * 100;
        Vector3 screenSize = topRight - bottomLeft;
        float screenRation = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;

        if(screenRation > desiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height, 0);
        }
        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width * desiredRatio, width, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
