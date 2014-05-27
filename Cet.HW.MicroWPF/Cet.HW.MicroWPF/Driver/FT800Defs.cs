using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
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
    /*APIs for Host Commands*/
    public enum GpuHostCommands
    {
        FT_GPU_INTERNAL_OSC = 0x48, //default
        FT_GPU_EXTERNAL_OSC = 0x44,

        FT_GPU_PLL_48M = 0x62,  //default
        FT_GPU_PLL_36M = 0x61,
        FT_GPU_PLL_24M = 0x64,

        FT_GPU_ACTIVE_M = 0x00,
        FT_GPU_STANDBY_M = 0x41,//default
        FT_GPU_SLEEP_M = 0x42,
        FT_GPU_POWERDOWN_M = 0x50,

        FT_GPU_CORE_RESET = 0x68,
    }


    /* FT800 font table structure */
    /* Font table address in ROM can be found by reading the address from 0xFFFFC location. */
    /* 16 font tables are present at the address read from location 0xFFFFC */
    public struct FT_Gpu_Fonts_t
    {
        /* All the values are in bytes */
        /* Width of each character font from 0 to 127 */
        public byte[] FontWidth; // [FT_GPU_NUMCHAR_PERFONT];
        /* Bitmap format of font wrt bitmap formats supported by FT800 - L1, L4, L8 */
        public uint FontBitmapFormat;
        /* Font line stride in FT800 ROM */
        public uint FontLineStride;
        /* Font width in pixels */
        public uint FontWidthInPixels;
        /* Font height in pixels */
        public uint FontHeightInPixels;
        /* Pointer to font graphics raw data */
        public uint PointerToFontGraphicsData;
    }


    public enum GpuFunc
    {
        NEVER = 0,
        LESS = 1,
        LEQUAL = 2,
        GREATER = 3,
        GEQUAL = 4,
        EQUAL = 5,
        NOTEQUAL = 6,
        ALWAYS = 7
    }


    public enum GpuBlendFunc
    {
        ZERO = 0,
        ONE = 1,
        SRC_ALPHA = 2,
        DST_ALPHA = 3,
        ONE_MINUS_SRC_ALPHA = 4,
        ONE_MINUS_DST_ALPHA = 5
    }


    public enum GpuPrimitive
    {
        BITMAPS = 1,        //Bitmap drawing primitive
        POINTS = 2,         //Point drawing primitive
        LINES = 3,          //Line drawing primitive
        LINE_STRIP = 4,     //Line strip drawing primitive
        EDGE_STRIP_R = 5,   //Edge strip right side drawing primitive
        EDGE_STRIP_L = 6,   //Edge strip left side drawing primitive
        EDGE_STRIP_A = 7,   //Edge strip above drawing primitive
        EDGE_STRIP_B = 8,   //Edge strip below side drawing primitive
        RECTS = 9           //Rectangle drawing primitive    
    }


    public enum GpuBitmapLayout
    {
        ARGB1555 = 0,
        L1 = 1,
        L4 = 2,
        L8 = 3,
        RGB332 = 4,
        ARGB2 = 5,
        ARGB4 = 6,
        RGB565 = 7,
        PALETTED = 8,
        TEXT8X8 = 9,
        TEXTVGA = 10,
        BARGRAPH = 11
    }


    public enum GpuBitmapFiltering
    {
        Nearest = 0,
        Bilinear = 1,
    }


    public enum GpuBitmapWrap
    {
        Border = 0,
        Repeat = 1,
    }


    public enum GpuStencilOp
    {
        ZERO = 0,
        KEEP = 1,
        REPLACE = 2,
        INCR = 3,
        DECR = 4,
        INVERT = 5
    }


    public class FT800Defs
    {

        /* Definitions used for FT800 co processor command buffer */
        public const int FT_DL_SIZE = (8 * 1024);  //8KB Display List buffer size
        public const int FT_CMD_FIFO_SIZE = (4 * 1024);  //4KB coprocessor Fifo size
        public const int FT_CMD_SIZE = (4);       //4 byte per coprocessor command of EVE

        public const string FT800_VERSION = "1.9.0";
        public const int ADC_DIFFERENTIAL = 1;
        public const int ADC_SINGLE_ENDED = 0;
        public const int ADPCM_SAMPLES = 2;
        //public const int ALWAYS = 7;
        //public const int ARGB1555 = 0;
        //public const int ARGB2 = 5;
        //public const int ARGB4 = 6;
        //public const int BARGRAPH = 11;
        //public const int BILINEAR = 1;
        //public const int BITMAPS = 1;
        //public const int BORDER = 0;

        public const int CMDBUF_SIZE = 0x00001000;

        public const uint CMD_APPEND = 0xFFFFFF1Eu;
        public const uint CMD_BGCOLOR = 0xFFFFFF09u;
        public const uint CMD_BITMAP_TRANSFORM = 0xFFFFFF21u;
        public const uint CMD_BUTTON = 0xFFFFFF0Du;
        public const uint CMD_CALIBRATE = 0xFFFFFF15u;
        public const uint CMD_CLOCK = 0xFFFFFF14u;
        public const uint CMD_COLDSTART = 0xFFFFFF32u;
        public const uint CMD_CRC = 0xFFFFFF03u;
        public const uint CMD_DIAL = 0xFFFFFF2Du;
        public const uint CMD_DLSTART = 0xFFFFFF00u;
        public const uint CMD_EXECUTE = 0xFFFFFF07u;
        public const uint CMD_FGCOLOR = 0xFFFFFF0Au;
        public const uint CMD_GAUGE = 0xFFFFFF13u;
        public const uint CMD_GETMATRIX = 0xFFFFFF33u;
        public const uint CMD_GETPOINT = 0xFFFFFF08u;
        public const uint CMD_GETPROPS = 0xFFFFFF25u;
        public const uint CMD_GETPTR = 0xFFFFFF23u;
        public const uint CMD_GRADCOLOR = 0xFFFFFF34u;
        public const uint CMD_GRADIENT = 0xFFFFFF0Bu;
        public const uint CMD_HAMMERAUX = 0xFFFFFF04u;
        public const uint CMD_IDCT = 0xFFFFFF06u;
        public const uint CMD_INFLATE = 0xFFFFFF22u;
        public const uint CMD_INTERRUPT = 0xFFFFFF02u;
        public const uint CMD_KEYS = 0xFFFFFF0Eu;
        public const uint CMD_LOADIDENTITY = 0xFFFFFF26u;
        public const uint CMD_LOADIMAGE = 0xFFFFFF24u;
        public const uint CMD_LOGO = 0xFFFFFF31u;
        public const uint CMD_MARCH = 0xFFFFFF05u;
        public const uint CMD_MEMCPY = 0xFFFFFF1Du;
        public const uint CMD_MEMCRC = 0xFFFFFF18u;
        public const uint CMD_MEMSET = 0xFFFFFF1Bu;
        public const uint CMD_MEMWRITE = 0xFFFFFF1Au;
        public const uint CMD_MEMZERO = 0xFFFFFF1Cu;
        public const uint CMD_NUMBER = 0xFFFFFF2Eu;
        public const uint CMD_PROGRESS = 0xFFFFFF0Fu;
        public const uint CMD_REGREAD = 0xFFFFFF19u;
        public const uint CMD_ROTATE = 0xFFFFFF29u;
        public const uint CMD_SCALE = 0xFFFFFF28u;
        public const uint CMD_SCREENSAVER = 0xFFFFFF2Fu;
        public const uint CMD_SCROLLBAR = 0xFFFFFF11u;
        public const uint CMD_SETFONT = 0xFFFFFF2Bu;
        public const uint CMD_SETMATRIX = 0xFFFFFF2Au;
        public const uint CMD_SKETCH = 0xFFFFFF30u;
        public const uint CMD_SLIDER = 0xFFFFFF10u;
        public const uint CMD_SNAPSHOT = 0xFFFFFF1Fu;
        public const uint CMD_SPINNER = 0xFFFFFF16u;
        public const uint CMD_STOP = 0xFFFFFF17u;
        public const uint CMD_SWAP = 0xFFFFFF01u;
        public const uint CMD_TEXT = 0xFFFFFF0Cu;
        public const uint CMD_TOGGLE = 0xFFFFFF12u;
        public const uint CMD_TOUCH_TRANSFORM = 0xFFFFFF20u;
        public const uint CMD_TRACK = 0xFFFFFF2Cu;
        public const uint CMD_TRANSLATE = 0xFFFFFF27u;

        //public const int DECR = 4;
        public const int DECR_WRAP = 7;
        public const int DLSWAP_DONE = 0;
        public const int DLSWAP_FRAME = 2;
        public const int DLSWAP_LINE = 1;
        //public const int DST_ALPHA = 3;
        //public const int EDGE_STRIP_A = 7;
        //public const int EDGE_STRIP_B = 8;
        //public const int EDGE_STRIP_L = 6;
        //public const int EDGE_STRIP_R = 5;
        //public const int EQUAL = 5;
        //public const int GEQUAL = 4;
        //public const int GREATER = 3;
        //public const int INCR = 3;
        public const int INCR_WRAP = 6;
        public const int INT_CMDEMPTY = 32;
        public const int INT_CMDFLAG = 64;
        public const int INT_CONVCOMPLETE = 128;
        public const int INT_PLAYBACK = 16;
        public const int INT_SOUND = 8;
        public const int INT_SWAP = 1;
        public const int INT_TAG = 4;
        public const int INT_TOUCH = 2;
        //public const int INVERT = 5;

        //public const int KEEP = 1;
        //public const int L1 = 1;
        //public const int L4 = 2;
        //public const int L8 = 3;
        //public const int LEQUAL = 2;
        //public const int LESS = 1;
        public const int LINEAR_SAMPLES = 0;
        //public const int LINES = 3;
        //public const int LINE_STRIP = 4;
        //public const int NEAREST = 0;
        //public const int NEVER = 0;
        //public const int NOTEQUAL = 6;
        //public const int ONE = 1;
        //public const int ONE_MINUS_DST_ALPHA = 5;
        //public const int ONE_MINUS_SRC_ALPHA = 4;

        public const int OPT_CENTER = 0x00000600;
        public const int OPT_CENTERX = 0x00000200;
        public const int OPT_CENTERY = 0x00000400;
        public const int OPT_FLAT = 0x00000100;
        public const int OPT_MONO = 0x00000001;
        public const int OPT_NOBACK = 0x00001000;
        public const int OPT_NODL = 0x00000002;
        public const int OPT_NOHANDS = 0x0000C000;
        public const int OPT_NOHM = 0x00004000;
        public const int OPT_NOPOINTER = 0x00004000;
        public const int OPT_NOSECS = 0x00008000;
        public const int OPT_NOTICKS = 0x00002000;
        public const int OPT_RIGHTX = 0x00000800;
        public const int OPT_SIGNED = 0x00000100;
        //public const int PALETTED = 0x00000008;
        //public const int FTPOINTS = 0x00000002;
        //public const int RECTS = 0x00000009;

        public const int RAM_CMD = 0x00108000;
        public const int RAM_DL = 0x00100000;
        public const int RAM_G = 0x00000000;
        public const int RAM_PAL = 0x00102000;
        public const int RAM_REG = 0x00102400;
        public const int RAM_FONT_ADDR = 0x000FFFFC;


        public const int REG_ANALOG = 0x00102538;
        public const int REG_ANA_COMP = 0x00102570;
        public const int REG_CLOCK = 0x00102408;
        public const int REG_CMD_DL = 0x001024EC;
        public const int REG_CMD_READ = 0x001024E4;
        public const int REG_CMD_WRITE = 0x001024E8;
        public const int REG_CPURESET = 0x0010241C;
        public const int REG_CRC = 0x00102568;
        public const int REG_CSPREAD = 0x00102464;
        public const int REG_CYA0 = 0x001024D0;
        public const int REG_CYA1 = 0x001024D4;
        public const int REG_CYA_TOUCH = 0x00102534;
        public const int REG_DATESTAMP = 0x0010253C;
        public const int REG_DITHER = 0x0010245C;
        public const int REG_DLSWAP = 0x00102450;
        public const int REG_FRAMES = 0x00102404;
        public const int REG_FREQUENCY = 0x0010240C;
        public const int REG_GPIO = 0x00102490;
        public const int REG_GPIO_DIR = 0x0010248C;
        public const int REG_HCYCLE = 0x00102428;
        public const int REG_HOFFSET = 0x0010242C;
        public const int REG_HSIZE = 0x00102430;
        public const int REG_HSYNC0 = 0x00102434;
        public const int REG_HSYNC1 = 0x00102438;
        public const int REG_ID = 0x00102400;
        public const int REG_INT_EN = 0x0010249C;
        public const int REG_INT_FLAGS = 0x00102498;
        public const int REG_INT_MASK = 0x001024A0;
        public const int REG_MACRO_0 = 0x001024C8;
        public const int REG_MACRO_1 = 0x001024CC;
        public const int REG_OUTBITS = 0x00102458;
        public const int REG_PCLK = 0x0010246C;
        public const int REG_PCLK_POL = 0x00102468;
        public const int REG_PLAY = 0x00102488;
        public const int REG_PLAYBACK_FORMAT = 0x001024B4;
        public const int REG_PLAYBACK_FREQ = 0x001024B0;
        public const int REG_PLAYBACK_LENGTH = 0x001024A8;
        public const int REG_PLAYBACK_LOOP = 0x001024B8;
        public const int REG_PLAYBACK_PLAY = 0x001024BC;
        public const int REG_PLAYBACK_READPTR = 0x001024AC;
        public const int REG_PLAYBACK_START = 0x001024A4;
        public const int REG_PWM_DUTY = 0x001024C4;
        public const int REG_PWM_HZ = 0x001024C0;
        public const int REG_RENDERMODE = 0x00102410;
        public const int REG_ROMSUB_SEL = 0x001024E0;
        public const int REG_ROTATE = 0x00102454;
        public const int REG_SNAPSHOT = 0x00102418;
        public const int REG_SNAPY = 0x00102414;
        public const int REG_SOUND = 0x00102484;
        public const int REG_SWIZZLE = 0x00102460;
        public const int REG_TAG = 0x00102478;
        public const int REG_TAG_X = 0x00102470;
        public const int REG_TAG_Y = 0x00102474;
        public const int REG_TAP_CRC = 0x00102420;
        public const int REG_TAP_MASK = 0x00102424;
        public const int REG_TOUCH_ADC_MODE = 0x001024F4;
        public const int REG_TOUCH_CHARGE = 0x001024F8;
        public const int REG_TOUCH_DIRECT_XY = 0x00102574;
        public const int REG_TOUCH_DIRECT_Z1Z2 = 0x00102578;
        public const int REG_TOUCH_MODE = 0x001024F0;
        public const int REG_TOUCH_OVERSAMPLE = 0x00102500;
        public const int REG_TOUCH_RAW_XY = 0x00102508;
        public const int REG_TOUCH_RZ = 0x0010250C;
        public const int REG_TOUCH_RZTHRESH = 0x00102504;
        public const int REG_TOUCH_SCREEN_XY = 0x00102510;
        public const int REG_TOUCH_SETTLE = 0x001024FC;
        public const int REG_TOUCH_TAG = 0x00102518;
        public const int REG_TOUCH_TAG_XY = 0x00102514;
        public const int REG_TOUCH_TRANSFORM_A = 0x0010251C;
        public const int REG_TOUCH_TRANSFORM_B = 0x00102520;
        public const int REG_TOUCH_TRANSFORM_C = 0x00102524;
        public const int REG_TOUCH_TRANSFORM_D = 0x00102528;
        public const int REG_TOUCH_TRANSFORM_E = 0x0010252C;
        public const int REG_TOUCH_TRANSFORM_F = 0x00102530;
        public const int REG_TRACKER = 0x00109000;
        public const int REG_TRIM = 0x0010256C;
        public const int REG_VCYCLE = 0x0010243C;
        public const int REG_VOFFSET = 0x00102440;
        public const int REG_VOL_PB = 0x0010247C;
        public const int REG_VOL_SOUND = 0x00102480;
        public const int REG_VSIZE = 0x00102444;
        public const int REG_VSYNC0 = 0x00102448;
        public const int REG_VSYNC1 = 0x0010244C;

        //public const int REPEAT = 1;
        //public const int REPLACE = 2;
        //public const int RGB332 = 4;
        //public const int RGB565 = 7;
        //public const int SRC_ALPHA = 2;
        //public const int TEXT8X8 = 9;
        //public const int TEXTVGA = 10;
        public const int TOUCHMODE_CONTINUOUS = 3;
        public const int TOUCHMODE_FRAME = 2;
        public const int TOUCHMODE_OFF = 0;
        public const int TOUCHMODE_ONESHOT = 1;
        public const int ULAW_SAMPLES = 1;
        //public const int ZERO = 0;


        //#define VERTEX2F(x,y) ((1UL<<30)|(((x)&32767UL)<<15)|(((y)&32767UL)<<0))
        //#define VERTEX2II(x,y,handle,cell) ((2UL<<30)|(((x)&511UL)<<21)|(((y)&511UL)<<12)|(((handle)&31UL)<<7)|(((cell)&127UL)<<0))
        //#define BITMAP_SOURCE(addr) ((1UL<<24)|(((addr)&1048575UL)<<0))
        //#define CLEAR_COLOR_RGB(red,green,blue) ((2UL<<24)|(((red)&255UL)<<16)|(((green)&255UL)<<8)|(((blue)&255UL)<<0))
        //#define TAG(s) ((3UL<<24)|(((s)&255UL)<<0))
        //#define COLOR_RGB(red,green,blue) ((4UL<<24)|(((red)&255UL)<<16)|(((green)&255UL)<<8)|(((blue)&255UL)<<0))
        //#define BITMAP_HANDLE(handle) ((5UL<<24)|(((handle)&31UL)<<0))
        //#define CELL(cell) ((6UL<<24)|(((cell)&127UL)<<0))
        //#define BITMAP_LAYOUT(format,linestride,height) ((7UL<<24)|(((format)&31UL)<<19)|(((linestride)&1023UL)<<9)|(((height)&511UL)<<0))
        //#define BITMAP_SIZE(filter,wrapx,wrapy,width,height) ((8UL<<24)|(((filter)&1UL)<<20)|(((wrapx)&1UL)<<19)|(((wrapy)&1UL)<<18)|(((width)&511UL)<<9)|(((height)&511UL)<<0))
        //#define ALPHA_FUNC(func,ref) ((9UL<<24)|(((func)&7UL)<<8)|(((ref)&255UL)<<0))
        //#define STENCIL_FUNC(func,ref,mask) ((10UL<<24)|(((func)&7UL)<<16)|(((ref)&255UL)<<8)|(((mask)&255UL)<<0))
        //#define BLEND_FUNC(src,dst) ((11UL<<24)|(((src)&7UL)<<3)|(((dst)&7UL)<<0))
        //#define STENCIL_OP(sfail,spass) ((12UL<<24)|(((sfail)&7UL)<<3)|(((spass)&7UL)<<0))
        //#define POINT_SIZE(size) ((13UL<<24)|(((size)&8191UL)<<0))
        //#define LINE_WIDTH(width) ((14UL<<24)|(((width)&4095UL)<<0))
        //#define CLEAR_COLOR_A(alpha) ((15UL<<24)|(((alpha)&255UL)<<0))
        //#define COLOR_A(alpha) ((16UL<<24)|(((alpha)&255UL)<<0))
        //#define CLEAR_STENCIL(s) ((17UL<<24)|(((s)&255UL)<<0))
        //#define CLEAR_TAG(s) ((18UL<<24)|(((s)&255UL)<<0))
        //#define STENCIL_MASK(mask) ((19UL<<24)|(((mask)&255UL)<<0))
        //#define TAG_MASK(mask) ((20UL<<24)|(((mask)&1UL)<<0))
        //#define BITMAP_TRANSFORM_A(a) ((21UL<<24)|(((a)&131071UL)<<0))
        //#define BITMAP_TRANSFORM_B(b) ((22UL<<24)|(((b)&131071UL)<<0))
        //#define BITMAP_TRANSFORM_C(c) ((23UL<<24)|(((c)&16777215UL)<<0))
        //#define BITMAP_TRANSFORM_D(d) ((24UL<<24)|(((d)&131071UL)<<0))
        //#define BITMAP_TRANSFORM_E(e) ((25UL<<24)|(((e)&131071UL)<<0))
        //#define BITMAP_TRANSFORM_F(f) ((26UL<<24)|(((f)&16777215UL)<<0))
        //#define SCISSOR_XY(x,y) ((27UL<<24)|(((x)&511UL)<<9)|(((y)&511UL)<<0))
        //#define SCISSOR_SIZE(width,height) ((28UL<<24)|(((width)&1023UL)<<10)|(((height)&1023UL)<<0))
        //#define CALL(dest) ((29UL<<24)|(((dest)&65535UL)<<0))
        //#define JUMP(dest) ((30UL<<24)|(((dest)&65535UL)<<0))
        //#define BEGIN(prim) ((31UL<<24)|(((prim)&15UL)<<0))
        //#define COLOR_MASK(r,g,b,a) ((32UL<<24)|(((r)&1UL)<<3)|(((g)&1UL)<<2)|(((b)&1UL)<<1)|(((a)&1UL)<<0))
        //#define CLEAR(c,s,t) ((38UL<<24)|(((c)&1UL)<<2)|(((s)&1UL)<<1)|(((t)&1UL)<<0))
        //#define END() ((33UL<<24))
        //#define SAVE_CONTEXT() ((34UL<<24))
        //#define RESTORE_CONTEXT() ((35UL<<24))
        //#define RETURN() ((36UL<<24))
        //#define MACRO(m) ((37UL<<24)|(((m)&1UL)<<0))
        //#define DISPLAY() ((0UL<<24))

        public const int FT_GPU_NUMCHAR_PERFONT = (128);
        public const int FT_GPU_FONT_TABLE_SIZE = (148);

        public const float PixelQuant = 16.0f;
    }
}
