﻿using System.Diagnostics;

namespace EasySaveGUI.model {
    public class NetworkUtils {

        /// <summary>
        /// Get the network load as a percentage of the maximum bandwidth.
        /// </summary>
        /// <returns>The network load as a percentage of the maximum bandwidth set.</returns>
        public static double GetNetworkLoad() {
            try {
                const string categoryName = "Network Interface";

                var category = new PerformanceCounterCategory(categoryName);
                var instanceNames = category.GetInstanceNames();
                if (instanceNames.Length == 0) {
                    throw new Exception("No network interfaces found.");
                }

                // Select the first network interface
                string instanceName = instanceNames[0];

                // Create performance counters for the bytes sent and received/sec
                using var bytesSentCounter = new PerformanceCounter(categoryName, "Bytes Sent/sec", instanceName);
                using var bytesReceivedCounter = new PerformanceCounter(categoryName, "Bytes Received/sec", instanceName);

                // Read the current value
                float sendRate = bytesSentCounter.NextValue();
                float receiveRate = bytesReceivedCounter.NextValue();

                // 100 Mbps
                const float maxBandwidth = 100_000_000;

                // Calculate the total bytes per second
                float totalBytesPerSecond = sendRate + receiveRate;

                // Calculate the network load as a percentage of the maximum bandwidth
                double networkLoadPercentage = (totalBytesPerSecond / maxBandwidth) * 100;

                return networkLoadPercentage;
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred while calculating network load: {ex.Message}");
                return -1;
            }
        }
    }
}
