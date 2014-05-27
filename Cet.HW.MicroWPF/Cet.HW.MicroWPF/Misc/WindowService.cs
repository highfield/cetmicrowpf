using System;
using Microsoft.SPOT;
using System.Collections;
using Cet.HW.MicroWPF.Controls;

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
    public sealed class WindowService
    {
        private const int MaxWindows = 4;
        private const int MaxTags = 32; //FT800 limit


        #region Singleton pattern

        private WindowService() { }

        private static WindowService _instance;

        public static WindowService Instance
        {
            get
            {
                if (_instance == null) _instance = new WindowService();
                return _instance;
            }
        }

        #endregion


        private readonly WeakReference[] _hitTags = new WeakReference[MaxTags];
        private int _hitTagCount;
        private int _lastTag;

        private readonly Window[] _windowStack = new Window[MaxWindows];
        private int _windowCount;


        public Window MainWindow 
        {
            get { return this._windowCount > 0 ? this._windowStack[0] : (Window)null; }
            set
            {
                if (value == null) 
                    throw new ArgumentNullException("value");

                this._windowStack[0] = value;
                if (this._windowCount == 0)
                    this._windowCount = 1;
                this._invalidation = 3;
            }
        }


        internal void PushWindow(Window w)
        {
            if (this._windowCount < MaxWindows &&
                w != null
                )
            {
                this._windowStack[this._windowCount++] = w;
                this._invalidation = 3;
            }
        }


        internal void PopWindow(Window w)
        {
            if (this._windowCount > 1)
            {
                this._windowStack[this._windowCount--] = null;
                this._invalidation = 3;
            }
        }


        #region Layout rendering process

        private int _invalidation;


        public void Invalidate(bool complete)
        {
            if (complete)
            {
                this._invalidation = 2;
            }
            else if (this._invalidation == 0)
            {
                this._invalidation = 1;
            }
        }


        public int AddTag(FrameworkElement source)
        {
            this._hitTags[++this._hitTagCount] = new WeakReference(source);
            return this._hitTagCount;
        }


        public void Clock(FT800Device dc)
        {
            if (this._windowCount > 0 &&
                this._windowStack[this._windowCount - 1].Navigator(dc))
            {
                this._invalidation = 3;
            }

            int invalidation = this._invalidation;
            this._invalidation = 0;

            if (invalidation > 2)
                this._lastTag = -1;

            if (invalidation > 1 &&
                this._windowCount > 0)
            {
                var size = new Size(dc.ScreenWidth, dc.ScreenHeight);
                for (int i = 0; i < this._windowCount; i++)
                {
                    Window w;
                    (w = this._windowStack[i]).Measure(size);
                    w.Arrange(new Rect(0, 0, dc.ScreenWidth, dc.ScreenHeight));
                }
            }

            if (invalidation > 0)
            {
                this._hitTagCount = 0;

                var cc = new RenderContext();
                cc.Device = dc;

                dc.BeginCommandTransaction();
                dc.CmdProcessorDlstart();
                //dc.Ft_Gpu_CoCmd_ClearColorRGB(
                //dc.Ft_Gpu_CoCmd_ClearTag(255);
                dc.CmdClear(1, 1, 1);

                for (int i = 0; i < this._windowCount; i++)
                {
                    cc.CurrentIsHitEnabled = i == (this._windowCount - 1);
                    this._windowStack[i].Render(cc, 0, 0);
                }

                dc.CmdProcessorDisplay();
                dc.CmdProcessorSwap();
                dc.EndTransaction();
            }

            if (this._windowCount > 0 &&
                this._hitTagCount > 0)
            {
                int tag;
                if ((tag = (int)dc.ReadUInt32Immediate(FT800Defs.REG_TOUCH_TAG)) > 0)
                {
                    if (this._lastTag >= 0 &&
                        tag <= this._hitTagCount)
                    {
                        uint tracker = dc.ReadUInt32Immediate(FT800Defs.REG_TRACKER);
                        ((FrameworkElement)this._hitTags[tag].Target).HitTest(dc, (int)(tracker >> 16));
                        this._lastTag = tag;
                    }
                }
                else if (this._lastTag > 0)
                {
                    if (this._lastTag <= this._hitTagCount)
                    {
                        ((FrameworkElement)this._hitTags[this._lastTag].Target).HitTest(dc, -1);
                    }

                    this._lastTag = 0;
                }
                else
                {
                    this._lastTag = 0;
                }
            }
        }

        #endregion

    }
}
