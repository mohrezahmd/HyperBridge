using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject leftP, midP, rightP;
    [SerializeField] GameObject stickCenter, stickTip, spriteMask;

    float stickVerticalSpeed, stickRotationSpeed;

    private float currentRotation = 0f;
    private float targetRotation = -90.0f;
    private bool isStickRotating = false;

    private void Start()
    {
        UpdatePlatform();
    }

    public void UpdatePlatform()
    {
        leftP.transform.position = new Vector3(GetComponent<SpriteRenderer>().bounds.min.x, GetComponent<SpriteRenderer>().bounds.max.y, 0f);
        rightP.transform.position = new Vector3(GetComponent<SpriteRenderer>().bounds.max.x, GetComponent<SpriteRenderer>().bounds.max.y, 0f);
        stickCenter.transform.position = new Vector3(GetComponent<SpriteRenderer>().bounds.max.x, GetComponent<SpriteRenderer>().bounds.max.y, 0f);
        stickTip.transform.position = new Vector3(GetComponent<SpriteRenderer>().bounds.max.x, GetComponent<SpriteRenderer>().bounds.max.y, 0f);
        //spriteMask.transform.position = new Vector3(GetComponent<SpriteRenderer>().bounds.max.x, GetComponent<SpriteRenderer>().bounds.max.y, 0f);
    }

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
        float rotationStep = stickRotationSpeed * Time.deltaTime;
        if (currentRotation - rotationStep >= targetRotation)
        {
            stickTip.transform.SetParent(stickCenter.transform);
            stickCenter.transform.Rotate(0, 0, -rotationStep,Space.World);
            currentRotation -= rotationStep;
        }
        else
        {
            isStickRotating = false;
            stickCenter.transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        yield return null;
    }

    public bool IsStickRotating() { return isStickRotating; }

    public void MovePlatform(float moveSpeed)
    {
        gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    public void ResetPlatform()
    {
        stickCenter.transform.Rotate(0, 0, 90);

        currentRotation = 0f;
        targetRotation = -90.0f;
        isStickRotating = false;
        spriteMask.SetActive(false);
        UnparentTipCenter();
        UpdatePlatform();
    }
    public void UnparentTipCenter() { stickTip.transform.SetParent(gameObject.transform); }
    public void ParentTipCenter() { stickTip.transform.SetParent(stickCenter.transform); }
    public void StartStickRotation() { isStickRotating = true; }
    public void SetRotationHeightenSpeed(float _heightenSpeed, float _rotationSpeed) { stickRotationSpeed = _rotationSpeed; stickVerticalSpeed = _heightenSpeed; }
    public GameObject GetTip() { return  stickTip; }
    public GameObject GetSpriteMask() { return spriteMask; }
}
