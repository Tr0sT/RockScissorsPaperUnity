#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RSP.Controllers;
using RSP.Models;
using Sirenix.Utilities;
using UnityEngine;

namespace RSP.Views
{
    public class RoundSequenceAnimation : MonoBehaviour
    {
        [SerializeField] private List<RSPShapeView> enemyShapeViews = new();

        [SerializeField] private TextMesh gameResultText = null!;
        
        private List<RSPShapeView> activeShapeViews = null!;
        
        public void Init(List<RSPShapeView> activeShapeViews)
        {
            this.activeShapeViews = activeShapeViews;
        }
        
        public void Launch(ShapeType playerShape, ShapeType enemyShape, RSPGameResult gameResult, Action onChooseAnimationEnd)
        {
            activeShapeViews.Where(v => v.ShapeType != playerShape).ForEach(v => v.Hide());

            var enemyShapeView = enemyShapeViews.Find(v => v.ShapeType == enemyShape);
            enemyShapeView.Show();

            gameResultText.text = gameResult.ToString();
            DOVirtual.DelayedCall(RSPShapeView.AnimationDuration / 2, () => gameResultText.gameObject.SetActive(true));
            
            DOVirtual.DelayedCall(RSPShapeView.AnimationDuration, () => onChooseAnimationEnd());
        }

        public void Stop()
        {
            enemyShapeViews.ForEach(v => v.DeInit());
            gameResultText.gameObject.SetActive(false);
        }
    }
}