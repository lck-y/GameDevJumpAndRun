using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private bool isJumping = false;
    private float jumpCooldownTimer;
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction jumpAction;
    
    [SerializeField] Animator animator;
    private AudioSource jumpSound;
    private AudioSource runningSound;
    [SerializeField] ParticleSystem jumpParticles;
    
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float gravity;
    [SerializeField] private float characterSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float dampening;
    [SerializeField] private Transform cameraTransform;

    private Vector3 characterMovement;
    private Vector3 jumpVelocity;
    private Vector3 characterGravity;
    private Vector3 platformVelocity;
    

    void Start()
    {
        this.controller = this.GetComponent<CharacterController>();
        this.moveAction = InputSystem.actions.FindAction("Move");
        this.jumpAction = InputSystem.actions.FindAction("Jump");
        this.jumpCooldownTimer = 0.0f;
        this.animator = this.GetComponent<Animator>();
        AudioSource[] audioSources = this.GetComponents<AudioSource>();
        this.runningSound = audioSources[1];
        this.jumpSound = audioSources[0];
    }

    void HandleJumping()
    {
        if (this.controller.isGrounded && this.isJumping && this.jumpCooldownTimer <= 0.0f)
        {
            this.jumpVelocity = Vector3.zero;
            this.isJumping = false;
        }
        if (this.controller.isGrounded && !this.isJumping && this.jumpAction.WasPressedThisFrame())
        {
            this.characterGravity = Vector3.zero;
            this.jumpVelocity = Vector3.zero;
            this.jumpVelocity.y = this.jumpSpeed;
            this.jumpCooldownTimer = this.jumpCooldown;
            this.isJumping = true;
            this.jumpSound.PlayOneShot(this.jumpSound.clip);
            this.jumpParticles.Play();
        }
        if (this.jumpVelocity.y > 0.0f)
        {
            this.jumpVelocity.y -= Time.fixedDeltaTime;
        }
        else
        {
            this.jumpVelocity = Vector3.zero;
        }
        this.jumpCooldownTimer -= Time.fixedDeltaTime;
    }

    Vector3 GetPlatformVelocity()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 2.0f, platformLayer))
        {
            MovingPlatform platform = hit.collider.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                return platform.GetVelocity();
            }
        }
        return Vector3.zero;
    }

    void FixedUpdate()
    {
        
        this.HandleJumping();

        var inputMovement = this.moveAction.ReadValue<Vector2>();
        var inputRightDirection = this.cameraTransform.right;
        var inputForwardDirection = this.cameraTransform.forward;
        inputRightDirection.y = 0.0f;
        inputForwardDirection.y = 0.0f;
        inputRightDirection.Normalize();
        inputForwardDirection.Normalize();
        
        this.SetAnimationState(inputMovement);
        this.SoundState(inputMovement);

        if (this.controller.isGrounded)
        {
            this.characterGravity.y = 0.0f;
        }
        this.characterGravity.y += this.gravity * Time.fixedDeltaTime;
        this.characterMovement += this.characterGravity * Time.fixedDeltaTime;
        this.characterMovement += this.jumpVelocity * Time.fixedDeltaTime;
        this.characterMovement += inputRightDirection * inputMovement.x * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement += inputForwardDirection * inputMovement.y * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement *= (1 - this.dampening);

        Vector3 characterForward = this.characterMovement;
        characterForward.y = 0.0f;
        if (characterForward.sqrMagnitude > 0.0f && characterForward != Vector3.zero)
        {
            this.transform.forward = characterForward.normalized;
        }

        // Platform velocity applied separately so it's exact (not dampened)
        if (!this.isJumping)
        {
            this.platformVelocity = GetPlatformVelocity();
        }
        else
        {
            this.platformVelocity = Vector3.zero;
        }

        var combinedMovement = this.characterMovement + this.platformVelocity * Time.fixedDeltaTime;
        this.controller.Move(combinedMovement);
    }
    void SetAnimationState(Vector2 inputMovement) {
        
        
        this.animator.SetBool("isJumping", this.isJumping);
        this.animator.SetBool("isRunning", inputMovement != Vector2.zero);
        this.animator.SetFloat("MovementForward", inputMovement.magnitude);

        
    }
    
    void SoundState(Vector2 inputMovement)
    {
        if (inputMovement != Vector2.zero && !this.runningSound.isPlaying) {
            this.runningSound.Play();
        } else if (inputMovement == Vector2.zero || this.isJumping || !controller.isGrounded) {
            this.runningSound.Stop();
        }
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)

    {
        if (hit.normal.y < 0.7f) return;

        Flatten flatten = hit.collider.GetComponentInParent<Flatten>();
        if (flatten != null) flatten.Animate();

    } 
}