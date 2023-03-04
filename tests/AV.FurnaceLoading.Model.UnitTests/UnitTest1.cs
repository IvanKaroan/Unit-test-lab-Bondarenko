using Xunit;
using AV.FurnaceLoading.Model;
using FluentAssertions;
using System.Collections.Generic;

namespace AV.FurnaceLoading.Model.UnitTests;

public class UnitTest1
{
  
    public static IEnumerable<object[]> TestDataTrueWithBox =>
       new List<object[]>
        {
                //Круг с безопасной зоной близкой к границе
                new object[] {new CircleLoadPlace(40, new Coordinate(101, 101)), new Cassette(202, 202)},
                //Прямоугольник параллельный осям
                new object[] {new RectLoadPlace(60, 100, RectLoadPlace.RotationType.None, new Coordinate(131, 111)), new Cassette(230, 270)},
                //Прямоугольник повернутый на 45 градусов с безопасной зоной близкой к границе
                new object[] {new RectLoadPlace(60, 100, RectLoadPlace.RotationType.By45Degree, new Coordinate(171, 171)), new Cassette(350, 350)},
                //Первый прямоугольник повёрнут на 135 градусов с безопасной зоной близкой к границе
                new object[] {new RectLoadPlace(60, 100, RectLoadPlace.RotationType.By135Degree, new Coordinate(171, 171)), new Cassette(350, 350) }
        };
  
    [Theory, MemberData(nameof(TestDataTrueWithBox))]
    public void ShouldBeTrueValidWithBox(LoadPlace place, Cassette cassette)
    {
        LoadSchema load = new LoadSchema();
        load.Places.Add(place);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeTrue();
    }


    public static IEnumerable<object[]> TestDataFalseWithBox =>
        new List<object[]>
        {
                //Круг с безопасной зоной касающейся границ
                new object[] {new CircleLoadPlace(40, new Coordinate(100, 100)), new Cassette(200, 200)},
                //Прямоугольник параллельный осям, который нижней частью безопасной зоной касается границы
                new object[] {new RectLoadPlace(60, 100, RectLoadPlace.RotationType.None, new Coordinate(131, 110)), new Cassette(230, 270)},
                //Прямоугольник повернутый на 45 градусов, который нижней частью безопасной зоной касается границы
                new object[] {new RectLoadPlace(60, 100, RectLoadPlace.RotationType.By45Degree, new Coordinate(170, 150)), new Cassette(350, 350)},
                //Первый прямоугольник повёрнут на 135 градусов, который верхней частью безопасной зоной касается границы
                new object[] {new RectLoadPlace(60, 100, RectLoadPlace.RotationType.By135Degree, new Coordinate(170, 170)), new Cassette(320, 350) }
        };
    [Theory, MemberData(nameof(TestDataFalseWithBox))]
    public void ShouldBeFalseValidWithBox(LoadPlace place, Cassette cassette)
    {
        LoadSchema load = new LoadSchema();
        load.Places.Add(place);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldBeTrueValidCircleVsRect45Degree()
    {
        Cassette cassette = new Cassette(700, 700);

        CircleLoadPlace circle = new CircleLoadPlace(40, new Coordinate(110, 110));
        RectLoadPlace rect = new RectLoadPlace(60, 60, RectLoadPlace.RotationType.By45Degree, new Coordinate(240, 240));

        LoadSchema load = new LoadSchema();
        load.Places.Add(circle);
        load.Places.Add(rect);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldBeTrueValidCircleVsCircle()
    {
        Cassette cassette = new Cassette(240, 360);
        CircleLoadPlace circleLeft = new CircleLoadPlace(40, new Coordinate(110, 120));
        CircleLoadPlace circleRight = new CircleLoadPlace(60, new Coordinate(221, 120));

        LoadSchema load = new LoadSchema();
        load.Places.Add(circleLeft);
        load.Places.Add(circleRight);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldBeFalseValidBigCircleVSSmallCircle()
    {
        Cassette cassette = new Cassette(500, 500);
        CircleLoadPlace circleLeft = new CircleLoadPlace(210, new Coordinate(250, 250));
        CircleLoadPlace circleRight = new CircleLoadPlace(40, new Coordinate(110, 110));

        LoadSchema load = new LoadSchema();
        load.Places.Add(circleLeft);
        load.Places.Add(circleRight);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeFalse();
    }

    [Fact]
    public void Ura()
    {
        Cassette cassette = new Cassette(700, 500);
        RectLoadPlace rect = new RectLoadPlace( 50, 50, RectLoadPlace.RotationType.None, new Coordinate(110, 110));
        RectLoadPlace rectRight = new RectLoadPlace( 50, 50, RectLoadPlace.RotationType.None, new Coordinate(220, 110));
        RectLoadPlace rectUp = new RectLoadPlace( 50, 50, RectLoadPlace.RotationType.None, new Coordinate(110, 220));

        LoadSchema load = new LoadSchema();
        load.Places.Add(rect);
        load.Places.Add(rectRight);
        load.Places.Add(rectUp);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeFalse();
    }

    [Fact]
    public void Timur()
    {
        RectLoadPlace rect1 = new RectLoadPlace( 40, 40, RectLoadPlace.RotationType.None, new Coordinate(100, 300));
        RectLoadPlace rect = new RectLoadPlace( 40, 40, RectLoadPlace.RotationType.None, new Coordinate(140, 340));
        //В точке 120 320 пересекаются
        Collision te = new Collision();
        var Res = te.RectVsRectCheck(rect1, rect);
        Res.Should().BeTrue();
    }

    public static IEnumerable<object[]> TestDataRect =>
        new List<object[]>
        {
            //Два прямоугольника параллельных осям
            new object[] {new RectLoadPlace(50, 100, RectLoadPlace.RotationType.None, new Coordinate(140, 120)),
                new RectLoadPlace(80, 40, RectLoadPlace.RotationType.None, new Coordinate(280, 140)) },
            
            //Первый прямоугольник повёрнут на 45 градусов, второй паралелен осям
            new object[] {new RectLoadPlace(50, 100, RectLoadPlace.RotationType.By45Degree, new Coordinate(240, 220)),
                new RectLoadPlace(80, 40, RectLoadPlace.RotationType.None, new Coordinate(360, 360)) },

            //Первый прямоугольник повёрнут на 135 градусов, второй паралелен осям
            new object[] {new RectLoadPlace(50, 100, RectLoadPlace.RotationType.By135Degree, new Coordinate(240, 220)),
                new RectLoadPlace(80, 40, RectLoadPlace.RotationType.None, new Coordinate(360, 340)) }
        };
    
    [Theory, MemberData(nameof(TestDataRect))]
    public void ShouldBeTrueValidRectVsRect(RectLoadPlace leftRect, RectLoadPlace rightRect)
    {
        Cassette cassette = new Cassette(1000, 1000);

        LoadSchema load = new LoadSchema();
        load.Places.Add(leftRect);
        load.Places.Add(rightRect);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldBeFalseRect45InRect()
    {
        Cassette cassette = new Cassette(1000, 1000);

        RectLoadPlace leftRect = new RectLoadPlace(100, 100, RectLoadPlace.RotationType.None, new Coordinate(500, 500));
        RectLoadPlace rightRect = new RectLoadPlace(20, 20, RectLoadPlace.RotationType.By45Degree, new Coordinate(500, 500));


        LoadSchema load = new LoadSchema();
        load.Places.Add(leftRect);
        load.Places.Add(rightRect);

        SchemaValidator validator = new SchemaValidator();
        var result = validator.SchemaValidFor(load, cassette);
        result.Should().BeFalse();
    }
}
