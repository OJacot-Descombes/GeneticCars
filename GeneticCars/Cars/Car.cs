using GeneticCars.Generation;
using Microsoft.VisualBasic;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Joints;

namespace GeneticCars.Cars;

public class Car : Individual
{
    public const int WheelCount = 2;
    public const int ChassisCount = 12;
    public const float MotorSpeed = 20f;

    private static readonly SKPaint _eliteChassisStrokePaint = new() {
        Color = SKColors.Blue,
        IsStroke = true,
        IsAntialias = true
    };
    private static readonly SKPaint _chassisStrokePaint = new() {
        Color = SKColors.Chocolate,
        IsStroke = true,
        IsAntialias = true
    };
    private static readonly SKPaint _wheelHubFillPaint = new() {
        Color = SKColors.White,
        IsStroke = false,
        IsAntialias = false
    };
    private static readonly SKPaint _wheelStrokePaint = new() {
        Color = SKColors.Black,
        IsStroke = true,
        IsAntialias = true
    };

    private readonly Vertices _chassisVertices;
    private readonly Body _chassis;
    private readonly Body[] _wheels = new Body[2];

    public static readonly FloatRange WheelRadiusRange = new(0.2f, 0.5f);
    public static readonly FloatRange WheelDensityRange = new(40f, 100f);
    public static readonly FloatRange WheelIndexRange = new(0, 8);
    public static readonly FloatRange ChassisDensityRange = new(30f, 300f);
    public static readonly FloatRange ChassisAxisRange = new(0.1f, 1.1f);

    public Car(Gene[] genes, int generation, string name, World world, Vector2 position) : base(genes, generation, name)
    {
        (_chassis, _chassisVertices) = CreateChassis(world, position);
        float carMass = _chassis.Mass;
        for (int i = 0; i < WheelCount; i++) {
            _wheels[i] = CreateWheel(world, i);
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

    private Body CreateWheel(World world, int i)
    {
        Body wheel = world.CreateBody(Vector2.Zero, 0f, BodyType.Dynamic);
        var fixture = wheel.CreateCircle(
            radius: WheelRadius(i).Value,
            density: WheelDensity(i).Value,
            offset: Vector2.Zero);
        fixture.Friction = 1;
        fixture.Restitution = 0.2f;
        fixture.CollisionGroup = -1;
        return wheel;
    }

    public Gene WheelRadius(int index) => Genes[index];
    public Gene WheelDensity(int index) => Genes[WheelCount + index];
    public Gene WheelDensityFraction(int index) => Genes[WheelCount + index];
    public Gene WheelIndex(int index) => Genes[2 * WheelCount + index];
    public Gene ChassisDensity => Genes[3 * WheelCount];
    public Gene ChassisDensityFraction => Genes[3 * WheelCount];
    public Gene ChassisAxis(int index) => Genes[3 * WheelCount + 1 + index];

    public bool IsElite { get; set; }

    public void Draw(SKCanvas canvas)
    {
        // Wheels
        for (int i = 0; i < WheelCount; i++) {
            var wheelCenter = _wheels[i].Position;//  _chassisVertices[WheelIndex(i).IntValue] + _chassis.Position;
            using var wheelFillPaint = GetWheelFillPaint(i);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, WheelRadius(i).Value, wheelFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, WheelRadius(i).Value, _wheelStrokePaint);
        }

        // Chassis
        var matrix = canvas.TotalMatrix;
        canvas.RotateRadians(_chassis.Rotation, _chassis.Position.X, _chassis.Position.Y);
        using var path = new SKPath();
        Vector2 vector = _chassisVertices[0] + _chassis.Position;
        path.MoveTo(vector.X, vector.Y);
        for (int i = 1; i < _chassisVertices.Count; i++) {
            vector = _chassisVertices[i] + _chassis.Position;
            path.LineTo(vector.X, vector.Y);
        }
        path.Close();

        using SKPaint chassisFillPaint = GetChassisFillPaint();
        SKPaint chassisStrokePaint = IsElite ? _eliteChassisStrokePaint : _chassisStrokePaint;
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
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, 0.07f, _wheelHubFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, 0.07f, chassisStrokePaint);
        }
    }

    private SKPaint GetChassisFillPaint()
    {
        SKColor light = IsElite ? SKColors.LightCyan : SKColors.PapayaWhip;
        SKColor dark = IsElite ? SKColors.SkyBlue : SKColors.SandyBrown;
        float p = ChassisDensity.Fraction;
        float r = 1f - p;
        var color = new SKColor(
            (byte)(light.Red * p + dark.Red * r),
            (byte)(light.Green * p + dark.Green * r),
            (byte)(light.Blue * p + dark.Blue * r));
        return new SKPaint {
            Color = color,
            IsStroke = false,
            IsAntialias = false
        };

    }

    private SKPaint GetWheelFillPaint(int index)
    {
        SKColor light = SKColors.Gainsboro;
        SKColor dark = SKColors.DimGray;
        float p = WheelDensity(index).Fraction;
        float r = 1f - p;
        var color = new SKColor(
            (byte)(light.Red * p + dark.Red * r),
            (byte)(light.Green * p + dark.Green * r),
            (byte)(light.Blue * p + dark.Blue * r));
        return new SKPaint {
            Color = color,
            IsStroke = false,
            IsAntialias = false
        };

    }

    public static Car CreateRandom(int generation, World world, Vector2 position)
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
        return new Car(genes, generation, "name", world, position);
    }
}
