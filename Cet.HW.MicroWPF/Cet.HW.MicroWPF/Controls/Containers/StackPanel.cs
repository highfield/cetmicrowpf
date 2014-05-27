using System;
using Microsoft.SPOT;

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
namespace Cet.HW.MicroWPF.Controls
{
    public class StackPanel 
        : Panel
    {
        public bool HorizontalOrientation { get; set; }


        protected override Size MeasureOverride(Size available)
        {
            var desired = new Size();
            var constraint = available;

            if (this.HorizontalOrientation)
            {
                constraint.Height = float.MaxValue;

                for (int i = 0, count = this.Children.Count; i < count; i++)
                {
                    var child = (FrameworkElement)this.Children[i];
                    child.Measure(available);
                    desired.Width += child.DesiredSize.Width;
                    if (child.DesiredSize.Height > desired.Height)
                        desired.Height = child.DesiredSize.Height;
                }
            }
            else
            {
                constraint.Width = float.MaxValue;

                for (int i = 0, count = this.Children.Count; i < count; i++)
                {
                    var child = (FrameworkElement)this.Children[i];
                    child.Measure(available);
                    if (child.DesiredSize.Width > desired.Width)
                        desired.Width = child.DesiredSize.Width;
                    desired.Height += child.DesiredSize.Height;
                }
            }

            return desired;
        }


        protected override void ArrangeOverride(Size finalSize)
        {
            if (this.HorizontalOrientation)
            {
                float offset = 0;
                for (int i = 0, count = this.Children.Count; i < count; i++)
                {
                    var child = (FrameworkElement)this.Children[i];
                    var rect = new Rect(offset, 0, child.DesiredSize.Width, finalSize.Height);
                    child.Arrange(rect);
                    offset += child.DesiredSize.Width;
                }
            }
            else
            {
                float offset = 0;
                for (int i = 0, count = this.Children.Count; i < count; i++)
                {
                    var child = (FrameworkElement)this.Children[i];
                    var rect = new Rect(0, offset, finalSize.Width, child.DesiredSize.Height);
                    child.Arrange(rect);
                    offset += child.DesiredSize.Height;
                }
            }
        }

    }
}
