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
    public class ContentControl
        : FrameworkElement
    {

        #region PROP Content

        private FrameworkElement _content;
        public FrameworkElement Content
        {
            get { return this._content; }
            set
            {
                if (this._content != value)
                {
                    this._content = value;

                    if (this._content != null)
                        this._content.Parent = this;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP BorderColor

        private uint _borderColor;
        public uint BorderColor
        {
            get { return this._borderColor; }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        #region PROP BorderThickness

        private Thickness _borderThickness;
        public Thickness BorderThickness
        {
            get { return this._borderThickness; }
            set
            {
                if (this._borderThickness != value)
                {
                    this._borderThickness = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        protected override Size MeasureOverride(Size available)
        {
            if (this._content != null)
            {
                this._content.Measure(available);
                return this._content.DesiredSize;
            }
            else
            {
                return Size.Empty;
            }
        }


        protected override void ArrangeOverride(Size finalSize)
        {
            if (this._content != null)
            {
                var rect = new Rect(
                    this._borderThickness.Left / 2,
                    this._borderThickness.Top / 2,
                    finalSize.Width - (this._borderThickness.Left + this._borderThickness.Right) / 2,
                    finalSize.Height - (this._borderThickness.Top + this._borderThickness.Bottom) / 2
                    );

                this._content.Arrange(rect);
            }
        }


        protected override void OnRender(RenderContext cc, Rect rect)
        {
            cc.Device.DrawRect(
                rect,
                this.Background,
                this.BorderColor,
                this.BorderThickness
                );

            if (this._content != null)
            {
                this._content.Render(cc, rect.X, rect.Y);
            }
        }

    }
}
