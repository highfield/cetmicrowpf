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

        public void CmdRegRead(
            uint ptr, 
            uint result
            )
        {
            this.WriteUInt32(FT800Defs.CMD_REGREAD);
            this.WriteUInt32(ptr);
            this.WriteUInt32(0);

        }


        public void CmdMemcpy(
            uint dest, 
            uint src, 
            uint num
            )
        {
            this.WriteUInt32(FT800Defs.CMD_MEMCPY);
            this.WriteUInt32(dest);
            this.WriteUInt32(src);
            this.WriteUInt32(num);
        }


        public void CmdMemWrite(
            uint ptr, 
            uint num
            )
        {
            this.WriteUInt32(FT800Defs.CMD_MEMWRITE);
            this.WriteUInt32(ptr);
            this.WriteUInt32(num);
        }


        public void CmdMemSet(
            uint ptr,
            uint value,
            uint num
            )
        {
            this.WriteUInt32(FT800Defs.CMD_MEMSET);
            this.WriteUInt32(ptr);
            this.WriteUInt32(value);
            this.WriteUInt32(num);
        }


        public void CmdMemZero(
            uint ptr, 
            uint num
            )
        {
            this.WriteUInt32(FT800Defs.CMD_MEMZERO);
            this.WriteUInt32(ptr);
            this.WriteUInt32(num);
        }


        public void CmdGetPtr(
            uint result
            )
        {
            this.WriteUInt32(FT800Defs.CMD_GETPTR);
            this.WriteUInt32(result);
        }


        public void CmdMemCrc(
            uint ptr,
            uint num,
            uint result
            )
        {
            this.WriteUInt32(FT800Defs.CMD_MEMCRC);
            this.WriteUInt32(ptr);
            this.WriteUInt32(num);
            this.WriteUInt32(result);
        }


        public void CmdInflate(
            uint ptr
            )
        {
            this.WriteUInt32(FT800Defs.CMD_INFLATE);
            this.WriteUInt32(ptr);
        }

    }
}
