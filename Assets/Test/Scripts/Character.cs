using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float moveSpeed, fallSpeed;
    
    public float GetMoveSpeed() { return moveSpeed; }

    public IEnumerator MoveCharacter()
    {
        gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        yield return null;
    }

    public void Fall()
    {
        gameObject.transform.position -= new Vector3( 0, fallSpeed * Time.deltaTime, 0);
    }

    public void MoveCharacter(float moveLeftSpeed)
    {
        gameObject.transform.position -= new Vector3(moveLeftSpeed * Time.deltaTime, 0, 0);

    }
}
