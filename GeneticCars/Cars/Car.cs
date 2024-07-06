﻿using GeneticCars.Generation;
using nkast.Aether.Physics2D.Dynamics.Joints;

namespace GeneticCars.Cars;

public class Car : Individual, IIndividualFactory<Car>
{
    public const int WheelCount = 2;
    public const int ChassisCount = 12;
    public const float MotorSpeed = 20f;

    private static readonly SKFont _carFont = SKTypeface.FromFamilyName("Arial").ToFont(0.5f);


    private readonly Vertices _chassisVertices;
    private readonly Body _chassis;
    private readonly Body[] _wheels = new Body[2];

    public static readonly FloatRange WheelRadiusRange = new(0.2f, 0.5f);
    public static readonly FloatRange WheelDensityRange = new(40f, 100f);
    public static readonly FloatRange WheelIndexRange = new(0, 8);
    public static readonly FloatRange ChassisDensityRange = new(30f, 300f);
    public static readonly FloatRange ChassisAxisRange = new(0.1f, 1.1f);

    public Car(Class @class, Gene[] genes, int generation, string name, World world, Vector2 position)
        : base(@class, genes, generation, name)
    {
        Health = Game.MaxCarHealth;
        (_chassis, _chassisVertices) = CreateChassis(world, position);
        float carMass = _chassis.Mass;
        for (int i = 0; i < WheelCount; i++) {
            _wheels[i] = CreateWheel(world, position, i);
            carMass += _wheels[i].Mass;
        }

        for (int i = 0; i < WheelCount; i++) {
            float torque = carMass * Game.Gravity / WheelRadius(i).Value;
            Vector2 wheelCenter = _chassisVertices[WheelIndex(i).IntValue];
            var joint = new RevoluteJoint(_chassis, _wheels[i], wheelCenter, Vector2.Zero) {
                MaxMotorTorque = torque,
                MotorSpeed = -MotorSpeed,
                MotorEnabled = true
            };
            world.Add(joint);
        }
    }

    public static Car Create(Class @class, Gene[] genes, int generation, string name, World world, Vector2 position)
        => new(@class, genes, generation, name, world, position);

    public static Car CreateRandom(World world, Vector2 position)
    {
        Gene[] genes = [
            new(WheelRadiusRange),
            new(WheelRadiusRange),
            new(WheelDensityRange),
            new(WheelDensityRange),
            new(WheelIndexRange),
            new(WheelIndexRange),

            new(ChassisDensityRange),

            new(ChassisAxisRange),
            new(ChassisAxisRange),
            new(ChassisAxisRange),
            new(ChassisAxisRange),

            new(ChassisAxisRange),
            new(ChassisAxisRange),
            new(ChassisAxisRange),
            new(ChassisAxisRange),

            new(ChassisAxisRange),
            new(ChassisAxisRange),
            new(ChassisAxisRange),
            new(ChassisAxisRange),
        ];
        while (genes[5].IntValue == genes[4].IntValue) {
            genes[5] = new(WheelIndexRange);
        }
        return new Car(Class.New, genes, 0, GenerateName(6), world, position);
    }

    public override float Fitness => _chassis.Position.X;

    public Body Body => _chassis;

    private (Body, Vertices) CreateChassis(World world, Vector2 position)
    {
        // Chassis ![](..\..\Documentation\ChassisIndexes.png;;;0.03403,0.03054)

        Body chassis = world.CreateBody(position, 0f, BodyType.Dynamic);
        Vertices chassisVertices = [
            new(ChassisAxis(0).Value, 0f),
            new(ChassisAxis(1).Value, ChassisAxis(2).Value),
            new(0f, ChassisAxis(3).Value),
            new(-ChassisAxis(4).Value, ChassisAxis(5).Value),
            new(-ChassisAxis(6).Value, 0f),
            new(-ChassisAxis(7).Value, -ChassisAxis(8).Value),
            new(0f, -ChassisAxis(9).Value),
            new(ChassisAxis(10).Value, -ChassisAxis(11).Value)
        ];
        var fixture = chassis.CreatePolygon(chassisVertices, ChassisDensity.Value);
        fixture.Friction = 10;
        fixture.Restitution = 0.2f;
        fixture.CollisionGroup = -1;
        return (chassis, chassisVertices);
    }

    private Body CreateWheel(World world, Vector2 position, int i)
    {
        Body wheel = world.CreateBody(position, 0f, BodyType.Dynamic);
        var fixture = wheel.CreateCircle(
            radius: WheelRadius(i).Value,
            density: WheelDensity(i).Value,
            offset: Vector2.Zero);
        fixture.Friction = 1;
        fixture.Restitution = 0.2f;
        fixture.CollisionGroup = -1;
        return wheel;
    }

    public Gene WheelRadius(int index) => Genome[index];
    public Gene WheelDensity(int index) => Genome[WheelCount + index];
    public Gene WheelIndex(int index) => Genome[2 * WheelCount + index];
    public Gene ChassisDensity => Genome[3 * WheelCount];


    public Gene ChassisAxis(int index) => Genome[3 * WheelCount + 1 + index];

    public void RemoveFrom(World world)
    {
        world.Remove(_chassis);
        foreach (Body wheel in _wheels) {
            world.Remove(wheel);
        }
    }

    public void Draw(SKCanvas canvas)
    {
        var matrix = canvas.TotalMatrix;
        canvas.Scale(1, -1, 0, _chassis.Position.Y);
        string text = Name + " " + Generation;   // $"{_chassis.LinearVelocity.X:n3}  H = {Health}"
        SKPaint textPaint = IsAlive ? _textPaint : _deadTextPaint;
        canvas.DrawText(text, _chassis.Position.X - 1, _chassis.Position.Y - 3, _carFont, textPaint);
        canvas.SetMatrix(matrix);

        // Wheels
        for (int i = 0; i < WheelCount; i++) {
            var wheelCenter = _wheels[i].Position;
            using var wheelFillPaint = CreateNeutralFillPaint(WheelDensity(i).Fraction);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, WheelRadius(i).Value, wheelFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, WheelRadius(i).Value, NeutralStrokePaint);
        }

        // Chassis
        canvas.RotateRadians(_chassis.Rotation, _chassis.Position.X, _chassis.Position.Y);
        using var path = new SKPath();
        Vector2 vector = _chassisVertices[0] + _chassis.Position;
        path.MoveTo(vector.X, vector.Y);
        for (int i = 1; i < _chassisVertices.Count; i++) {
            vector = _chassisVertices[i] + _chassis.Position;
            path.LineTo(vector.X, vector.Y);
        }
        path.Close();

        SKPaint chassisStrokePaint = ColoredStrokePaint;
        using SKPaint chassisFillPaint = CreateColoredFillPaint(ChassisDensity.Fraction);
        canvas.DrawPath(path, chassisFillPaint);
        canvas.DrawPath(path, chassisStrokePaint);
        foreach (Vector2 v in _chassisVertices) {
            var v1 = v + _chassis.Position;
            canvas.DrawLine(_chassis.Position.X, _chassis.Position.Y, v1.X, v1.Y, chassisStrokePaint);
        }
        canvas.SetMatrix(matrix);

        // Wheel hubs
        for (int i = 0; i < WheelCount; i++) {
            var wheelCenter = _wheels[i].Position;
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, 0.07f, _neutralFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, 0.07f, chassisStrokePaint);
        }
    }

    public override string ToString() => $"""Car[{Class} "{Name} {Generation}", X={_chassis.Position.X:n2}, Velocity={_chassis.LinearVelocity.X:n2}]""";
}
