using Sirenix.OdinInspector;
using UnityEngine;

namespace JIH.Player
{
    [CreateAssetMenu(menuName = "Player Stats/Move")]
    public class ScriptableMoveStats : ScriptableObject
    {
        [FoldoutGroup("Movement", order: 0)]
        [Tooltip("The top horizontal movement speed")]
        [ShowInInspector] public float MaxSpeed { get; private set; } = 14;

        [FoldoutGroup("Movement")]
        [Tooltip("The player's capacity to gain horizontal speed")]
        [ShowInInspector] public float Acceleration { get; private set; } = 120;

        [FoldoutGroup("Movement")]
        [Tooltip("The immediate velocity applied when jumping")]
        [ShowInInspector] public float JumpPower { get; private set; }= 36;

        [FoldoutGroup("Gravity", order: 1)]
        [Tooltip("The pace at which the player comes to a stop")]
        [ShowInInspector] public float GroundResistence { get; private set; } = 60;

        [FoldoutGroup("Gravity")]
        [Tooltip("Deceleration in air only after stopping input mid-air")]
        [ShowInInspector] public float AirResistence { get; private set; } = 30;

        [FoldoutGroup("Gravity")]
        [Tooltip("A constant downward force applied while grounded. Helps on slopes")]
        [Range(0f, -10f)]
        [SerializeField] private float _artificialGravity = -1.5f;
        public float ArtificialGravity => _artificialGravity;

        [FoldoutGroup("Gravity")]
        [Tooltip("The maximum vertical movement speed")]
        [ShowInInspector] public float MaxFallSpeed { get; private set; } = 40;

        [FoldoutGroup("Gravity")]
        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        [ShowInInspector] public float AirArtificialGravity { get; private set; } = 110;

        [FoldoutGroup("Detections", order: 2)]
        [Tooltip("Set this to the layer your player is on")]
        [ShowInInspector] public LayerMask PlayerLayer { get; private set; }

        [FoldoutGroup("Detections")]
        [Tooltip("The detection distance for grounding and roof detection")]
        [Range(0f, 0.5f)]
        [SerializeField] private float _grounderDistance = 0.05f;
        public float GrounderDistance => _grounderDistance;
    }
}
