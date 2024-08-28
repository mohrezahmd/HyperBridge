using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    int state = 0;
    [SerializeField] Platform activePlatform, platform2;
    [SerializeField] float stickVerticalSpeed, stickRotationSpeed;

    [SerializeField] GameObject player;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;
    Platform PF_Controller_1, PF_Controller_2, PF_Controller_3;

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

    void State0() { }
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
        if (activePlatform.IsStickRotating()) //stickObj.transform.rotation.eulerAngles.z <= 90)
        {
            StartCoroutine(activePlatform.RotateStick());
        }
        else
        {
            activePlatform.UnparentTipCenter();
            state = 3;
        }
    }

    void State3()
    {
        Debug.Log("state3");
        GameObject P2PLeft = platform2.GetPlatformPoint(0);
        GameObject P2PRight = platform2.GetPlatformPoint(2);

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

        if (player.transform.position.x < platform2.GetPlatformPoint(2).transform.position.x)
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
        GameObject P2PLeft = platform2.GetPlatformPoint(0);

        if (platform2.transform.position.x > -2.81f)
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