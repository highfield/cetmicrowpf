/**
 * Enable the requested switch at once
 **/

#define BasicSetup
//#define HelloWorld
//#define TouchCalib
//#define PushButton
//#define ToggleSwitch
//#define Slider
//#define DialKnob

//#define StackPanel
//#define ContentControl
//#define GridSingle
//#define GridRowCol
//#define GridSpanning

//#define Windowing
//#define Navigation
//#define Font
//
//#define CustomWidget


using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Cet.HW.MicroWPF.Controls;

/*
 * Copyright 2014 by Mario Vernari, Cet Electronics
 * Part of "Cet MicroWPF" (http://cetmicrowpf.codeplex.com/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Cet.HW.MicroWPF.Tutorial
{
    public class Program
    {
        private static OutputPort _pd;


        public static void Main()
        {
            //define and open the CS output port
            _pd = new OutputPort(Pins.GPIO_PIN_D8, false);

            //setup the SPI configuration
            SPI.Configuration config = new SPI.Configuration(
                SPI_mod: SPI.SPI_module.SPI1,
                ChipSelect_Port: Pins.GPIO_PIN_D10,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: 10000
                );

            //open the SPI port
            using (SPI spi = new SPI(config))
            {
                //create and init the FT800 device context
                var context = new FT800Device(spi, _pd);
                context.Init();

                //create a new window to fit the entire screen area
                var w = new Window();
                w.Width = context.ScreenWidth;
                w.Height = context.ScreenHeight;

                //set the background shading
                w.Background = Colors.DimGray;

                //provide a thin border
                w.BorderColor = Colors.Gainsboro;
                w.BorderThickness = new Thickness(1);

                //set the window as main
                WindowService.Instance.MainWindow = w;

#if BasicSetup
                //do nothing
#elif HelloWorld
                var tb = new TextBlock();
                tb.Text = "Hello world!";
                tb.Foreground = Colors.Fuchsia;
                tb.Font = 28;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                w.Content = tb;
#elif TouchCalib
                //show the calibration overlayed "dialog"
                new CalibrationWindow().ShowDialog();
#elif PushButton
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(PushButtonDemo.Create);

                //show the calibration overlayed "dialog"
                //(will be closed once the calibration is done)
                new CalibrationWindow().ShowDialog();
#elif ToggleSwitch
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(ToggleSwitchDemo.Create);

                //show the calibration overlayed "dialog"
                //(will be closed once the calibration is done)
                new CalibrationWindow().ShowDialog();
#elif Slider
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(SliderDemo.Create);

                //show the calibration overlayed "dialog"
                //(will be closed once the calibration is done)
                new CalibrationWindow().ShowDialog();
#elif DialKnob
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(DialKnobDemo.Create);

                //show the calibration overlayed "dialog"
                //(will be closed once the calibration is done)
                new CalibrationWindow().ShowDialog();
#elif StackPanel
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(StackPanelDemo.Create);
#elif ContentControl
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(ContentControlDemo.Create);
#elif GridSingle
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(Grid1Demo.Create);
#elif GridRowCol
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(GridRowColDemo.Create);
#elif GridSpanning
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(GridSpanningDemo.Create);
#elif Windowing
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(WindowingDemo.Create);

                //show the calibration overlayed "dialog"
                //(will be closed once the calibration is done)
                new CalibrationWindow().ShowDialog();
#elif Navigation
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(NavigationDemo.Create);

                //show the calibration overlayed "dialog"
                //(will be closed once the calibration is done)
                new CalibrationWindow().ShowDialog();
#elif Font
                //invoke the navigation to the demo page
                //(NOTE: the argument is a delegate, not the actual content)
                w.Navigate(FontDemo.Create);
#elif CustomWidget

#endif

                //loop endlessly for the framework heartbeating
                while (true)
                {
                    WindowService.Instance.Clock(context);
                    Thread.Sleep(100);  //recommended delay
                }
            }
        }

    }
}
