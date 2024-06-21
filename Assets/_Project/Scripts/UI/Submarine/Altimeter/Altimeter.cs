using _Project.Scripts.Logic.Submarine;
using _Project.Scripts.Model;
using UnityEngine;

namespace _Project.Scripts.UI.Submarine.Altimeter
{
    public class Altimeter : MonoBehaviour
    {
        // Make vertical layout group. Group moves instead of individual elements.
        // When group reaches certain point/height: Destroy top element, create new one at bottom, move down 420 pixels
        // Rinse and repeat
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GameObject depthChunkTemplate;
        private float _prevDepth = 0.0f;
        private int _chunkCounter = 5;

        private void CreateChunk(int depth)
        {
            var newChunk = Instantiate(depthChunkTemplate, transform);
            newChunk.name = depth + "m";
            var newScript = newChunk.GetComponent<AltimeterChunk>();
            newScript.SetDepth(depth);
        }
    
        void Awake()
        {
            ShipBehaviour.OnGenericUIToBeUpdated += OnGenericUIToBeUpdated;
        }

        private void Start()
        {
            for (int i = 0; i < _chunkCounter; i++)
            {
                var depth = i * 1000;
                CreateChunk(depth);
            }
        }

        private void OnGenericUIToBeUpdated(object sender, GameData e)
        {
            var currentDepth = e.Ship.Altimeter;
            var movementMeters = currentDepth - _prevDepth;
            // 42 pixels per 100 meters
            var movementPixels = 0.42f * movementMeters;
            var position = rectTransform.anchoredPosition;
            position += Vector2.up * movementPixels;
        
            // This could be replaced by if in 99% of cases, but is "for" for debug purposes (needed for very high starting depth, or insanely fast speed)
            for(int i = 0; position.y >= 210; i++)
            {
                position += Vector2.up * -420;
                Destroy(transform.GetChild(i).gameObject);
                CreateChunk(_chunkCounter * 1000);
                _chunkCounter += 1;
            }
        
            rectTransform.anchoredPosition = position;
            _prevDepth = e.Ship.Altimeter;
        }
    }
}
