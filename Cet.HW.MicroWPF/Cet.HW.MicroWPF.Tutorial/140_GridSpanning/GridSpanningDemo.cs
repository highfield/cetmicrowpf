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
    public sealed class GridSpanningDemo
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
            return new GridSpanningDemo();
        }


        private GridSpanningDemo()
        {
            //create a grid container
            var grid = new Grid();
            grid.Width = 400;
            grid.Height = 200;
            grid.Margin = new Thickness(0, 20, 0, 10);

            //slice the total width into 9 equally-sized columns
            for (int i = 0; i < 9; i++)
                grid.AddColumnDefinition(1, GridUnitType.Star);

            //slice the total height into 5 equally-sized rows
            for (int i = 0; i < 5; i++)
                grid.AddRowDefinition(1, GridUnitType.Star);

            //create a series of boxes, shaded differently,
            //then make every box spanning some cells
            for (int r = 0; r < 5; r++)
            {
                //create a box, shaded as the sequence progression
                var box = this.CreateBox(_colors[r]);

                //define the location (row and column) and the spanning
                grid.SetRowCol(box, r, 4 - r, 1, 1 + r * 2);

                //add the box as child of the grid
                grid.Children.Add(box);
            }

            for (int c = 0; c < 4; c++)
            {
                {
                    //create a box, shaded as the sequence progression
                    var box = this.CreateBox(_colors[c]);

                    //define the location (row and column) and the spanning
                    grid.SetRowCol(box, 0, c, 4 - c, 1);

                    //add the box as child of the grid
                    grid.Children.Add(box);
                }
                {
                    //create a box, shaded as the sequence progression
                    var box = this.CreateBox(_colors[c]);

                    //define the location (row and column) and the spanning
                    grid.SetRowCol(box, 0, c + 5, c + 1, 1);

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
                new TextBlock() { Text = "Grid cells' row- and column-spanning." }
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
