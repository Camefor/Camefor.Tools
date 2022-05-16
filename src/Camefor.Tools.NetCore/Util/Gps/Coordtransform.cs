﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Camefor.Tools.NetCore.Util.Gps
{
    /// <summary>
    /// 提供百度坐标系(bd-09)、火星坐标系(国测局坐标系、gcj02)、WGS84坐标系直接的坐标互转，也提供了解析百度地址的方法的c#版本
    /// 参照：http://wandergis.com/coordTransform_py/
    /// 代码来源：https://github.com/topcss/coordtransform/blob/master/Coordtransform.cs
    /// </summary>
    public class Coordtransform
    {
        //定义一些常量
        private const double x_PI = 3.14159265358979324 * 3000.0 / 180.0;
        private const double PI = 3.1415926535897932384626;// π
        private const double a = 6378245.0;// 长半轴
        private const double ee = 0.00669342162296594323;// 偏心率平方

        /// <summary>
        ///  百度坐标系 (BD-09) 与 火星坐标系 (GCJ-02)的转换
        ///  即 百度 转 谷歌、高德
        /// </summary>
        /// <param name="bd_lon"></param>
        /// <param name="bd_lat"></param>
        /// <returns></returns>
        public static double[] Bd09togcj02(double bd_lon, double bd_lat)
        {
            double x = bd_lon - 0.0065;
            double y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_PI);
            double gg_lng = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);

            return new[] { gg_lng, gg_lat };
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换
        /// 即谷歌、高德 转 百度
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static double[] Gcj02tobd09(double lng, double lat)
        {
            double z = Math.Sqrt(lng * lng + lat * lat) + 0.00002 * Math.Sin(lat * x_PI);
            double theta = Math.Atan2(lat, lng) + 0.000003 * Math.Cos(lng * x_PI);
            double bd_lng = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;

            return new[] { bd_lng, bd_lat };
        }

        /// <summary>
        /// WGS84转GCj02
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static double[] Wgs84togcj02(double lng, double lat)
        {
            if (Out_of_china(lng, lat))
            {
                return new[] { lng, lat };
            }
            else
            {
                double dlat = Transformlat(lng - 105.0, lat - 35.0);
                double dlng = Transformlng(lng - 105.0, lat - 35.0);
                double radlat = lat / 180.0 * PI;
                double magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                double sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                dlng = (dlng * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * PI);
                double mglat = lat + dlat;
                double mglng = lng + dlng;

                return new[] { mglng, mglat };
            }
        }

        /// <summary>
        ///  GCJ02 转换为 WGS84
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static double[] Gcj02towgs84(double lng, double lat)
        {
            if (Out_of_china(lng, lat))
            {
                return new[] { lng, lat };
            }
            else
            {
                double dlat = Transformlat(lng - 105.0, lat - 35.0);
                double dlng = Transformlng(lng - 105.0, lat - 35.0);
                double radlat = lat / 180.0 * PI;
                double magic = Math.Sin(radlat);
                magic = 1 - ee * magic * magic;
                double sqrtmagic = Math.Sqrt(magic);
                dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
                dlng = (dlng * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * PI);
                double mglat = lat + dlat;
                double mglng = lng + dlng;

                return new[] { lng * 2 - mglng, lat * 2 - mglat };
            }
        }

        public static double Transformlat(double lng, double lat)
        {
            double ret = -100.0 + 2.0 * lng + 3.0 * lat + 0.2 * lat * lat + 0.1 * lng * lat + 0.2 * Math.Sqrt(Math.Abs(lng));
            ret += (20.0 * Math.Sin(6.0 * lng * PI) + 20.0 * Math.Sin(2.0 * lng * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * PI) + 40.0 * Math.Sin(lat / 3.0 * PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lat / 12.0 * PI) + 320 * Math.Sin(lat * PI / 30.0)) * 2.0 / 3.0;

            return ret;
        }

        public static double Transformlng(double lng, double lat)
        {
            double ret = 300.0 + lng + 2.0 * lat + 0.1 * lng * lng + 0.1 * lng * lat + 0.1 * Math.Sqrt(Math.Abs(lng));
            ret += (20.0 * Math.Sin(6.0 * lng * PI) + 20.0 * Math.Sin(2.0 * lng * PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lng * PI) + 40.0 * Math.Sin(lng / 3.0 * PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lng / 12.0 * PI) + 300.0 * Math.Sin(lng / 30.0 * PI)) * 2.0 / 3.0;

            return ret;
        }

        /// <summary>
        /// 判断是否在国内，不在国内则不做偏移
        /// 纬度3.86~53.55,经度73.66~135.05 
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static bool Out_of_china(double lng, double lat)
        {
            return !(lng > 73.66 && lng < 135.05 && lat > 3.86 && lat < 53.55); ;
        }

        /// <summary>
        /// WGS84坐标系->百度坐标系
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static double[] Wgs84tobd09(double lon, double lat)
        {
            var gcj02 = Wgs84togcj02(lon, lat);

            return Gcj02tobd09(gcj02[0], gcj02[1]);
        }

        /// <summary>
        /// 百度坐标系->WGS84坐标系
        /// </summary>
        /// <param name="bd_lon"></param>
        /// <param name="bd_lat"></param>
        /// <returns></returns>
        public static double[] Bd09towgs84(double bd_lon, double bd_lat)
        {
            var gcj02 = Bd09togcj02(bd_lon, bd_lat);

            return Gcj02towgs84(gcj02[0], gcj02[1]);
        }





        /// <summary>
        /// gps经纬度格式转换：
        ///  rawGps 为 【度 . 分 . 分 格式】
        /// 转换为
        /// 【度 . 度 格式】
        /// 注意 该方法未转换为其他坐标系类型：
        /// 要想在袋鼠云大屏上正确显示 还需要
        /// 转化为 BD-09 百度地图上的坐标
        /// </summary>
        /// <param name="data">【度 . 分 . 分 格式】</param>
        /// <returns></returns>
        public static double GpsDataDmToDd(double data)
        {
            try
            {
                double result;
                double dd, mm, temp;

                dd = (int)data / 100;//得到度dd
                mm = data - dd * 100;//得到分mm.mmmm
                temp = mm / 60;
                result = dd + temp;
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
