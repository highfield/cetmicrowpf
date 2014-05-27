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
    public sealed class GridSingleDemo
        : Page
    {

        public static Page Create()
        {
            return new GridSingleDemo();
        }


        private GridSingleDemo()
        {
            //create a grid container
            var grid = new Grid();
            grid.Width = 320;
            grid.Height = 200;

            //add some text blocks as children of the grid panel
            //at that point you can arrange them by using the margin
            //and the horizontal/vertical alignment properties
            {
                var tb = this.CreateText(
                    "Top-left aligned",
                    Colors.Pink,
                    new Thickness(50, 20),
                    HorizontalAlignment.Left,
                    VerticalAlignment.Top
                    );

                grid.Children.Add(tb);
            }
            {
                var tb = this.CreateText(
                    "centered",
                    Colors.LightCyan,
                    new Thickness(),
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center
                    );

                grid.Children.Add(tb);
            }
            {
                var tb = this.CreateText(
                    "Bottom-right aligned",
                    Colors.LightGreen,
                    new Thickness(20, 50),
                    HorizontalAlignment.Right,
                    VerticalAlignment.Bottom
                    );

                grid.Children.Add(tb);
            }

            //wrap the grid around a ContentControl, so that
            //can be drawn a border
            var cc = new ContentControl();
            cc.BorderColor = Colors.Gainsboro;
            cc.BorderThickness = new Thickness(2);
            cc.Margin = new Thickness(20);
            cc.Content = grid;

            //create a container whose children will be stacked vertically
            var vertical = new StackPanel();
            vertical.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;
            vertical.VerticalAlignment = MicroWPF.VerticalAlignment.Center;

            vertical.Children.Add(
                cc
                );

            vertical.Children.Add(
                new TextBlock() { Text = "The Grid element as a single-cell container." }
                );

            //set the group as content of the page
            this.Content = vertical;
        }


        private FrameworkElement CreateText(
            string text,
            uint color,
            Thickness margin,
            HorizontalAlignment halign,
            VerticalAlignment valign
            )
        {
            //create a text label
            var tb = new TextBlock();
            tb.Text = text;
            tb.Foreground = color;
            tb.Font = 28;
            tb.Margin = margin;
            tb.HorizontalAlignment = halign;
            tb.VerticalAlignment = valign;
            return tb;
        }

    }
}
