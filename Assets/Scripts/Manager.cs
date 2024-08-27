using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    int state = 0;
    [SerializeField] Platform activePlatform;
    [SerializeField] float stickVerticalSpeed, stickRotationSpeed;


    private void Start()
    {
        state = 1;
        activePlatform.SetRotationHeightenSpeed(stickVerticalSpeed, stickRotationSpeed);
    }
    void Update()
    {
        //Debug.Log("state: " + state);

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
    void State1() {
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

    void State2() {
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

    void State3() { }
    void State4() { }
    void State5() { }
    void State6() { }

    public void DebugReloadButton() { SceneManager.LoadScene(0); }


}