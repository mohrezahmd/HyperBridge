using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWorkgroundForCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    //[SerializeField] Canvas mainCanvas;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        //transform.position = new Vector3(mainCanvas.transform.position.x, mainCanvas.transform.position.y, 0);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;
        //Vector3 bottomLeft = mainCanvas.GetComponent<RectTransform>().anchorMin;

        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height, 0)) * 100;
        //Vector3 topRight = mainCanvas.GetComponent<RectTransform>().anchorMax;

        Vector3 screenSize = topRight - bottomLeft;
        float screenRation = screenSize.x / screenSize.y;
        float desiredRation = transform.localScale.x / transform.localScale.y;

        if(screenRation > desiredRation)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRation, height, 0);
        }
        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width * desiredRation, width, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
