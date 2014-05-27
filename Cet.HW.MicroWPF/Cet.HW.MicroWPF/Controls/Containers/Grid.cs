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
    public class Grid
        : Panel
    {
        private const int ArrayBlockSize = 8;

        private static readonly MapEntry DummyEntry = new MapEntry()
        {
            IsValid = true,
        };

        private static readonly GridLength[] Unsplit = new GridLength[1] 
        { 
            new GridLength { Value = 1, IsStar = true }
        };


        public Grid()
        {
            this._entries[0] = DummyEntry;
            this.ClearDefs();
        }


        private MapEntry[] _entries = new MapEntry[ArrayBlockSize];
        private int _entryCount = 1;
        private int[] _emap = new int[ArrayBlockSize];
        private bool _emap_dirty;
        private int _checkMarker;
        private Rect _rect = new Rect();


        protected override Size MeasureOverride(Size available)
        {
            var result = default(Size);
            var childrenCount = this.Children.Count;

            if (this._emap_dirty ||
                this.Children.CheckMarker != this._checkMarker)
            {
                this.BuildMap();
            }

            //reset rows/cols measurements
            result.Height = Grid.ResolveStars(
                this._rowdefs,
                this._rowdefCount,
                (float)available.Height
                );

            result.Width = Grid.ResolveStars(
                this._coldefs,
                this._coldefCount,
                (float)available.Width
                );

            //gruppo auto-auto
            for (int i = 0; i < childrenCount; i++)
            {
                MapEntry entry;
                if ((entry = this._entries[this._emap[i]]).IsValid == false)
                    continue;

                this.CalculateBounds(
                    entry,
                    ref this._rect,
                    ref available
                    );

                FrameworkElement fe;
                (fe = (FrameworkElement)this.Children[i]).Measure(this._rect.Size);
            }

#if DEBUG
            for (int i = 0; i < this._coldefCount; i++)
            {
                Debug.Print("COL #" + i + ": " + this._coldefs[i]);
            }

            for (int i = 0; i < this._rowdefCount; i++)
            {
                Debug.Print("ROW #" + i + ": " + this._rowdefs[i]);
            }
#endif

            return result;
        }


        protected override void ArrangeOverride(Size finalSize)
        {
            var childrenCount = this.Children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                MapEntry entry;
                if ((entry = this._entries[this._emap[i]]).IsValid == false)
                    continue;

                this.CalculateBounds(
                    entry,
                    ref this._rect,
                    ref finalSize
                    );

#if DEBUG
                Debug.Print(this.Name + "\tarrange over=" + this._rect.ToString());
#endif
                FrameworkElement fe;
                (fe = (FrameworkElement)this.Children[i]).Arrange(this._rect);
            }
        }


        private static float ResolveStars(
            GridLength[] array,
            int count,
            float available
            )
        {
            if (array == Unsplit)
            {
                return available;
            }

            float result;
            float star_total = 0;
            float pixel_total = 0;

            for (int i = count - 1; i >= 0; i--)
            {
                GridLength glen;
                if ((glen = array[i]).IsStar)
                {
                    glen.ActualSize = 0;
                    star_total += glen.Value;
                }
                else
                {
                    glen.ActualSize = glen.Value;
                    pixel_total += glen.Value;
                }
            }

            float alpha = 0;
            if (star_total > 0)
            {
                alpha = (available - pixel_total) / star_total;
                result = available;
            }
            else
            {
                result = pixel_total;
            }

            float offset = 0;
            for (int i = 0; i < count; i++)
            {
                GridLength glen;
                (glen = array[i]).Offset = offset;

                if (glen.IsStar)
                {
                    glen.ActualSize = alpha * glen.Value;
                }

                offset += glen.ActualSize;
            }

            return result;
        }


        private void CalculateBounds(
            MapEntry entry,
            ref Rect rect,
            ref Size available
            )
        {
            if (this._coldefs == Unsplit)
            {
                rect.X = 0;
                rect.Width = available.Width;
            }
            else
            {
                rect.X = this._coldefs[entry.Col].Offset;
                rect.Width = 0;

                int k = entry.ColLast;
                do
                {
                    rect.Width += this._coldefs[k].ActualSize;
                }
                while (--k >= entry.Col);
            }

            if (this._rowdefs == Unsplit)
            {
                rect.Y = 0;
                rect.Height = available.Height;
            }
            else
            {
                rect.Y = this._rowdefs[entry.Row].Offset;
                rect.Height = 0;

                int k = entry.RowLast;
                do
                {
                    rect.Height += this._rowdefs[k].ActualSize;
                }
                while (--k >= entry.Row);
            }
        }


        #region Row defs

        private GridLength[] _rowdefs;
        private int _rowdefCount;

        public void AddRowDefinition(
            float value,
            GridUnitType unit = GridUnitType.Pixel
            )
        {
            Grid.AddDefinition(
                ref this._rowdefs,
                ref this._rowdefCount,
                new GridLength
                {
                    Value = value,
                    IsStar = unit == GridUnitType.Star
                });
        }

        #endregion


        #region Column defs

        private GridLength[] _coldefs;
        private int _coldefCount;

        public void AddColumnDefinition(
            float value,
            GridUnitType unit = GridUnitType.Pixel
            )
        {
            Grid.AddDefinition(
                ref this._coldefs,
                ref this._coldefCount,
                new GridLength
                {
                    Value = value,
                    IsStar = unit == GridUnitType.Star
                });
        }

        #endregion


        public void ClearDefs()
        {
            this._rowdefs = Unsplit;
            this._coldefs = Unsplit;
            this._rowdefCount = this._coldefCount = 1;
        }


        private static void AddDefinition(
            ref GridLength[] array,
            ref int count,
            GridLength entry
            )
        {
            if (array == Unsplit)
            {
                array = new GridLength[ArrayBlockSize];
                count = 0;
            }
            else if (count >= array.Length)
            {
                var temp = array;
                array = new GridLength[count + ArrayBlockSize];
                Array.Copy(
                    temp,
                    array,
                    count
                    );
            }

            array[count++] = entry;
        }


        public void SetRowCol(
            FrameworkElement child,
            int row,
            int col,
            int rowspan = 1,
            int colspan = 1
            )
        {
            int hash = child.GetHashCode();

            MapEntry entry = null;
            int pos;
            for (pos = 1; pos < this._entryCount; ++pos)
            {
                if ((entry = this._entries[pos]).Hash == 0 ||
                    entry.Hash == hash)
                {
                    break;
                }
            }

            if (pos >= this._entryCount)
            {
                if (this._entryCount >= this._entries.Length)
                {
                    //get the array bigger
                    var map = this._entries;
                    this._entries = new MapEntry[this._entryCount + ArrayBlockSize];
                    Array.Copy(
                        map,
                        this._entries,
                        map.Length
                        );
                }

                this._entries[this._entryCount++] = entry = new MapEntry();
            }

            entry.Hash = hash;
            entry.Row = (byte)row;
            entry.Col = (byte)col;
            entry.RowLast = (byte)(row + (rowspan > 0 ? rowspan : 1) - 1);
            entry.ColLast = (byte)(col + (colspan > 0 ? colspan : 1) - 1);

            this._emap_dirty = true;
        }


        private void BuildMap()
        {
            int count;
            if ((count = this.Children.Count) > this._emap.Length)
            {
                this._emap = new int[count + ArrayBlockSize];
            }

            for (int i = 0; i < count; i++)
            {
                int hash = this.Children[i].GetHashCode();
                int k;
                for (k = this._entryCount - 1; k > 0; k--)
                {
                    MapEntry entry;
                    if ((entry = this._entries[k]).Hash == hash)
                    {
                        break;
                    }
                }

                this._emap[i] = k;
            }

            for (int k = this._entryCount - 1; k > 0; k--)
            {
                MapEntry entry;
                if ((entry = this._entries[k]).Hash != 0)
                {
                    int i = count;
                    while (--i >= 0 && this.Children[i].GetHashCode() != entry.Hash) ;

                    if (i < 0) entry.Hash = 0;
                }

                entry.IsValid =
                    entry.Hash != 0 &&
                    entry.RowLast < this._rowdefCount &&
                    entry.ColLast < this._coldefCount;
            }

            this._emap_dirty = false;
            this._checkMarker = this.Children.CheckMarker;
        }


        private class MapEntry
        {
            public int Hash;
            public byte Row;
            public byte Col;
            public byte RowLast;
            public byte ColLast;
            public bool IsValid;
        }


        private class GridLength
        {
            public bool IsStar;
            public float Value = 1;

            public float ActualSize;
            public float Offset;

#if DEBUG
            public override string ToString()
            {
                return "offset=" + this.Offset + "; size=" + this.ActualSize;
            }
#endif
        }

    }


    public enum GridUnitType
    {
        //Auto,
        Star,
        Pixel,
    }
}
