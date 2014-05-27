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
    public class FrameworkElement
    {
        public FrameworkElement()
        {
            this._hash = Helpers.GetHash();
        }


        private Rect _renderRect;
        public string Name;

        public FrameworkElement Parent { get; internal set; }


        #region PROP Width

        private float _width = -float.MaxValue;
        public float Width
        {
            get { return this._width; }
            set
            {
                if (this._width != value)
                {
                    this._width = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP Height

        private float _height = -float.MaxValue;
        public float Height
        {
            get { return this._height; }
            set
            {
                if (this._height != value)
                {
                    this._height = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP Margin

        private Thickness _margin;
        public Thickness Margin
        {
            get { return this._margin; }
            set
            {
                if (this._margin != value)
                {
                    this._margin = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP HorizontalAlignment

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Stretch;
        public HorizontalAlignment HorizontalAlignment
        {
            get { return this._horizontalAlignment; }
            set
            {
                if (this._horizontalAlignment != value)
                {
                    this._horizontalAlignment = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP VerticalAlignment

        private VerticalAlignment _verticalAlignment = VerticalAlignment.Stretch;
        public VerticalAlignment VerticalAlignment
        {
            get { return this._verticalAlignment; }
            set
            {
                if (this._verticalAlignment != value)
                {
                    this._verticalAlignment = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP Foreground

        private uint _foreground;
        public uint Foreground
        {
            get { return this._foreground; }
            set
            {
                if (this._foreground != value)
                {
                    this._foreground = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        #region PROP Background

        private uint _background;
        public uint Background
        {
            get { return this._background; }
            set
            {
                if (this._background != value)
                {
                    this._background = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        #region PROP Font

        private int _font = -1;
        public int Font
        {
            get { return this._font; }
            set
            {
                if (this._font != value)
                {
                    this._font = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP Visibility

        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return this._visibility; }
            set
            {
                if (this._visibility != value)
                {
                    this._visibility = value;
                    this.Invalidate(true);
                }
            }
        }

        #endregion


        #region PROP IsEnabled

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                if (this._isEnabled != value)
                {
                    this._isEnabled = value;
                    this.Invalidate(false);
                }
            }
        }

        #endregion


        public Size DesiredSize { get; private set; }
        public float ActualWidth { get; private set; }
        public float ActualHeight { get; private set; }


        public void Invalidate(bool complete)
        {
            WindowService.Instance.Invalidate(complete);
        }


        public void Measure(Size available)
        {
            if (this.Visibility == MicroWPF.Visibility.Collapsed)
            {
                this.DesiredSize = new Size();
            }
            else
            {
                float left_right = this.Margin.Left + this.Margin.Right;
                float top_bottom = this.Margin.Top + this.Margin.Bottom;

                available.Width -= left_right;
                if (available.Width < 0) available.Width = 0;

                available.Height -= top_bottom;
                if (available.Height < 0) available.Height = 0;

                if (this._width >= 0)
                {
                    available.Width = this._width;
                }

                if (this._height >= 0)
                {
                    available.Height = this._height;
                }

#if DEBUG
                //Debug.Print(this.GetType() + "; Name=" + this.Name + " M=" + available.ToString());
#endif
                Size desired = this.MeasureOverride(available);

#if DEBUG
                Debug.Print(this.Name + "\tconst=" + available + "\tdesid=" + desired);
#endif

                if (this._width >= 0)
                {
                    desired.Width = this._width;
                }

                if (this._height >= 0)
                {
                    desired.Height = this._height;
                }

                desired.Width += left_right;
                if (desired.Width > available.Width) desired.Width = available.Width;

                desired.Height += top_bottom;
                if (desired.Height > available.Height) desired.Height = available.Height;

                this.DesiredSize = desired;
            }
        }


        protected virtual Size MeasureOverride(Size available)
        {
            return Size.Empty;
        }


        public void Arrange(Rect finalRect)
        {
            if (this.Visibility == MicroWPF.Visibility.Collapsed)
            {
                return;
            }

            HorizontalAlignment horizontalAlignment = this.HorizontalAlignment;
            VerticalAlignment verticalAlignment = this.VerticalAlignment;

            float left_right = this.Margin.Left + this.Margin.Right;
            float top_bottom = this.Margin.Top + this.Margin.Bottom;

            float finalWidth, finalHeight;
            float clientWidth = finalWidth = finalRect.Width - left_right;
            float clientHeight = finalHeight = finalRect.Height - top_bottom;

            if (this._width >= 0 &&
                finalWidth > this._width)
            {
                finalWidth = this._width;
            }

            if (this._height >= 0 &&
                finalHeight > this._height)
            {
                finalHeight = this._height;
            }

            var desw = this.DesiredSize.Width - left_right;
            if (desw < 0) desw = 0;
            var desh = this.DesiredSize.Height - top_bottom;
            if (desh < 0) desh = 0;

            if (finalWidth < desw ||
                horizontalAlignment != HorizontalAlignment.Stretch)
            {
                finalWidth = desw;
            }

            if (finalHeight < desh ||
                verticalAlignment != VerticalAlignment.Stretch)
            {
                finalHeight = desh;
            }

            if (this._width >= 0 &&
                finalWidth < this._width)
            {
                finalWidth = this._width;
            }

            if (this._height >= 0 &&
                finalHeight < this._height)
            {
                finalHeight = this._height;
            }
#if DEBUG
            //Debug.Print(this.GetType() + "; Name=" + this.Name + " A=" + finalSize.ToString());
#endif

            var finalSize = new Size(
                finalWidth,
                finalHeight
                );

            this.ArrangeOverride(finalSize);

#if DEBUG
            Debug.Print(this.Name + "\tarrange=" + finalSize + "\tdesid=" + this.DesiredSize);
#endif

#if true
            if (horizontalAlignment == HorizontalAlignment.Stretch && 
                finalWidth > clientWidth)
            {
                horizontalAlignment = HorizontalAlignment.Left;
            }

            if (horizontalAlignment == HorizontalAlignment.Center || 
                horizontalAlignment == HorizontalAlignment.Stretch)
            {
                this._renderRect.X = (clientWidth - finalWidth) * 0.5f;
            }
            else
            {
                if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    this._renderRect.X = clientWidth - finalWidth;
                }
                else
                {
                    this._renderRect.X = 0.0f;
                }
            }

            if (verticalAlignment == VerticalAlignment.Stretch && 
                finalHeight > clientHeight)
            {
                verticalAlignment = VerticalAlignment.Top;
            }

            if (verticalAlignment == VerticalAlignment.Center || 
                verticalAlignment == VerticalAlignment.Stretch)
            {
                this._renderRect.Y = (clientHeight - finalHeight) * 0.5f;
            }
            else
            {
                if (verticalAlignment == VerticalAlignment.Bottom)
                {
                    this._renderRect.Y = clientHeight - finalHeight;
                }
                else
                {
                    this._renderRect.Y = 0.0f;
                }
            }

            this._renderRect.X += finalRect.X + this.Margin.Left;
            this._renderRect.Y += finalRect.Y + this.Margin.Top;
            this._renderRect.Width = finalWidth;
            this._renderRect.Height = finalHeight;

#else
            var dw = this._renderRect.Width = finalSize.Width;
            //var dw = this._renderRect.Width = this.DesiredSize.Width - left_right;
            switch (this._horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    this._renderRect.X = finalRect.X + this.Margin.Left;
                    break;

                case HorizontalAlignment.Center:
                    this._renderRect.X = finalRect.X + (finalRect.Width - dw) * 0.5f;
                    break;

                case HorizontalAlignment.Right:
                    this._renderRect.X = finalRect.X + finalRect.Width - dw - this.Margin.Right;
                    break;

                case HorizontalAlignment.Stretch:
                    if (dw > finalSize.Width)
                    {
                        this._renderRect.X = finalRect.X + this.Margin.Left;
                    }
                    else
                    {
                        this._renderRect.X = finalRect.X + (finalRect.Width - dw) * 0.5f;
                        this._renderRect.Width = finalSize.Width;
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }

            var dh = this._renderRect.Height = finalSize.Height;
            //var dh = this._renderRect.Height = this.DesiredSize.Height - top_bottom;
            switch (this._verticalAlignment)
            {
                case VerticalAlignment.Top:
                    this._renderRect.Y = finalRect.Y + this.Margin.Top;
                    break;

                case VerticalAlignment.Center:
                    this._renderRect.Y = finalRect.Y + (finalRect.Height - dh) * 0.5f;
                    break;

                case VerticalAlignment.Bottom:
                    this._renderRect.Y = finalRect.Y + finalRect.Height - dh - this.Margin.Bottom;
                    break;

                case VerticalAlignment.Stretch:
                    if (dh > finalSize.Height)
                    {
                        this._renderRect.Y = finalRect.Y + this.Margin.Top;
                    }
                    else
                    {
                        this._renderRect.Y = finalRect.Y + (finalRect.Height - dh) * 0.5f;
                        this._renderRect.Height = finalSize.Height;
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
#endif
            this.ActualWidth = finalWidth;
            this.ActualHeight = finalHeight;
        }


        protected virtual void ArrangeOverride(Size finalSize)
        {
            //do nothing
        }


        public void Render(
            RenderContext cc,
            float x,
            float y
            )
        {
            if (this.Visibility != MicroWPF.Visibility.Visible)
                return;

            if (this.Foreground != 0) cc.CurrentForeground = this.Foreground;
            if (this.IsEnabled == false) cc.CurrentIsEnabled = false;

            var rect = this._renderRect;
            rect.X += x;
            rect.Y += y;
#if DEBUG
            Debug.Print(this.Name + "\trender=" + rect.ToString());
#endif
            this.OnRender(cc, rect);
        }


        protected virtual void OnRender(
            RenderContext cc,
            Rect rect
            )
        {
            //do nothing
        }


        public virtual void HitTest(
            FT800Device device,
            int value
            )
        {
            //do nothing
        }


        protected int GetActualFont()
        {
            if (this._font >= 0)
            {
                return this._font;
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetActualFont();
            }
            else
            {
                return 23;  //default font
            }
        }


        #region GetHashCode override

        //http://forums.netduino.com/index.php?/topic/10835-gethashcode-returns-zero-supposed-bug/
        private readonly int _hash;

        public override int GetHashCode()
        {
            return this._hash;
        }

        #endregion

    }
}
