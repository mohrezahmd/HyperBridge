using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    int state = 0;
    int score = 0;
    int characterLoseCheck = 0;

    [SerializeField] float stickHightenSpeed, stickRotationSpeed, maxStickHeight = 6;

    [SerializeField] GameObject player, background;

    Animator playerAnimator;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;
    Platform PF_Controller_1, PF_Controller_2, PF_Controller_3;

    Platform activePlatform;
    GameObject P2PLeft, P2PRight; // Second platform's left and right point

    [SerializeField] Text scoreText;

    [SerializeField] float playerForwardSpeed, backwardSpeed, playerPositionOffsetX, playerPositionOffsetY, backgroundBackwardSpeed;

    [SerializeField] float minDistance, maxDistance, debugDistance, distanceOfNewPlatform, debugParentScale; // coefficient of min and max for calculating new position for exited platform


    private void Start()
    {
        state = 1;

        debugDistance = Random.Range(minDistance, maxDistance);

        PF_Controller_1 = PF_A_Obj.GetComponent<Platform>();
        PF_Controller_2 = PF_B_Obj.GetComponent<Platform>();
        PF_Controller_3 = PF_C_Obj.GetComponent<Platform>();

        activePlatform = PF_Controller_1;
        activePlatform.SetStickValues(maxStickHeight, stickHightenSpeed, stickRotationSpeed);

        PF_Controller_1.SetPlayerPosition(player, PF_Controller_1.GetPlatformPoint(2).transform.position.x,
           PF_Controller_1.GetPlatformPoint(2).transform.position.y + 0.1f);

        P2PLeft = PF_Controller_2.GetPlatformPoint(0);
        P2PRight = PF_Controller_2.GetPlatformPoint(2);

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
            case 7: // Rotating stikc after lose
                State7();
                break;

        }
    }

    void State0()
    {
        debugDistance = Random.Range(minDistance, maxDistance);

        //float distanceOffset = PF_Controller_2.GetComponent<SpriteRenderer>().size.x / 2 + PF_Controller_3.GetComponent<SpriteRenderer>().size.x / 2;
        //distanceOfNewPlatform = ( debugParentScale * debugDistance ) - distanceOffset;

        activePlatform.transform.position = new Vector3(debugDistance, activePlatform.transform.position.y, 0);
        activePlatform.ResetPlatform();

        Platform PF_Controller_TMP = PF_Controller_1;
        PF_Controller_1 = PF_Controller_2;
        PF_Controller_2 = PF_Controller_3;
        PF_Controller_3 = PF_Controller_TMP;
        activePlatform = PF_Controller_1;

        activePlatform.SetStickValues(maxStickHeight, stickHightenSpeed, stickRotationSpeed);
        activePlatform.GetSpriteMask().SetActive(true);

        P2PLeft = PF_Controller_2.GetPlatformPoint(0);
        P2PRight = PF_Controller_2.GetPlatformPoint(2);

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
            StopCoroutine(activePlatform.RotateStick());
            state = 3;
        }
    }

    void State3()
    {
        player.transform.position += new Vector3(0, 0.07f, 0);

        if (activePlatform.GetTip().transform.position.x >= P2PLeft.transform.position.x &&
                activePlatform.GetTip().transform.position.x <= P2PRight.transform.position.x)
        {
            Debug.Log("Collided");
            score++;
            scoreText.text = score.ToString();
            state = 4;
        }
        else
        {           
            Debug.Log("Lose");
            state = 6;
        }
    }

    int isPassing1StickTip2BeforeP2 = 0;
    void State4()
    {
        playerAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation

        if (player.transform.position.x < PF_Controller_2.GetPlatformPoint(2).transform.position.x + playerPositionOffsetX)
        //if (player.transform.position.x < activePlatform.GetTip().transform.position.x)
        {
            player.GetComponent<Player>().MovePlayer(playerForwardSpeed);

            background.GetComponent<Background>().UpdateBackgroundPosition(backgroundBackwardSpeed);

            if (player.transform.position.x > activePlatform.GetTip().transform.position.x && isPassing1StickTip2BeforeP2 == 0)
            {
                player.transform.position -= new Vector3(0, 0.07f, 0);
                isPassing1StickTip2BeforeP2++;
            }
        }
        else
        {
            if(isPassing1StickTip2BeforeP2 == 0)
            {
                player.transform.position -= new Vector3(0, 0.07f, 0);
            }
            else
            {
                isPassing1StickTip2BeforeP2 = 0;
            }

            playerAnimator.SetBool("IdleToWalk", false); 
            state = 5;
        }


    }

    void State5()
    {
        P2PLeft = PF_Controller_2.GetPlatformPoint(0);

        if (P2PLeft.transform.position.x > gameObject.GetComponentInParent<SpriteRenderer>().bounds.min.x)
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

    void State6()
    {
        playerAnimator.SetBool("IdleToWalk", true);

        if (player.transform.position.x < PF_Controller_1.GetTip().transform.position.x && characterLoseCheck == 0)
        {
            player.GetComponent<Player>().MovePlayer(playerForwardSpeed);

            background.GetComponent<Background>().UpdateBackgroundPosition(backgroundBackwardSpeed);
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
            StopCoroutine(activePlatform.Lose_RotateStick());
            state = -1;
            Debug.Log("state : " + state);
        }
    }

    public void DebugReloadButton() { SceneManager.LoadScene(0); }
}