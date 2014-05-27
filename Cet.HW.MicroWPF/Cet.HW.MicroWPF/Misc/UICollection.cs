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
namespace Cet.HW.MicroWPF
{
    public class UICollection
        : IEnumerable
    {
        public UICollection(FrameworkElement owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.Owner = owner;
        }


        private ArrayList _collection = new ArrayList();

        public readonly FrameworkElement Owner;
        public int CheckMarker { get; private set; }


        public int Count
        {
            get { return this._collection.Count; }
        }


        public FrameworkElement this[int index]
        {
            get { return (FrameworkElement)this._collection[index]; }
            set
            {
                value.Parent = this.Owner;
                this._collection[index] = value;
                this.CheckMarker++;
            }
        }


        public virtual int Add(FrameworkElement child)
        {
            child.Parent = this.Owner;
            this._collection.Add(child);
            this.CheckMarker++;
            return this._collection.Count - 1;
        }


        public virtual void Clear()
        {
            this._collection.Clear();
            this.CheckMarker++;
        }


        public virtual void Insert(int index, FrameworkElement child)
        {
            child.Parent = this.Owner;
            this._collection.Insert(index, child);
            this.CheckMarker++;
        }


        public virtual void Remove(FrameworkElement child)
        {
            this._collection.Remove(child);
            this.CheckMarker++;
        }


        public virtual void RemoveAt(int index)
        {
            this._collection.RemoveAt(index);
            this.CheckMarker++;
        }


        public IEnumerator GetEnumerator()
        {
            return this._collection.GetEnumerator();
        }

    }
}
