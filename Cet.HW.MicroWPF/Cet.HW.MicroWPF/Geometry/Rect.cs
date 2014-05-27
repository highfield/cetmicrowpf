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
    public struct Rect
    {
        public static readonly Rect Empty = new Rect();


        public Rect(
            float x, 
            float y, 
            float width, 
            float height
            )
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }


        public float X;
        public float Y;
        public float Width;
        public float Height;


        public float Right
        {
            get { return this.X + this.Width; }
        }


        public float Bottom
        {
            get { return this.Y + this.Height; }
        }


        public Size Size
        {
            get 
            { 
                return new Size(
                    this.Width, 
                    this.Height
                    ); 
            }
        }


        public override bool Equals(object obj)
        {
            return (obj is Rect)
                ? this == (Rect)obj
                : false;
        }


        public static bool operator ==(Rect a, Rect b)
        {
            return
                a.X == b.X &&
                a.Y == b.Y &&
                a.Width == b.Width &&
                a.Height == b.Height;
        }


        public static bool operator !=(Rect a, Rect b)
        {
            return !(a == b);
        }


        public override int GetHashCode()
        {
            return
                this.X.GetHashCode() ^
                this.Y.GetHashCode() ^
                this.Width.GetHashCode() ^
                this.Height.GetHashCode();
        }

#if DEBUG
        public override string ToString()
        {
            return "{" + this.X + "; " + this.Y + "; " + this.Width + "; " + this.Height + "}";
        }
#endif

    }
}
