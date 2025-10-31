using DT.Source.Configs;
using DT.Source.Globals;
using DT.Source.Graphics;
using DT.Source.Interfaces;
using DT.Source.Managers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using static DT.Source.Configs.Devices;

namespace DT.Source.Windows;

class LampDeviceWindow : LayoutManager, IWindow
{
    const string DEVICE_NAME = "LampDevice";

    List<Device> AllDevices;
    Device ThisDevice;

    bool IsDeviceOnline = false;

    Window LampWindow = new();
    Button LampButton = new();
    Button PopUpButton = new();
    public async void Initialize()
    {
        // Get device info from devices.json
        AllDevices = await Devices.GetDevicesData();
        ThisDevice = Devices.GetDeviceByName(AllDevices, DEVICE_NAME);

        // -- Window ---

        // Set window title
        LampWindow.Title = "Example Lamp";
        // Show window
        await LampWindow.Initialize();

        // -- Lamp button ---

        // Set button properties
        LampButton.SvgPath = Svg.Lamp;
        LampButton.IconSize = new(164, 200);
        LampButton.OnCreated = (s, a) =>
        {
            TryGetDeviceState();
        };
        LampButton.OnPointerReleased = (s, a) =>
        {
            TryChangeDeviceState();
        };
        // Initialize button
        LampButton.Initialize();

        // Attach button to a window
        LampWindow.Add(LampButton.GetRoot());

        // -- PopUp button ---

        // Set button properties
        PopUpButton.SvgPath = Svg.Gear;
        PopUpButton.IconSize = new(182, 200);
        PopUpButton.OnPointerReleased = (s, a) =>
        {
            LampWindow.ShowPopUp(ThisDevice.IP, (s, a) => 
            {
                // Change device ip by popup input
                ThisDevice.IP = LampWindow.GetPopUpInput();
                
                // Get a new list of devices with a modified one
                var newAllDevices = Devices.GetModifiedDevicesData(AllDevices, ThisDevice);

                // Save data in devices.json
                Devices.WriteDevicesData(newAllDevices);

                // Try to get device state again
                TryGetDeviceState();

                // Show loading to prevent user to get bored
                LampWindow.ShowLoading();
            });
        };
        // Initialize button
        PopUpButton.Initialize();

        // Attach button to a window
        LampWindow.Add(PopUpButton.GetRoot());
    }
    async void TryGetDeviceState()
    {
        Task<bool> task = new(() =>
        {
            IsDeviceOnline = false;

            var url = $"http://{ThisDevice.IP}:80/state";
            var response = NetworkManager.Send(url, "WHALE");

            if (response.Code == 0)
            {
                LampButton.IsActive = false;
                MainPage.Log += $"\n[{Time.Current}] {response.Message}";
                return false;
            }
            else
            {
                if (response.Message == "1" || response.Message == "0")
                {
                    LampButton.IsActive = response.Message == "1";
                    var status = LampButton.IsActive ? "On" : "Off";
                    MainPage.Log += $"\n[{Time.Current}] Device [{ThisDevice.IP}] state: {status}";
                }
                else
                {
                    LampButton.IsActive = false;
                    MainPage.Log += $"\n[{Time.Current}] Unable to get a device [{ThisDevice.IP}] state. Device response: {response.Message}";
                }
                return true;
            }
        });
        task.Start();

        while (!task.IsCompleted)
        {
            MainPage.Log += $"\n[{Time.Current}] Waiting device [{ThisDevice.IP}] to respond...";
            await Task.Delay(1000);
        }

        IsDeviceOnline = task.Result;

        LampButton.Update();
        LampWindow.HideLoading();
    }
    void TryChangeDeviceState()
    {
        if (!IsDeviceOnline)
        {
            Animation.ErrorBlink((SolidColorBrush)LampButton.GetRoot().Background, Colors.SoftGrey);
            Animation.Blink((SolidColorBrush)LampButton.GetIcon().Fill, Colors.Grey);
            return;
        }

        var url = LampButton.IsActive ? $"http://{ThisDevice.IP}:80/off" : $"http://{ThisDevice.IP}:80/on";
        var response = NetworkManager.Send(url, "WHALE");

        if (response.Code == 0)
        {
            MainPage.Log += $"\n[{Time.Current}] {response.Message}";
        }
        else
        {
            if (response.Code == HttpStatusCode.OK)
            {
                LampButton.IsActive = !LampButton.IsActive;
                LampButton.Update();
                var status = LampButton.IsActive ? "On" : "Off";
                MainPage.Log += $"\n[{Time.Current}] Device [{ThisDevice.IP}] state: {status}";
            }
            else
            {
                MainPage.Log += $"\n[{Time.Current}] Unable to change device [{ThisDevice.IP}] state. Device response: {response.Message}";
            }
        }
    }
}
