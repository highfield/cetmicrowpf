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

        private const int FontSets = 16;

        private static byte[][] _tblfonts = new byte[FontSets][];


        private void RetrieveFontData()
        {
            const int TableSize = 148;

            uint base_address = this.ReadUInt32Immediate(FT800Defs.RAM_FONT_ADDR);

            for (int i = 0; i < 16; i++)
            {
                uint address = base_address + (uint)(TableSize * i);
                this._outgoingBuffer[0] = (byte)(address >> 16);
                this._outgoingBuffer[1] = (byte)(address >> 8);
                this._outgoingBuffer[2] = (byte)address;

                byte[] fdata;
                _tblfonts[i] = fdata = new byte[TableSize];

                this._port.WriteRead(
                    this._outgoingBuffer,
                    0,
                    3,
                    fdata,
                    0,
                    TableSize,
                    4);
            }
        }


        public static Size MeasureText(
            string text,
            int font
            )
        {
            var size = new Size();

            var fidx = font - 16;
            byte[] fdata = _tblfonts[fidx];
            var fwidth = fdata[136];

            for (int i = 0, count = (text ?? string.Empty).Length; i < count; i++)
            {
                int code;
                size.Width += ((code = (int)text[i]) < 128)
                    ? fdata[code]
                    : fwidth;
            }

            size.Height = fdata[140];
            //Debug.Print("text=" + text + "; size=" + size);
            return size;
        }


        public void CmdDisplayText(
            float x,
            float y,
            int font,
            int options,
            string s
            )
        {
            this.WriteUInt32(FT800Defs.CMD_TEXT);
            this.WriteInt32(x, y);
            this.WriteInt32(font, options);
            this.WriteString(s);
        }


        public void CmdDisplayNumber(
            float x,
            float y,
            int font,
            int options,
            int n
            )
        {
            this.WriteUInt32(FT800Defs.CMD_NUMBER);
            this.WriteInt32(x, y);
            this.WriteInt32(font, options);
            this.WriteInt32(n);
        }

    }
}
