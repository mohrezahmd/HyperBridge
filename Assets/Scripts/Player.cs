using UnityEngine;

public class Player : MonoBehaviour
{
    public void MovePlayer(float playerSpeed)
    {
        gameObject.transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
    }

}
