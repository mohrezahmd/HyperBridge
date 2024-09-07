using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject leftP, midP, rightP;
    [SerializeField] GameObject stickCenter, stickTip, spriteMask;

    [SerializeField] MultiSpriteObject spriteSheetSObject;
    SpriteRenderer spriteRenderer;
    [SerializeField] float maskOffset, maskSpeed;

    float stickVerticalSpeed, stickRotationSpeed;

    private float currentRotation = 0f;
    private float targetRotation = -90.0f;
    private bool isStickRotating = false;

    private void Start()
    {


        //gameObject.transform.position = new Vector3(transform.position.x, -3f, 0);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UpdatePlatform();
    }

    public void UpdatePlatform()
    {
        spriteRenderer.sprite = spriteSheetSObject.spriteArray[Random.Range(0, 20)];


        leftP.transform.position = new Vector3(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.y, 0f);
        rightP.transform.position = new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.max.y, 0f);
        stickCenter.transform.position = new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.max.y, 0f);
        stickTip.transform.position = new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.max.y, 0f);

        StartCoroutine(calculateMaskMostRight());

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

    IEnumerator calculateMaskMostRight()
    {
        while(spriteMask.GetComponent<SpriteMask>().bounds.max.x >= GetComponent<SpriteRenderer>().bounds.max.x - maskOffset)
        {
            spriteMask.transform.position -= new Vector3(maskSpeed * Time.deltaTime, 0, 0);
        }
        yield break;
    }
    public void UnparentTipCenter() { stickTip.transform.SetParent(gameObject.transform); }
    public void ParentTipCenter() { stickTip.transform.SetParent(stickCenter.transform); }
    public void StartStickRotation() { isStickRotating = true; }
    public void SetRotationHeightenSpeed(float _heightenSpeed, float _rotationSpeed) { stickRotationSpeed = _rotationSpeed; stickVerticalSpeed = _heightenSpeed; }
    public GameObject GetTip() { return  stickTip; }
    public GameObject GetSpriteMask() { return spriteMask; }
    public void SetPlayerPosition(GameObject player, float playerPositionOffsetX, float playerPositionOffsetY) { player.transform.position = new Vector3(rightP.transform.position.x + playerPositionOffsetX,
        stickCenter.transform.position.y + playerPositionOffsetY, 0); }
}
