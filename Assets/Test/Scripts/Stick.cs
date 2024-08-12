    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Stick : MonoBehaviour
    {
        RectTransform rectTransform;
        [SerializeField] float HeightenSpeed, rotationSpeed;

        private float currentRotation = 0f;
        private float targetRotation = -90.0f;
        private bool isRotating = false;

        [SerializeField] GameObject stickTip;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();

        }

        void Update()
        {
        
        }

        public void Heighten()
        {
            rectTransform.sizeDelta += new Vector2(0, HeightenSpeed);
        }

        public IEnumerator RotateStick()
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (currentRotation - rotationStep >= targetRotation)
            {
                gameObject.transform.Rotate(0, 0, -rotationStep);
                currentRotation -= rotationStep;
            }
            else
            {
                isRotating = false;
                transform.rotation = Quaternion.Euler(0, 0, -90f);
            }
            yield return null;
        }

        public bool IsRotating() { return isRotating; }

        public void StartRotating() { isRotating = true; }

        public GameObject GetTip() {  return stickTip; }
    }
