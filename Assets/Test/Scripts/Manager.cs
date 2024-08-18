using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject CharacterObj;

    GameObject P2PLeft, P2PRight;

    Character characterController;
    int state = 1;

    [SerializeField] float platformMoveSpd;

    //[SerializeField] GameObject minRespawnObj, maxRespawnObj;
    [SerializeField] float minRespawnX, maxRespawnX;

    [SerializeField] GameObject PF_A_Obj, PF_B_Obj, PF_C_Obj;

    Platform PT_Controller_1, PT_Controller_2, PT_Controller_3;
    Stick activeStick_Controller;

    [SerializeField] Animator characterAnimator;

    void Start()
    {
        //minRespawnX = minRespawnObj.transform.position.x;
        //maxRespawnX = maxRespawnObj.transform.position.x;

        PT_Controller_1 = PF_A_Obj.GetComponent<Platform>();
        PT_Controller_2 = PF_B_Obj.GetComponent<Platform>();
        PT_Controller_3 = PF_C_Obj.GetComponent<Platform>();

        characterController = CharacterObj.GetComponent<Character>();

        activeStick_Controller = PT_Controller_1.GetStick().GetComponent<Stick>();
        activeStick_Controller.gameObject.SetActive(true);

        //PT_Controller_2.GetStick().GetComponent<Stick>().gameObject.SetActive(false);
        //PT_Controller_3.GetStick().GetComponent<Stick>().gameObject.SetActive(false);
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

    public IEnumerator RotateStick()
    {
        yield return StartCoroutine(activeStick_Controller.RotateStick());
    }

    public void State0()
    {
        //PT_Controller_3.transform.position.x + Random.Range(minRespawnX, maxRespawnX)
        PT_Controller_1.transform.position = new Vector3( 4, 
            PT_Controller_1.transform.position.y,
            PT_Controller_1.transform.position.z);

        PT_Controller_1.GetComponent<RectTransform>().sizeDelta = new Vector2(Random.Range(100, 300),
            PT_Controller_1.GetComponent<RectTransform>().sizeDelta.y);

        activeStick_Controller.ResetStick();

        Platform PT_Controller_TMP = PT_Controller_1;
        PT_Controller_1 = PT_Controller_2;
        PT_Controller_2 = PT_Controller_3;
        PT_Controller_3 = PT_Controller_TMP;


        //activeStick_Controller.gameObject.SetActive(true);
        activeStick_Controller = PT_Controller_1.GetStick().GetComponent<Stick>();


        //PT_Controller_2.GetStick().SetActive(false);
        //PT_Controller_3.GetStick().SetActive(false);


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
        {
            Debug.Log("Collided");
            state = 4;
        }
        else
        {
            Debug.Log("Lose");
            state = -1;
        }

    }

    public void State4()
    {
        characterAnimator.SetBool("IdleToWalk", true);
        // Play hero moving animation

        if (CharacterObj.transform.position.x < PT_Controller_2.GetPlatformPoint(2).transform.position.x - .2f)
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

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
