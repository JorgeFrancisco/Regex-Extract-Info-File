using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ExtractInfoSaveSettlementsLog.Utils.String;
using ServiceStack;

namespace ExtractInfoSaveSettlementsLog.Application
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            string sourceFullPath = Array.Find(args, n => n.ToLower().Contains("--source_path")).RightPart("=");
            string destinationFullPath = Array.Find(args, n => n.ToLower().Contains("--destination_path")).RightPart("=");

            bool executeHelp = string.IsNullOrEmpty(Array.Find(args, n => n.ToLower().Contains("--help"))) == false;

            bool showProgress = string.IsNullOrEmpty(Array.Find(args, n => n.ToLower().Contains("--show_progress"))) == false;

            if (executeHelp)
            {
                PrintHelp();
                Console.ReadKey();

                return;
            }

            if (File.Exists(sourceFullPath) == false)
            {
                Console.WriteLine($"File '{sourceFullPath}' not exists in source path!");

                Console.ReadKey();

                return;
            }

            Console.WriteLine("Extracting infos...");

            if (string.IsNullOrEmpty(sourceFullPath) == false)
            {
                if (string.IsNullOrEmpty(destinationFullPath))
                {
                    destinationFullPath = Path.GetDirectoryName(sourceFullPath) + "\\" + Path.GetFileNameWithoutExtension(sourceFullPath) + ".csv";
                }

                if (File.Exists(destinationFullPath))
                {
                    Console.WriteLine($"File '{destinationFullPath}' already exists in destination path! It's not possible override!");

                    return;
                }

                IEnumerable<string> lines = File.ReadLines(sourceFullPath);

                List<string> linesOutput = new List<string>();

                Regex patternTimestamp =
                    new Regex(@"((?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})T(?<hour>\d{2})\:(?<minutes>\d{2})\:(?<seconds>\d{2})\.(?<milli>\d{3}))");

                Regex patternTimes =
                    new Regex(@"Saving SettlementKey: (?<saveSettlementKey>[0-9A-Fa-f]{8}[-]?([0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12})\. Save LegalEntity: (?<saveLegalEntity>\d+) ms\. Save Settlement: (?<saveSettlement>\d+) ms\. Post notification: (?<postNotification>\d+) ms\. Post Auto-Assign message: (?<postAutoAssign>\d+) ms\. Total time: (?<totalTime>\d+) ms\.");

                int countLines = lines.Count();
                int positionLine = 0;

                string timestamp = "";

                Stopwatch watch = new Stopwatch();

                watch.Start();

                foreach (string line in lines)
                {
                    ++positionLine;

                    Match matchTimes = patternTimes.Match(line);
                    Match matchTimestamp = patternTimestamp.Match(line);

                    if (matchTimestamp.Success)
                    {
                        timestamp =
                            matchTimestamp.Groups["year"].ToString() +
                            matchTimestamp.Groups["month"] +
                            matchTimestamp.Groups["day"] +
                            matchTimestamp.Groups["hour"] +
                            matchTimestamp.Groups["minutes"] +
                            matchTimestamp.Groups["seconds"] +
                            matchTimestamp.Groups["milli"];
                    }

                    if (matchTimes.Success)
                    {
                        string lineOutput =
                            timestamp + ";" +
                            matchTimes.Groups["saveSettlementKey"] + ";" + matchTimes.Groups["saveLegalEntity"] + ";" +
                            matchTimes.Groups["saveSettlement"] + ";" + matchTimes.Groups["postNotification"] + ";" +
                            matchTimes.Groups["postAutoAssign"] + ";" + matchTimes.Groups["totalTime"];

                        //Console.WriteLine(lineOutput);
                        linesOutput.Add(lineOutput);
                    }

                    if (showProgress)
                    {
                        Console.WriteLine(
                            $"{positionLine}/{countLines} | {FormatString.FormatToDoubleString(positionLine / (double) countLines * 100.0)} %");
                    }
                }

                if (File.Exists(destinationFullPath) == false)
                {
                    File.AppendAllText(destinationFullPath, "Timestamp;Save SettlementKey;Save LegalEntity;Save Settlement;Post notification;Post Auto-Assign;Total time");
                    File.AppendAllText(destinationFullPath, "\r\n");
                    File.AppendAllLines(destinationFullPath, linesOutput);
                }
                else
                {
                    Console.WriteLine($"File '{destinationFullPath}' already exists in destination path! It's not possible override!");
                }
            }
            else
            {
                PrintHelp();
            }

            Console.WriteLine("Extract ended!");

            Console.ReadKey();
        }

        private static void PrintHelp()
        {
            Console.WriteLine(
                $"{Environment.NewLine}Options:" +
                $"{Environment.NewLine}1) ExtractInfoSaveSettlementsLog.exe --source_path=C:\\Log-20180723(2).txt" +
                $"{Environment.NewLine}2) ExtractInfoSaveSettlementsLog.exe --source_path=C:\\Log-20180723(2).txt" +
                "--destination_path=C:\\Log-20180723(2).csv" +
                $"{ Environment.NewLine}3) ExtractInfoSaveSettlementsLog.exe--source_path = C:\\Log - 20180723(2).txt " +
                "--destination_path=C:\\Log-20180723(2).csv --hide_progress"
            );
        }
    }
}
