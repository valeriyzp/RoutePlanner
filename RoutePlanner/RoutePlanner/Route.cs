using System.Collections.Generic;

namespace RoutePlanner
{
    public class Route : List<Chain> { }

    public class Chain
    {
        public City City { get; set; } = new City();
        public double Length { get; set; } = 0;
        public bool Target { get; set; } = false;

        public Chain(City city, double length, bool target)
        {
            City = city;
            Length = length;
            Target = target;
        }

        public override string ToString()
        {
            string res = "";
            if (!Target)
                res += "    ";
            res += $"{City.Ukr} ({Length} км)";
            return res;
        }
    }
}
