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
    public class Window
        : ContentControl
    {
        public Window()
        {
            this.Foreground = Colors.White;
            this.HorizontalAlignment = MicroWPF.HorizontalAlignment.Center;
            this.VerticalAlignment = MicroWPF.VerticalAlignment.Center;
        }


        #region Window life-cycle

        internal bool AsDialog;


        public void Show()
        {
            WindowService.Instance.PushWindow(this);
        }


        public void ShowDialog()
        {
            this.AsDialog = true;
            WindowService.Instance.PushWindow(this);
        }


        public void Close()
        {
            WindowService.Instance.PopWindow(this);
        }

        #endregion


        #region Navigation

        private PageCreationDelegate[] _navigationStack = new PageCreationDelegate[4];
        private int _currentIndex = -1;
        private int _nextIndex = -1;
        private PageCreationDelegate _nextPageGen;


        public void Navigate(PageCreationDelegate generator)
        {
            if (generator != null)
            {
                this._nextPageGen = generator;
                this._nextIndex = this._currentIndex + 1;
            }
        }


        public bool CanGoBack
        {
            get { return this._currentIndex > 0; }
        }


        public void GoBack()
        {
            if (this._currentIndex > 0)
            {
                this._nextIndex = this._currentIndex - 1;
                this._nextPageGen = null;
            }
        }


        internal virtual bool Navigator(FT800Device dc)
        {
            if (this._nextIndex == this._currentIndex)
                return false;

            bool proceed = true;
            if (this._currentIndex >= 0)
            {
                var args = new NavigationLeaveArgs();
                args.GoingBack = this._nextIndex < this._currentIndex;
                ((Page)this.Content).InvokeBeforeLeave(dc, args);
                if (args.Cancel) proceed = false;
            }

            if (proceed)
            {
                if (this._nextIndex > this._currentIndex)
                {
                    int len = this._navigationStack.Length;
                    if (this._nextPageGen != null)
                    {
                        if (this._nextIndex >= len)
                        {
                            var array = new PageCreationDelegate[len + 4];
                            this._navigationStack.CopyTo(array, 0);
                            this._navigationStack = array;
                        }

                        this._navigationStack[++this._currentIndex] = this._nextPageGen;
                    }
                    else if (this._nextIndex < len)
                    {
                        this._currentIndex = this._nextIndex;
                    }
                }
                else
                {
                    this._currentIndex = this._nextIndex;
                }

                Page current;
                this.Content = current = this._navigationStack[this._currentIndex]();
                current.InvokeEnter(dc);
                current.Invalidate(true);
                this._nextPageGen = null;
            }

            return proceed;
        }

        #endregion


        public static Window GetWindow(FrameworkElement source)
        {
            Window w;

            while ((w = source as Window) == null && source != null)
            {
                source = source.Parent;
            }

            return w;
        }

    }


    public class NavigationLeaveArgs
    {
        public bool GoingBack { get; internal set; }
        public bool Cancel;
    }
}
