using System;
using Microsoft.SPOT;
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
    public sealed class ToggleSwitchDemo
        : Page
    {

        public static Page Create()
        {
            return new ToggleSwitchDemo();
        }


        private ToggleSwitchDemo()
        {
            //create a container whose children will be stacked vertically
            var container = new StackPanel();

            //create a text label
            var tb = new TextBlock();
            tb.Text = "---";
            tb.Foreground = Colors.Yellow;
            tb.Font = 30;
            tb.Margin = new Thickness(30);

            //add the label as child of the stack-panel
            container.Children.Add(tb);

            //create a toggle-switch
            var toggle = new ToggleSwitch();
            toggle.Width = 200;
            toggle.Height = 80;
            toggle.Click += (s, e) =>
            {
                tb.Text = "The upper switch yields " + toggle.IsChecked;
            };

            //create another toggle-switch
            var btn = new ToggleSwitch();
            btn.LabelOff = "Default";
            btn.LabelOn = "Custom";
            btn.Width = 200;
            btn.Height = 80;
            btn.Margin = new Thickness(30);
            btn.Click += (s, e) =>
            {
                if (btn.IsChecked)
                {
                    toggle.LabelOff = "Roma";
                    toggle.LabelOn = "Venezia";
                }
                else
                {
                    toggle.LabelOff = "Off";
                    toggle.LabelOn = "On";
                }
            };

            //add both the switches as children of the stack-panel
            //(also siblings of the label)
            container.Children.Add(toggle);
            container.Children.Add(btn);

            //set the group as content of the page
            this.Content = container;
        }

    }
}
