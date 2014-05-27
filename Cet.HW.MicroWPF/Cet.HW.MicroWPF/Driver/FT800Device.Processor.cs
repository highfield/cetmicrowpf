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

        public void CmdProcessorDlstart()
        {
            this.WriteUInt32(FT800Defs.CMD_DLSTART);
        }


        public void CmdProcessorDisplay()
        {
            this.WriteUInt32(0x00000000);
        }


        public void CmdProcessorSwap()
        {
            this.WriteUInt32(FT800Defs.CMD_SWAP);
        }


        public void CmdProcessorStop()
        {
            this.WriteUInt32(FT800Defs.CMD_STOP);
        }


        public void CmdProcessorCall(
            int dest
            )
        {
            this.WriteUInt32(
                0x1D000000u |
                (ushort)dest
                );
        }


        public void CmdProcessorJump(
            int dest
            )
        {
            this.WriteUInt32(
                0x1E000000u |
                (ushort)dest
                );
        }


        public void CmdProcessorBegin(
            GpuPrimitive prim
            )
        {
            this.WriteUInt32(
                0x1F000000u |
                (byte)prim
                );
        }


        public void CmdProcessorEnd()
        {
            this.WriteUInt32(0x21000000u);
        }


        public void CmdProcessorReturn()
        {
            this.WriteUInt32(0x24000000u);
        }


        public void CmdProcessorMacro(
            int m
            )
        {
            this.WriteInt32(
                0x25000000 |
                (m & 0x01)
                );
        }

    }
}
