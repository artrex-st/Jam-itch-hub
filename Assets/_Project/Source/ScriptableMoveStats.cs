using Sirenix.OdinInspector;
using UnityEngine;

namespace Source
{
    [CreateAssetMenu(menuName = "Player Stats/Move")]
    public class ScriptableMoveStats : ScriptableObject
    {
        [FoldoutGroup("Movement")]
        [Tooltip("The top horizontal movement speed")]
        public float MaxSpeed = 14;

        [FoldoutGroup("Movement")]
        [Tooltip("The player's capacity to gain horizontal speed")]
        public float Acceleration = 120;

        [FoldoutGroup("Movement")]
        [Tooltip("The immediate velocity applied when jumping")]
        public float JumpPower = 36;

        [FoldoutGroup("Gravity")]
        [Tooltip("The pace at which the player comes to a stop")]
        public float GroundResistence = 60;

        [FoldoutGroup("Gravity")]
        [Tooltip("Deceleration in air only after stopping input mid-air")]
        public float AirResistence = 30;

        [FoldoutGroup("Gravity")]
        [Tooltip("A constant downward force applied while grounded. Helps on slopes")]
        [Range(0f, -10f)]
        public float ArtificialGravity = -1.5f;

        [FoldoutGroup("Gravity")]
        [Tooltip("The maximum vertical movement speed")]
        public float MaxFallSpeed = 40;

        [FoldoutGroup("Gravity")]
        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        public float AirArtificialGravity = 110;

        [FoldoutGroup("Detections")]
        [Tooltip("Set this to the layer your player is on")]
        public LayerMask PlayerLayer;

        [FoldoutGroup("Detections")]
        [Tooltip("The detection distance for grounding and roof detection")]
        [Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;
    }
}
