using osuTK;
using System;

namespace osu.Game.Rulesets.Swing.Extensions
{
    public static class MathExtensions
    {
        public static float Map(float value, float lowerCurrent, float upperCurrent, float lowerTarget, float upperTarget)
        {
            value = Math.Clamp(value, lowerCurrent, upperCurrent);
            return (value - lowerCurrent) / (upperCurrent - lowerCurrent) * (upperTarget - lowerTarget) + lowerTarget;
        }

        public static double Map(double value, double lowerCurrent, double upperCurrent, double lowerTarget, double upperTarget)
        {
            value = Math.Clamp(value, lowerCurrent, upperCurrent);
            return (value - lowerCurrent) / (upperCurrent - lowerCurrent) * (upperTarget - lowerTarget) + lowerTarget;
        }

        public static double Distance(Vector2 input, Vector2 comparison) => Math.Sqrt(Pow(input.X - comparison.X) + Pow(input.Y - comparison.Y));

        public static double Pow(double input) => input * input;

        public static float GetSafeAngle(float angle)
        {
            if (angle < 0)
            {
                while (angle < 0)
                    angle += 360;

                return angle;
            }

            if (angle > 360)
            {
                angle %= 360f;
                return angle;
            }

            return angle;
        }

        public static Vector2 GetRotatedPosition(Vector2 input, Vector2 origin, float angle)
        {
            double newX = origin.X + (input.X - origin.X) * Math.Cos(angle * Math.PI / 180) - ((input.Y - origin.Y) * Math.Sin(angle * Math.PI / 180));
            double newY = origin.Y + (input.Y - origin.Y) * Math.Cos(angle * Math.PI / 180) - ((input.X - origin.X) * Math.Sin(angle * Math.PI / 180));

            return new Vector2((float)newX, (float)newY);
        }
    }
}
