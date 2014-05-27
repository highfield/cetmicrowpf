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
    public sealed class NavigationDemo
        : Page
    {

        public static Page Create()
        {
            return new NavigationDemo();
        }


        private NavigationDemo()
        {
            //create a grid-layout container
            var container = new Grid();

            container.AddColumnDefinition(1, GridUnitType.Star);
            container.AddColumnDefinition(1, GridUnitType.Star);

            container.AddRowDefinition(1, GridUnitType.Star);
            container.AddRowDefinition(80);

            //create a text label
            var tb = new TextBlock();
            tb.Text = "This is page #1.";
            tb.Foreground = Colors.Yellow;
            tb.Font = 28;
            tb.Margin = new Thickness(20);
            tb.VerticalAlignment = MicroWPF.VerticalAlignment.Top;

            //add the label as child of the container
            container.Children.Add(tb);
            container.SetRowCol(tb, 0, 0, 1, 2);

            {
                //create a pushbutton
                var btn_next = new PushButton();
                btn_next.Text = "Next";
                btn_next.Margin = new Thickness(20, 10);
                btn_next.HorizontalAlignment = MicroWPF.HorizontalAlignment.Right;
                btn_next.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
                btn_next.Click += new EventHandler(btn_next_Click);

                //add the button as child of the container
                container.Children.Add(btn_next);
                container.SetRowCol(btn_next, 1, 1);
            }

            //set the group as content of the page
            this.Content = container;
        }


        void btn_next_Click(object sender, EventArgs e)
        {
            //get this page owning window
            var window = Window.GetWindow(this);

            //navigate to a new page
            window.Navigate(NavigationDemo2.Create);
        }

    }
}
