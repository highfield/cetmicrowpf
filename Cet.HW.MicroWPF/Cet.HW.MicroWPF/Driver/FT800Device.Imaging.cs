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

        public void CmdLoadImage(
            uint ptr,
            uint options
            )
        {
            this.WriteUInt32(FT800Defs.CMD_LOADIMAGE);
            this.WriteUInt32(ptr);
            this.WriteUInt32(options);
        }


        public void CmdBitmapSource(
            int address
            )
        {
            this.WriteInt32(
                0x01000000 |
                (address & 0x0FFFFF)
                );
        }


        public void CmdBitmapHandle(
            int handle
            )
        {
            this.WriteInt32(
                0x05000000 |
                (handle & 0x1F)
                );
        }


        public void CmdBitmapLayout(
            GpuBitmapLayout format,
            int linestride,
            int height
            )
        {
            this.WriteInt32(
                0x07000000 |
                (((int)format & 0x1F) << 19) |
                ((linestride & 0x3FF) << 9) |
                (height & 0x1FF)
                );
        }


        public void CmdBitmapSize(
            GpuBitmapFiltering filter,
            GpuBitmapWrap wrapx,
            GpuBitmapWrap wrapy,
            float width,
            float height
            )
        {
            this.WriteInt32(
                0x08000000 |
                (((int)filter & 0x01) << 20) |
                (((int)wrapx & 0x01) << 19) |
                (((int)wrapy & 0x01) << 18) |
                (((int)width & 0x1FF) << 9) |
                ((int)height & 0x1FF)
                );
        }


        public void CmdCell(
            int cell
            )
        {
            this.WriteInt32(
                0x06000000 |
                (cell & 0x3F)
                );
        }

    }
}
