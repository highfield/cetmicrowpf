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
    public class DialKnob
        : FrameworkElement
    {
        private const float RawMax = 65535;

        private int _value_old;


        #region PROP Min

        private float _min;
        public float Min
        {
            get { return this._min; }
            set
            {
                if (this._min != value)
                {
                    this._min = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        #region PROP Max

        private float _max;
        public float Max
        {
            get { return this._max; }
            set
            {
                if (this._max != value)
                {
                    this._max = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        #region PROP Value

        private float _value;
        public float Value
        {
            get { return this._value; }
            set
            {
                if (this._value != value)
                {
                    this._value = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        public float MinAngle = 1 / 12f;
        public float MaxAngle = 11 / 12f;


        protected override Size MeasureOverride(Size available)
        {
            return new Size(this.Width, this.Width);
        }


        protected override void OnRender(RenderContext cc, Rect rect)
        {
            float raw;
            if (this.Min < this.Max)
            {
                raw = RawMax * (this.Value - this.Min) / (this.Max - this.Min);
                if (raw < 0)
                    raw = 0;
                else if (raw > RawMax)
                    raw = RawMax;

                if (this.MinAngle < this.MaxAngle)
                {
                    raw = this.MinAngle * RawMax + (this.MaxAngle - this.MinAngle) * raw;
                }
            }
            else
            {
                raw = RawMax / 2;
            }

            float r = rect.Width / 2;
            float cx = rect.X + r;
            float cy = rect.Y + r;

            if (cc.CurrentIsHitEnabled)
            {
                var tag = WindowService.Instance.AddTag(this);
                cc.Device.CmdTagMask(1);
                cc.Device.CmdTag(tag);

                cc.Device.CmdWidgetDial(
                    cx,
                    cy,
                    r,
                    0,
                    (int)raw
                    );

                cc.Device.CmdTagMask(0);
                cc.Device.CmdTrack(
                    cx,
                    cy,
                    1,
                    1,
                    tag);
            }
            else
            {
                cc.Device.CmdWidgetDial(
                    cx,
                    cy,
                    r,
                    0,
                    (int)raw
                    );
            }
        }


        public override void HitTest(FT800Device device, int value)
        {
            if (value >= 0 &&
                value != this._value_old &&
                this.Min < this.Max)
            {
                //Debug.Print(value.ToString());
                this._value_old = value;
                float raw = value / RawMax;
                if (this.MinAngle < this.MaxAngle)
                {
                    raw = (raw - this.MinAngle) / (this.MaxAngle - this.MinAngle);
                }
                this.Value = this.Min + (this.Max - this.Min) * raw;
                this.OnValueChanged();
            }
        }


        #region EVT ValueChanged

        public event EventHandler ValueChanged;

        protected void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        #endregion

    }
}
