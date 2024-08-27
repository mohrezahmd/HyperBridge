using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject leftP, midP, rightP;
    [SerializeField] GameObject stickCenter, stickTip, stickObj;

    [SerializeField] float stickVerticalSpeed, StickRotationSpeed;

    private float currentRotation = 0f;
    private float targetRotation = -90.0f;
    private bool isStickRotating = false;

    public GameObject GetPlatformPoint(int index)
    {
        switch (index)
        {
            case 0: return leftP;
            case 1: return midP;
            case 2: return rightP;

            default: return null;
        }
    }

    public void HeightenStickTip() { stickTip.transform.Translate(0, stickVerticalSpeed * Time.deltaTime, 0); }

    public IEnumerator RotateStick()
    {
        float rotationStep = StickRotationSpeed * Time.deltaTime;
        if (currentRotation - rotationStep >= targetRotation)
        {
            gameObject.transform.Rotate(0, 0, -rotationStep);
            currentRotation -= rotationStep;
        }
        else
        {
            isStickRotating = false;
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        yield return null;
    }

    public bool IsStickRotating() { return isStickRotating; }

    public void MovePlatform(float moveSpeed)
    {
        gameObject.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

}
