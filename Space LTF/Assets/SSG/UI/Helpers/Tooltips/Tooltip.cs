using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tooltip : UIElement
{
        [SerializeField]
        private RectTransform _mainTransform;
        private GameObject _causeTransform;

//        private Transform baseParent;
        public bool Displayed { get; private set; }
        public TextMeshProUGUI Field;
        private Vector2 _offset;

        public void Init(string info,GameObject causeTransform)
        {
            _causeTransform = causeTransform;
            Field.text = info;
            base.Init();
            transform.SetParent(WindowManager.Instance.TopPanel);
            Show();
        }
        
        protected void Show(Vector2 offset = default(Vector2), float delay = 0)
        {
            _offset = offset;
            
            SetPosition(Input.mousePosition);

            ShowGameObject();
            Displayed = true;
        }

        private void SetPosition(Vector2 position)
        {
            position.x = Mathf.Clamp(position.x, 0, Screen.width - _mainTransform.rect.width);
            position.y = Mathf.Clamp(position.y, 0, Screen.height - _mainTransform.rect.height);

            transform.position = position + _offset;
        }

        private void OnDisable()
        {
            Close();
        }

        public override void Close()
        {
            base.Close();
            Displayed = false;
        }

        private void Update()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            foreach (RaycastResult result in raycastResults)
            {
                var tmpSlot = result.gameObject;
                if (tmpSlot != null)
                {
                    if (tmpSlot == gameObject)
                    {
                        return;
                    }

                    if (_causeTransform == tmpSlot)
                    {
                        return;
                    }
                }
            }
            Close();

//            SetPosition(Input.mousePosition);
        }

        public void SetBaseParent(Transform baseParent)
        {
//            this.baseParent = baseParent;
            transform.SetParent(baseParent, false);
            _mainTransform = baseParent.GetComponent<RectTransform>();

        }
    }
