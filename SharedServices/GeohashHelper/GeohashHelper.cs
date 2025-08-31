using System;
using System.Text;

public class GeohashHelper
{
    private const string Base32 = "0123456789bcdefghjkmnpqrstuvwxyz";

    public static string Encode(double latitude, double longitude, int precision = 5)
    {
        int[] bits = { 16, 8, 4, 2, 1 };
        bool isLongitude = true;
        int bit = 0, ch = 0;
        StringBuilder geohash = new StringBuilder();
        double[] lat = { -90.0, 90.0 };
        double[] lon = { -180.0, 180.0 };

        while (geohash.Length < precision)
        {
            double mid;
            if (isLongitude)
            {
                mid = (lon[0] + lon[1]) / 2;
                if (longitude >= mid)
                {
                    ch |= bits[bit];
                    lon[0] = mid;
                }
                else
                {
                    lon[1] = mid;
                }
            }
            else
            {
                mid = (lat[0] + lat[1]) / 2;
                if (latitude >= mid)
                {
                    ch |= bits[bit];
                    lat[0] = mid;
                }
                else
                {
                    lat[1] = mid;
                }
            }

            isLongitude = !isLongitude;

            if (bit < 4)
            {
                bit++;
            }
            else
            {
                geohash.Append(Base32[ch]);
                bit = 0;
                ch = 0;
            }
        }
        return geohash.ToString();
    }
}