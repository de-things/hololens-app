using DT.Source.Globals;
using DT.Source.Graphics;
using DT.Source.Interfaces;
using DT.Source.Managers;
using System;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.Lights;
using Windows.UI.Xaml.Media;

namespace DT.Source.Windows;

class ExampleLampWindow : LayoutManager, IWindow
{
    bool IsDeviceOnline = false;

    Window LampWindow = new();
    Button LampButton = new();
    Button PopUpButton = new();
    public async void Initialize()
    {
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
            LampWindow.ShowPopUp(IpList.LampIp, (s, a) => { IpList.LampIp = LampWindow.GetPopUpInput(); TryGetDeviceState(); LampWindow.ShowLoading(); });
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

            var url = $"http://{IpList.LampIp}:80/state";
            var response = NetworkManager.Send(url, "WHALE");

            if (response.Code == 0)
            {
                LampButton.IsActive = false;
                MainPage.Log += $"\n[{Time.Current}] Device [{IpList.LampIp}] cannot be reached. Is IP valid or is device online?";
                return false;
            }
            else
            {
                if (response.Message == "1" || response.Message == "0")
                {
                    LampButton.IsActive = response.Message == "1";
                    var status = LampButton.IsActive ? "On" : "Off";
                    MainPage.Log += $"\n[{Time.Current}] Device [{IpList.LampIp}] state: {status}";
                }
                else
                {
                    LampButton.IsActive = false;
                    MainPage.Log += $"\n[{Time.Current}] Unable to get a device [{IpList.LampIp}] state. Device response: {response.Message}";
                }
                return true;
            }
        });
        task.Start();

        while (!task.IsCompleted)
        {
            MainPage.Log += $"\n[{Time.Current}] Waiting device [{IpList.LampIp}] to respond...";
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

        var url = LampButton.IsActive ? $"http://{IpList.LampIp}:80/off" : $"http://{IpList.LampIp}:80/on";
        var response = NetworkManager.Send(url, "WHALE");

        if (response.Code == 0)
        {
            MainPage.Log += $"\n[{Time.Current}] Device [{IpList.LampIp}] cannot be reached. Is IP valid or is device online?";
        }
        else
        {
            if (response.Code == HttpStatusCode.OK)
            {
                LampButton.IsActive = !LampButton.IsActive;
                LampButton.Update();
                var status = LampButton.IsActive ? "On" : "Off";
                MainPage.Log += $"\n[{Time.Current}] Device [{IpList.LampIp}] state: {status}";
            }
            else
            {
                MainPage.Log += $"\n[{Time.Current}] Unable to change device [{IpList.LampIp}] state. Device response: {response.Message}";
            }
        }
    }
}
