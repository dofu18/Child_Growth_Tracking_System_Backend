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
    { 24, new Dictionary<int, decimal> { {5, 14.0m}, {10, 14.5m}, {25, 15.0m}, {50, 16.0m}, {75, 17.5m}, {85, 18.0m}, {95, 19.5m} } },
    { 36, new Dictionary<int, decimal> { {5, 14.5m}, {10, 15.0m}, {25, 15.5m}, {50, 16.5m}, {75, 18.0m}, {85, 18.5m}, {95, 20.0m} } }
};

        public static readonly Dictionary<int, Dictionary<int, decimal>> WHO_BMI_TABLE_FEMALE = new()
{
    { 24, new Dictionary<int, decimal> { {5, 14.2m}, {10, 14.7m}, {25, 15.3m}, {50, 16.2m}, {75, 17.6m}, {85, 18.2m}, {95, 19.7m} } },
    { 36, new Dictionary<int, decimal> { {5, 14.7m}, {10, 15.2m}, {25, 15.8m}, {50, 16.8m}, {75, 18.3m}, {85, 18.8m}, {95, 20.3m} } }
};

    }
}
