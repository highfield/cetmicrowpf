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
    public sealed class WindowingDemo
        : Page
    {

        public static Page Create()
        {
            return new WindowingDemo();
        }


        private WindowingDemo()
        {
            //create a single-cell container
            var container = new Grid();

            //create a text label
            var tb = new TextBlock();
            tb.Text = "this is the main window.";
            tb.Foreground = Colors.Yellow;
            tb.Font = 30;
            tb.Margin = new Thickness(30);
            tb.VerticalAlignment = MicroWPF.VerticalAlignment.Top;

            //add the label as child of the container
            container.Children.Add(tb);

            //create a pushbutton
            var button = new PushButton();
            button.Text = "Open dialog!";
            button.Width = 150;
            button.Height = 50;
            button.Margin = new Thickness(30);
            button.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
            button.Click += new EventHandler(button_Click);

            //add the button as child of the container
            //(also sibling of the label)
            container.Children.Add(button);

            //set the group as content of the page
            this.Content = container;
        }


        void button_Click(object sender, EventArgs e)
        {
            //create a secondary window which will host the container
            var dialog = new Window();
            dialog.Background = Colors.Gray;
            dialog.BorderColor = Colors.Gainsboro;
            dialog.BorderThickness = new Thickness(2);
            dialog.Width = 300;
            dialog.Height = 200;

            //create a grid-layout container
            var container = new Grid();

            container.AddColumnDefinition(1, GridUnitType.Star);
            container.AddColumnDefinition(1, GridUnitType.Star);

            container.AddRowDefinition(1, GridUnitType.Star);
            container.AddRowDefinition(80);

            //set the container as content of the window
            dialog.Content = container;

            //create a text label
            var tb = new TextBlock();
            tb.Text = "this is a dialog window.";
            tb.Foreground = Colors.Yellow;
            tb.Font = 28;
            tb.Margin = new Thickness(20);
            tb.VerticalAlignment = MicroWPF.VerticalAlignment.Top;

            //add the label as child of the container
            container.Children.Add(tb);
            container.SetRowCol(tb, 0, 0, 1, 2);

            {
                //create a pushbutton
                var button = new PushButton();
                button.Text = "Close";
                button.Margin = new Thickness(20, 10);
                button.HorizontalAlignment = MicroWPF.HorizontalAlignment.Left;
                button.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
                button.Click += (s_, e_) => dialog.Close();

                //add the button as child of the container
                //(also sibling of the label)
                container.Children.Add(button);
                container.SetRowCol(button, 1, 0);
            }
            {
                //create a pushbutton
                var button = new PushButton();
                button.Text = "Options";
                button.Margin = new Thickness(20, 10);
                button.HorizontalAlignment = MicroWPF.HorizontalAlignment.Right;
                button.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
                button.Click += new EventHandler(button2_Click);

                //add the button as child of the container
                //(also sibling of the label)
                container.Children.Add(button);
                container.SetRowCol(button, 1, 1);
            }

            //finally, show the dialog
            dialog.ShowDialog();
        }


        void button2_Click(object sender, EventArgs e)
        {
            //create a secondary window which will host the container
            var dialog = new Window();
            dialog.Background = Colors.Navy;
            dialog.BorderColor = Colors.Gainsboro;
            dialog.BorderThickness = new Thickness(2);
            dialog.Width = 220;
            dialog.Height = 120;

            //create a grid-layout container
            var container = new Grid();

            container.AddColumnDefinition(1, GridUnitType.Star);
            container.AddColumnDefinition(1, GridUnitType.Star);

            container.AddRowDefinition(1, GridUnitType.Star);
            container.AddRowDefinition(80);

            //set the container as content of the window
            dialog.Content = container;

            //create a text label
            var tb = new TextBlock();
            tb.Text = "Fry Netduino?";
            tb.Foreground = Colors.Yellow;
            tb.Font = 28;
            tb.Margin = new Thickness(20);
            tb.VerticalAlignment = MicroWPF.VerticalAlignment.Top;

            //add the label as child of the container
            container.Children.Add(tb);
            container.SetRowCol(tb, 0, 0, 1, 2);

            {
                //create a pushbutton
                var button = new PushButton();
                button.Text = "Yes!";
                button.Margin = new Thickness(10);
                button.HorizontalAlignment = MicroWPF.HorizontalAlignment.Left;
                button.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
                button.Click += (s_, e_) => dialog.Close();

                //add the button as child of the container
                //(also sibling of the label)
                container.Children.Add(button);
                container.SetRowCol(button, 1, 0);
            }
            {
                //create a pushbutton
                var button = new PushButton();
                button.Text = "Do it!";
                button.Margin = new Thickness(10);
                button.HorizontalAlignment = MicroWPF.HorizontalAlignment.Right;
                button.VerticalAlignment = MicroWPF.VerticalAlignment.Bottom;
                button.Click += (s_, e_) => dialog.Close();

                //add the button as child of the container
                //(also sibling of the label)
                container.Children.Add(button);
                container.SetRowCol(button, 1, 1);
            }

            //finally, show the dialog
            dialog.ShowDialog();
        }

    }
}
