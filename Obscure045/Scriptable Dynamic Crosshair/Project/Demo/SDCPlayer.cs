using System;
using UnityEngine;
using UnityEngine.UI;
using Obscure.SDC.Inputs;

namespace Obscure.SDC.Demo {

    public class SDCPlayer : MonoBehaviour
    {
        [Space(10)]
        public Crosshair crosshair;
        public SampleInputs inputs;
        [Header("Controller")]
        public CharacterController myController;
        public Camera fpsCamera;
        public float speed = 6.0f;
        [Header("Ally")]
        public GameObject ally;
        public Vector2 allyCrosshairSize = new Vector2(100, 100);
        public Color allyCrosshairColor = Color.green;
        public float allySmoothSpeed = 0.1f;
        [Header("Enemy")]
        public GameObject enemy;
        public Vector2 enemyCrosshairSize = new Vector2(100, 100);
        public Color enemyCrosshairColor = Color.red;
        public float enemySmoothSpeed = 0.1f;
        [Header("Default")]
        public Vector2 defaultCrosshairSize = new Vector2(100, 100);
        public Color defaultCrosshairColor = Color.white;
        public float defaultSmoothSpeed = 0.1f;
        [Header("None")]
        public Vector2 noneCrosshairSize = new Vector2(100, 100);
        public Color noneCrosshairColor = Color.black;
        public float noneSmoothSpeed = 0.1f;
        [Header("Debug")]
        public Vector2 currentCrosshairSize;
        public GameObject currentTargetedObject;
        [Space]
        public Text sizeText;
        public Text targetText;

        
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            inputs = new SampleInputs();
        }

        void OnEnable()
        {
            inputs.Enable();
        }

        void OnDisable()
        {
            inputs.Disable();
        }

        public void Update()
        {
            GameObject target = crosshair.GetTarget();
            if (target != null)
            {
                currentTargetedObject = target;
                if (target.gameObject == ally)
                {
                    crosshair.SetSize(allyCrosshairSize, allySmoothSpeed);
                    crosshair.SetColor(allyCrosshairColor, allySmoothSpeed);
                }
                else if (target.gameObject == enemy)
                {
                    crosshair.SetSize(enemyCrosshairSize, enemySmoothSpeed);
                    crosshair.SetColor(enemyCrosshairColor, enemySmoothSpeed);
                }
                else
                {
                    crosshair.SetSize(defaultCrosshairSize, defaultSmoothSpeed);
                    crosshair.SetColor(defaultCrosshairColor, defaultSmoothSpeed);
                }
            }
            else if (target == null)
            {
                currentTargetedObject = null;
                crosshair.SetSize(noneCrosshairSize, noneSmoothSpeed);
                crosshair.SetColor(noneCrosshairColor, noneSmoothSpeed);
            }
            currentCrosshairSize = crosshair.GetSize();
            Debug.Log(crosshair.GetHitPoint());
            Move();
            ConfigureDebug();
        }

        void Move()
        {
            Vector2 input = inputs.Player.Move.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0, input.y);
            movement = movement.normalized * speed * Time.deltaTime;
            movement = Quaternion.Euler(0, fpsCamera.transform.eulerAngles.y, 0) * movement;
            myController.Move(movement);
        }

        private void ConfigureDebug()
        {
            sizeText.text = (int) currentCrosshairSize.x + ", " + (int) currentCrosshairSize.y;
            if (currentTargetedObject != null)
            {
                targetText.text = currentTargetedObject.name;
            }
            else
            {
                targetText.text = "None";
            }
        }
    }
}

