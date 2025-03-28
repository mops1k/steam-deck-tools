﻿using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static PerformanceOverlay.Sensors;
using CommonHelpers;

namespace PerformanceOverlay
{
    internal class Sensors : IDisposable
    {
        public abstract class Sensor
        {
            public abstract string? GetValue(Sensors sensors);
        }

        public abstract class ValueSensor : Sensor
        {
            public string? Format { get; set; }
            public float Multiplier { get; set; } = 1.0f;
            public bool IgnoreZero { get; set; }

            protected string? ConvertToString(float? value)
            {
                switch (value)
                {
                    case null:
                    case 0 when IgnoreZero:
                        return null;
                    default:
                        value *= Multiplier;
                        return value.Value.ToString(Format, CultureInfo.GetCultureInfo("en-US"));
                }
            }
        }

        public class UserValueSensor : ValueSensor
        {
            public delegate float? ValueDelegate();

            public ValueDelegate Value { get; set; }

            public override string? GetValue(Sensors sensors)
            {
                return ConvertToString(Value());
            }
        }

        public class UserStringValueSensor : ValueSensor
        {
            public delegate string? ValueDelegate();

            public ValueDelegate Value { get; set; }

            public override string? GetValue(Sensors sensors)
            {
                return Value();
            }
        }

        public class HardwareSensor : ValueSensor
        {
            public string HardwareName { get; set; } = "";
            public bool GetHardwareNameAsValue { get; set; } = false;
            public IList<string> HardwareNames { get; set; } = new List<string>();
            public HardwareType HardwareType { get; set; }
            public string SensorName { get; set; } = "";
            public SensorType SensorType { get; set; }
            private string HardwareComputedName { get; set; } = "";

            public bool Matches(ISensor sensor)
            {
                return sensor != null &&
                       sensor.Hardware.HardwareType == HardwareType &&
                       MatchesHardwareName(sensor.Hardware.Name) &&
                       sensor.SensorType == SensorType &&
                       sensor.Name == SensorName;
            }

            private bool MatchesHardwareName(string sensorHardwareName)
            {
                if (HardwareNames.Count > 0)
                {
                    if (HardwareNames.Any(hardwareName => sensorHardwareName.StartsWith(hardwareName)))
                        return true;
                }

                return HardwareName.Length == 0 || sensorHardwareName.StartsWith(HardwareName);
            }

            public string? GetValue(ISensor sensor)
            {
                if (GetHardwareNameAsValue)
                {
                    return HardwareComputedName;
                }
                
                return !sensor.Value.HasValue ? null : ConvertToString(sensor.Value.Value);
            }

            public override string? GetValue(Sensors sensors)
            {
                foreach (var hwSensor in sensors.AllHardwareSensors)
                {
                    if (Matches(hwSensor))
                    {
                        HardwareComputedName = hwSensor.Hardware.Name;
                        return GetValue(hwSensor);
                    }
                }

                return null;
            }
        }

        public class CustomHardwareSensor: HardwareSensor
        {
            public delegate string? ValueDelegate(string data);

            public ValueDelegate Value { get; set; }

            public override string? GetValue(Sensors sensors)
            {
                var data = base.GetValue(sensors);

                return data == null ? null : Value(data);
            }
        }

        public class CompositeSensor : ValueSensor
        {
            public enum AggregateType
            {
                First,
                Min,
                Max,
                Avg
            };

            public IList<Sensor> Sensors { get; set; } = new List<Sensor>();
            public AggregateType Aggregate { get; set; } = AggregateType.First;
            public string? Format { get; set; }

            private IEnumerable<string> GetValues(Sensors sensors)
            {
                foreach (var sensor in Sensors)
                {
                    var result = sensor.GetValue(sensors);
                    if (result is not null)
                        yield return result;
                }
            }

            private IEnumerable<float> GetNumericValues(Sensors sensors)
            {
                return GetValues(sensors).Select((value) => float.Parse(value));
            }

            public override string? GetValue(Sensors sensors)
            {
                if (Aggregate == AggregateType.First)
                    return GetValues(sensors).FirstOrDefault();

                var numbers = GetNumericValues(sensors);
                if (numbers.Count() == 0)
                    return null;

                switch (Aggregate)
                {
                    case AggregateType.Min:
                        return ConvertToString(numbers.Min());

                    case AggregateType.Max:
                        return ConvertToString(numbers.Max());

                    case AggregateType.Avg:
                        return ConvertToString(numbers.Average());
                }

                return null;
            }
        }

        private readonly Dictionary<string, Sensor> AllSensors = new Dictionary<string, Sensor>
        {
            {
                "CPU_%", new HardwareSensor()
                {
                    HardwareType = HardwareType.Cpu,
                    HardwareNames = { "AMD Custom APU 0405", "AMD Custom APU 0932" },
                    SensorType = SensorType.Load,
                    SensorName = "CPU Total",
                    Format = "F0"
                }
            },
            {
                "CPU_W", new HardwareSensor()
                {
                    HardwareType = HardwareType.Cpu,
                    HardwareNames = { "AMD Custom APU 0405", "AMD Custom APU 0932" },
                    SensorType = SensorType.Power,
                    SensorName = "Package",
                    Format = "F1"
                }
            },
            {
                "CPU_T", new HardwareSensor()
                {
                    HardwareType = HardwareType.Cpu,
                    HardwareNames = { "AMD Custom APU 0405", "AMD Custom APU 0932" },
                    SensorType = SensorType.Temperature,
                    SensorName = "Core (Tctl/Tdie)",
                    Format = "F1",
                    IgnoreZero = true
                }
            },
            {
                "CPU_MHZ", new CompositeSensor()
                {
                    Format = "F0",
                    Aggregate = CompositeSensor.AggregateType.Max,
                    Sensors = Enumerable.Range(1, 4).Select((index) =>
                    {
                        return new HardwareSensor()
                        {
                            HardwareType = HardwareType.Cpu,
                            HardwareNames = { "AMD Custom APU 0405", "AMD Custom APU 0932" },
                            SensorType = SensorType.Clock,
                            SensorName = "Core #" + index.ToString(),
                            Format = "F0",
                            IgnoreZero = true
                        };
                    }).ToList<Sensor>()
                }
            },
            {
                "MEM_GB", new HardwareSensor()
                {
                    HardwareType = HardwareType.Memory,
                    HardwareName = "Generic Memory",
                    SensorType = SensorType.Data,
                    SensorName = "Memory Used",
                    Format = "F1"
                }
            },
            {
                "MEM_MB", new HardwareSensor()
                {
                    HardwareType = HardwareType.Memory,
                    HardwareName = "Generic Memory",
                    SensorType = SensorType.Data,
                    SensorName = "Memory Used",
                    Format = "F0",
                    Multiplier = 1024
                }
            },
            {
                "GPU_%", new HardwareSensor()
                {
                    HardwareType = HardwareType.GpuAmd,
                    HardwareNames =
                    {
                        "AMD Custom GPU 0932", "AMD Custom GPU 0405", "AMD Radeon 670M", "AMD Radeon RX 670 Graphics"
                    },
                    SensorType = SensorType.Load,
                    SensorName = "D3D 3D",
                    Format = "F0"
                }
            },
            {
                "GPU_MB", new HardwareSensor()
                {
                    HardwareType = HardwareType.GpuAmd,
                    HardwareNames =
                    {
                        "AMD Custom GPU 0932", "AMD Custom GPU 0405", "AMD Radeon 670M", "AMD Radeon RX 670 Graphics"
                    },
                    SensorType = SensorType.SmallData,
                    SensorName = "D3D Dedicated Memory Used",
                    Format = "F0"
                }
            },
            {
                "GPU_GB", new HardwareSensor()
                {
                    HardwareType = HardwareType.GpuAmd,
                    HardwareNames =
                    {
                        "AMD Custom GPU 0932", "AMD Custom GPU 0405", "AMD Radeon 670M", "AMD Radeon RX 670 Graphics"
                    },
                    SensorType = SensorType.SmallData,
                    SensorName = "D3D Dedicated Memory Used",
                    Format = "F0",
                    Multiplier = 1.0f / 1024.0f
                }
            },
            {
                "GPU_W", new HardwareSensor()
                {
                    HardwareType = HardwareType.GpuAmd,
                    HardwareNames =
                    {
                        "AMD Custom GPU 0932", "AMD Custom GPU 0405", "AMD Radeon 670M", "AMD Radeon RX 670 Graphics"
                    },
                    SensorType = SensorType.Power,
                    SensorName = "GPU SoC",
                    Format = "F1"
                }
            },
            {
                "GPU_MHZ", new HardwareSensor()
                {
                    HardwareType = HardwareType.GpuAmd,
                    HardwareNames =
                    {
                        "AMD Custom GPU 0932", "AMD Custom GPU 0405", "AMD Radeon 670M", "AMD Radeon RX 670 Graphics"
                    },
                    SensorType = SensorType.Clock,
                    SensorName = "GPU Core",
                    Format = "F0"
                }
            },
            {
                "GPU_T", new HardwareSensor()
                {
                    HardwareType = HardwareType.GpuAmd,
                    HardwareNames =
                    {
                        "AMD Custom GPU 0932", "AMD Custom GPU 0405", "AMD Radeon 670M", "AMD Radeon RX 670 Graphics"
                    },
                    SensorType = SensorType.Temperature,
                    SensorName = "GPU Temperature",
                    Format = "F1",
                    IgnoreZero = true
                }
            },
            {
                "BATT_%", new HardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.Level,
                    SensorName = "Charge Level",
                    Format = "F0"
                }
            },
            {
                "BATT_MIN", new HardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.TimeSpan,
                    SensorName = "Remaining Time (Estimated)",
                    Format = "F0",
                    Multiplier = 1.0f / 60.0f
                }
            },
            {
                "BATT_TIME_H", new CustomHardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.TimeSpan,
                    SensorName = "Remaining Time (Estimated)",
                    Format = "F0",
                    Multiplier = 1.0f / 60.0f,
                    Value = delegate(string data)
                    {
                        if(!int.TryParse(data, out int minutes)) {
                            return "~";
                        }

                        var timeSpan = TimeSpan.FromMinutes(minutes);

                        return $"{timeSpan.Hours}";
                    }
                }
            },
            {
                "BATT_TIME_M", new CustomHardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.TimeSpan,
                    SensorName = "Remaining Time (Estimated)",
                    Format = "F0",
                    Multiplier = 1.0f / 60.0f,
                    Value = delegate(string data)
                    {
                        if(!int.TryParse(data, out int minutes)) {
                            return "~";
                        }

                        return $"{minutes % 60}";
                    }
                }
            },
            {
                "BATT_W", new HardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.Power,
                    SensorName = "Discharge Rate",
                    Format = "F1"
                }
            },
            {
                "BATT_CHARGE_W", new HardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.Power,
                    SensorName = "Charge Rate",
                    Format = "F1"
                }
            },
            {
                "BATT_T", new HardwareSensor()
                {
                    HardwareType = HardwareType.Battery,
                    SensorType = SensorType.Temperature,
                    SensorName = "Temperature",
                    Format = "F0"
                }
            },
            {
                "SSD_NAME", new HardwareSensor()
                {
                    HardwareType = HardwareType.Storage,
                    SensorName = "Temperature",
                    SensorType = SensorType.Temperature,
                    GetHardwareNameAsValue = true
                }
            },
            {
                "SSD_T", new HardwareSensor()
                {
                    HardwareType = HardwareType.Storage,
                    SensorName = "Temperature",
                    SensorType = SensorType.Temperature,
                    Format = "F0"
                }
            },
            {
                "FAN_RPM", new UserValueSensor()
                {
                    Value = () => Vlv0100.Instance.GetFanRPM(),
                    Format = "F0"
                }
            },
            {
                "CURR_TIME", new UserStringValueSensor()
                {
                    Value = delegate()
                    {
                        var localDate = DateTime.Now;
                        return localDate.ToString("t");
                    },
                    Format = "F0"
                }
            }
        };

        private IList<ISensor> AllHardwareSensors { get; set; } = new List<ISensor>();

        public void Dispose()
        {
        }

        public void Update()
        {
            Instance.WithGlobalMutex(200, () =>
            {
                var allSensors = new List<ISensor>();

                foreach (var hardware in Instance.HardwareComputer.Hardware)
                {
                    try
                    {
                        hardware.Update();
                    }
                    catch (SystemException)
                    {
                    }

                    hardware.Accept(new SensorVisitor(sensor => allSensors.Add(sensor)));
                }

                this.AllHardwareSensors = allSensors;
                return true;
            });
        }

        public string? GetValue(string name)
        {
            if (!AllSensors.TryGetValue(name, out var sensor))
                return null;

            return sensor.GetValue(this);
        }
    }
}