using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Collections;
using Cet.HW.MicroWPF.Controls;

/*
 * Copyright 2014 by Mario Vernari, Cet Electronics
 * Part of "Cet Open Toolbox" (http://cetdevelop.codeplex.com/)
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
namespace Cet.HW.MicroWPF
{
    public class Program
    {
        private static OutputPort _pd;


        public static void Main()
        {
            _pd = new OutputPort(Pins.GPIO_PIN_D8, false);

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

            using (SPI spi = new SPI(config))
            {
                var context = new FT800Device(spi, _pd);
                context.Init();

                var w = new Window();
                w.Width = context.ScreenWidth;
                w.Height = context.ScreenHeight;
                w.Background = Colors.DimGray;
                w.BorderColor = Colors.Gainsboro;
                w.BorderThickness = new Thickness(1);
                WindowService.Instance.MainWindow = w;

                w.Navigate(DemoFont.Create);

                new CalibrationWindow().ShowDialog();

                while (true)
                {
                    WindowService.Instance.Clock(context);
                    Thread.Sleep(100);
                }
            }
        }

    }


    public class DemoFont : Page
    {
        public static Page Create()
        {
            return new DemoFont();
        }

        private DemoFont()
        {
            var grid = new Grid();
            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1.5f, GridUnitType.Star);

            int c = 0;
            StackPanel stack = null;
            for (int i = 16; i < 32; i++)
            {
                if (i == 25) stack = null;
                if (stack == null)
                {
                    stack = new StackPanel();
                    grid.Children.Add(stack);
                    grid.SetRowCol(stack, 0, c++);
                }

                var tb = new TextBlock()
                {
                    Font = i,
                    Text = i + ": Betty the cat!",
                    HorizontalAlignment = MicroWPF.HorizontalAlignment.Left,
                    VerticalAlignment = MicroWPF.VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };

                if ((i & 1) != 0)
                {
                    tb.Background = Colors.DarkBlue;
                    tb.Foreground = Colors.Yellow;
                }
                else
                {
                    tb.Background = Colors.DarkOrange;
                    tb.Foreground = Colors.Lime;
                }

                stack.Children.Add(tb);
            }

            var btn_next = new PushButton() { Text = "Next" };
            btn_next.Click += new EventHandler(btn_next_Click);
            btn_next.HorizontalAlignment = MicroWPF.HorizontalAlignment.Right;
            btn_next.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
            grid.Children.Add(btn_next);

            this.Content = grid;
        }

        void btn_next_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Navigate(DemoGridAlign.Create);
        }
    }


    public class DemoGridAlign : Page
    {
        public static Page Create()
        {
            return new DemoGridAlign();
        }

        private DemoGridAlign()
        {
            var grid = new Grid();
            grid.Name = "LayoutRoot";
            grid.Margin = new Thickness(20, 10);
            grid.Width = 400;
            grid.Height = 220;
            grid.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;
            grid.VerticalAlignment = MicroWPF.VerticalAlignment.Center;

            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);

            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    var sp = new StackPanel();
                    sp.Name = "S" + r + c;
                    sp.Background = ((c + r) & 1) == 0
                        ? Colors.Gray
                        : Colors.DimGray;

                    grid.SetRowCol(sp, r, c);
                    grid.Children.Add(sp);

                    var btn = new PushButton();
                    btn.Name = "B" + r + c;
                    btn.Text = btn.Name;
                    btn.HorizontalAlignment = (HorizontalAlignment)c;
                    btn.VerticalAlignment = (VerticalAlignment)r;
                    grid.SetRowCol(btn, r, c);
                    grid.Children.Add(btn);

                    if (r == 3)
                    {
                        if (c == 0)
                        {
                            btn.Text = "Prev";
                            btn.Click += new EventHandler(btn_prev_Click);
                        }
                        else if (c == 3)
                        {
                            btn.Text = "Next";
                            btn.Click += new EventHandler(btn_next_Click);
                        }
                    }
                }
            }

            this.Content = grid;
        }

        void btn_prev_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).GoBack();
        }

        void btn_next_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Navigate(DemoGridSpan.Create);
        }
    }


    public class DemoGridSpan : Page
    {
        public static Page Create()
        {
            return new DemoGridSpan();
        }

        private DemoGridSpan()
        {
            var grid = new Grid();
            grid.Margin = new Thickness(20, 10);
            grid.Width = 400;
            grid.Height = 220;
            grid.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;
            grid.VerticalAlignment = MicroWPF.VerticalAlignment.Center;

            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(2, GridUnitType.Star);
            grid.AddColumnDefinition(3, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);

            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(2, GridUnitType.Star);
            grid.AddRowDefinition(3, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);

            var btn_prev = new PushButton() { Text = "Prev" };
            btn_prev.Click += new EventHandler(btn_prev_Click);
            btn_prev.HorizontalAlignment = HorizontalAlignment.Left;
            grid.SetRowCol(btn_prev, 0, 0, 1, 2);

            var btn_next = new PushButton() { Text = "Next" };
            btn_next.Click += new EventHandler(btn_next_Click);
            grid.SetRowCol(btn_next, 0, 2, 1, 2);
            grid.Children.Add(btn_next);

            var btn_dlg = new PushButton() { Margin = new Thickness(10, 5), HorizontalAlignment = HorizontalAlignment.Stretch };
            btn_dlg.Text = "Dialog";
            btn_dlg.Click += new EventHandler(btn_dlg_Click);
            grid.SetRowCol(btn_dlg, 1, 0, 3, 2);
            grid.Children.Add(btn_dlg);

            var vstack2 = new StackPanel() { Name = "V2", Background = Colors.Red };
            vstack2.Children.Add(new TextBlock()
            {
                Font = 21,
                Text = "Betty the cat!",
                HorizontalAlignment = MicroWPF.HorizontalAlignment.Left,
                Background = Colors.DarkOrange,
                Foreground = Colors.Lime,
                Margin = new Thickness(20, 20),
            });
            grid.Children.Add(vstack2);
            grid.SetRowCol(vstack2, 1, 2, 1, 2);

            {
                var toggle = new ToggleSwitch() { VerticalAlignment = MicroWPF.VerticalAlignment.Center, Font = 31 };
                grid.SetRowCol(toggle, 2, 2);
                grid.Children.Add(toggle);
            }
            {
                var toggle = new ToggleSwitch() { VerticalAlignment = MicroWPF.VerticalAlignment.Center, Font = 20 };
                grid.SetRowCol(toggle, 3, 2);
                grid.Children.Add(toggle);
            }

            this.Content = grid;
        }

        void btn_dlg_Click(object sender, EventArgs e)
        {
            var dlg = new Window();

            var btn = new PushButton();
            btn.Click += (s_, e_) => dlg.Close();
            btn.Margin = new Thickness(50, 30);

            var stack = new StackPanel();
            stack.Children.Add(new TextBlock() { Text = "This is a dialog window!" });
            stack.Children.Add(btn);

            dlg.Content = stack;
            dlg.Background = Colors.DarkKhaki;
            dlg.BorderColor = Colors.Gainsboro;
            dlg.BorderThickness = new Thickness(2);
            dlg.Width = 200;
            dlg.Height = 150;
            dlg.ShowDialog();
        }

        void btn_prev_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).GoBack();
        }

        void btn_next_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Navigate(DemoStackPanels.Create);
        }
    }


    public class DemoStackPanels : Page
    {
        public static Page Create()
        {
            return new DemoStackPanels();
        }

        private DemoStackPanels()
        {
            var btn_prev = new PushButton() { Margin = new Thickness(10, 5), Text = "Prev" };
            btn_prev.Click += new EventHandler(btn_prev_Click);

            var btn_next = new PushButton() { Margin = new Thickness(10, 5), Text = "Next" };
            btn_next.Click += new EventHandler(btn_next_Click);
            btn_next.HorizontalAlignment = HorizontalAlignment.Center;

            var hstack = new StackPanel() { HorizontalOrientation = true };
            hstack.Background = Colors.Blue;
            hstack.Children.Add(btn_prev);

            var vstack = new StackPanel();
            vstack.Background = Colors.Pink;
            hstack.Children.Add(vstack);

            hstack.Children.Add(btn_next);

            vstack.Children.Add(
                new PushButton() { Margin = new Thickness(10, 5) }
                );

            {
                var ctr = new StackPanel() { HorizontalOrientation = true };
                ctr.Background = Colors.DarkGreen;
                vstack.Children.Add(ctr);

                for (int i = 0; i < 3; i++)
                {
                    var btn = new PushButton() { Text = i.ToString() };
                    btn.Margin = new Thickness(5);
                    ctr.Children.Add(btn);
                }
            }

            vstack.Children.Add(
                new PushButton() { Margin = new Thickness(10, 5) }
                );

            this.Content = hstack;
        }

        void btn_prev_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).GoBack();
        }

        void btn_next_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Navigate(DemoSliders.Create);
        }
    }


    public class DemoSliders : Page
    {
        public static Page Create()
        {
            return new DemoSliders();
        }

        private DemoSliders()
        {
            var btn_prev = new PushButton() { Margin = new Thickness(10, 5), Text = "Prev" };
            btn_prev.Click += new EventHandler(btn_prev_Click);
            btn_prev.HorizontalAlignment = MicroWPF.HorizontalAlignment.Left;
            btn_prev.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;

            var btn_next = new PushButton() { Margin = new Thickness(10, 5), Text = "Next" };
            btn_next.Click += new EventHandler(btn_next_Click);
            btn_next.HorizontalAlignment = HorizontalAlignment.Right;
            btn_next.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;

            var grid = new Grid();
            grid.Name = "GRID";
            grid.Margin = new Thickness(20);

            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(2, GridUnitType.Star);

            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);

            {
                var vslider = new Slider();
                vslider.Max = 100;
                vslider.Width = 20;
                vslider.Height = 200;
                grid.SetRowCol(vslider, 0, 0, 3, 1);
                grid.Children.Add(vslider);
            }
            {
                var vslider = new Slider();
                vslider.Max = 100;
                vslider.Width = 20;
                vslider.Height = 200;
                grid.SetRowCol(vslider, 0, 1, 3, 1);
                grid.Children.Add(vslider);
            }
            {
                var hslider = new Slider();
                hslider.Max = 100;
                hslider.Width = 200;
                hslider.Height = 20;
                grid.SetRowCol(hslider, 0, 2);
                grid.Children.Add(hslider);
            }
            {
                var hslider = new Slider();
                hslider.Max = 100;
                hslider.Width = 200;
                hslider.Height = 20;
                grid.SetRowCol(hslider, 1, 2);
                grid.Children.Add(hslider);
            }
            {
                grid.SetRowCol(btn_prev, 2, 2);
                grid.Children.Add(btn_prev);
            }
            {
                grid.SetRowCol(btn_next, 2, 2);
                grid.Children.Add(btn_next);
            }

            this.Content = grid;
        }

        void btn_prev_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).GoBack();
        }

        void btn_next_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).Navigate(DemoKnobs.Create);
        }
    }


    public class DemoKnobs : Page
    {
        public static Page Create()
        {
            return new DemoKnobs();
        }

        private DemoKnobs()
        {
            var btn_prev = new PushButton() { Margin = new Thickness(10, 5), Text = "Prev" };
            btn_prev.Click += new EventHandler(btn_prev_Click);

            //var btn_next = new WidgetButton() { Margin = new Thickness(10, 5), Text = "Next" };
            //btn_next.Click += new EventHandler(btn_next_Click);
            //btn_next.HAlign = HorizontalAlignment.Right;

            var grid = new Grid();
            grid.Name = "GRID";

            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(1, GridUnitType.Star);

            grid.AddRowDefinition(3, GridUnitType.Star);
            grid.AddRowDefinition(1, GridUnitType.Star);

            {
                var dial = new DialKnob();
                dial.Max = 100;
                dial.Width = 135;
                dial.HorizontalAlignment = HorizontalAlignment.Center;
                dial.VerticalAlignment = VerticalAlignment.Center;
                grid.SetRowCol(dial, 0, 0);
                grid.Children.Add(dial);
            }
            {
                var dial = new DialKnob();
                dial.Max = 100;
                dial.Width = 135;
                dial.HorizontalAlignment = HorizontalAlignment.Center;
                dial.VerticalAlignment = VerticalAlignment.Center;
                grid.SetRowCol(dial, 0, 1);
                grid.Children.Add(dial);
            }
            {
                var dial = new DialKnob();
                dial.Max = 100;
                dial.Width = 135;
                dial.HorizontalAlignment = HorizontalAlignment.Center;
                dial.VerticalAlignment = VerticalAlignment.Center;
                grid.SetRowCol(dial, 0, 2);
                grid.Children.Add(dial);
            }
            {
                grid.SetRowCol(btn_prev, 1, 0);
                grid.Children.Add(btn_prev);
            }
            //{
            //    grid.SetRowCol(btn_next, 2, 1);
            //    grid.Children.Add(btn_next);
            //}

            this.Content = grid;
        }

        void btn_prev_Click(object sender, EventArgs e)
        {
            Window.GetWindow(this).GoBack();
        }

        void btn_next_Click(object sender, EventArgs e)
        {
            //NavigationService.Instance.Navigate(new DemoPage2());
        }
    }
}
