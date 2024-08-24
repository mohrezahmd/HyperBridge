using System.Collections;
using UnityEngine;

public class Stick : MonoBehaviour
{
    RectTransform rectTransform;
    float HeightenSpeed, rotationSpeed;

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
        rectTransform.sizeDelta = new Vector2(30, 50);
    }

    public void SetRotationHeightenSpeed(float _heightenSpeed, float _rotationSpeed) { rotationSpeed = _rotationSpeed; HeightenSpeed = _heightenSpeed; }

    public void SetStickDimension(float _width, float _height) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _height); }
    public bool IsRotating() { return isRotating; }

    public void StartRotating() { isRotating = true; }

    public GameObject GetTip() { return stickTip; }
}
