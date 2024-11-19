using System;
using System.Net.NetworkInformation;
using System.Text;

public class CustomGuidGenerator
{
    private static Random random = new Random();

    public static string GenerateGuid()
    {
        try
        {
            byte[] guidBytes = new byte[16];

            // time_low (Octet 0-3)
            byte[] timeLowBytes = BitConverter.GetBytes((uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Array.Copy(timeLowBytes, 0, guidBytes, 0, 4);

            // time_mid (Octet 4-5)
            byte[] timeMidBytes = BitConverter.GetBytes((ushort)DateTimeOffset.UtcNow.Millisecond);
            Array.Copy(timeMidBytes, 0, guidBytes, 4, 2);

            // time_hi_and_version (Octet 6-7)
            byte[] timeHiAndVersionBytes = BitConverter.GetBytes((ushort)DateTimeOffset.UtcNow.Year);
            Array.Copy(timeHiAndVersionBytes, 0, guidBytes, 6, 2);

            // clock_seq_hi_and_reserved (Octet 8)
            guidBytes[8] = (byte)random.Next(0, 256);

            // clock_seq_low (Octet 9)
            guidBytes[9] = (byte)random.Next(0, 256);

            var macAddr =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();


            // node (Octet 10-15): mac address or Spatially unique node identifier
            if (macAddr != null)
            {
                byte[] macBytes = Encoding.UTF8.GetBytes(macAddr);
                Array.Copy(macBytes, 0, guidBytes, 10, 5);
            }
            else
            {
                random.NextBytes(guidBytes);
            }
            // Convert byte array to GUID format
            Guid customGuid = new Guid(guidBytes);
            return customGuid.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating custom GUID: " + ex.Message);
            return string.Empty;
        }
    }

    static void Main()
    {
        string customGuid = GenerateGuid();
        if (!string.IsNullOrEmpty(customGuid))
        {
            Console.WriteLine("Generated Custom GUID: " + customGuid);
        }
    }
}