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
    public struct Thickness
    {
        public static readonly Thickness None = new Thickness();


        public Thickness(
            float uniform
            )
            : this(uniform, uniform, uniform, uniform)
        {
        }


        public Thickness(
            float left_right, 
            float top_bottom
            )
            : this(left_right, top_bottom, left_right, top_bottom)
        {
        }


        public Thickness(
            float left, 
            float top, 
            float right, 
            float bottom
            )
            : this()
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }


        public float Left;
        public float Top;
        public float Right;
        public float Bottom;


        public override bool Equals(object obj)
        {
            return (obj is Thickness)
                ? this == (Thickness)obj
                : false;
        }


        public static bool operator ==(Thickness a, Thickness b)
        {
            return
                a.Left == b.Left &&
                a.Right == b.Right &&
                a.Top == b.Top &&
                a.Bottom == b.Bottom;
        }


        public static bool operator !=(Thickness a, Thickness b)
        {
            return !(a == b);
        }


        public override int GetHashCode()
        {
            return
                this.Left.GetHashCode() ^
                this.Right.GetHashCode() ^
                this.Top.GetHashCode() ^
                this.Bottom.GetHashCode();
        }

    }
}
