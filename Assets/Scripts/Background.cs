using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Background : MonoBehaviour
{
    [SerializeField] MultiSpriteObject spriteSource;
    [SerializeField] GameObject accessFrame;

    [SerializeField] List<GameObject> spriteTargetObjects;
    [SerializeField] Vector3 initialPos;
    bool isOdd = true;
    float constantDistance;

    private void Start()
    {
        transform.position = initialPos;
        constantDistance = (spriteSource.spriteArray[0].bounds.size.x / 2) + (spriteSource.spriteArray[1].bounds.size.x / 2);

        spriteTargetObjects = new List<GameObject>();

        GameObject tmpObject;
        for (int i = 0; i < 3; i++)
        {
            tmpObject = new GameObject();
            tmpObject.transform.SetParent(transform.parent);
            tmpObject.transform.position = initialPos + new Vector3(constantDistance * i, 0, 0);

            SpriteRenderer tmpRenderer = tmpObject.AddComponent<SpriteRenderer>();
            tmpRenderer.sortingLayerName = "Background";
            tmpRenderer.sprite = ChooseSpriteRandomly();

            spriteTargetObjects.Add(tmpObject);
        }
        /*for (int i = 0; i < spriteSourceNumber; i++)
        {
            tmpObject = new GameObject();
            tmpRenderer = tmpObject.AddComponent<SpriteRenderer>();
            if (spriteSource.spriteArray[i] != null)
            {
                tmpRenderer.sprite = spriteSource.spriteArray[i];
            }

            spriteSourceObjects.Add(tmpObject);
            tmpObject.SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            tmpObject = new GameObject();
            tmpObject.transform.SetParent(gameObject.transform);
            tmpObject.transform.position = initialPos;

            tmpRenderer = tmpObject.AddComponent<SpriteRenderer>();
            tmpRenderer.sortingLayerName = "Background";

            if ((i) % 2 == 0)
            {
                int tmpSpriteIndex = Random.Range(0, spriteSourceNumber / 2) * 2;
                //Debug.Log("tmpSpriteIndex: " + tmpSpriteIndex);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////

                tmpRenderer.sprite = spriteSourceObjects[tmpSpriteIndex].GetComponent<SpriteRenderer>().sprite;
                spriteTargetObjects.Add(tmpObject);
                SpriteRenderer tmpSpriteBefore = null;
                float tmpSpriteOffset = 0;
                if (i != 0) { tmpSpriteBefore = spriteTargetObjects[i - 1].GetComponent<SpriteRenderer>(); }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (tmpSpriteBefore != null) { tmpSpriteOffset = (tmpRenderer.sprite.bounds.size.x / 2) + (tmpSpriteBefore.sprite.bounds.size.x / 2); }
                else { tmpSpriteOffset = tmpRenderer.sprite.bounds.size.x / 2; }
                //Debug.Log("tmpSpriteOffset zoj: " + tmpSpriteOffset);

                tmpObject.transform.position += (new Vector3(tmpSpriteOffset, 0, 0) * i);
                
            }
            else
            {
                int tmpSpriteIndex = (Random.Range(0, spriteSourceNumber / 2) * 2) + 1;
                //Debug.Log("tmpSpriteIndex: " + tmpSpriteIndex);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////

                tmpRenderer.sprite = spriteSourceObjects[tmpSpriteIndex].GetComponent<SpriteRenderer>().sprite;
                spriteTargetObjects.Add(tmpObject);
                SpriteRenderer tmpSpriteBefore = spriteTargetObjects[i - 1].GetComponent<SpriteRenderer>();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                float tmpSpriteOffset = (tmpRenderer.sprite.bounds.size.x / 2) + (tmpSpriteBefore.sprite.bounds.size.x / 2);
                //Debug.Log("tmpSpriteOffset fard: " + tmpSpriteOffset);

                tmpObject.transform.position += (new Vector3(tmpSpriteOffset, 0, 0) * i);
            }

        } */
    }

    //private void Update()
    //{
    //    spriteTargetObjects.transform.position = initialPos;
    //}

    public GameObject CycleSprite()
    {
        GameObject tmpTargetObject = spriteTargetObjects[0];

        spriteTargetObjects.Remove(spriteTargetObjects[0]);
        tmpTargetObject.GetComponent<SpriteRenderer>().sprite = ChooseSpriteRandomly();
        ChangeSpriteObjectPosition(tmpTargetObject, spriteTargetObjects[spriteTargetObjects.Count - 1]);

        spriteTargetObjects.Add(tmpTargetObject);

        return new GameObject();
    }

    Sprite ChooseSpriteRandomly()
    {
        Sprite mySprite = null;
        if (!isOdd)
        {
            int tmpSpriteIndex = Random.Range(0, spriteSource.spriteArray.Length / 2) * 2;
            mySprite = spriteSource.spriteArray[tmpSpriteIndex];
        }
        else
        {
            int tmpSpriteIndex = (Random.Range(0, spriteSource.spriteArray.Length / 2) * 2) + 1;
            mySprite = spriteSource.spriteArray[tmpSpriteIndex];
        }

        isOdd = !isOdd;
        return mySprite;
    }

    void ChangeSpriteObjectPosition(GameObject sprite0, GameObject spriteLast)
    {
        sprite0.transform.position = spriteLast.transform.position + new Vector3((spriteLast.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2 +
                                                                                 sprite0.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2) 
          , 0, 0);
    }

    public void UpdateBackgroundPosition(float backgroundBackwardSpeed)
    {
        //initialPos += new Vector3(backgroundBackwardSpeed * Time.deltaTime, 0, 0);
        for (int i = 0; i < spriteTargetObjects.Count; i++)
        {
            spriteTargetObjects[i].transform.position += new Vector3(backgroundBackwardSpeed * Time.deltaTime, 0, 0);
        }
        float maxBoundX = spriteTargetObjects[0].GetComponent<SpriteRenderer>().bounds.max.x;
        float negativeRect = (-accessFrame.GetComponent<RectTransform>().sizeDelta.x / 2) / 512;

        //Debug.Log("transform position x: " + spriteTargetObjects[0].transform.position.x);
        //Debug.Log("bound max: " + maxBoundX);
        //Debug.Log("negative rect: " + negativeRect);

        if (maxBoundX <= negativeRect)
        {
            //Debug.Log("bound max x: " + tmpFloat);
            //Debug.Log(6);
            CycleSprite();
        }
    }
}
