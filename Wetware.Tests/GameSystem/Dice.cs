using FluentAssertions;
using Wetware.GameSystem;

namespace Wetware.Tests.GameSystem;

public class DiceTests
{
    [Test]
    public void IsSeeded_ReturnsCorrectly()
    {
        Dice.IsSeeded()
            .Should()
            .BeFalse();
        
        Dice.Seed(new Random());
        
        Dice.IsSeeded()
            .Should()
            .BeTrue();
    }

    [Test]
    public void Roll_WithBonus_AddsBonus()
    {
        const int numOfDice = 1;
        const int numOfSides = 1;
        const int expectedResult = 1;

        Dice.Roll(numOfDice, numOfSides)
            .Should()
            .Be(expectedResult);

        const int bonus = 1;
        const int expectedResultWithBonus = expectedResult + bonus;
        
        Dice.Roll(numOfDice, numOfSides, bonus)
            .Should()
            .Be(expectedResultWithBonus);
    }

    [Test]
    public void Roll_Single_Dice_WithinExpectedRange()
    {
        const int numOfDice = 1;
        const int numOfSides = 6;
        
        for (var i = 0; i < 1e2; i++)
            Dice.Roll(numOfDice, numOfSides)
                .Should()
                .BeInRange(numOfDice, numOfSides);
    }

    [Test]
    public void Roll_MultipleDice_WithinExpectedRange()
    {
        const int numberOfDice = 3;
        const int numberOfSides = 20;
        
        for (var i = 0; i < 1e2; i++)
            Dice.Roll(numberOfDice, numberOfSides)
                .Should()
                .BeInRange(numberOfDice, numberOfSides * numberOfDice);
    }
}