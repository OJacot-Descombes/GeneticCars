﻿using GeneticCars.Evolution;
using nkast.Aether.Physics2D.Common.Decomposition;
using nkast.Aether.Physics2D.Dynamics.Joints;

namespace GeneticCars.Cars;

public partial class Car : Individual, IIndividualFactory<Car>
{
    public const int WheelCount = 2;
    public const int ChassisCount = 12;
    public const float MotorSpeed = 20f;

    private readonly Vertices _chassisVertices;
    private readonly Body _chassis;
    private readonly Body[] _wheels = new Body[2];
    private readonly RevoluteJoint[] _joints = new RevoluteJoint[2];
    public static readonly FloatRange WheelRadiusRange = new(0.2f, 0.5f);
    public static readonly FloatRange WheelDensityRange = new(40f, 100f);
    public static readonly FloatRange FirstWheelIndexRange = new(0, 2);
    public static readonly FloatRange SecondWheelIndexDeltaRange = new(1, 4);
    private static readonly FloatRange ChassisDensityRange = new(30f, 300f);
    private static readonly FloatRange ChassisAxisRange = new(0.1f, 1.1f);

    public Car(Class @class, Gene[] genes, Identity identity, Individual? ancestor1, Individual? ancestor2,
        World world, Vector2 position)
        : base(@class, genes, identity, ancestor1, ancestor2)
    {
        _lastLabelY = position.Y + 3;
        Health = Game.MaxCarHealth;
        (_chassis, _chassisVertices) = CreateChassis(world, position);
        float carMass = _chassis.Mass;
        for (int i = 0; i < WheelCount; i++) {
            _wheels[i] = CreateWheel(world, position + GetWheelCassisPosition(i), i);
            carMass += _wheels[i].Mass;
        }

        for (int i = 0; i < WheelCount; i++) {
            float torque = carMass * Game.Gravity / WheelRadius(i).Value;
            Vector2 wheelCenter = GetWheelCassisPosition(i);
            var joint = new RevoluteJoint(_chassis, _wheels[i], wheelCenter, Vector2.Zero) {
                MaxMotorTorque = torque,
                MotorSpeed = -MotorSpeed,
                MotorEnabled = true
            };
            _joints[i] = joint;
            world.Add(joint);
        }
        _chassis.LinearVelocity = new Vector2(10, 0);
    }

    public static Car Create(Class @class, Gene[] genes, Identity identity,
        Individual? ancestor1, Individual? ancestor2,
        World world, Vector2 position)
        => new(@class, genes, identity, ancestor1, ancestor2, world, position);

    public static Car CreateRandom(World world, Vector2 position, Class @class = Class.New)
    {
        Gene[] genes = [
            new(WheelRadiusRange),
            new(WheelRadiusRange),
            new(WheelDensityRange),
            new(WheelDensityRange),
            new(FirstWheelIndexRange),
            new(SecondWheelIndexDeltaRange),

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
        return new Car(@class, genes, new Identity(Name.GenerateRandom(6), 0, 0), null, null, world, position);
    }

    private float _maxFitness;
    public override float Fitness
    {
        get {
            if (IsAlive) {
                _maxFitness = Math.Max(_maxFitness, _chassis.Position.X);
            }
            return _maxFitness;
        }
    }

    public float LastPositionX { get; set; }

    public Body Body => _chassis;

    protected override void Dying()
    {
        foreach (var joint in _joints) {
            joint.MotorSpeed = 0;
        }
        _chassis.AngularDamping = 10f;
    }

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
        AlignWheelsHorizontally(chassisVertices);

        // Collisions are not accurately calculated for concave polygons, so we divide them into convex polygons.
        List<Vertices> convexPieces =
            Triangulate.ConvexPartition(chassisVertices, TriangulationAlgorithm.Flipcode);
        var fixtures = chassis.CreateCompoundPolygon(convexPieces, ChassisDensity.Value);
        foreach (var fixture in fixtures) {
            fixture.Friction = 10;
            fixture.Restitution = 0.2f;
            fixture.CollisionGroup = -1;
        }

        return (chassis, chassisVertices);
    }

    private void AlignWheelsHorizontally(Vertices chassisVertices)
    {
        int w0 = GetWheelCassisIndex(1);
        int w1 = GetWheelCassisIndex(0);

        // Align wheel axis
        Vector2 delta = chassisVertices[w1] - chassisVertices[w0];
        delta.Y = -delta.Y;
        delta.Normalize();
        for (int i = 0; i < chassisVertices.Count; i++) {
            chassisVertices[i] = new Vector2(
                delta.X * chassisVertices[i].X - delta.Y * chassisVertices[i].Y,  // cos * x - sin * y
                delta.Y * chassisVertices[i].X + delta.X * chassisVertices[i].Y); // sin * x + cos * y
        }

        // Correction for unequal wheel radii
        float dr = WheelRadius(0).Value - WheelRadius(1).Value;
        float dx = chassisVertices[w1].X - chassisVertices[w0].X;
        if (dx > MathF.Abs(dr) + 0.01f) {
            float sin = dr / dx;
            float cos = MathF.Sqrt(1f - sin * sin);
            for (int i = 0; i < chassisVertices.Count; i++) {
                chassisVertices[i] = new Vector2(
                    cos * chassisVertices[i].X - sin * chassisVertices[i].Y,
                    sin * chassisVertices[i].X + cos * chassisVertices[i].Y);
            }
        }
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
    public Gene FirstWheelIndex => Genome[2 * WheelCount];
    public Gene SecondWheelIndexDelta => Genome[2 * WheelCount + 1];
    public Gene ChassisDensity => Genome[3 * WheelCount];

    public int GetWheelCassisIndex(int wheelIndex)
    {
        int value = FirstWheelIndex.IntValue;
        (int firstIndex, int reference) = value == 0
            ? (0, 8)
            : (7, 7);
        return wheelIndex == 0
            ? firstIndex
            : reference - SecondWheelIndexDelta.IntValue;
    }

    private Vector2 GetWheelCassisPosition(int wheelIndex) => _chassisVertices[GetWheelCassisIndex(wheelIndex)];

    public Gene ChassisAxis(int index) => Genome[3 * WheelCount + 1 + index];

    public void RemoveFrom(World world)
    {
        world.Remove(_chassis);
        foreach (Body wheel in _wheels) {
            world.Remove(wheel);
        }
    }

    public override string ToString() => $"""Car[{Class} "{Identity}", X={_chassis.Position.X:n2}, Velocity={_chassis.LinearVelocity.X:n2}]""";
}
