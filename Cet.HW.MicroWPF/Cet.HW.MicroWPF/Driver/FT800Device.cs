using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

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

        private const int OutgoingBufferSize = 0x1000;
        private const int IncomingBufferSize = 0x0020;


        public FT800Device(
            SPI port,
            OutputPort powerDown
            )
        {
            this._outgoingBuffer = new byte[OutgoingBufferSize];
            this._incomingBuffer = new byte[IncomingBufferSize];
            this._port = port;
            this._powerDown = powerDown;
        }


        private readonly byte[] _outgoingBuffer;
        private int _outgoingLength;

        private readonly byte[] _incomingBuffer;
        private int _incomingLength;

        private SPI _port;
        private OutputPort _powerDown;
        private uint _addressOffset;
        private bool _isCommandTransaction;

        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }


        #region Transaction

        public void BeginDisplayTransaction()
        {
            uint address = 0x800000 | FT800Defs.RAM_DL;
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;
            this._outgoingLength = 3;
            this._isCommandTransaction = false;
        }


        public void BeginCommandTransaction()
        {
            uint address = 0x800000 | (FT800Defs.RAM_CMD + this._addressOffset);
            this._outgoingBuffer[0] = (byte)(address >> 16);
            this._outgoingBuffer[1] = (byte)(address >> 8);
            this._outgoingBuffer[2] = (byte)address;
            this._outgoingLength = 3;
            this._isCommandTransaction = true;
        }


        public void EndTransaction()
        {
            this._port.WriteRead(
                this._outgoingBuffer,
                0,
                this._outgoingLength,
                this._incomingBuffer,
                0,
                0,
                0);

            if (this._isCommandTransaction)
            {
                //this._addressOffset = (uint)(this._addressOffset + this._outgoingLength - 3) & 0x0FFF;
                //this._addressOffset = (uint)(this._addressOffset + 3) & 0x0FFC;
                this._addressOffset = (uint)(this._addressOffset + this._outgoingLength) & 0x0FFC;
                this.WriteUInt16Immediate(FT800Defs.REG_CMD_WRITE, (ushort)this._addressOffset);
            }
            else
            {
                this.WriteUInt32Immediate(FT800Defs.REG_DLSWAP, FT800Defs.DLSWAP_FRAME);
            }

            this._outgoingLength = 0;
        }

        #endregion


        //???
        public void CmdGetProps(
            uint ptr,
            uint w,
            uint h
            )
        {
            this.WriteUInt32(FT800Defs.CMD_GETPROPS);
            this.WriteUInt32(ptr);
            this.WriteUInt32(w);
            this.WriteUInt32(h);
        }


        public void CmdBgColor(
            uint c
            )
        {
            this.WriteUInt32(FT800Defs.CMD_BGCOLOR);
            this.WriteUInt32(c & 0x00FFFFFF);
        }


        public void CmdInterrupt(
            int ms
            )
        {
            this.WriteUInt32(FT800Defs.CMD_INTERRUPT);
            this.WriteInt32(ms);
        }


        public void CmdFgColor(
            uint c
            )
        {
            this.WriteUInt32(FT800Defs.CMD_FGCOLOR);
            this.WriteUInt32(c & 0x00FFFFFF);
        }


        public void CmdSketch(
            float x,
            float y,
            float w,
            float h,
            uint ptr,
            int format
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SKETCH);
            this.WriteInt32(x, y);
            this.WriteInt32(w, h);
            this.WriteUInt32(ptr);
            this.WriteInt32(format);
        }


        public void CmdGradColor(
            uint c
            )
        {
            this.WriteUInt32(FT800Defs.CMD_GRADCOLOR);
            this.WriteUInt32(c & 0x00FFFFFF);
        }


        public void CmdSetFont(
            uint font,
            uint ptr
            )
        {
            this.WriteUInt32(FT800Defs.CMD_SETFONT);
            this.WriteUInt32(font);
            this.WriteUInt32(ptr);
        }


        public void CmdAppend(
            uint ptr,
            uint num
            )
        {
            this.WriteUInt32(FT800Defs.CMD_APPEND);
            this.WriteUInt32(ptr);
            this.WriteUInt32(num);
        }


        public void CmdGradient(
            float x0,
            float y0,
            uint rgb0,
            float x1,
            float y1,
            uint rgb1
            )
        {
            this.WriteUInt32(FT800Defs.CMD_GRADIENT);
            this.WriteInt32(x0, y0);
            this.WriteUInt32(rgb0 & 0x00FFFFFF);
            this.WriteInt32(x1, y1);
            this.WriteUInt32(rgb1 & 0x00FFFFFF);
        }


        public void CmdColdStart()
        {
            this.WriteUInt32(FT800Defs.CMD_COLDSTART);
        }


        public void CmdVertex2F(
            float x,
            float y
            )
        {
            this.WriteInt32(
                0x40000000 |
                (((int)(x * FT800Defs.PixelQuant) & 0x7FFF) << 15) |
                ((int)(y * FT800Defs.PixelQuant) & 0x7FFF)
                );
        }


        public void CmdVertex2II(
            float x,
            float y,
            int handle,
            int cell
            )
        {
            var temp = (uint)(
                (((int)x & 0x1FF) << 21) |
                (((int)y & 0x1FF) << 12) |
                ((handle & 0x1F) << 7) |
                (cell & 0x3F)
                );

            this.WriteUInt32(
                0x80000000u |
                temp
                );
        }


        public void CmdClearColorRGB(
            uint color
            )
        {
            this.WriteUInt32(
                0x02000000 |
                (color & 0x00FFFFFF)
                );
        }


        public void CmdClearColorRGB(
            int red,
            int green,
            int blue
            )
        {
            this.WriteInt32(
                0x02000000 |
                ((byte)red << 16) |
                ((byte)green << 8) |
                (byte)blue
                );
        }


        public void CmdColorRGB(
            uint color = Colors.White
            )
        {
            this.WriteUInt32(
                0x04000000 |
                (color & 0x00FFFFFF)
                );
        }


        public void CmdColorRGB(
            int red,
            int green,
            int blue
            )
        {
            this.WriteInt32(
                0x04000000 |
                ((byte)red << 16) |
                ((byte)green << 8) |
                (byte)blue
                );
        }


        public void CmdAlphaFunc(
            GpuFunc func,
            int @ref
            )
        {
            this.WriteInt32(
                0x09000000 |
                (((int)func & 0x07) << 8) |
                (byte)@ref
                );
        }


        public void CmdStencilFunc(
            GpuFunc func,
            int @ref,
            int mask
            )
        {
            this.WriteInt32(
                0x0A000000 |
                (((int)func & 0x0F) << 16) |
                ((byte)@ref << 8) |
                (byte)mask
                );
        }


        public void CmdBlendFunc(
            GpuBlendFunc src,
            GpuBlendFunc dest
            )
        {
            this.WriteInt32(
                0x0B000000 |
                (((int)src & 0x07) << 3) |
                ((int)dest & 0x07)
                );
        }


        public void CmdStencilOp(
            GpuStencilOp sfail,
            GpuStencilOp spass
            )
        {
            this.WriteInt32(
                0x0C000000 |
                (((int)sfail & 0x07) << 3) |
                ((int)spass & 0x07)
                );
        }


        public void CmdPointSize(
            float size
            )
        {
            this.WriteInt32(
                0x0D000000 |
                ((int)(size * FT800Defs.PixelQuant) & 0xFFFF)
                );
        }


        public void CmdLineWidth(
            float width = 1.0f
            )
        {
            this.WriteInt32(
                0x0E000000 |
                ((int)(width * FT800Defs.PixelQuant) & 0x0FFF)
                );
        }


        public void CmdClearColorA(
            int alpha
            )
        {
            this.WriteInt32(
                0x0F000000 |
                (byte)alpha
                );
        }


        public void CmdColorA(
            int alpha
            )
        {
            this.WriteInt32(
                0x10000000 |
                (byte)alpha
                );
        }


        public void CmdClearStencil(
            int s
            )
        {
            this.WriteInt32(
                0x11000000 |
                (byte)s
                );
        }


        public void CmdStencilMask(
            int mask
            )
        {
            this.WriteInt32(
                0x13000000 |
                (byte)mask
                );
        }


        public void CmdScrissorXY(
            float x,
            float y
            )
        {
            this.WriteInt32(
                0x1B000000 |
                (((int)x & 0x1FF) << 9) |
                ((int)y & 0x1FF)
                );
        }


        public void CmdScrissorSize(
            float width,
            float height
            )
        {
            this.WriteInt32(
                0x1C000000 |
                (((int)width & 0x1FF) << 9) |
                ((int)height & 0x1FF)
                );
        }


        public void CmdColorMask(
            int red,
            int green,
            int blue,
            int alpha
            )
        {
            this.WriteInt32(
                0x20000000 |
                ((red & 0x01) << 3) |
                ((green & 0x01) << 2) |
                ((blue & 0x01) << 1) |
                (alpha & 0x01)
                );
        }


        public void CmdClear(
            int c,
            int s,
            int t
            )
        {
            this.WriteInt32(
                0x26000000 |
                ((c & 0x01) << 2) |
                ((s & 0x01) << 1) |
                (t & 0x01)
                );
        }

    }
}
