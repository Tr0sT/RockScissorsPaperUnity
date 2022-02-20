#nullable enable
using System;
using System.Linq;
using LeopotamGroup.Globals;
using NSubstitute;
using NuclearBand;
using NUnit.Framework;
using RSP.Controllers;
using RSP.Models;

public class AIStrategyTests
{
    [Test]
    public void HalfRandomStrategyTest()
    {
        var rspRules = SODatabaseInternal.GetModelsForEdit<RSPRules>(RSPRules.Path).First();
        var game = Substitute.For<RSPGame>();
        Service<RSPGame>.Set(game);
        
        var halfRandomStrategyWithP0 = new HalfRandomStrategy(new HalfRandomAIStrategySettings
        {
           P = 0
        }, rspRules);
        
        var halfRandomStrategyWithP1 = new HalfRandomStrategy(new HalfRandomAIStrategySettings
        {
            P = 1
        }, rspRules);
        
        game.OnPlayerChooseShape += Raise.Event<Action<ShapeType>>(ShapeType.Rock);

        Assert.True(halfRandomStrategyWithP0.ChooseShape() != ShapeType.Paper);
        Assert.True(halfRandomStrategyWithP1.ChooseShape() == ShapeType.Paper);
    }
}
