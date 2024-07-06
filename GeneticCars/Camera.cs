namespace GeneticCars;

public class Camera
{
    public Vector2 GetFocus(Cars.Car focusedCar, Floor floor)
    {
        Vector2[] vertices = floor.Vertices;
        float y;
        float carX = focusedCar.Fitness;
        if (carX <= vertices[0].X) {
            y = vertices[0].Y;
        } else if (carX >= vertices[^1].X) {
            y = vertices[^1].Y;
        } else {
            int left = 0, right = vertices.Length - 1;
            while (left + 1 < right) {
                int mid = (left + right) / 2;
                if (carX < vertices[mid].X) {
                    right = mid;
                } else {
                    left = mid;
                }
            }
            Vector2 v0 = vertices[left];
            Vector2 v1 = vertices[right];
            y = v0.Y + (v1.Y - v0.Y) / (v1.X - v0.X) * (carX - v0.X);
        }
        return new Vector2(carX, y);
    }
}
