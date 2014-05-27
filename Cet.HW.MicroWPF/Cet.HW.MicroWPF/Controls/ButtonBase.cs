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
    public abstract class ButtonBase
        : FrameworkElement
    {
        protected ButtonBase() { }


        #region PROP IsChecked

        private bool _isChecked;
        public bool IsChecked
        {
            get { return this._isChecked; }
            set
            {
                if (this._isChecked != value)
                {
                    this._isChecked = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        protected override void OnRender(RenderContext cc, Rect rect)
        {
            if (cc.CurrentIsHitEnabled)
            {
                var tag = WindowService.Instance.AddTag(this);
                cc.Device.CmdTagMask(1);
                cc.Device.CmdTag(tag);

                this.RenderButton(cc, rect);

                cc.Device.CmdTagMask(0);
            }
            else
            {
                this.RenderButton(cc, rect);
            }
        }


        protected abstract void RenderButton(RenderContext cc, Rect rect);


        #region EVT Click

        public event EventHandler Click;

        protected void OnClick()
        {
            if (this.Click != null)
            {
                this.Click(this, new EventArgs());
            }
        }

        #endregion

    }
}
