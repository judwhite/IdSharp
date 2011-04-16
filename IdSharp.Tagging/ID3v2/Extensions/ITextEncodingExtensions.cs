using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.ID3v2.Extensions
{
    internal static class ITextEncodingExtensions
    {
        public static bool RequiresFix(this ITextEncoding frame, ID3v2TagVersion tagVersion, string value, byte[] data)
        {
            if (frame.TextEncoding != EncodingType.ISO88591)
                return false;

            if (value != null && value != ID3v2Utils.ReadString(frame.TextEncoding, data, data.Length))
            {
                frame.TextEncoding = tagVersion == ID3v2TagVersion.ID3v24 ? EncodingType.UTF8 : EncodingType.Unicode;
                return true;
            }

            return false;
        }
    }
}
