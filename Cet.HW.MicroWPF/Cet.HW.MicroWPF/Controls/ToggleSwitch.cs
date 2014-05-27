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
    public class ToggleSwitch
        : ButtonBase
    {
        public ToggleSwitch()
        {
            this.LabelOff = "Off";
            this.LabelOn = "On";
        }


        private bool _isHit;


        #region PROP LabelOff

        private string _labelOff;
        public string LabelOff
        {
            get { return this._labelOff; }
            set
            {
                if (this._labelOff != value)
                {
                    this._labelOff = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP LabelOn

        private string _labelOn;
        public string LabelOn
        {
            get { return this._labelOn; }
            set
            {
                if (this._labelOn != value)
                {
                    this._labelOn = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        protected override Size MeasureOverride(Size available)
        {
            var font = this.GetActualFont();
            var szoff = FT800Device.MeasureText(this._labelOff, font);
            var szon = FT800Device.MeasureText(this._labelOn, font);

            var h = (float)System.Math.Max(szoff.Height, szon.Height) * 20f / 16;
            return new Size(
                (float)System.Math.Max(szoff.Width, szon.Width) + 1.5f * h,
                h
                );
        }


        protected override void RenderButton(RenderContext cc, Rect rect)
        {
            var h = this.DesiredSize.Height - this.Margin.Top - this.Margin.Bottom;
            cc.Device.CmdWidgetToggle(
                rect.X + h / 2,
                rect.Y + h * 0.25f,   //adding h/4 takes the top-offset correct, but why?
                rect.Width - h,
                this.GetActualFont(),
                (this._isHit ? FT800Defs.OPT_FLAT : 0),
                (this.IsChecked ? 0xFFFF : 0),
                (this._labelOff + (char)0xFF) + this._labelOn
                );
        }


        public override void HitTest(FT800Device device, int value)
        {
            if (this._isHit)
            {
                if (value < 0)
                {
                    this._isHit = false;
                    this.Invalidate(false);
                }
            }
            else if (value >= 0)
            {
                this._isHit = true;
                this.IsChecked = !this.IsChecked;
                this.OnClick();
            }
        }

    }
}
