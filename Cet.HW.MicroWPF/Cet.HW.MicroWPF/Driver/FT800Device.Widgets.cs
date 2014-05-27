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
namespace Cet.HW.MicroWPF
{
    public partial class FT800Device
    {

        public void CmdWidgetToggle(
            float x,
            float y,
            float w,
            int font,
            int options,
            int state,
            string s
            )
        {
            this.WriteUInt32(FT800Defs.CMD_TOGGLE);
            this.WriteInt32(x, y);
            this.WriteInt32((int)w, font);
            this.WriteInt32(options, state);
            this.WriteString(s);
        }


        /* Error handling for val is not done, so better to always use range of 65535 in order that needle is drawn within display region */
        public void CmdWidgetGauge(
            float x,
            float y,
            float r,
            int options,
            int major,
            int minor,
            int val,
            int range
            )
        {
            this.WriteUInt32(FT800Defs.CMD_GAUGE);
            this.WriteInt32(x, y);
            this.WriteUInt32((int)r, options);
            this.WriteUInt32(major, minor);
            this.WriteUInt32(val, range);
        }


        public void CmdWidgetSpinner(
            float x,
            float y,
            int style,
            int scale
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SPINNER);
            this.WriteInt32(x, y);
            this.WriteUInt32(style, scale);
        }


        public void CmdWidgetSlider(
            float x,
            float y,
            float w,
            float h,
            int options,
            int val,
            int range
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SLIDER);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32(options, val);
            this.WriteInt32((ushort)range);
        }


        public void CmdWidgetButton(
            float x,
            float y,
            float w,
            float h,
            int font,
            int options,
            string s
            )
        {
            this.WriteUInt32(FT800Defs.CMD_BUTTON);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32(font, options);
            this.WriteString(s);
        }


        public void CmdWidgetScrollbar(
            float x,
            float y,
            float w,
            float h,
            int options,
            int val,
            int size,
            int range
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SCROLLBAR);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32(options, val);
            this.WriteUInt32(size, range);
        }


        public void CmdWidgetKeys(
            float x,
            float y,
            float w,
            float h,
            int font,
            int options,
            string s
            )
        {
            this.WriteUInt32(FT800Defs.CMD_KEYS);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32(font, options);
            this.WriteString(s);
        }


        public void CmdWidgetDial(
            float x,
            float y,
            float r,
            int options,
            int val
            )
        {
            this.WriteUInt32(FT800Defs.CMD_DIAL);
            this.WriteInt32(x, y);
            this.WriteUInt32((int)r, options);
            this.WriteInt32((ushort)val);
        }


        public void CmdWidgetClock(
            float x,
            float y,
            float r,
            int options,
            int h,
            int m,
            int s,
            int ms
            )
        {
            this.WriteUInt32(FT800Defs.CMD_CLOCK);
            this.WriteInt32(x, y);
            this.WriteInt32((int)r, options);
            this.WriteUInt32(h, m);
            this.WriteUInt32(s, ms);
        }


        public void CmdWidgetProgress(
            float x,
            float y,
            float w,
            float h,
            int options,
            int val,
            int range
            )
        {
            this.WriteUInt32(FT800Defs.CMD_PROGRESS);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32(options, val);
            this.WriteInt32((ushort)range);
        }

    }
}
