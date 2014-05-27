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
    public struct Size
    {
        public static readonly Size Empty = new Size();


        public Size(
            float width, 
            float height
            )
        {
            this.Width = width;
            this.Height = height;
        }


        public float Width;
        public float Height;


        public override bool Equals(object obj)
        {
            return (obj is Size)
                ? this == (Size)obj
                : false;
        }


        public static bool operator ==(Size a, Size b)
        {
            return
                a.Width == b.Width &&
                a.Height == b.Height;
        }


        public static bool operator !=(Size a, Size b)
        {
            return !(a == b);
        }


        public override int GetHashCode()
        {
            return
                this.Width.GetHashCode() ^
                this.Height.GetHashCode();
        }

#if DEBUG
        public override string ToString()
        {
            return "{" + this.Width + "; " + this.Height + "}";
        }
#endif

    }
}
