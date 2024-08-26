using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject leftP, midP, rightP, stickObj;
    //Stick stick;

    [SerializeField] GameObject PF_SpriteMask, PF_SpriteTexture;

    void Start()
    {
        //stick = stickObj.GetComponent<Stick>();

        if(PF_SpriteMask != null && PF_SpriteTexture != null)
        {
            PF_SpriteMask.transform.localScale = new Vector3(
                (
                gameObject.GetComponent<RectTransform>().rect.width * PF_SpriteMask.transform.localScale.y)/ gameObject.GetComponent<RectTransform>().rect.height,
                PF_SpriteMask.transform.localScale.y,
                  PF_SpriteMask.transform.localScale.z);

            PF_SpriteMask.transform.position = new Vector3(gameObject.transform.position.x, PF_SpriteMask.transform.position.y,
                PF_SpriteMask.transform.position.z);

            PF_SpriteTexture.transform.position = new Vector3(gameObject.transform.position.x, PF_SpriteTexture.transform.position.y,
                PF_SpriteTexture.transform.position.z);

        }

    }

    public void UpdateSpriteMaskState()
    {
        if (PF_SpriteMask != null && PF_SpriteTexture != null)
        {
            PF_SpriteMask.transform.localScale = new Vector3(
                (
                gameObject.GetComponent<RectTransform>().rect.width * PF_SpriteMask.transform.localScale.y) / gameObject.GetComponent<RectTransform>().rect.height,
                PF_SpriteMask.transform.localScale.y,
                  PF_SpriteMask.transform.localScale.z);

            PF_SpriteMask.transform.position = new Vector3(gameObject.transform.position.x, PF_SpriteMask.transform.position.y,
                PF_SpriteMask.transform.position.z);

            PF_SpriteTexture.transform.position = new Vector3(gameObject.transform.position.x, PF_SpriteTexture.transform.position.y,
                PF_SpriteTexture.transform.position.z);

        }
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
        UpdateSpriteMaskState();
    }
}
