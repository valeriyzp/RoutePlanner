using System.Collections.Generic;

namespace RoutePlanner
{
    public class CitiesDictionary : List<City>
    {
        public CitiesDictionary()
        {
            string[] lines = Properties.Resources.Cities.Split('\n');
            foreach (string line in lines)
            {
                string[] lang = line.Split(',');
                City city = new City()
                {
                    Eng = lang[0],
                    Ukr = lang[1]
                };
                Add(city);
            }
        }

        public string EngToUkr(string eng)
        {
            foreach (var item in this)
                if (item.Eng == eng)
                    return item.Ukr;
            return "";
        }

        public string UkrToEng(string ukr)
        {
            foreach (var item in this)
                if (item.Ukr == ukr)
                    return item.Eng;
            return "";
        }

        public int GetIndexEng(string eng)
        {
            for (int i = 0; i < Count; ++i)
                if (this[i].Eng.ToString() == eng)
                    return i;
            return -1;
        }

        public int GetIndexUkr(string ukr)
        {
            for (int i = 0; i < Count; ++i)
                if (this[i].Ukr.ToString() == ukr)
                    return i;
            return -1;
        }
    }

    public class City
    {
        public string Eng { get; set; }
        public string Ukr { get; set; }
    }
}
