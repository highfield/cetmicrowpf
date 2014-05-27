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
    public class CalibrationWindow
        : Window
    {
        public CalibrationWindow()
        {
            var tb = new TextBlock();
            tb.Text = "Please tap on the dot";
            tb.HorizontalAlignment = MicroWPF.HorizontalAlignment.Left;
            tb.VerticalAlignment = MicroWPF.VerticalAlignment.Top;
            tb.Margin = new Thickness(80);
            this.Content = tb;
        }


        private int _status;


        protected override void OnRender(RenderContext cc, Rect rect)
        {
            base.OnRender(cc, rect);
            cc.Device.CmdSpecialCalibrate(0);
            this._status = 1;
        }


        internal override bool Navigator(FT800Device dc)
        {
            if (this._status == 1 &&
                dc.WaitCommandBufferIdle(blocking: false))
            {
                this._status = 2;
                this.OnCompleted();
                this.Close();
                return true;
            }

            return false;
        }


        #region EVT Completed

        public event EventHandler Completed;

        private void OnCompleted()
        {
            if (this.Completed != null)
            {
                this.Completed(this, new EventArgs());
            }
        }

        #endregion

    }
}
