using System;

namespace WhereIsMakkah.Util
{
    public static class GeoDistanceCalculator
    {
        private const double _earthRadiusInMiles = 3959.0;
        private const double _earthRadiusInKilometers = 6371.0;

        public static double DistanceInMiles(double lat1, double lng1, double lat2, double lng2)
        {
            return Distance(lat1, lng1, lat2, lng2, _earthRadiusInMiles);
        }

        public static double DistanceInKilometers(double lat1, double lng1, double lat2, double lng2)
        {
            return Distance(lat1, lng1, lat2, lng2, _earthRadiusInKilometers);
        }

        public static double InitialBearing(double lat1, double lng1, double lat2, double lng2)
        {
            var rLat = ToRadians(lat2 - lat1);
            var rLng = ToRadians(lng2 - lng1);
            var rLat1 = ToRadians(lat1);
            var rLat2 = ToRadians(lat2);
            var cosLat2 = Math.Cos(rLat2);

            var x = Math.Sin(rLat) * cosLat2;
            var y = Math.Cos(rLat1) * Math.Sin(rLat2) - Math.Sin(rLat1) * cosLat2 * Math.Cos(rLng);
            return ToDegree(Math.Atan2(y, x));
        }

        private static double Distance(double lat1, double lng1, double lat2, double lng2, double radius)
        {
            // Implements the Haversine formulat http://en.wikipedia.org/wiki/Haversine_formula
            //
            var lat = ToRadians(lat2 - lat1);
            var lng = ToRadians(lng2 - lng1);
            var sinLat = Math.Sin(0.5 * lat);
            var sinLng = Math.Sin(0.5 * lng);
            var cosLat1 = Math.Cos(ToRadians(lat1));
            var cosLat2 = Math.Cos(ToRadians(lat2));
            var h1 = sinLat * sinLat + cosLat1 * cosLat2 * sinLng * sinLng;
            var h2 = Math.Sqrt(h1);
            var h3 = 2 * Math.Asin(Math.Min(1, h2));
            return radius * h3;
        }

        private static double ToRadians(double degrees)
        {
            return 2.0 * Math.PI * degrees / 360.0;
        }

        private static double ToDegree(double radians)
        {
            return 360.0 * radians / 2.0 / Math.PI;
        }
    }
}
