using System;

namespace Ogg
{
    public static class OggTools
    {
        public static double GetOggLength(string filePath)
        {
            return new NAudio.Vorbis.VorbisWaveReader(filePath).TotalTime.TotalMinutes;
        }
    }
}
