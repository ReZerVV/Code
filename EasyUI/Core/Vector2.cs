namespace EasyUI.Core
{
    public class Vector2
    {
        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2 vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        public static bool operator ==(Vector2? left, Vector2? right)
        {
            return EqualityComparer<Vector2>.Default.Equals(left, right);
        }

        public static bool operator !=(Vector2? left, Vector2? right)
        {
            return !(left == right);
        }

        public static Vector2 operator +(Vector2? left, Vector2? right)
        {
            return new Vector2(left.x + right.x, left.y + right.y);
        }

        public static Vector2 operator +(Vector2? left, int right)
        {
            return new Vector2(left.x + right, left.y + right);
        }

        public static Vector2 operator -(Vector2? left, Vector2? right)
        {
            return new Vector2(left.x - right.x, left.y - right.y);
        }

        public static Vector2 operator -(Vector2? left, int right)
        {
            return new Vector2(left.x - right, left.y - right);
        }

        public static Vector2 Zero
        {
            get
            {
                return new Vector2(0, 0);
            }
        }


    }
}
