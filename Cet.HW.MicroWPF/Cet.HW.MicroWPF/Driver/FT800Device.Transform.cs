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

        public void CmdRotate(
            float angle
            )
        {
            const float k = 0x10000 / 360.0f;
            this.WriteUInt32(FT800Defs.CMD_ROTATE);
            this.WriteInt32((int)(angle * k));
        }


        public void CmdLoadIdentity()
        {
            this.WriteUInt32(FT800Defs.CMD_LOADIDENTITY);
        }


        public void CmdTranslate(
            float tx,
            float ty
            )
        {
            this.WriteUInt32(FT800Defs.CMD_TRANSLATE);
            this.WriteInt32((int)(tx * 0x10000));
            this.WriteInt32((int)(ty * 0x10000));
        }


        //???
        public void CmdTouchTransform(
            int x0,
            int y0,
            int x1,
            int y1,
            int x2,
            int y2,
            int tx0,
            int ty0,
            int tx1,
            int ty1,
            int tx2,
            int ty2,
            ushort result
            )
        {
            this.WriteUInt32(FT800Defs.CMD_TOUCH_TRANSFORM);
            this.WriteInt32(x0);
            this.WriteInt32(y0);
            this.WriteInt32(x1);
            this.WriteInt32(y1);
            this.WriteInt32(x2);
            this.WriteInt32(y2);
            this.WriteInt32(tx0);
            this.WriteInt32(ty0);
            this.WriteInt32(tx1);
            this.WriteInt32(ty1);
            this.WriteInt32(tx2);
            this.WriteInt32(ty2);
            this.WriteUInt32(result);
        }


        //???
        public void CmdGetMatrix(
            int a,
            int b,
            int c,
            int d,
            int e,
            int f
            )
        {
            this.WriteUInt32(FT800Defs.CMD_GETMATRIX);
            this.WriteInt32(a);
            this.WriteInt32(b);
            this.WriteInt32(c);
            this.WriteInt32(d);
            this.WriteInt32(e);
            this.WriteInt32(f);
        }


        //???
        public void CmdBitmapTransform(
            int x0,
            int y0,
            int x1,
            int y1,
            int x2,
            int y2,
            int tx0,
            int ty0,
            int tx1,
            int ty1,
            int tx2,
            int ty2,
            ushort result
            )
        {
            this.WriteUInt32(FT800Defs.CMD_BITMAP_TRANSFORM);
            this.WriteInt32(x0);
            this.WriteInt32(y0);
            this.WriteInt32(x1);
            this.WriteInt32(y1);
            this.WriteInt32(x2);
            this.WriteInt32(y2);
            this.WriteInt32(tx0);
            this.WriteInt32(ty0);
            this.WriteInt32(tx1);
            this.WriteInt32(ty1);
            this.WriteInt32(tx2);
            this.WriteInt32(ty2);
            this.WriteUInt32(result);
        }


        public void CmdScale(
            float sx,
            float sy
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SCALE);
            this.WriteInt32((int)(sx * 0x10000));
            this.WriteInt32((int)(sy * 0x10000));
        }


        public void CmdSetMatrix()
        {
            this.WriteUInt32(FT800Defs.CMD_SETMATRIX);
        }


        public void CmdBitmapTransformA(
            float a
            )
        {
            this.WriteInt32(
                0x15000000 |
                ((int)(a * 0x0100) & 0xFFFF)
                );
        }


        public void CmdBitmapTransformB(
            float b
            )
        {
            this.WriteInt32(
                0x16000000 |
                ((int)(b * 0x0100) & 0xFFFF)
                );
        }


        public void CmdBitmapTransformC(
            float c
            )
        {
            this.WriteInt32(
                0x17000000 |
                ((int)(c * 0x0100) & 0x00FFFFFF)
                );
        }


        public void CmdBitmapTransformD(
            float d
            )
        {
            this.WriteInt32(
                0x18000000 |
                ((int)(d * 0x0100) & 0xFFFF)
                );
        }


        public void CmdBitmapTransformE(
            float e
            )
        {
            this.WriteInt32(
                0x19000000 |
                ((int)(e * 0x0100) & 0xFFFF)
                );
        }


        public void CmdBitmapTransformF(
            float f
            )
        {
            this.WriteInt32(
                0x1A000000 |
                ((int)(f * 0x0100) & 0x00FFFFFF)
                );
        }

    }
}
