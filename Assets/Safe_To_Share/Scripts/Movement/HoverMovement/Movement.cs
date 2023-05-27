using Safe_To_Share.Scripts.Movement.HoverMovement.Modules;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class Movement : MoveCharacter
    {
        [field: SerializeField] public WalkingModule WalkingModule { get; private set; }
        [SerializeField] SwimmingModule swimmingModule;
        [SerializeField] GroundChecker groundChecker;
        [SerializeField] MoveInputs inputs;
        [SerializeField] Transform ori;
        [SerializeField] float maxFallSpeed = 20f;
        [SerializeField] Transform cameraTarget;

        [SerializeField] Transform mainCamera;

        [SerializeField] Transform avatarOffsetTransform;


        [SerializeField, Range(2f, 20f),] float directionChangeBoostMultiplier = 5f;

        BaseMoveModule currentModule;

        readonly PhysicLayerHandler layerHandler = new();
        Vector3 moveDir;

        void Start()
        {
            WalkingModule.OnStart(Rigid, capsule, groundChecker, Stats, inputs, avatarOffsetTransform);
            swimmingModule.OnStart(Rigid, capsule, groundChecker, Stats, inputs, avatarOffsetTransform);
            currentModule = WalkingModule;
            currentModule.OnEnter(null);
        }
        
        void FixedUpdate()
        {
            AlignWithCamera();
            groundChecker.CheckGround();
            layerHandler.OnFixedUpdate(this,IsGrounded(), groundChecker.LastHit.collider);
            currentModule.OnGravity();
           
            Move();
            SpeedLimit();
            if (inputs.Moving is false)
                currentModule.ApplyBraking();
            currentModule.OnUpdateAvatarOffset();
        }

        void Move()
        {
            moveDir = MovementSettings.Strafe ? ThirdPersonTurn() : ori.forward * inputs.Move.y + ori.right * inputs.Move.x;
            if (currentModule == WalkingModule)
                moveDir = IsGrounded() ? groundChecker.SlopeDir(moveDir) : moveDir.normalized;

            var force = DirectionChangeBoost(moveDir);
            currentModule.OnMove(force);
        }

        void OnTriggerEnter(Collider other)
        {
            HandleSwimming(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Water")) return;
            if (currentModule == swimmingModule)
                ChangeModule(WalkingModule, other);
        }

        void OnTriggerStay(Collider other)
        {
            HandleSwimming(other);
        }

        void SpeedLimit()
        {
            var flatVel = MovementTools.FlatVel(Rigid.velocity);
            if (flatVel.magnitude > MaxSpeed)
            {
                var temp = flatVel.normalized * MaxSpeed;
                temp.y = Rigid.velocity.y;
                Rigid.velocity = temp;
            }

            if (Mathf.Abs(Rigid.velocity.y) > maxFallSpeed)
            {
                var temp = Rigid.velocity;
                temp.y = temp.normalized.y * maxFallSpeed;
                Rigid.velocity = temp;
            }
        }

        Vector3 ThirdPersonTurn()
        {
            var euler = cameraTarget.eulerAngles;
            euler.y += inputs.Move.x;
            euler.z = 0;
            cameraTarget.rotation = Quaternion.Euler(euler);
            return ori.forward * inputs.Move.y;
        }

        void AlignWithCamera()
        {
            if (inputs.Move.y == 0) return;
            var cameraRot = ori.eulerAngles;
            cameraRot.y = mainCamera.eulerAngles.y;
            ori.rotation = Quaternion.Euler(cameraRot);
        }

        void HandleSwimming(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Water"))
                return;
            if (swimmingModule.ShouldSwim(other))
            {
                StartSwimming(other);
                return;
            }

            StartWalking(other);
        }

     

        void StartWalking(Collider other)
        {
            if (currentModule != swimmingModule) return;
            ChangeModule(WalkingModule, other);
            CurrentMode = MoveModes.Walking;
        }

        void StartSwimming(Collider other)
        {
            if (currentModule == swimmingModule)
                return;
            ChangeModule(swimmingModule, other);
            CurrentMode = MoveModes.Swimming;
        }

        void ChangeModule(BaseMoveModule newModule, Collider other)
        {
            currentModule.OnExit();
            currentModule = newModule;
            currentModule.OnEnter(other);
        }


        Vector3 DirectionChangeBoost(Vector3 force)
        {
            var dot = Vector3.Dot(MovementTools.FlatVel(force.normalized),
                MovementTools.FlatVel(Rigid.velocity.normalized));
            var boost = 1f - dot;
            force *= 1f + boost * directionChangeBoostMultiplier;
            return force;
        }

        public override bool IsCrouching() => currentModule.IsCrouching;


        public override bool IsSprinting() => inputs.Sprinting;

        public override bool IsGrounded() => currentModule.IsGrounded();

        public override bool WasGrounded() => currentModule.WarGrounded();

        public override Vector3 GetLocalMoveDirection() => ori.InverseTransformDirection(moveDir);

        public override Vector3 GetUpVector() => ori.rotation * Vector3.up;

        public override bool IsJumping() => currentModule.IsJumping();

        public void Stop()
        {
            if (IsGrounded())
                Rigid.velocity = Vector3.zero;
        }
    }
}