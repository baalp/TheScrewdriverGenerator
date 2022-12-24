using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;
using ScrewdriverGenerator.Model;
using ScrewdriverGenerator.Wrapper;

namespace ScrewdriverGenerator.StressTest
{
    class Program
    {
        private static void Main()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var screwdriverBuilder = new ScrewdriverBuilder();
            var screwdriverData = new ScrewdriverData();
            screwdriverData.SetParameters(0, 0.2, 4, 15, 25, 15, 5);

            var streamWriter = new StreamWriter($"StressTest.txt", true);
            var modelCounter = 0;
            var computerInfo = new ComputerInfo();
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ulong usedMemory = 0;
            while (usedMemory * 0.96 <= computerInfo.TotalPhysicalMemory)
            {
                screwdriverBuilder.BuildScrewdriver(screwdriverData, "C:\\Users\\Alexandr\\Desktop");
                usedMemory = (computerInfo.TotalPhysicalMemory - computerInfo.AvailablePhysicalMemory);
                streamWriter.WriteLine(
                    $"{++modelCounter}\t{stopWatch.Elapsed:hh\\:mm\\:ss}\t{usedMemory}\t{cpuCounter.NextValue()}%");
                streamWriter.Flush();
            }
            stopWatch.Stop();
            streamWriter.WriteLine("END");
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}
