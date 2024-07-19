using System;

namespace RefractionConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter refraction (e.g. +0.50 -3.25 x070):");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No input provided. Exiting...");
                return;
            }

            try
            {
                var refraction = ParseRefraction(input);
                var convertedRefraction = ConvertRefraction(refraction);
                Console.WriteLine($"Converted refraction: {FormatRefraction(convertedRefraction)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static (double sphere, double cylinder, int axis) ParseRefraction(string input)
        {
            var parts = input.Split(new[] { ' ', 'x' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid refraction format.");
            }

            double sphere = double.Parse(parts[0]);
            double cylinder = double.Parse(parts[1]);
            int axis = int.Parse(parts[2]);

            if (axis < 1 || axis > 180)
            {
                throw new ArgumentException("Axis must be between 001 and 180.");
            }

            return (sphere, cylinder, axis);
        }

        static (double sphere, double cylinder, int axis) ConvertRefraction((double sphere, double cylinder, int axis) refraction)
        {
            double sphere = refraction.sphere + refraction.cylinder;
            double cylinder = -refraction.cylinder;
            int axis = (refraction.axis + 90) % 180;
            if (axis == 0) axis = 180;

            return (sphere, cylinder, axis);
        }

        static string FormatRefraction((double sphere, double cylinder, int axis) refraction)
        {
            string sphereStr = refraction.sphere.ToString("0.00");
            string cylinderStr = refraction.cylinder > 0 ? $"+{refraction.cylinder:0.00}" : refraction.cylinder.ToString("0.00");
            string axisStr = refraction.axis.ToString("D3");

            return $"{sphereStr} {cylinderStr} x{axisStr}";
        }
    }
}
