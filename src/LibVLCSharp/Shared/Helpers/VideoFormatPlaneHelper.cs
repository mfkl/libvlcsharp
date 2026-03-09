using System;

namespace LibVLCSharp.Shared
{
    /// <summary>
    /// Helper methods for working with multi-plane values exposed through video format callbacks.
    /// </summary>
    public static class VideoFormatPlaneHelper
    {
        /// <summary>
        /// Copies plane pitches and line counts from the callback parameters into managed arrays.
        /// </summary>
        /// <param name="pitches">The <c>pitches</c> argument received by the video format callback.</param>
        /// <param name="lines">The <c>lines</c> argument received by the video format callback.</param>
        /// <param name="planeCount">The number of planes to copy.</param>
        /// <param name="pitchValues">Destination array for pitch values.</param>
        /// <param name="lineValues">Destination array for line counts.</param>
        public static unsafe void Read(ref uint pitches, ref uint lines, int planeCount, uint[] pitchValues, uint[] lineValues)
        {
            ValidatePlaneCount(planeCount);
            ValidateBuffer(pitchValues, planeCount, nameof(pitchValues));
            ValidateBuffer(lineValues, planeCount, nameof(lineValues));

            fixed (uint* pitchPtr = &pitches)
            fixed (uint* linePtr = &lines)
            {
                for (var i = 0; i < planeCount; i++)
                {
                    pitchValues[i] = pitchPtr[i];
                    lineValues[i] = linePtr[i];
                }
            }
        }

        /// <summary>
        /// Copies managed plane pitches and line counts into the callback parameters.
        /// </summary>
        /// <param name="pitches">The <c>pitches</c> argument received by the video format callback.</param>
        /// <param name="lines">The <c>lines</c> argument received by the video format callback.</param>
        /// <param name="planeCount">The number of planes to copy.</param>
        /// <param name="pitchValues">Source array for pitch values.</param>
        /// <param name="lineValues">Source array for line counts.</param>
        public static unsafe void Write(ref uint pitches, ref uint lines, int planeCount, uint[] pitchValues, uint[] lineValues)
        {
            ValidatePlaneCount(planeCount);
            ValidateBuffer(pitchValues, planeCount, nameof(pitchValues));
            ValidateBuffer(lineValues, planeCount, nameof(lineValues));

            fixed (uint* pitchPtr = &pitches)
            fixed (uint* linePtr = &lines)
            {
                for (var i = 0; i < planeCount; i++)
                {
                    pitchPtr[i] = pitchValues[i];
                    linePtr[i] = lineValues[i];
                }
            }
        }

        static void ValidatePlaneCount(int planeCount)
        {
            if (planeCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(planeCount));
        }

        static void ValidateBuffer(uint[] values, int planeCount, string paramName)
        {
            if (values == null)
                throw new ArgumentNullException(paramName);

            if (values.Length < planeCount)
                throw new ArgumentException("Buffer must contain at least planeCount values.", paramName);
        }
    }
}
