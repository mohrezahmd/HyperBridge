using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager_L : MonoBehaviour
{
    [SerializeField] GameObject CharacterObj;
    [SerializeField] GameObject background1, background2;

    [SerializeField] GameObject bridgeBlock;

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

    //[SerializeField] GameObject minRespawnObj, maxRespawnObj;
    [SerializeField] float minRespawnX, maxRespawnX;
    [SerializeField] Animation loseAnimation;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;

    Platform_L PT_Controller_1, PT_Controller_2, PT_Controller_3;
    Stick_L activeStick_Controller;

    [SerializeField] Animator characterAnimator;

    [System.Obsolete]
    void Start()
    {
        //minRespawnX = minRespawnObj.transform.position.x;
        //maxRespawnX = maxRespawnObj.transform.position.x;
        scoreText.text = "0";

        PT_Controller_1 = PF_A_Obj.GetComponent<Platform_L>();
        PT_Controller_2 = PF_B_Obj.GetComponent<Platform_L>();
        PT_Controller_3 = PF_C_Obj.GetComponent<Platform_L>();

        characterController = CharacterObj.GetComponent<Character>();

        Stick_L stick1_tmp = PT_Controller_1.GetStick().GetComponent<Stick_L>();
        Stick_L stick2_tmp = PT_Controller_2.GetStick().GetComponent<Stick_L>();
        Stick_L stick3_tmp = PT_Controller_3.GetStick().GetComponent<Stick_L>();

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

    // Update is called once per frame
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
        activeStick_Controller.Heighten();
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

    public IEnumerator RotateStickAfterLose()
    {
        yield return StartCoroutine(activeStick_Controller.RotateStickAfterLose());

    }

    public void State0()
    {
        //PT_Controller_3.transform.position.x + Random.Range(minRespawnX, maxRespawnX)

        PT_Controller_1.transform.position = new Vector3(PT_Controller_3.transform.position.x + Random.Range(minRespawnX, maxRespawnX),
            PT_Controller_1.transform.position.y,
            PT_Controller_1.transform.position.z);

        PT_Controller_1.UpdateSpriteMaskState();

        //PT_Controller_1.GetComponent<RectTransform>().sizeDelta = new Vector2(Random.Range(100, 400),
        //PT_Controller_1.GetComponent<RectTransform>().sizeDelta = new Vector2(Random.Range(PF_Width.x, PF_Width.y),
        //    PT_Controller_1.GetComponent<RectTransform>().sizeDelta.y);

        activeStick_Controller.ResetStick();
        activeStick_Controller.gameObject.SetActive(false);

        // Swap
        Platform_L PT_Controller_TMP = PT_Controller_1;
        PT_Controller_1 = PT_Controller_2;
        PT_Controller_2 = PT_Controller_3;
        PT_Controller_3 = PT_Controller_TMP;

        activeStick_Controller.SetStickDimension(_stickDimensions.x, _stickDimensions.y);
        activeStick_Controller.SetStickDimension(_stickDimensions.x, _stickDimensions.y);
        activeStick_Controller.SetStickDimension(_stickDimensions.x, _stickDimensions.y);

        activeStick_Controller = PT_Controller_1.GetStick().GetComponent<Stick_L>();
        activeStick_Controller.gameObject.SetActive(true);

        state = 1;

    }

    public void State1()
    {
        if (Input.GetMouseButton(0))
        {
            activeStick_Controller.Heighten();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = 2;
            activeStick_Controller.StartRotating();
        }
    }

    public void State2()
    {
        if (activeStick_Controller.IsRotating()) //stickObj.transform.rotation.eulerAngles.z <= 90)
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
        P2PLeft = PT_Controller_2.GetPlatformPoint(0);
        P2PRight = PT_Controller_2.GetPlatformPoint(2);

        if (activeStick_Controller.GetTip().transform.position.x >= P2PLeft.transform.position.x &&
            activeStick_Controller.GetTip().transform.position.x <= P2PRight.transform.position.x)

        if (activeStick_Controller.GetTip().transform.position.x >= P2PLeft.transform.position.x - .1f &&
            activeStick_Controller.GetTip().transform.position.x <= P2PRight.transform.position.x + .1f)
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
            MoveBackgrounds(characterController.GetMoveSpeed());
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
        //   background2.transform.position -= new Vector3(moveSpeed * .5f * Time.deltaTime, 0, 0);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
