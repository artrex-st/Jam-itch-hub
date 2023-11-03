using JIH.Input;
using Coimbra.Services.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ScriptableMoveStats _stats;
        private InputManager _inputManager;
        private readonly List<EventHandle> _eventHandles = new();
        private Rigidbody2D _rigidbody2D => GetComponent<Rigidbody2D>();
        private CapsuleCollider2D _collider2D => GetComponent<CapsuleCollider2D>();
        private FrameInput _frameInput;

        public void Start()
        {
            Initialize();
        }

        private void Update()
        {
            ProcessMovement();
        }

        private void Initialize()
        {
            _inputManager = gameObject.AddComponent<InputManager>();
            _eventHandles.Add(RequestInputMoveEvent.AddListener(HandlerRequestInputMoveEvent));
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };
            
        }

        private void ProcessMovement()
        {
            
        }

        private void HandlerRequestInputMoveEvent(ref EventContext context, in RequestInputMoveEvent e)
        {
            
        }
    }
}
