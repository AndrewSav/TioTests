﻿using System;
using System.IO;
using Newtonsoft.Json;

namespace TioTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Execute(args);
            }
            catch (Exception e)
            {
                Logger.LogLine("Unexpected error: " + e);
                Environment.Exit(-1);
            }
        }

        private static void Execute(string[] args)
        {
            string configPath = "config.json";
            Config config = File.Exists(configPath) ?
                JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath)) :
                new Config { TrimWhitespacesFromResults = true, DisplayDebugInfoOnError = true, UseConsoleCodes = true };

            CommandLine.ApplyCommandLineArguments(args, config);

            if (Directory.Exists(config.Test))
            {
                string[] files = Directory.GetFiles(config.Test, "*.json");
                Array.Sort(files);
                int counter = 0;
                foreach (string file in files)
                {
                    //Logger.Log($"[{++counter}/{files.Length}] ");
                    TestRunner.RunTest(file, config, $"[{++counter}/{files.Length}]");
                }
            }
            else if (File.Exists(config.Test))
            {
                TestRunner.RunTest(config.Test, config,"[1/1]");
            }
            else if (File.Exists(config.Test + ".json"))
            {
                TestRunner.RunTest(config.Test + ".json", config,"1/1");
            }
            else
            {
                Logger.LogLine($"{config.Test} not found");
            }
        }
    }
}
