using UnityEngine;

[CreateAssetMenu(fileName = "SpriteObject", menuName = "ScriptableObjects/SpriteSheet", order = 1)]
public class MultiSpriteObject : ScriptableObject
{
    public Sprite[] spriteArray;
    public Texture2D[] texture2Ds;
}
