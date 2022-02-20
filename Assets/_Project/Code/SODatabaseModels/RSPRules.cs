#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using NuclearBand;
using Sirenix.OdinInspector;

namespace RSP.Models
{
    public enum ShapeType
    {
        Rock,
        Scissors,
        Paper,
        Lizard,
        Spock
    }
    
    public enum RSPGameResult
    {
        Win,
        Lose,
        Draw
    }

    public class ShapeRule
    {
        public ShapeType Shape;
        public List<ShapeType> Victims = new();
    }

    public class RSPRules : DataNode
    {
        public static string Path = "Rules";

        [InfoBox("@Verify()", InfoMessageType.Error, "HasError")]
        public List<ShapeRule> ShapeRules = new();
        
        #region Verify
        private string? Verify()
        {
            var validators = new List<Func<int, string?>>
            {
                ErrorIfDuplicates, ErrorIfWrongVictim, ErrorIfImmortal
            };
            
            // SelectMany ;)
            return validators
                .SelectMany(v => ShapeRules.Select((_, i) => v(i)))
                .FirstOrDefault(e => e != null);
        }

        private bool HasError() => Verify() != null;

        private string? ErrorIfDuplicates(int shapeRuleIndex)
        {
            var shapeRule = ShapeRules[shapeRuleIndex];
            for (var j = shapeRuleIndex + 1; j < ShapeRules.Count; j++)
                if (shapeRule.Shape == ShapeRules[j].Shape)
                    return $"{shapeRuleIndex + 1} & {j + 1} are same";

            return null;
        }

        private string? ErrorIfWrongVictim(int shapeRuleIndex)
        {
            var shapeRule = ShapeRules[shapeRuleIndex];
            for (var j = 0; j < shapeRule.Victims.Count; j++)
                if (shapeRule.Victims[j] == shapeRule.Shape)
                    return $"{shapeRuleIndex + 1} shape has wrong {j + 1} victim";

            return null;
        }

        private string? ErrorIfImmortal(int shapeRuleIndex)
        {
            var shapeRule = ShapeRules[shapeRuleIndex];
            var immortal = ShapeRules
                .Where((t, j) => shapeRuleIndex != j)
                .All(t => !t.Victims.Contains(shapeRule.Shape));

            return immortal ? $"{shapeRuleIndex + 1} shape is immortal" : null;
        }

        #endregion
    }
}