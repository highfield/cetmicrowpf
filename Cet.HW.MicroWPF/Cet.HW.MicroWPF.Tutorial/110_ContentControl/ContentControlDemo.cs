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
    public sealed class ContentControlDemo
        : Page
    {

        public static Page Create()
        {
            return new ContentControlDemo();
        }


        private ContentControlDemo()
        {
            //create a container whose children will be stacked vertically
            var vertical = new StackPanel();
            vertical.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;

            //create another stack-panel, but set for the horizontal arrangement
            var horizontal = new StackPanel();
            horizontal.HorizontalOrientation = true;
            horizontal.Margin = new Thickness(15);

            horizontal.Children.Add(
                this.CreateBoxedText("Green", Colors.Green, Colors.White)
                );

            horizontal.Children.Add(
                this.CreateBoxedText("White", Colors.White, Colors.Black)
                );

            horizontal.Children.Add(
                this.CreateBoxedText("Red", Colors.Red, Colors.White)
                );

            //add the latter panel to the main container
            vertical.Children.Add(horizontal);

            //create another boxed text...
            var box = this.CreateBoxedText(
                "Just the Italian flag... ;)",
                Colors.None,
                Colors.LightGreen
                );
            
            //...and adjust the size
            box.Width = 400;
            box.Height = 50;

            //also define a different-sized edge
            box.BorderThickness = new Thickness(3, 1, 3, 1);

            //finally add the latter box to the main container
            vertical.Children.Add(box);

            //set the group as content of the page
            this.Content = vertical;
        }


        private ContentControl CreateBoxedText(
            string text,
            uint background,
            uint foreground
            )
        {
            //create a text label
            var tb = new TextBlock();
            tb.Text = text;
            tb.Foreground = foreground;
            tb.Font = 30;
            tb.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;
            tb.VerticalAlignment = MicroWPF.VerticalAlignment.Center;

            //create a box to enclose it
            var cc = new ContentControl();
            cc.Background = background;
            cc.BorderColor = Colors.Gainsboro;
            cc.BorderThickness = new Thickness(2);
            cc.Width = 130;
            cc.Height = 180;

            //set the content as the text label
            cc.Content = tb;

            return cc;
        }

    }
}
