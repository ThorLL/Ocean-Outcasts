using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Effects
{
    public class NewBehaviourScript : MonoBehaviour
    {

        public float fps = 15.0f;
        public List<Sprite> sprites;
        private int _frameIndex;
    
        [SerializeField] private SpriteRenderer spriteRenderer;
 
        private void Start()
        {
            InvokeRepeating(nameof(NextFrame), 0, 1 / fps);
        }
 
        private void NextFrame()
        {
            spriteRenderer.sprite = sprites[_frameIndex];
            _frameIndex = (_frameIndex + 1) % sprites.Count;
        }

    }
}
