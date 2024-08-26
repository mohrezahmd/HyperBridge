using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject CharacterObj;
    [SerializeField] GameObject background1;

    //[SerializeField] GameObject bridgeBlock;

    [SerializeField] Vector3 stickHeightenSpeed;
    [SerializeField] Vector3 stickRotationSpeed;
    [SerializeField] Vector2 _stickDimensions;
    [SerializeField] Vector2 PF_Width;

    GameObject P2PLeft, P2PRight;

    Character characterController;
    int state = 1;

    [SerializeField] float platformMoveSpd;
    [SerializeField] Text scoreText;
    int score = 0;

    [SerializeField] float minRespawnX, maxRespawnX;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;

    Platform PT_Controller_1, PT_Controller_2, PT_Controller_3;
    Stick activeStick_Controller;

    [SerializeField] Animator characterAnimator, bridgeAnimator;

    void Start()
    {
        scoreText.text = "0";

        PT_Controller_1 = PF_A_Obj.GetComponent<Platform>();
        PT_Controller_2 = PF_B_Obj.GetComponent<Platform>();
        PT_Controller_3 = PF_C_Obj.GetComponent<Platform>();

        characterController = CharacterObj.GetComponent<Character>();

        Stick stick1_tmp = PT_Controller_1.GetStick().GetComponent<Stick>();
        Stick stick2_tmp = PT_Controller_2.GetStick().GetComponent<Stick>();
        Stick stick3_tmp = PT_Controller_3.GetStick().GetComponent<Stick>();

        stick1_tmp.SetRotationHeightenSpeed(stickHeightenSpeed.x, stickRotationSpeed.x);
        stick2_tmp.SetRotationHeightenSpeed(stickHeightenSpeed.y, stickRotationSpeed.y);
        stick3_tmp.SetRotationHeightenSpeed(stickHeightenSpeed.z, stickRotationSpeed.z);

        stick1_tmp.SetStickDimension(_stickDimensions.x, _stickDimensions.y);
        stick2_tmp.SetStickDimension(_stickDimensions.x, _stickDimensions.y);
        stick3_tmp.SetStickDimension(_stickDimensions.x, _stickDimensions.y);

        activeStick_Controller = stick1_tmp;
        activeStick_Controller.gameObject.SetActive(true);

        stick2_tmp.gameObject.SetActive(false);
        stick3_tmp.gameObject.SetActive(false);
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

    public IEnumerator RotateStick()
    {
        yield return StartCoroutine(activeStick_Controller.RotateStick());
    }

    public void State0()
    {
        activeStick_Controller.gameObject.SetActive(false);
        PT_Controller_1.transform.position = new Vector3(PT_Controller_3.transform.position.x + Random.Range(minRespawnX, maxRespawnX),
            PT_Controller_1.transform.position.y,
            PT_Controller_1.transform.position.z);

        //PT_Controller_1.transform.localScale = new Vector2(Random.Range(PF_Width.x, PF_Width.y),
        //    PT_Controller_1.transform.localScale.y);

        
        activeStick_Controller.GetTip().transform.position = new Vector3(
            (activeStick_Controller.GetComponent<SpriteRenderer>().bounds.min.x + activeStick_Controller.GetComponent<SpriteRenderer>().bounds.max.x) / 2,
            activeStick_Controller.GetComponent<SpriteRenderer>().bounds.max.y,
            0);

        activeStick_Controller.ResetStick();
        activeStick_Controller.GetTip().SetActive(false);
        activeStick_Controller.gameObject.SetActive(false);

        // Swap
        Platform PT_Controller_TMP = PT_Controller_1;
        PT_Controller_1 = PT_Controller_2;
        PT_Controller_2 = PT_Controller_3;
        PT_Controller_3 = PT_Controller_TMP;

        PT_Controller_1.GetStick().SetActive(true);
        PT_Controller_1.GetStick().GetComponent<Stick>().GetTip().SetActive(true);

        activeStick_Controller.SetStickDimension(_stickDimensions.x, _stickDimensions.y);
        activeStick_Controller.SetStickDimension(_stickDimensions.x, _stickDimensions.y);
        activeStick_Controller.SetStickDimension(_stickDimensions.x, _stickDimensions.y);

        activeStick_Controller = PT_Controller_1.GetStick().GetComponent<Stick>();
        activeStick_Controller.gameObject.SetActive(true);

        state = 1;

    }

    public void State1()
    {
        if (Input.GetMouseButton(0))
        {
            activeStick_Controller.Heighten();
            //activeStick_Controller.Heighten();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = 2;
            Debug.Log(state);
            activeStick_Controller.StartRotating();
        }
    }

    public void State2()
    {
        if (activeStick_Controller.IsRotating()) 
        {
            StartCoroutine(RotateStick());
        }
        else
        {
            state = 3;
        }
    }

    //public void State3()
    //{
    //    P2PLeft = PT_Controller_2.GetPlatformPoint(0);
    //    P2PRight = PT_Controller_2.GetPlatformPoint(2);

    //    if (activeStick_Controller.GetTip().transform.position.x >= P2PLeft.transform.position.x - .1f &&
    //        activeStick_Controller.GetTip().transform.position.x <= P2PRight.transform.position.x + .1f)
    //    {
    //        Debug.Log("Collided");

    //        score++;
    //        scoreText.text = score.ToString();

    //        state = 4;
    //    }
    //    else
    //    {
    //        Debug.Log("Lose");
    //        state = 6;
    //    }

    //}

    public void State3()
    {
        GameObject P2_Left = PT_Controller_2.GetPlatformPoint(0);
        GameObject P2_Right = PT_Controller_2.GetPlatformPoint(2);
        GameObject P1_Right = PT_Controller_1.GetPlatformPoint(2);
        
        float disPF1_Right_to_PF2_Left = P2_Left.transform.position.x - P1_Right.transform.position.x;
        float disPF1_Right_to_PF2_right = P2_Right.transform.position.x - P1_Right.transform.position.x;
        float disPF1_Right_to_TipPosition_X = activeStick_Controller.GetTip().transform.position.x - P1_Right.transform.position.x;

        Debug.Log("disP1P2: " + disPF1_Right_to_PF2_Left);
        Debug.Log("disP1Tip: " + disPF1_Right_to_TipPosition_X);

        if ( disPF1_Right_to_TipPosition_X >= disPF1_Right_to_PF2_Left)// && activeStick_Controller.GetHeight() <= distance2)
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

    public void State4()
    {
        characterAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation

        if (CharacterObj.transform.position.x < PT_Controller_2.GetPlatformPoint(2).transform.position.x - .2f)
        {
            StartCoroutine(MoveCharacter());
            //MoveBackgrounds(characterController.GetMoveSpeed());
        }
        else
        {
            characterAnimator.SetBool("IdleToWalk", false);
            state = 5;
        }
    }

    public void State5()
    {
        P2PLeft = PT_Controller_2.GetPlatformPoint(0);

        if (PT_Controller_2.transform.position.x > -2.81f)
        {
            gameObject.transform.position -= new Vector3(platformMoveSpd * Time.deltaTime, 0, 0);
            MovePlatforms();
            MoveCharacter(platformMoveSpd);
        }
        else
        {
            state = 0;
        }
    }

    public void State6()
    {
        characterAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation
        if (CharacterObj.transform.position.x < activeStick_Controller.GetTip().transform.position.x - .2f)
        {
            StartCoroutine(MoveCharacter());
        }
        else
        {
            characterAnimator.SetBool("IdleToWalk", false);
            characterAnimator.SetBool("IdleToLose", true);
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
        PT_Controller_1.MovePlatform(platformMoveSpd);
        PT_Controller_2.MovePlatform(platformMoveSpd);
        PT_Controller_3.MovePlatform(platformMoveSpd);
    }

    public void MoveBackgrounds(float moveSpeed)
    {
        background1.transform.position -= new Vector3(moveSpeed * .3f * Time.deltaTime, 0, 0);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void Test_ADB()
    {
        Debug.Log("jjjjjjjjjjjjjjjssssssssssssssssssssssjjjjjjjjjjjjjjjjjjj");
    }
}
