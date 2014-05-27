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
    public class TextBlock 
        : FrameworkElement
    {

        #region PROP Text

        private string _text;
        public string Text
        {
            get { return this._text; }
            set
            {
                if (this._text != value)
                {
                    this._text = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        protected override Size MeasureOverride(Size available)
        {
            return FT800Device.MeasureText(
                this._text,
                this.GetActualFont()
                );
        }


        protected override void ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);
        }


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

            cc.Device.CmdColorRGB(cc.CurrentForeground);
            cc.Device.CmdDisplayText(
                rect.X,
                rect.Y,
                this.GetActualFont(),
                0,
                this.Text
                );

            cc.Device.CmdColorRGB();
        }

    }
}
