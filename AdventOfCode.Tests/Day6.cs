using FluentAssertions;

namespace AdventOfCode.Tests;

public class Day6
{
	// time
	// holdTime
	// distance
    
    // distance = holdTime * ( time - holdTime )
    
    // a = holdTime
    // b = time
    // c = distance
    
    // ax2 + bx + c
    
    // Solve for x
    // x = -b +- root b2 -4ac / 2a
    
    // a = 
    
    // x = -10

    [Test]
    public void EvaluationTest()
    {
	    float time = 7;
	    Evaluate(0, time).Should().Be(0);
	    Evaluate(1, time).Should().Be(6);
	    Evaluate(2, time).Should().Be(10);
	    Evaluate(3, time).Should().Be(12);
	    Evaluate(4, time).Should().Be(12);
	    Evaluate(5, time).Should().Be(10);
	    Evaluate(6, time).Should().Be(6);
	    Evaluate(7, time).Should().Be(0);
    }

    [Test]
    public void WaysToWinTest()
    {
	    /*
			Time:      7  15   30
			Distance:  9  40  200
	     */

	    WaysToWin(7, 9).Should().Be(4);
	    WaysToWin(15, 40).Should().Be(8);
	    WaysToWin(30, 200).Should().Be(9);
    }
    
    private int WaysToWin(int time, long distance)
    {
	    int waysToWin = 0;
	    for (int i = 0; i < time; i++)
	    {
		    if (Evaluate(i, time) >= distance)
		    {
			    waysToWin++;
		    }
	    }

	    return waysToWin;
    }

    private float Evaluate(float holdTime, float time)
    {
	    return holdTime * (time - holdTime);
    }

    [Test]
    public void Part1()
    {
	    int answer = 4403592;
	    
	    var a = WaysToWin(49, 263);
	    var b = WaysToWin(97, 1532);
	    var c = WaysToWin(94, 1378);
	    var d = WaysToWin(94, 1851);
	    
	    (a*b*c*d).Should().Be(answer);
    }

    [Test]
    public void part2Example()
    {
	    WaysToWin(71530, 940200).Should().Be(71503);
    }

    [Test]
    public void Part2()
    {
	    // 38017588 too high
	    Console.WriteLine(WaysToWin(49979494, 263153213781851));
    }
}