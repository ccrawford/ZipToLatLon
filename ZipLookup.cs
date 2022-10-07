using System.Reflection;

namespace ZipToLatLon
{
    public class ZipLatLon
    {
        public string zip { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }

    }

    public sealed class ZipLookup
    {
        private ZipLookup() { }
        private static ZipLookup instance = null;
        public static ZipLookup Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ZipLookup();
                    instance.loadZips();
                }

                return instance;
            }
        }

        private static Dictionary<string, ZipLatLon>? _zips { set; get; } = new Dictionary<string, ZipLatLon>();
        private int loadZips()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var lines = File.ReadLines(buildDir + @"\zips.csv");
            foreach (var line in lines)
            {
                var fields = line.Split(',');
                _zips.Add(fields[0], new ZipLatLon() { zip = fields[0], lat = Math.Round(Decimal.Parse(fields[1]), 4), lon = Math.Round(Decimal.Parse(fields[2]), 4) });
            }
            return _zips.Count();
        }

        public ZipLatLon? GetLatLon(string zip)
        {
            if (_zips == null || _zips.Count() == 0)
            {
                loadZips();
            }
            ZipLatLon retval;
            _zips.TryGetValue(zip, out retval);
            return retval;
        }

    }
}