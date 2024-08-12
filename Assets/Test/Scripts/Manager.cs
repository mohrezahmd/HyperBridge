using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject stickObj, CharacterObj;

    GameObject P1Stick, P1PRight, P2PLeft, P2PRight;

    Stick stickController;
    Character characterController;
    int state = 1;
    [SerializeField] float platformMoveSpd;

    [SerializeField] GameObject platform1Obj, platform2Obj, platform3Obj;
    Platform platform1Controller, platform2Controller, platform3Controller;

    [SerializeField] Animator characterAnimator;

    void Start()
    {
        stickController = stickObj.GetComponent<Stick>();
        platform1Controller = platform1Obj.GetComponent<Platform>();
        platform2Controller = platform2Obj.GetComponent<Platform>();
        platform3Controller = platform3Obj.GetComponent<Platform>();
        characterController = CharacterObj.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
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
        }
        /*
 1 - Generate stickController
     {
         2 - Release stickController
         3 - Stick drops
         4 - Stick stops
    }
 5 - Detect collision type
 6.1 - Collided the second platform
 6.2 - Not collided the second platform or overpass it
 7 - Move the character from first platform to second one
 8 - Move platforms to the left
 9 - stop the platforms when the second one reached left border
10 - put the outranged platform at the last position out of the right side
11 - 
 */
    }

    public void HoldScreen()
    {
        stickController.Heighten();
    }

    public IEnumerator RotateStick()
    {
        yield return StartCoroutine(stickController.RotateStick());
    }


    public void State1()
    {
        if (Input.GetMouseButton(0))
        {
            stickController.Heighten();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = 2;
            stickController.StartRotating();
        }
    }

    public void State2()
    {
        if (stickController.IsRotating()) //stickObj.transform.rotation.eulerAngles.z <= 90)
        {
            StartCoroutine(RotateStick());
        }
        else
        {
            state = 3;
        }
    }

    public void State3()
    {
        P1Stick = platform1Controller.GetStick();

        P1PRight = platform1Controller.GetPlatformPoint(2);
        P2PLeft = platform2Controller.GetPlatformPoint(0);
        P2PRight = platform2Controller.GetPlatformPoint(2);

        if (P1Stick.GetComponent<Stick>().GetTip().transform.position.x >= P2PLeft.transform.position.x &&
            P1Stick.GetComponent<Stick>().GetTip().transform.position.x <= P2PRight.transform.position.x)
        {
            Debug.Log("Collided");
            state = 4;
        }
        else
        {
            Debug.Log("Lose");
            state = -1;
        }
        //Debug.Log("tmpStickTip pos: " + tmpStickTip.transform.position);
        //Debug.Log("tmpPlatformLeftTip pos: " + tmpPlatformLeftTip.transform.position);
    }

    public void State4()
    {
        characterAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation
        if (CharacterObj.transform.position.x < platform2Obj.transform.position.x)
        {
            StartCoroutine(MoveCharacter());
        }
        else
        {
            characterAnimator.SetBool("IdleToWalk", false);
            state = 5;
        }
    }

    public void State5()
    {
        P2PLeft = platform2Controller.GetPlatformPoint(0);

        //Debug.Log(platform2Controller.GetPlatformPoint(0).transform.position.x);
        //gameObject.transform.position -= new Vector3(platformMoveSpd * Time.deltaTime, 0, 0);

        Debug.Log(P2PLeft.transform.position.x);
        Debug.Log("ss: " + -GetComponent<RectTransform>().rect.width / 2);

        if (P2PLeft.transform.position.x > -1.4f)
        {
            gameObject.transform.position -= new Vector3(platformMoveSpd * Time.deltaTime, 0, 0);
            Debug.Log(2);
            MovePlatforms();
            MoveCharacter(platformMoveSpd);

        }
        else
        {
           
        }
    }

    public IEnumerator MoveCharacter()
    {
        yield return StartCoroutine(characterController.MoveCharacter());
    }

    public void MoveCharacter(float moveSpeed)
    {
        characterController.MoveCharacter(moveSpeed);
    }

    public void MovePlatforms()
    {
        platform1Controller.MovePlatform(platformMoveSpd);
        platform2Controller.MovePlatform(platformMoveSpd);
        platform3Controller.MovePlatform(platformMoveSpd);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
