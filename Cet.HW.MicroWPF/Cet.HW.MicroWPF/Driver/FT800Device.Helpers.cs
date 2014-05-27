using System;
using Microsoft.SPOT;
using System.Threading;

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

        public void WriteCommandImmediate(
            GpuHostCommands command
            )
        {
            this._outgoingBuffer[0] = (byte)command;
            this._outgoingBuffer[1] = 0;
            this._outgoingBuffer[2] = 0;

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3,
                this._incomingBuffer,
                0,
                0,
                0);

            this._outgoingLength = 0;
        }


        public void WriteByteImmediate(
            uint address,
            int value
            )
        {
            address |= 0x800000;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;
            this._outgoingBuffer[3] = (byte)value;

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3 + 1,
                this._incomingBuffer,
                0,
                0,
                0);

            this._outgoingLength = 0;
        }


        public void WriteUInt16Immediate(
            uint address,
            int value
            )
        {
            address |= 0x800000;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;

            this._outgoingBuffer[3] = (byte)value;
            this._outgoingBuffer[4] = (byte)(value >> 8);

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3 + 2,
                this._incomingBuffer,
                0,
                0,
                0);

            this._outgoingLength = 0;
        }


        public void WriteUInt32Immediate(
            uint address,
            uint value
            )
        {
            address |= 0x800000;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;

            this._outgoingBuffer[3] = (byte)value;
            this._outgoingBuffer[4] = (byte)(value >> 8);
            this._outgoingBuffer[5] = (byte)(value >> 16);
            this._outgoingBuffer[6] = (byte)(value >> 24);

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3 + 4,
                this._incomingBuffer,
                0,
                0,
                0);

            this._outgoingLength = 0;
        }


        public byte ReadByteImmediate(
            uint address
            )
        {
            address &= ~(uint)0xC00000;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;
            this._outgoingBuffer[3] = 0;

            this._incomingLength = 1 + 1;

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3 + this._incomingLength,
                this._incomingBuffer,
                0,
                this._incomingLength,
                3);

            this._outgoingLength = 0;

            return this._incomingBuffer[1];
        }


        public ushort ReadUInt16Immediate(
            uint address
            )
        {
            address &= ~(uint)0xC00000;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;
            this._outgoingBuffer[3] = 0;

            this._incomingLength = 1 + 2;

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3 + this._incomingLength,
                this._incomingBuffer,
                0,
                this._incomingLength,
                3);

            this._outgoingLength = 0;

            return (ushort)(
                this._incomingBuffer[1] |
                ((uint)this._incomingBuffer[2] << 8)
                );
        }


        public uint ReadUInt32Immediate(
            uint address
            )
        {
            address &= ~(uint)0xC00000;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;
            this._outgoingBuffer[3] = 0;

            this._incomingLength = 1 + 4;

            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                3 + this._incomingLength,
                this._incomingBuffer,
                0,
                this._incomingLength,
                3);

            this._outgoingLength = 0;

            return
                this._incomingBuffer[1] |
                ((uint)this._incomingBuffer[2] << 8) |
                ((uint)this._incomingBuffer[3] << 16) |
                ((uint)this._incomingBuffer[4] << 24);
        }


        /// <summary>
        /// Add four <see cref="System.Byte"/> to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteBytes(
            int value0,
            int value1,
            int value2,
            int value3
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)value0;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(value1);
            this._outgoingBuffer[this._outgoingLength + 2] = (byte)(value2);
            this._outgoingBuffer[this._outgoingLength + 3] = (byte)(value3);
            this._outgoingLength += 4;
        }


        /// <summary>
        /// Add an <see cref="System.Int16"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt16(
            int value
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)value;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(value >> 8);
            this._outgoingLength += 2;
        }


        /// <summary>
        /// Add an <see cref="System.UInt16"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt16(
            int value
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)value;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(value >> 8);
            this._outgoingLength += 2;
        }


        /// <summary>
        /// Add an <see cref="System.Int32"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt32(
            int value
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)value;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(value >> 8);
            this._outgoingBuffer[this._outgoingLength + 2] = (byte)(value >> 16);
            this._outgoingBuffer[this._outgoingLength + 3] = (byte)(value >> 24);
            this._outgoingLength += 4;
        }


        /// <summary>
        /// Add an <see cref="System.UInt32"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt32(
            int low,
            int high
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)low;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(low >> 8);
            this._outgoingBuffer[this._outgoingLength + 2] = (byte)high;
            this._outgoingBuffer[this._outgoingLength + 3] = (byte)(high >> 8);
            this._outgoingLength += 4;
        }


        /// <summary>
        /// Add an <see cref="System.UInt32"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt32(
            float low,
            float high
            )
        {
            this.WriteInt32((int)low, (int)high);
        }


        /// <summary>
        /// Add an <see cref="System.UInt32"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt32(
            uint value
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)value;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(value >> 8);
            this._outgoingBuffer[this._outgoingLength + 2] = (byte)(value >> 16);
            this._outgoingBuffer[this._outgoingLength + 3] = (byte)(value >> 24);
            this._outgoingLength += 4;
        }


        /// <summary>
        /// Add an <see cref="System.UInt32"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt32(
            int low,
            int high
            )
        {
            this._outgoingBuffer[this._outgoingLength] = (byte)low;
            this._outgoingBuffer[this._outgoingLength + 1] = (byte)(low >> 8);
            this._outgoingBuffer[this._outgoingLength + 2] = (byte)high;
            this._outgoingBuffer[this._outgoingLength + 3] = (byte)(high >> 8);
            this._outgoingLength += 4;
        }


        public void WriteString(
            string s
            )
        {
            int length = s != null
                ? s.Length
                : 0;

            for (int i = 0; i < length; i++)
            {
                this._outgoingBuffer[this._outgoingLength + i] = (byte)s[i];
            }

            this._outgoingBuffer[this._outgoingLength + length] = 0;
            this._outgoingLength += (length + 4) & ~3;
        }


        public bool WaitCommandBufferIdle(
            bool blocking
            )
        {
            bool complete;
            ushort wr = this.ReadUInt16Immediate(FT800Defs.REG_CMD_WRITE);
            while (
                (complete = (this.ReadUInt16Immediate(FT800Defs.REG_CMD_READ) == wr)) == false &&
                blocking)
            {
                Thread.Sleep(1);
            }

            return complete;
        }


        public void DrawRect(
            Rect rect,
            uint backcolor,
            uint strokecolor,
            Thickness thickness)
        {
            var x0 = rect.X;
            var y0 = rect.Y;
            var x1 = rect.X + rect.Width;
            var y1 = rect.Y + rect.Height;

            bool restore_color = false;

            if (backcolor != Colors.None)
            {
                this.CmdColorRGB(backcolor);
                this.CmdProcessorBegin(GpuPrimitive.RECTS);
                this.CmdVertex2F(x0, y0);
                this.CmdVertex2F(x1, y1);
                this.CmdProcessorEnd();
                restore_color = true;
            }

            if (strokecolor != Colors.None)
            {
                this.CmdColorRGB(strokecolor);
                this.CmdProcessorBegin(GpuPrimitive.LINES);

                if (thickness.Left > 0)
                {
                    //left edge
                    this.CmdLineWidth(thickness.Left);
                    this.CmdVertex2F(x0, y0);
                    this.CmdVertex2F(x0, y1);
                }

                if (thickness.Top > 0)
                {
                    //top edge
                    this.CmdLineWidth(thickness.Top);
                    this.CmdVertex2F(x0, y0);
                    this.CmdVertex2F(x1, y0);
                }

                if (thickness.Right > 0)
                {
                    //right edge
                    this.CmdLineWidth(thickness.Right);
                    this.CmdVertex2F(x1, y0);
                    this.CmdVertex2F(x1, y1);
                }

                if (thickness.Bottom > 0)
                {
                    //bottom edge
                    this.CmdLineWidth(thickness.Bottom);
                    this.CmdVertex2F(x0, y1);
                    this.CmdVertex2F(x1, y1);
                }

                this.CmdLineWidth();
                this.CmdProcessorEnd();
                restore_color = true;
            }

            if (restore_color)
                this.CmdColorRGB();
        }


        public void Init()
        {
            /* Default is WQVGA - 480x272 */
            const int FT_DispWidth = 480;
            const int FT_DispHeight = 272;
            const int FT_DispHCycle = 548;
            const int FT_DispHOffset = 43;
            const int FT_DispHSync0 = 0;
            const int FT_DispHSync1 = 41;
            const int FT_DispVCycle = 292;
            const int FT_DispVOffset = 12;
            const int FT_DispVSync0 = 0;
            const int FT_DispVSync1 = 10;
            const int FT_DispPCLK = 5;
            const int FT_DispSwizzle = 0;
            const int FT_DispPCLKPol = 1;

            //power-down cycle
            this._powerDown.Write(false);
            Thread.Sleep(20);
            this._powerDown.Write(true);
            Thread.Sleep(20);

            this.WriteCommandImmediate(GpuHostCommands.FT_GPU_ACTIVE_M);
            Thread.Sleep(20);

            this.WriteCommandImmediate(GpuHostCommands.FT_GPU_EXTERNAL_OSC);
            Thread.Sleep(10);

            this.WriteCommandImmediate(GpuHostCommands.FT_GPU_PLL_48M);
            Thread.Sleep(10);

            this.WriteCommandImmediate(GpuHostCommands.FT_GPU_CORE_RESET);

            byte chip_id;
            while ((chip_id = this.ReadByteImmediate(FT800Defs.REG_ID)) != 0x7C)
                Thread.Sleep(1);

            this.WriteByteImmediate(FT800Defs.REG_PCLK, 0);
            this.WriteByteImmediate(FT800Defs.REG_PWM_DUTY, 0);

            //setup display
            this.WriteUInt16Immediate(FT800Defs.REG_HSIZE, FT_DispWidth);
            this.WriteUInt16Immediate(FT800Defs.REG_VSIZE, FT_DispHeight);
            this.WriteUInt16Immediate(FT800Defs.REG_HCYCLE, FT_DispHCycle);
            this.WriteUInt16Immediate(FT800Defs.REG_HOFFSET, FT_DispHOffset);
            this.WriteUInt16Immediate(FT800Defs.REG_HSYNC0, FT_DispHSync0);
            this.WriteUInt16Immediate(FT800Defs.REG_HSYNC1, FT_DispHSync1);
            this.WriteUInt16Immediate(FT800Defs.REG_VCYCLE, FT_DispVCycle);
            this.WriteUInt16Immediate(FT800Defs.REG_VOFFSET, FT_DispVOffset);
            this.WriteUInt16Immediate(FT800Defs.REG_VSYNC0, FT_DispVSync0);
            this.WriteUInt16Immediate(FT800Defs.REG_VSYNC1, FT_DispVSync1);
            this.WriteByteImmediate(FT800Defs.REG_SWIZZLE, FT_DispSwizzle);
            this.WriteByteImmediate(FT800Defs.REG_PCLK_POL, FT_DispPCLKPol);

            this.BeginDisplayTransaction();
            this.CmdClearColorRGB(0, 0, 0);
            this.CmdClear(1, 1, 1);
            this.CmdProcessorDisplay();
            this.EndTransaction();

            this.WriteUInt32Immediate(FT800Defs.REG_DLSWAP, FT800Defs.DLSWAP_FRAME);

            this.WriteByteImmediate(
                FT800Defs.REG_GPIO_DIR,
                (byte)(0x80 | this.ReadByteImmediate(FT800Defs.REG_GPIO_DIR))
                );

            this.WriteByteImmediate(
                FT800Defs.REG_GPIO,
                (byte)(0x80 | this.ReadByteImmediate(FT800Defs.REG_GPIO))
                );

            //after this display is visible on the LCD
            this.WriteByteImmediate(FT800Defs.REG_PCLK, FT_DispPCLK);
            this.WriteByteImmediate(FT800Defs.REG_PWM_DUTY, 32);    //max 128

            /**
             * Touch configuration - configure the resistance value to 1200
             * this value is specific to customer requirement and derived by experiment
             **/
            this.WriteUInt16Immediate(FT800Defs.REG_TOUCH_RZTHRESH, 1200);

            Thread.Sleep(20);

            //read embedded font sets data
            this.RetrieveFontData();

            this.ScreenWidth = FT_DispWidth;
            this.ScreenHeight = FT_DispHeight;
        }

    }
}
