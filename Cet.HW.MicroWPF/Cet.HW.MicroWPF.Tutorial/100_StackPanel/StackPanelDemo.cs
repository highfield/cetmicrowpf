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
    public sealed class StackPanelDemo
        : Page
    {

        public static Page Create()
        {
            return new StackPanelDemo();
        }


        private StackPanelDemo()
        {
            var vmargin = new Thickness(30, 10);
            var hmargin = new Thickness(10, 30);

            //create a container whose children will be stacked vertically
            var vertical = new StackPanel();

            vertical.Children.Add(
                this.CreateText("Those are", Colors.LimeGreen, vmargin)
                );

            vertical.Children.Add(
                this.CreateText("stacked", Colors.White, vmargin)
                );

            vertical.Children.Add(
                this.CreateText("vertically!", Colors.Red, vmargin)
                );

            //create another stack-panel, but set for the horizontal arrangement
            var horizontal = new StackPanel();
            horizontal.HorizontalOrientation = true;

            horizontal.Children.Add(
                this.CreateText("Those are", Colors.Blue, hmargin)
                );

            horizontal.Children.Add(
                this.CreateText("stacked", Colors.LightBlue, hmargin)
                );

            horizontal.Children.Add(
                this.CreateText("horizontally instead!", Colors.White, hmargin)
                );

            //add the latter panel to the main container:
            //it will be stacked accordingly
            vertical.Children.Add(horizontal);

            //set the group as content of the page
            this.Content = vertical;
        }


        private FrameworkElement CreateText(
            string text,
            uint color,
            Thickness margin
            )
        {
            //create a text label
            var tb = new TextBlock();
            tb.Text = text;
            tb.Foreground = color;
            tb.Font = 28;
            tb.Margin = margin;
            return tb;
        }

    }
}
