using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField] float playerForwardSpeed;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void MovePlayer(float _stickSpeed)
    {
            gameObject.transform.position += new Vector3(_stickSpeed * Time.deltaTime, 0, 0);
    }
}
