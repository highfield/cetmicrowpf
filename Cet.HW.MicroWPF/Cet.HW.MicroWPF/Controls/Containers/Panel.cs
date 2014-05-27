using System;
using Microsoft.SPOT;
using System.Collections;

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
    public class Panel 
        : FrameworkElement
    {
        protected Panel()
        {
            this.Children = new UICollection(this);
        }


        public UICollection Children { get; private set; }


        protected override void OnRender(RenderContext cc, Rect rect)
        {
            if (this.Background != 0)
            {
                cc.Device.DrawRect(
                    rect,
                    this.Background,
                    0,
                    Thickness.None
                    );
            }

            for (int i = 0, count = this.Children.Count; i < count; i++)
            {
                var child = (FrameworkElement)this.Children[i];
                child.Render(cc, rect.X, rect.Y);
            }
        }

    }
}
