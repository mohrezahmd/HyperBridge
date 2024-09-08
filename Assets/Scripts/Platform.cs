using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject leftP, midP, rightP;
    [SerializeField] GameObject stickCenter, stickTip, spriteMask, stickBar;

    [SerializeField] MultiSpriteObject spriteSheetSObject;
    [SerializeField] Animator stickCenterAnimator;
    SpriteRenderer spriteRenderer;
    [SerializeField] float maskOffset, maskSpeed;

    float stickVerticalSpeed, stickRotationSpeed;

    float currentRotation = 0f;
    float targetRotation = -90.0f;
    bool isStickRotating = false;
    float maxStickHeight, stickDropSpeed;

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
            stickCenter.transform.Rotate(0, 0, -rotationStep, Space.World);
            currentRotation -= rotationStep;
        }
        else
        {
            isStickRotating = false;
            stickCenter.transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        yield return null;
    }

      float  lose_currentRotation = -90;
      float  lose_targetRotation = -160f;
      bool  lose_isStickRotating = false;
    public IEnumerator Lose_RotateStick()
    {
        float rotationStep = stickRotationSpeed * Time.deltaTime;
        if (lose_currentRotation - rotationStep >= lose_targetRotation)
        {
            stickTip.transform.SetParent(stickCenter.transform);
            stickCenter.transform.Rotate(0, 0, -rotationStep, Space.World);
            lose_currentRotation -= rotationStep;
        }
        else
        {
            lose_isStickRotating = false;
            stickCenter.transform.rotation = Quaternion.Euler(0, 0, -160f);
        }
        yield return null;
    }

    public bool Lose_IsStickRotating() { return lose_isStickRotating; }
    public void Lose_StartStickRotation() 
    {
        lose_isStickRotating = true; 
        spriteMask.gameObject.transform.SetParent(stickCenter.transform);
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
        while (spriteMask.GetComponent<SpriteMask>().bounds.max.x >= GetComponent<SpriteRenderer>().bounds.max.x - maskOffset)
        {
            spriteMask.transform.position -= new Vector3(maskSpeed * Time.deltaTime, 0, 0);
        }
        yield break;
    }

    public void SetStickNums(float _maxStickHeight, float _stickDropSpeed) { maxStickHeight = _maxStickHeight; stickDropSpeed = _stickDropSpeed; }
    public void UnparentTipCenter() { stickTip.transform.SetParent(gameObject.transform); }
    public void ParentTipCenter() { stickTip.transform.SetParent(stickCenter.transform); }
    public void StartStickRotation() { isStickRotating = true; }
    public void SetRotationHeightenSpeed(float _heightenSpeed, float _rotationSpeed) { stickRotationSpeed = _rotationSpeed; stickVerticalSpeed = _heightenSpeed; }
    public GameObject GetTip() { return stickTip; }
    public GameObject GetSpriteMask() { return spriteMask; }
    public void SetPlayerPosition(GameObject player, float playerPositionOffsetX, float playerPositionOffsetY)
    {
        player.transform.position = new Vector3(rightP.transform.position.x + playerPositionOffsetX,
        stickCenter.transform.position.y + playerPositionOffsetY, 0);
    }
}
