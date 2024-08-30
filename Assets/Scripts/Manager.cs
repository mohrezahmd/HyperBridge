using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    int state = 0;
    [SerializeField] Platform activePlatform;
    [SerializeField] float stickVerticalSpeed, stickRotationSpeed;

    [SerializeField] GameObject player, backSpriteMask;
    float initialBackgroundScale;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;
    Platform PF_Controller_1, PF_Controller_2, PF_Controller_3;

    GameObject P2PLeft;
    GameObject P2PRight;
    float goodDistance;
    float tipHeight;

    int score = 1;
    Text scoreText;

    [SerializeField] float playerForwardSpeed, backwardSpeed;

    private void Start()
    {
        state = 1;
        activePlatform.SetRotationHeightenSpeed(stickVerticalSpeed, stickRotationSpeed);

        PF_Controller_1 = PF_A_Obj.GetComponent<Platform>();
        PF_Controller_2 = PF_B_Obj.GetComponent<Platform>();
        PF_Controller_3 = PF_C_Obj.GetComponent<Platform>();

        initialBackgroundScale = backSpriteMask.transform.localScale.y;
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

            case 6:
                State6();
                break;

        }
    }

    void State0() {
        activePlatform.transform.position = new Vector3(5, activePlatform.transform.position.y, 0);
        activePlatform.ResetPlatform();

        Platform PF_Controller_TMP = PF_Controller_1;
        PF_Controller_1 = PF_Controller_2;
        PF_Controller_2 = PF_Controller_3;
        PF_Controller_3 = PF_Controller_TMP;
        activePlatform = PF_Controller_1;

        activePlatform.SetRotationHeightenSpeed(stickVerticalSpeed, stickRotationSpeed);
        activePlatform.GetSpriteMask().SetActive(true);

        backSpriteMask.transform.localScale = new Vector3(backSpriteMask.transform.localScale.x, initialBackgroundScale, backSpriteMask.transform.localScale.z);

        state = 1;
    }

    void State1()
    {
        if (Input.GetMouseButton(0))
        {
            activePlatform.HeightenStickTip();
            //backSpriteMask.GetComponent<SpriteRenderer>().transform.localScale += new Vector3(0, stickVerticalSpeed * Time.deltaTime, 0);
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
                Debug.Log("Collided");
                score++;
                //scoreText.text = score.ToString();

                state = 4;
            }
            else
            {
                Debug.Log("Lose");
                state = 6;
            }     
    }

    void State4() 
    {
        //characterAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation

        if (player.transform.position.x < PF_Controller_2.GetPlatformPoint(2).transform.position.x - .2f )
        {
            player.GetComponent<Player>().MovePlayer(playerForwardSpeed);
            //MoveBackgrounds(characterController.GetMoveSpeed());
        }
        else
        {
            //characterAnimator.SetBool("IdleToWalk", false);
            state = 5;
        }
    }

    void State5()
    {
        GameObject P2PLeft = PF_Controller_2.GetPlatformPoint(0);

        if (PF_Controller_2.transform.position.x > -2.81f)
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
    void State6() { }


    public void DebugReloadButton() { SceneManager.LoadScene(0); }


}