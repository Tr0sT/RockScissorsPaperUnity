#nullable enable
using System;
using System.Collections.Generic;
using DG.Tweening;
using RSP.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RSP.Views
{
    // вьюшку делал последней, поэтому такая каша, очень много времени в целом уже потратил на задание
    public class RSPShapeView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ShapeType shapeType;
        public ShapeType ShapeType => shapeType;

        [SerializeField] private List<SpriteRenderer> sprites = new();
        
        public const float AnimationDuration = 0.5f;

        private Action<ShapeType> onChooseShape = null!;

        private bool active;

        public void Init(Action<ShapeType> onChooseShape)
        {
            this.onChooseShape = onChooseShape;
            gameObject.SetActive(true);
            sprites.ForEach(s => s.color += new Color(0, 0, 0, -s.color.a + 1.0f));
            active = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!active)
                return;
            
            onChooseShape.Invoke(ShapeType);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            DOVirtual.Float(0.0f, 1.0f, AnimationDuration, v =>
            {
                sprites.ForEach(s => s.color += new Color(0, 0, 0, -s.color.a + v));
            });
        }
        
        public void Hide()
        {
            Disable();
            DOVirtual.Float(1.0f, 0.0f, AnimationDuration, v =>
            {
                sprites.ForEach(s => s.color += new Color(0, 0, 0, -s.color.a + v));
            });
        }

        public void Disable()
        {
            active = false;
        }

        public void DeInit()
        {
            Disable();
            gameObject.SetActive(false);
        }
    }
}