using Coimbra.Services.Events;
using System;
using UnityEngine;

namespace JIH.Levels
{
    [RequireComponent(typeof(Collider2D))]
    public class EndGameManager : MonoBehaviour
    {
        private Collider2D _collider2D => GetComponent<Collider2D>();

        private void OnEnable()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            new RequestEndLevelEvent().Invoke(this);
        }
    }
}
