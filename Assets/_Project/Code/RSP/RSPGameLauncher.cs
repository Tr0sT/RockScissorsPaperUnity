#nullable enable
using System.Collections.Generic;
using NuclearBand;
using RSP.Models;
using RSP.Views;
using UnityEngine;

namespace RSP.Controllers
{
    public class RSPGameLauncher : MonoBehaviour
    {
        public const string Path = "RockScissorsPaperGame";
        public List<RSPShapeView> ShapeViews = new();
        
        [SerializeField] private RoundSequenceAnimation roundSequenceAnimation = null!;
        public RoundSequenceAnimation RoundSequenceAnimation => roundSequenceAnimation; 
        
        public RSPGame Launch()
        {
            ShapeViews.ForEach(shapeView => shapeView.gameObject.SetActive(false));
            roundSequenceAnimation.Stop();
            
            var rspSettings = SODatabase.GetModel<RSPSettings>(RSPSettings.Path);
            
            return new RSPGame(this , rspSettings).Launch();
        }
    }
}