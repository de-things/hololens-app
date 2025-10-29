using DT.Source.Managers;
using DT.Source.Interfaces;
using DT.Source.Graphics;
using System.Net;
using System;
using DT.Source.Globals;

namespace DT.Source.Windows;

class ExampleLampWindow : LayoutManager, IWindow
{
    // TODO:
    // Make it selectable on window spawn.
    string IP = "10.62.41.175";

    PopUp  LampPopUp  = new();
    Window LampWindow = new();
    Button LampButton = new();
    public async void Initialize()
    {
        // Set window title
        LampWindow.Title = "Example Lamp";
        // Show window
        await LampWindow.Initialize();

        // Set button properties
        LampButton.SvgPath = Svg.Lamp;
        LampButton.IconSize = new(160, 195);
        LampButton.OnCreated = (e, a) =>
        {
            var url = $"http://{IP}:80/state";
            var respose = NetworkManager.Send(url, "WHALE");

            try
            {
                LampButton.IsActive = Convert.ToBoolean(int.Parse(respose.Message));
                LampButton.Update();
                MainPage.Log += $"\n[{Time.Current}] {IP} State: {respose.Message}";
            }
            catch
            {
                MainPage.Log += $"\n[{Time.Current}] Cannot get a proper device state. Device returned: {respose.Message}";
            }
        };
        LampButton.OnPointerReleased = (e, a) =>
        {
            var url = LampButton.IsActive ? $"http://{IP}:80/off" : $"http://{IP}:80/on";
            var respose = NetworkManager.Send(url, "WHALE");

            if (respose.Code == HttpStatusCode.OK)
            {
                LampButton.IsActive = !LampButton.IsActive;
                LampButton.Update();
                var status = LampButton.IsActive ? "On" : "Off";
                MainPage.Log += $"\n[{Time.Current}] State: {status}";
            }
        };
        // Initialize button
        LampButton.Initialize();

        // Attach button to a window
        LampWindow.Add(LampButton.GetRoot());

        // TODO:
        // Attach pop up of button to a window
        LampWindow.Add(LampPopUp.GetRoot());
    }
}
