using System;
using System.IO;

namespace IdSharp.Tagging.VorbisComment
{
    internal class FlacMetaDataBlock
    {
        public FlacMetaDataBlock(FlacMetaDataBlockType blockType)
        {
            if ((int)blockType > 6 || (int)blockType < 0)
            {
                throw new Exception(string.Format("BlockType ({0}) out of range", (int)blockType));
            }

            BlockType = blockType;
            BlockData = null;
        }

        public void SetBlockData(byte[] blockData)
        {
            BlockData = (byte[])blockData.Clone();
        }

        public void SetBlockData(Stream stream, int blockSize)
        {
            BlockData = new byte[blockSize];
            if (stream.Read(BlockData, 0, blockSize) != blockSize)
            {
                throw new InvalidDataException("EOF reached while reading stream");
            }
        }

        public void SetBlockDataZeroed(int blockSize)
        {
            BlockData = new byte[blockSize];
        }

        public int Size
        {
            get
            {
                if (BlockData == null)
                    return 0;
                else
                    return BlockData.Length;
            }
        }

        public FlacMetaDataBlockType BlockType { get; private set; }

        public byte[] BlockData { get; private set; }
    }
}
