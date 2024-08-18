using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] float HeightenSpeed, rotationSpeed;

    private float currentRotation = 0f;
    private float targetRotation = -90.0f;
    private bool isRotating = false;

    [SerializeField] GameObject stickTip;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Heighten()
    {
        rectTransform.sizeDelta += new Vector2(0, HeightenSpeed);
    }

    public IEnumerator RotateStick()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;
        if (currentRotation - rotationStep >= targetRotation)
        {
            gameObject.transform.Rotate(0, 0, -rotationStep);
            currentRotation -= rotationStep;
        }
        else
        {
            isRotating = false;
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        yield return null;
    }

    public void ResetStick()
    {
        currentRotation = 0f;
        targetRotation = -90.0f;
        isRotating = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(30, 5);
    }

    public bool IsRotating() { return isRotating; }

    public void StartRotating() { isRotating = true; }

    public GameObject GetTip() { return stickTip; }
}
