using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCsharp
{
    public interface IChannelImage
    {
        void SetPixel(int x, int y, byte value);
        byte GetPixel(int x, int y);
        int Height
        {
            get;
        }

        int Width
        {
            get;
        }
        /// <summary>
        /// creates IChannelImage with same width and height
        /// </summary>
        /// <returns></returns>
        IChannelImage CreateBlankCopy();
    }
}
