using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Windows.Storage;
using Windows.ApplicationModel;
using System;
using System.Threading.Tasks;

namespace DT.Source.Configs;

class Devices
{
    const string DEVICES_FILE_NAME = @"devices.json";
    public struct Device
    {
        public string Name { get; set; }
        public string IP { get; set; }
    }
    public List<Device> Cluster { get; set; }
    public static async void CreateDevicesDataIsNotExist()
    {
        try
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(DEVICES_FILE_NAME, CreationCollisionOption.FailIfExists);

            // --- Create and write default devices data after new file generated ---
            List<Device> devices = new List<Device>();

            devices.Add(new() { Name = "LampDevice", IP = "10.62.41.175" });

            WriteDevicesData(devices);
        }
        catch
        {
            // File already exists
        }
    }
    /// <summary>
    /// Returns list of devices read from `devices.json`
    /// </summary>
    public static async Task<List<Device>> GetDevicesData()
    {
        var folder = ApplicationData.Current.LocalFolder;
        var file = await folder.GetFileAsync(DEVICES_FILE_NAME);

        var content = await FileIO.ReadTextAsync(file);

        List<Device> devices = JsonSerializer.Deserialize<List<Device>>(content);
        return devices;
    }
    /// <summary>
    /// Returns device object by name on existing devices list specified.
    /// </summary>
    /// <param name="devices">Existing devices list.</param>
    /// <param name="deviceName">Device name.</param>
    public static Device GetDeviceByName(List<Device> devices, string deviceName)
    {
        return devices.FirstOrDefault(d => d.Name == deviceName);
    }
    /// <summary>
    /// Returns a new list of devices with modified device specified.
    /// </summary>
    /// <param name="devices">Exisiting device list.</param>
    /// <param name="modifiedDevice">Device to update on list.</param>
    public static List<Device> GetModifiedDevicesData(List<Device> devices, Device modifiedDevice)
    {
        var deviceIndex = devices.FindIndex(d => d.Name == modifiedDevice.Name);

        // Didn't find device by name
        if (deviceIndex == -1)
        {
            // Try to find by IP
            deviceIndex = devices.FindIndex(d => d.IP == modifiedDevice.IP);
        }

        // This method won't return devices list if ip and name was modified at once.
        // But I sincerely believe that name of device WON'T be ever modified at all.

        devices[deviceIndex] = modifiedDevice;

        return devices;
    }
    /// <summary>
    /// Writes `devices.json` file by devices object specified.
    /// </summary>
    /// <param name="devicesObject">List of devices to write.</param>
    public static async void WriteDevicesData(List<Device> devicesObject)
    {
        var rawJson = JsonSerializer.Serialize(devicesObject);

        var folder = ApplicationData.Current.LocalFolder;
        var file = await folder.GetFileAsync(DEVICES_FILE_NAME);

        await FileIO.WriteTextAsync(file, rawJson);
    }
}
