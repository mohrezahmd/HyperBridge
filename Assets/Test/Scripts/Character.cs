using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    
    public float GetMoveSpeed() { return moveSpeed; }

    public IEnumerator MoveCharacter()
    {
        gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        yield return null;
    }

    public void MoveCharacter(float moveLeftSpeed)
    {
        gameObject.transform.position -= new Vector3(moveLeftSpeed * Time.deltaTime, 0, 0);

    }
}
