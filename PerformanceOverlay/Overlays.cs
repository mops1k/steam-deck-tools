using CommonHelpers;
using System.Text;
using System.Text.RegularExpressions;

namespace PerformanceOverlay
{
    internal static class Overlays
    {
        public class Entry
        {
            public String? Text { get; init; }
            public IList<OverlayMode> Include { get; set; } = new List<OverlayMode>();
            private IList<OverlayMode> Exclude { get; set; } = new List<OverlayMode>();
            public IList<Entry> Nested { get; set; } = new List<Entry>();
            public String Separator { get; init; } = "";
            public bool IgnoreMissing { get; init; }

            private static readonly Regex attributeRegex =
                new Regex("{([^}]+)}", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            public Entry()
            {
            }

            public Entry(string text)
            {
                this.Text = text;
            }

            private IEnumerable<Match> AllAttributes => attributeRegex.Matches(Text ?? "");

            private string EvaluateText(Sensors sensors)
            {
                var output = Text ?? "";

                foreach (var attribute in AllAttributes)
                {
                    var attributeName = attribute.Groups[1].Value;
                    var value = sensors.GetValue(attributeName);
                    if (value is null && IgnoreMissing)
                        return "";
                    output = output.Replace(attribute.Value, value ?? "-");
                }

                return output;
            }

            public string? GetValue(OverlayMode mode, Sensors sensors)
            {
                if (Exclude.Count > 0 && Exclude.Contains(mode))
                    return null;
                if (Include.Count > 0 && !Include.Contains(mode))
                    return null;

                var output = EvaluateText(sensors);

                if (Nested.Count > 0)
                {
                    var outputs = Nested.Select(entry => entry.GetValue(mode, sensors)).Where(output => output != null);
                    var enumerable = outputs as string[] ?? outputs.ToArray();
                    if (!enumerable.Any())
                        return null;

                    output += string.Join(Separator, enumerable);
                }

                if (output == string.Empty)
                    return null;

                return output;
            }
        }

        private static readonly string[] Helpers =
        {
            "<C0=008040><C1=0080C0><C2=C08080><C3=FF0000><C4=FFFFFF><C250=FF8000>",
            "<A0=-4><A1=5><A2=-2><A3=-3><A4=-4><A5=-5><S0=-50><S1=50>",
        };

        private static readonly Entry Osd = new Entry
        {
            Nested =
            {
                // Simple just FPS
                new Entry
                {
                    Nested =
                    {
                        new Entry("<C4><FR><A1><S1> FPS<C><S><A>"),
                        new Entry
                        {
                            Text = "<C4>BAT<C>",
                            Nested =
                            {
                                new Entry("<C4><A3>{BATT_%}<A><A1><S1> %<C><S><A>") { IgnoreMissing = true },
                                new Entry("<C4><A4>{BATT_W}<A><A1><S1> W<C><S><A>") { IgnoreMissing = true },
                                new Entry("<C4><A3>{BATT_TIME_H}<A><A1><S1>h <S>{BATT_TIME_M}<S1>m <S><A><C>")
                                    { IgnoreMissing = true }
                            },
                            Include = { OverlayMode.FPSWithBattery }
                        },
                        new Entry("<C4><A5>{CURR_TIME}<A><C>")
                            { Include = { OverlayMode.FPSWithBattery, OverlayMode.FPSWithTime } }
                    },
                    Separator = "<C250><A4>|<A><C> ",
                    Include = { OverlayMode.FPS, OverlayMode.FPSWithBattery, OverlayMode.FPSWithTime }
                },
                // Battery
                new Entry
                {
                    Nested =
                    {
                        new Entry
                        {
                            Text = "<C4>BAT<C>",
                            Nested =
                            {
                                new Entry("<C4><A3>{BATT_%}<A><A1><S1> %<C><S><A>") { IgnoreMissing = true },
                                new Entry("<C4><A4>{BATT_W}<A><A1><S1> W<C><S><A>") { IgnoreMissing = true },
                                new Entry("<C4><A3>{BATT_TIME_H}<A><A1><S1>h <S>{BATT_TIME_M}<S1>m <S><A><C>") { IgnoreMissing = true }
                            }
                        },
                        new Entry("<C4><A5>{CURR_TIME}<A><C>") { Include = { OverlayMode.BatteryWithTime } }
                    },
                    Separator = "<C250>|<C> ",
                    Include = { OverlayMode.Battery, OverlayMode.BatteryWithTime }
                },

                // Minimal and Detail
                new Entry
                {
                    Nested =
                    {
                        new Entry
                        {
                            Text = "<C1>BAT<C>",
                            Nested =
                            {
                                new Entry("<C4><A3>{BATT_%}<A><A1><S1> %<S><A>"),
                                new Entry("<C4><A4>{BATT_W}<A><A1><S1> W<S><A>") { IgnoreMissing = true },
                                new Entry("<C4><A3>{BATT_TIME_H}<A><A1><S1>h <S>{BATT_TIME_M}<S1>m <C><S><A>") { IgnoreMissing = true },
                                new Entry("<C4><A4>{BATT_CHARGE_W}<A><A1><S1> W<S><A>")
                                    { IgnoreMissing = true, Include = { OverlayMode.Detail } }
                            }
                        },
                        new Entry
                        {
                            Text = "<C1>GPU<C>",
                            Nested =
                            {
                                new Entry("<C4><A3>{GPU_%}<A><A1><S1> %<S><A>"),
                                new Entry("<C4><A4>{GPU_W}<A><A1><S1> W<S><A>"),
                                new Entry("<C4><A4>{GPU_T}<A><A1><S1> C<S><A>")
                                    { IgnoreMissing = true, Include = { OverlayMode.Detail } }
                            }
                        },
                        new Entry
                        {
                            Text = "<C1>CPU<C>",
                            Nested =
                            {
                                new Entry("<C4><A3>{CPU_%}<A><A1><S1> %<S><A>"),
                                new Entry("<C4><A4>{CPU_W}<A><A1><S1> W<S><A>"),
                                new Entry("<C4><A4>{CPU_T}<A><A1><S1> C<S><A>")
                                    { IgnoreMissing = true, Include = { OverlayMode.Detail } }
                            }
                        },
                        new Entry
                        {
                            Text = "<C1>RAM<C>",
                            Nested = { new Entry("<C4><A5>{MEM_GB}<A><A1><S1> GiB<S><A>") }
                        },
                        new Entry
                        {
                            Text = "<C1>FAN<C>",
                            Nested = { new Entry("<C4><A5>{FAN_RPM}<A><A1><S1> RPM<S><A>") },
                            Include = { OverlayMode.Detail }
                        },
                        new Entry
                        {
                            Text = "<C2><APP><C>",
                            Nested = { new Entry("<C4><A4><FR><C><A><A1><S1><C4> FPS<C><S><A>") }
                        },
                        new Entry
                        {
                            Text = "<C2>[OBJ_FT_SMALL]<C><S1> <C4><A0><FT><A><A1> ms<A><S><C>",
                            Include = { OverlayMode.Detail }
                        }
                    },
                    Separator = "<C250>|<C> ",
                    Include = { OverlayMode.Minimal, OverlayMode.Detail }
                },

                new Entry
                {
                    Nested =
                    {
                        new Entry("<C1>CPU<C>\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5>{CPU_%}<A><A1><S1> %<S><A>"),
                                new Entry("<A5>{CPU_W}<A><A1><S1> W<S>"),
                                new Entry("<A5>{CPU_T}<A><A1><S1> C<S><A>") { IgnoreMissing = true },
                            }
                        },
                        new Entry("\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5>{MEM_MB}<A><A1><S1> MB<S>"),
                                new Entry("<A5>{CPU_MHZ}<A><A1><S1> MHz<S><A>")
                            }
                        },
                        new Entry("<C1>GPU<C>\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5>{GPU_%}<A><A1><S1> %<S><A>"),
                                new Entry("<A5>{GPU_W}<A><A1><S1> W<S><A>"),
                                new Entry("<A5>{GPU_T}<A><A1><S1> C<S><A>") { IgnoreMissing = true },
                            }
                        },
                        new Entry("\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5>{GPU_MB}<A><A1><S1> MB<S><A>"),
                                new Entry("<A5>{GPU_MHZ}<A><A1><S1> MHz<S><A>") { IgnoreMissing = true }
                            }
                        },
                        new Entry("<C1>FAN<C>\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5>{FAN_RPM}<A><A1><S1> RPM<S><A>"),
                            }
                        },
                        new Entry("<C2><APP><C>\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5><C4><FR><C><A><A1><S1><C4> FPS<C><S><A>"),
                                new Entry("<A5><C4><FT><C><A><A1><S1><C4> ms<C><S><A>"),
                            }
                        },
                        new Entry("<C1>BAT<C>\t  ")
                        {
                            Nested =
                            {
                                new Entry("<A5>{BATT_%}<A><A1><S1> %<S><A>"),
                                new Entry("<A5>{BATT_W}<A><A1><S1> W<S><A>") { IgnoreMissing = true },
                                new Entry("<A5>{BATT_TIME_H}<A><A1><S1>h <S>{BATT_TIME_M}<S1>m <S><A>") { IgnoreMissing = true },
                                new Entry("<A5>C{BATT_CHARGE_W}<A><A1><S1> W<S><A>") { IgnoreMissing = true }
                            }
                        },
                        new Entry("<C2><S1>Frametime<S>"),
                        new Entry("[OBJ_FT_LARGE]<S1> <A0><FT><A><A1> ms<A><S><C>")
                    },
                    Separator = "\r\n",
                    Include = { OverlayMode.Full }
                }
            }
        };

        public static string GetOsd(OverlayMode mode, Sensors sensors)
        {
            var sb = new StringBuilder();

            sb.AppendJoin("", Helpers);
            sb.Append(Osd.GetValue(mode, sensors) ?? "");

            return sb.ToString();
        }
    }
}