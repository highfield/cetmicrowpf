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
    public class PushButton 
        : ButtonBase
    {

        #region PROP Text

        private string _text = "Caption";
        public string Text
        {
            get { return this._text; }
            set
            {
                if (this._text != value)
                {
                    this._text = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        protected override Size MeasureOverride(Size available)
        {
            var font = this.GetActualFont();
            var size = FT800Device.MeasureText(this._text, font);
            size.Width += 16;
            size.Height += 8;
            return size;
        }


        protected override void RenderButton(RenderContext cc, Rect rect)
        {
            cc.Device.CmdWidgetButton(
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height,
                this.GetActualFont(),
                (this.IsChecked ? FT800Defs.OPT_FLAT : 0),
                this.Text
                );
        }


        public override void HitTest(FT800Device device, int value)
        {
            if (this.IsChecked)
            {
                if (value < 0)
                {
                    this.IsChecked = false;
                }
            }
            else if (value >= 0)
            {
                this.IsChecked = true;
                this.OnClick();
                //this.IsChecked = false;
            }
        }

    }
}
