using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    int state = 0;
    [SerializeField] Platform activePlatform;
    [SerializeField] float stickVerticalSpeed, stickRotationSpeed;
    [SerializeField] GameObject accessorScreen;

    [SerializeField] GameObject player, background;

    Animator playerAnimator;
    //float initialBackgroundScale;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;
    Platform PF_Controller_1, PF_Controller_2, PF_Controller_3;

    int score = 0;
    [SerializeField] Text scoreText;

    [SerializeField] float playerForwardSpeed, backwardSpeed, playerPositionOffsetX, playerPositionOffsetY, backgroundBackwardSpeed, littleOffsetOnBridge;
    [SerializeField] float minDistance, maxDistance, debugDistance, distanceOfNewPlatform, debugParentScale; // coefficient of min and max for calculating new position for exited platform
    [SerializeField] float maxStickHeight = 6, stickDropSpeed = 0.2f;

    public static float sizeRation10;

    private void Start()
    {
        sizeRation10 = transform.parent.localScale.y / 10;
        Debug.Log("manager ratio: " + sizeRation10);

        state = 1;
        activePlatform.SetRotationHeightenSpeed(stickVerticalSpeed, stickRotationSpeed);
        activePlatform.SetStickNums(maxStickHeight, stickDropSpeed);

        debugDistance = Random.Range(minDistance, maxDistance);

        PF_Controller_1 = PF_A_Obj.GetComponent<Platform>();
        PF_Controller_2 = PF_B_Obj.GetComponent<Platform>();
        PF_Controller_3 = PF_C_Obj.GetComponent<Platform>();

        PF_Controller_1.SetPlayerPosition(player, PF_Controller_1.GetPlatformPoint(2).transform.position.x,
           PF_Controller_1.GetPlatformPoint(2).transform.position.y + 0.1f);

        //player.transform.position = new Vector3( activePlatform.GetPlatformPoint(2).transform.position.x + playerPositionOffsetX, player.transform.position.characterLoseCheck, 0);
        playerAnimator = player.GetComponent<Animator>();
    }


    void Update()
    {
        switch (state)
        {
            case 0:
                State0();
                break;

            case 1:
                State1(); // Get holding input
                break;

            case 2:
                State2(); // Rotate stick
                break;

            case 3:
                State3(); // Detect collision or loose or overgone
                break;

            case 4: // Move character and play its animations
                State4();
                break;

            case 5: // Initialize the third platform, move the platforms to the left
                State5();
                break;

            case 6: // Falling after lose
                State6();
                break;
            case 7: // Falling after lose
                State7();
                break;

        }
    }

    void State0() {
        activePlatform.ResetPlatform();

        maxDistance = accessorScreen.GetComponent<RectTransform>().rect.width / 533.3f;
        minDistance = (activePlatform.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2) +
            (PF_Controller_3.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2) +
            (maxDistance / 1.1f);
        
        //Debug.Log("max dis: " + maxDistance);
        //Debug.Log("min dis: " + minDistance);

        debugDistance = Random.Range(minDistance, maxDistance);



        //debugParentScale = gameObject.transform.parent.transform.localScale.x;

        //float distanceOffset = PF_Controller_2.GetComponent<SpriteRenderer>().size.x / 2 + PF_Controller_3.GetComponent<SpriteRenderer>().size.x / 2;
        //distanceOfNewPlatform = ( debugParentScale * debugDistance ) - distanceOffset;

        activePlatform.transform.position = new Vector3(PF_Controller_3.transform.position.x + debugDistance, activePlatform.transform.position.y, 0);

        Platform PF_Controller_TMP = PF_Controller_1;
        PF_Controller_1 = PF_Controller_2;
        PF_Controller_2 = PF_Controller_3;
        PF_Controller_3 = PF_Controller_TMP;

        activePlatform = PF_Controller_1;

        activePlatform.SetRotationHeightenSpeed(stickVerticalSpeed, stickRotationSpeed);
        activePlatform.GetSpriteMask().SetActive(true);
        activePlatform.SetStickNums(maxStickHeight, stickDropSpeed);

        state = 1;
    }

    void State1()
    {
        if (Input.GetMouseButton(0))
        {
            activePlatform.HeightenStickTip();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = 2;
            activePlatform.StartStickRotation();
        }
    }

    void State2()
    {
        if (activePlatform.IsStickRotating()) 
        {
            StartCoroutine(activePlatform.RotateStick());
        }
        else
        {
            state = 3;
        }
    }

    void State3()
    {
        GameObject P2PLeft = PF_Controller_2.GetPlatformPoint(0);
        GameObject P2PRight = PF_Controller_2.GetPlatformPoint(2);

        if (activePlatform.GetTip().transform.position.x >= P2PLeft.transform.position.x &&
                activePlatform.GetTip().transform.position.x <= P2PRight.transform.position.x)
        {
            //Debug.Log("Collided");
            score++;
            scoreText.text = score.ToString();

            state = 4;
        }
        else
        {
            //Debug.Log("Lose");
            state = 6;
        }     
    }

    int isPassedToGoUpALittle = 0, isPassedToGoDownALittle = 0;
    void State4() 
    {
        playerAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation

        //Debug.Log("player pos: " + player.transform.position.x);
        //Debug.Log("PF_Controller2.point2.pos: " + PF_Controller_2.GetPlatformPoint(2).transform.position.x);
        if(player.transform.position.x > activePlatform.GetPlatformPoint(2).transform.position.x && isPassedToGoUpALittle == 0)
        {
            player.transform.position += new Vector3(0, littleOffsetOnBridge, 0);
            isPassedToGoUpALittle++;
        }

        if (player.transform.position.x < PF_Controller_2.GetPlatformPoint(2).transform.position.x + playerPositionOffsetX )
        {
            player.GetComponent<Player>().MovePlayer(playerForwardSpeed);

            background.GetComponent<Background>().UpdateBackgroundPosition(backgroundBackwardSpeed);

           // player.transform.position += new Vector3(playerForwardSpeed * Time.deltaTime, 0, 0);
           if(player.transform.position.x > activePlatform.GetTip().transform.position.x && isPassedToGoDownALittle == 0)
            {
                player.transform.position -= new Vector3(0, littleOffsetOnBridge, 0);
                isPassedToGoDownALittle++;
            }
        }
        else
        {
            if(isPassedToGoDownALittle == 0)
            {
                player.transform.position -= new Vector3(0, littleOffsetOnBridge, 0);
            }

            isPassedToGoUpALittle = 0;
            isPassedToGoDownALittle = 0;
            playerAnimator.SetBool("IdleToWalk", false);
            state = 5;
        }
    }

    void State5()
    {
        
        if (PF_Controller_2.GetPlatformPoint(0).transform.position.x > gameObject.GetComponentInParent<SpriteRenderer>().bounds.min.x)
        {
            player.GetComponent<Player>().MovePlayer(backwardSpeed);

            PF_Controller_1.MovePlatform(backwardSpeed);
            PF_Controller_2.MovePlatform(backwardSpeed);
            PF_Controller_3.MovePlatform(backwardSpeed);
        }
        else
        {
            state = 0;
        }
    }

    int characterLoseCheck = 0;
    void State6() {

        playerAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation
        if (player.transform.position.x > activePlatform.GetPlatformPoint(2).transform.position.x && isPassedToGoUpALittle == 0)
        {
            player.transform.position += new Vector3(0, littleOffsetOnBridge, 0);
            isPassedToGoUpALittle++;
        }

        //Debug.Log("player pos: " + player.transform.position.x);
        //Debug.Log("PF_Controller2.point2.pos: " + PF_Controller_2.GetPlatformPoint(2).transform.position.x);
        if (player.transform.position.x < PF_Controller_1.GetTip().transform.position.x && characterLoseCheck == 0)
        {
            player.GetComponent<Player>().MovePlayer(playerForwardSpeed);

            background.GetComponent<Background>().UpdateBackgroundPosition(backgroundBackwardSpeed);

            //player.transform.position += new Vector3(playerForwardSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            characterLoseCheck++;
            playerAnimator.SetBool("IdleToWalk", false);
            playerAnimator.SetBool("IdleToLose", true);

            activePlatform.Lose_StartStickRotation();
            state = 7;
        }

    }

    void State7()
    {
        if (activePlatform.Lose_IsStickRotating())
        {
            StartCoroutine(activePlatform.Lose_RotateStick());
        }
        else
        {
            state = -1;
            //Debug.Log("state : " + state);
        }

    }


    public void DebugReloadButton() { SceneManager.LoadScene(0); }

}