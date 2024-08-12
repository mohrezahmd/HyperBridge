using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject leftP, midP, rightP, stickObj;
    Stick stick;

    void Start()
    {
        stick = stickObj.GetComponent<Stick>();
    }

    void Update()
    {

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

    public GameObject GetStick()
    {
        return stickObj;
    }

    public void MovePlatform(float moveSpeed)
    {
        gameObject.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }
}
