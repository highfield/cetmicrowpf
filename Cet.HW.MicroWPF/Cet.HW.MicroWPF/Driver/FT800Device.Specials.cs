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

        public void CmdSaveContext()
        {
            this.WriteUInt32(0x22000000u);
        }


        public void CmdRestoreContext()
        {
            this.WriteUInt32(0x23000000u);
        }


        public void CmdSpecialSnapshot(
            uint ptr
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SNAPSHOT);
            this.WriteUInt32(ptr);
        }


        public void CmdSpecialScreenSaver()
        {
            this.WriteUInt32(FT800Defs.CMD_SCREENSAVER);
        }


        public void CmdSpecialCalibrate(uint result)
        {
            this.WriteUInt32(FT800Defs.CMD_CALIBRATE);
            this.WriteUInt32(result);
        }


        public void CmdSpecialLogo()
        {
            this.WriteUInt32(FT800Defs.CMD_LOGO);
        }

    }
}
