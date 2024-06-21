using System;
using _Project.Scripts.Logic.Submarine.Room;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.Logic.Submarine.Crew
{
    public class CrewMemberBehaviour : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Transform spriteTransform;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Animator animator;
        [SerializeField] private float animationScale;
        
        [SerializeField] private float defaultSpeed = 150f;
        
        public Canvas Canvas { get; set; }

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        
        private Transform _originalParent;
        private Vector2 _originalDragAnchoredPosition;
        private Vector2 _currentDirection = new(1.0f, 0.0f);
        private float _minAnchoredPositionX;
        private float _maxAnchoredPositionX;
        private float _speed;
        
        public ShipRoomManagerBehaviour ShipRoomManagerBehaviour { get; set; }
        public string Id { get; set; }
        public Action OnHovered { get; set; }
        public Action OnCrewMemberStartDrag { get; set; }
        public Action OnCrewMemberEndDrag { get; set; }

        private void InitAnchorPositionBounds()
        {
            var parentRectTransform = transform.parent.GetComponent<RectTransform>();
            _maxAnchoredPositionX = parentRectTransform.rect.width - _rectTransform.rect.width;
            _minAnchoredPositionX = 0.0f;
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _speed = defaultSpeed;
        }

        private void Start()
        {
            InitAnchorPositionBounds();
            
            nameText.text = ShipRoomManagerBehaviour.GetCrewMemberById(Id).Name;

            SetHealth(1.0f);
        }

        private void Update()
        {
            _rectTransform.anchoredPosition += _currentDirection * (_speed * Time.deltaTime);

            var shouldMove = _speed * Time.deltaTime * _currentDirection.x;
            
            if (shouldMove != 0 && (_rectTransform.anchoredPosition.x <= _minAnchoredPositionX || _rectTransform.anchoredPosition.x >= _maxAnchoredPositionX))
            {
                _currentDirection *= -1.0f;
                
                var scale = spriteTransform.localScale;
                scale.x *= -1.0f;
                spriteTransform.localScale = scale;
                
                // Clamp anchored position if out of bounds
                _rectTransform.anchoredPosition = new Vector2(
                    Mathf.Clamp(_rectTransform.anchoredPosition.x, _minAnchoredPositionX, _maxAnchoredPositionX), 
                    _rectTransform.anchoredPosition.y
                );
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            animator.speed = 0.0f;
            _speed = 0.0f;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnCrewMemberStartDrag?.Invoke();
            
            var t = transform;
            
            animator.speed = 0.0f;
            _speed = 0.0f;
            
            _originalParent = t.parent;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = .75f;
            _originalDragAnchoredPosition = _rectTransform.anchoredPosition;
            _currentDirection = Vector2.zero;

            transform.SetParent(t.root);
            transform.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnCrewMemberEndDrag?.Invoke();

            if (eventData.pointerDrag.transform.parent == transform.root)
            {
                transform.SetParent(_originalParent);
                _rectTransform.anchoredPosition = _originalDragAnchoredPosition;
            }
            else
            {
                var rectTransformAnchoredPosition = _rectTransform.anchoredPosition;
                _rectTransform.anchoredPosition = new Vector2(
                    Mathf.Clamp(rectTransformAnchoredPosition.x, _minAnchoredPositionX, _maxAnchoredPositionX), 
                    0.0f
                );
                InitAnchorPositionBounds();
            }
            
            animator.speed = 1.0f;
            _speed = defaultSpeed;
            
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
            _currentDirection = new Vector2(1.0f, 0.0f);
            
            var scale = spriteTransform.localScale;
            scale.x = animationScale;
            spriteTransform.localScale = scale;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }
        
        public void CancelDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
            OnEndDrag(eventData);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null) return;
            animator.speed = 0.0f;
            _speed = 0.0f;
            _rectTransform.SetAsLastSibling();
            
            OnHovered?.Invoke();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == gameObject) return;
            animator.speed = 1.0f;
            _speed = defaultSpeed;
        }
        
        public void SetHealth(float health)
        {
            healthBar.fillAmount = health / 100.0f;
        }
        
    }
}