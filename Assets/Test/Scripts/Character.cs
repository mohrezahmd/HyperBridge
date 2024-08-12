using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
