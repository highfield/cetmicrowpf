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
    public sealed class FontDemo
        : Page
    {
        public static Page Create()
        {
            return new FontDemo();
        }


        private FontDemo()
        {
            //create a grid-layout container
            var container = new Grid();

            container.AddColumnDefinition(1, GridUnitType.Star);
            container.AddColumnDefinition(1.5f, GridUnitType.Star);

            //the total native fonts are 16, so arrange a short sample
            //each in two columns. In every column the strings will
            //be stacked vertically
            int c = 0;
            StackPanel stack = null;
            for (int i = 16; i < 32; i++)
            {
                //create a stack-panel
                if (i == 25) stack = null;
                if (stack == null)
                {
                    stack = new StackPanel();
                    container.Children.Add(stack);
                    container.SetRowCol(stack, 0, c++);
                }

                //create a text block styled with the current font
                var tb = new TextBlock()
                {
                    Font = i,
                    Text = i + ": I love WPF!",
                    HorizontalAlignment = MicroWPF.HorizontalAlignment.Left,
                    VerticalAlignment = MicroWPF.VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };

                //alternate the text foreground color
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

                //add the text block to the stack-panel
                stack.Children.Add(tb);
            }

            //set the group as content of the page
            this.Content = container;
        }

    }
}
