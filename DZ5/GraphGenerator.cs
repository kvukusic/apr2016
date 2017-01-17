using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using APR.DZ1.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APR.DZ5
{
    public class GraphGenerator
    {
        public void GenerateLineChart(string[] labels, double[] time, params double[][][] variableValuesThroughTime)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "Graph/generate_graph.command";
            myProcess.StartInfo.Arguments = "Output";
            myProcess.StartInfo.Arguments += " /data:" + JsonConvert.ToString(JsonConvert.SerializeObject(
            new Chart(labels, time, variableValuesThroughTime)));
            myProcess.Start();
        }

        public class Chart
        {
            public Chart(string[] labels, double[] time, double[][][] variableValuesThroughTime)
            {
                var graphs = new Graph[variableValuesThroughTime.Length];
                for (int i = 0; i < variableValuesThroughTime.Length; i++)
                {
                    var lines = new Line[labels.Length];
                    for (int j = 0; j < lines.Length; j++)
                    {
                        lines[j] = new Line();
                        lines[j].label = labels[j];
                        lines[j].items = variableValuesThroughTime[i].Select((values, idx) => new LineItem()
                        {
                            time = time[idx],
                            variableValue = values[j]
                        }).ToArray();
                    }

                    graphs[i] = new Graph();
                    graphs[i].lines = lines;
                }

                this.graphs = graphs;
            }

            [JsonPropertyAttribute("graphs")]
            public Graph[] graphs { get; set; }
        }

        public class Graph
        {
            [JsonPropertyAttribute("lines")]
            public Line[] lines { get; set; }
        }

        public class Line
        {
            // label must not contain spaces ???
            [JsonPropertyAttribute("label")]
            public string label { get; set; }

            [JsonPropertyAttribute("items")]
            public LineItem[] items { get; set; }
        }

        public class LineItem
        {
            [JsonPropertyAttribute("time")]
            public double time { get; set; }
            
            [JsonPropertyAttribute("variableValue")]
            public double variableValue { get; set; }
        }

        public class EscapeQuoteConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString().Replace("'", "\\'"));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var value = JToken.Load(reader).Value<string>();
                return value.Replace("\\'", "'");
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(string);
            }
        }
    }
}