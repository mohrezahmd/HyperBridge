using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Background : MonoBehaviour
{
    [SerializeField] MultiSpriteObject spriteSource;
    int spriteSourceCounter, spriteTargetCounter;

    int spriteSourceNumber = 0;

    GameObject tmpObjSprite;
    SpriteRenderer spriteRenderer;
    [SerializeField] List<GameObject> spriteTargetObjects, spriteSourceObjects;
    [SerializeField] Vector3 initialPos;

    GameObject tmpObject;
    SpriteRenderer tmpRenderer;

    private void Start()
    {
        transform.position = initialPos;
        spriteSourceNumber = spriteSource.spriteArray.Length;

        spriteTargetObjects = new List<GameObject>();
        spriteSourceObjects = new List<GameObject>();

        for (int i = 0; i < spriteSourceNumber; i++)
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
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////

                tmpRenderer.sprite = spriteSourceObjects[tmpSpriteIndex].GetComponent<SpriteRenderer>().sprite;
                spriteTargetObjects.Add(tmpObject);
                SpriteRenderer tmpSpriteBefore = null;
                float tmpSpriteOffset = 0;
                if (i != 0) { tmpSpriteBefore = spriteTargetObjects[i - 1].GetComponent<SpriteRenderer>(); }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (tmpSpriteBefore != null) { tmpSpriteOffset = (tmpRenderer.sprite.bounds.size.x / 2) + (tmpSpriteBefore.sprite.bounds.size.x / 2); }
                else { tmpSpriteOffset = tmpRenderer.sprite.bounds.size.x / 2; }
                Debug.Log("tmpSpriteOffset zoj: " + tmpSpriteOffset);

                tmpObject.transform.position += (new Vector3(tmpSpriteOffset, 0, 0) * i);
                
            }
            else
            {
                int tmpSpriteIndex = (Random.Range(0, spriteSourceNumber / 2) * 2) + 1;
                Debug.Log("tmpSpriteIndex: " + tmpSpriteIndex);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////

                tmpRenderer.sprite = spriteSourceObjects[tmpSpriteIndex].GetComponent<SpriteRenderer>().sprite;
                spriteTargetObjects.Add(tmpObject);
                SpriteRenderer tmpSpriteBefore = spriteTargetObjects[i - 1].GetComponent<SpriteRenderer>();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                float tmpSpriteOffset = (tmpRenderer.sprite.bounds.size.x / 2) + (tmpSpriteBefore.sprite.bounds.size.x / 2);
                Debug.Log("tmpSpriteOffset fard: " + tmpSpriteOffset);

                tmpObject.transform.position += (new Vector3(tmpSpriteOffset, 0, 0) * i);
            }

        }

    }

    private void Update()
    {
        transform.position = initialPos;
    }

    public GameObject PickupSprite()
    {
        return new GameObject();
    }

    public void UpdateBackgroundPosition(float backgroundBackwardSpeed)
    {
        transform.position += new Vector3(backgroundBackwardSpeed * Time.deltaTime, 0, 0);
    }
}
