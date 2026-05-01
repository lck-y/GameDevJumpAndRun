using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothLever : MonoBehaviour
{
    private bool on = false;
    private bool interpolating = false;
    private bool playerInRange = false;
    private float currentInterpolationTime = 0.0f;
    private InputAction interactAction;
    
    private bool interactPressed = false;

    [SerializeField] private float switchTime;
    [SerializeField] private Transform onPosition;
    [SerializeField] private Transform offPosition;
    [SerializeField] private GameObject leverHandle;
    [SerializeField] private MovingPlatform targetPlatform;

    void Start()
    {
        this.interactAction = InputSystem.actions.FindAction("Interact");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            this.playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            this.playerInRange = false;
        }
    }

    IEnumerator InterpolateLeverCoroutine()
    {
        this.interpolating = true;
        Vector3 startPosition, targetPosition;
        Quaternion startRotation, targetRotation;
        if (this.on)
        {
            startPosition = this.offPosition.position;
            startRotation = this.offPosition.rotation;
            targetPosition = this.onPosition.position;
            targetRotation = this.onPosition.rotation;
        }
        else
        {
            startPosition = this.onPosition.position;
            startRotation = this.onPosition.rotation;
            targetPosition = this.offPosition.position;
            targetRotation = this.offPosition.rotation;
        }
        this.currentInterpolationTime = 0.0f;
        while (this.currentInterpolationTime < this.switchTime)
        {
            float percentage = this.currentInterpolationTime / this.switchTime;
            var currentPosition = Vector3.Lerp(startPosition, targetPosition, percentage);
            var currentRotation = Quaternion.Slerp(startRotation, targetRotation, percentage);
            this.leverHandle.transform.SetPositionAndRotation(currentPosition, currentRotation);
            yield return null;
            this.currentInterpolationTime += Time.deltaTime;
        }
        this.leverHandle.transform.SetPositionAndRotation(targetPosition, targetRotation);
        this.interpolating = false;
    }

    void ToggleLever()
    {
        this.on = !this.on;
        if (this.targetPlatform != null)
        {
            this.targetPlatform.SetActive(this.on);
        }
        this.StartCoroutine(this.InterpolateLeverCoroutine());
    }

    void Update()
    {
        if (this.playerInRange && this.interactAction.WasPressedThisFrame())
        {
            this.interactPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (this.playerInRange && this.interactPressed && !this.interpolating)
        {
            this.interactPressed = false;
            this.ToggleLever();
        }
    }
}