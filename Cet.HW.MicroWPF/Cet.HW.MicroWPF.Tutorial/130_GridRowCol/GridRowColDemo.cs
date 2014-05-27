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
    public sealed class GridRowColDemo
        : Page
    {
        private static uint[] _colors = new uint[9]
        {
            Colors.DarkRed,
            Colors.Red,
            Colors.Pink,
            Colors.DarkGreen,
            Colors.Green,
            Colors.Lime,
            Colors.DarkBlue,
            Colors.Blue,
            Colors.LightBlue,
        };


        public static Page Create()
        {
            return new GridRowColDemo();
        }


        private GridRowColDemo()
        {
            //create a grid container
            var grid = new Grid();
            grid.Width = 400;
            grid.Height = 200;
            grid.Margin = new Thickness(0, 20, 0, 10);

            //slice the total width into some columns
            grid.AddColumnDefinition(100);  //default unit is pixels
            grid.AddColumnDefinition(1, GridUnitType.Star);
            grid.AddColumnDefinition(3, GridUnitType.Star);

            //slice the total height into some rows
            grid.AddRowDefinition(1, GridUnitType.Star);
            grid.AddRowDefinition(50, GridUnitType.Pixel);
            grid.AddRowDefinition(1, GridUnitType.Star);

            //create a series of boxes, shaded differently,
            //then add each box to the respective cell
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    //create a box, shaded as the sequence progression
                    var box = this.CreateBox(_colors[c + r * 3]);

                    //define the location (row and column) where the box
                    //should be located
                    grid.SetRowCol(box, r, c);

                    //add the box as child of the grid
                    grid.Children.Add(box);
                }
            }

            //create a container whose children will be stacked vertically
            var vertical = new StackPanel();
            vertical.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;
            vertical.VerticalAlignment = MicroWPF.VerticalAlignment.Center;

            vertical.Children.Add(
                grid
                );

            vertical.Children.Add(
                new TextBlock() { Text = "The Grid is sliced along rows and columns." }
                );

            //set the group as content of the page
            this.Content = vertical;
        }


        private ContentControl CreateBox(
            uint background
            )
        {
            //create an empty shaded box
            var cc = new ContentControl();
            cc.Background = background;
            cc.BorderColor = Colors.Gainsboro;
            cc.BorderThickness = new Thickness(1);
            return cc;
        }

    }
}
