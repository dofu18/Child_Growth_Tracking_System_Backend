using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.BMI
{
    public static class DataWHO
    {
        // BMI percentiles chuẩn WHO
        public static readonly Dictionary<int, Dictionary<int, decimal>> WHO_BMI_TABLE_MALE = new()
{
     { 0, new Dictionary<int, decimal> { {3, 11.0m}, {15, 12.0m}, {50, 13.5m}, {85, 14.8m}, {97, 16.0m} } },
    { 12, new Dictionary<int, decimal> { {3, 14.0m}, {15, 14.8m}, {50, 16.0m}, {85, 18.2m}, {97, 18.6m} } },
    { 24, new Dictionary<int, decimal> { {3, 14.6m}, {15, 15.6m}, {50, 16.8m}, {85, 19.4m}, {97, 19.2m} } },
    { 36, new Dictionary<int, decimal> { {3, 14.7m}, {15, 15.7m}, {50, 17.2m}, {85, 18.7m}, {97, 20.2m} } },
    { 48, new Dictionary<int, decimal> { {3, 14.9m}, {15, 15.9m}, {50, 17.5m}, {85, 19.0m}, {97, 20.5m} } },
    { 60, new Dictionary<int, decimal> { {3, 15.0m}, {15, 16.0m}, {50, 17.6m}, {85, 19.2m}, {97, 20.7m} } },
    { 72, new Dictionary<int, decimal> { {3, 15.5m}, {15, 16.5m}, {50, 18.0m}, {85, 19.8m}, {97, 21.5m} } },
    { 84, new Dictionary<int, decimal> { {3, 16.0m}, {15, 17.0m}, {50, 18.5m}, {85, 20.5m}, {97, 22.5m} } },
    { 96, new Dictionary<int, decimal> { {3, 16.5m}, {15, 17.5m}, {50, 19.0m}, {85, 21.2m}, {97, 23.5m} } },
    { 108, new Dictionary<int, decimal> { {3, 17.0m}, {15, 18.0m}, {50, 19.8m}, {85, 22.0m}, {97, 24.5m} } },
    { 120, new Dictionary<int, decimal> { {3, 17.5m}, {15, 18.5m}, {50, 20.5m}, {85, 23.0m}, {97, 25.5m} } },
    { 132, new Dictionary<int, decimal> { {3, 18.0m}, {15, 19.0m}, {50, 21.2m}, {85, 24.0m}, {97, 26.5m} } },
    { 144, new Dictionary<int, decimal> { {3, 18.5m}, {15, 19.5m}, {50, 22.0m}, {85, 25.0m}, {97, 27.5m} } },
    { 156, new Dictionary<int, decimal> { {3, 19.0m}, {15, 20.0m}, {50, 22.8m}, {85, 26.0m}, {97, 28.5m} } },
    { 168, new Dictionary<int, decimal> { {3, 19.5m}, {15, 20.5m}, {50, 23.5m}, {85, 27.0m}, {97, 29.5m} } },
    { 180, new Dictionary<int, decimal> { {3, 20.0m}, {15, 21.0m}, {50, 24.2m}, {85, 28.0m}, {97, 30.0m} } },
    { 192, new Dictionary<int, decimal> { {3, 20.5m}, {15, 21.5m}, {50, 25.0m}, {85, 29.0m}, {97, 30.5m} } },
    { 204, new Dictionary<int, decimal> { {3, 21.0m}, {15, 22.0m}, {50, 25.5m}, {85, 29.5m}, {97, 31.0m} } },
    { 216, new Dictionary<int, decimal> { {3, 21.5m}, {15, 22.5m}, {50, 26.0m}, {85, 30.0m}, {97, 31.5m} } }
};

        public static readonly Dictionary<int, Dictionary<int, decimal>> WHO_BMI_TABLE_FEMALE = new()
{
    { 0, new Dictionary<int, decimal> { {3, 11.0m}, {15, 12.0m}, {50, 13.5m}, {85, 15.0m}, {97, 16.5m} } },
    { 12, new Dictionary<int, decimal> { {3, 14.0m}, {15, 15.0m}, {50, 16.5m}, {85, 18.0m}, {97, 19.5m} } },
    { 24, new Dictionary<int, decimal> { {3, 14.5m}, {15, 15.5m}, {50, 17.0m}, {85, 18.5m}, {97, 20.0m} } },
    { 36, new Dictionary<int, decimal> { {3, 14.7m}, {15, 15.7m}, {50, 17.2m}, {85, 18.7m}, {97, 20.2m} } },
    { 48, new Dictionary<int, decimal> { {3, 14.9m}, {15, 15.9m}, {50, 17.5m}, {85, 19.0m}, {97, 20.5m} } },
    { 60, new Dictionary<int, decimal> { {3, 15.0m}, {15, 16.0m}, {50, 17.6m}, {85, 19.2m}, {97, 20.7m} } },
    { 72, new Dictionary<int, decimal> { {3, 15.5m}, {15, 16.5m}, {50, 18.0m}, {85, 19.8m}, {97, 21.5m} } },
    { 84, new Dictionary<int, decimal> { {3, 16.0m}, {15, 17.0m}, {50, 18.5m}, {85, 20.5m}, {97, 22.5m} } },
    { 96, new Dictionary<int, decimal> { {3, 16.5m}, {15, 17.5m}, {50, 19.0m}, {85, 21.2m}, {97, 23.5m} } },
    { 108, new Dictionary<int, decimal> { {3, 17.0m}, {15, 18.0m}, {50, 19.8m}, {85, 22.0m}, {97, 24.5m} } },
    { 120, new Dictionary<int, decimal> { {3, 17.5m}, {15, 18.5m}, {50, 20.5m}, {85, 23.0m}, {97, 25.5m} } },
    { 132, new Dictionary<int, decimal> { {3, 18.0m}, {15, 19.0m}, {50, 21.2m}, {85, 24.0m}, {97, 26.5m} } },
    { 144, new Dictionary<int, decimal> { {3, 18.5m}, {15, 19.5m}, {50, 22.0m}, {85, 25.0m}, {97, 27.5m} } },
    { 156, new Dictionary<int, decimal> { {3, 19.0m}, {15, 20.0m}, {50, 22.8m}, {85, 26.0m}, {97, 28.5m} } },
    { 168, new Dictionary<int, decimal> { {3, 19.5m}, {15, 20.5m}, {50, 23.5m}, {85, 27.0m}, {97, 29.5m} } },
    { 180, new Dictionary<int, decimal> { {3, 20.0m}, {15, 21.0m}, {50, 24.2m}, {85, 28.0m}, {97, 30.0m} } },
    { 192, new Dictionary<int, decimal> { {3, 20.5m}, {15, 21.5m}, {50, 25.0m}, {85, 29.0m}, {97, 30.5m} } },
    { 204, new Dictionary<int, decimal> { {3, 21.0m}, {15, 22.0m}, {50, 25.5m}, {85, 29.5m}, {97, 31.0m} } },
    { 216, new Dictionary<int, decimal> { {3, 21.5m}, {15, 22.5m}, {50, 26.0m}, {85, 30.0m}, {97, 31.5m} } }
};

    }
}
