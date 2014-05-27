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

        public void CmdClearTag(
            int s
            )
        {
            this.WriteInt32(
                0x12000000 |
                (byte)s
                );
        }


        public void CmdTagMask(
            int mask
            )
        {
            this.WriteInt32(
                0x14000000 |
                (mask & 0x01)
                );
        }


        public void CmdTag(
            int s
            )
        {
            this.WriteInt32(
                0x03000000 |
                (byte)s
                );
        }


        public void CmdTrack(
            float x,
            float y,
            float w,
            float h,
            int tag
            )
        {
            this.WriteUInt32(FT800Defs.CMD_TRACK);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32((ushort)tag);
        }

    }
}
