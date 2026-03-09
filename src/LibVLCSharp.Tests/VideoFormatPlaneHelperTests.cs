using System;
using LibVLCSharp.Shared;
using NUnit.Framework;

namespace LibVLCSharp.Tests
{
    [TestFixture]
    public class VideoFormatPlaneHelperTests
    {
        [Test]
        public void ReadCopiesAllPlanes()
        {
            var pitches = new uint[] { 640, 320, 320 };
            var lines = new uint[] { 480, 240, 240 };
            var readPitches = new uint[3];
            var readLines = new uint[3];

            VideoFormatPlaneHelper.Read(ref pitches[0], ref lines[0], 3, readPitches, readLines);

            Assert.That(readPitches, Is.EqualTo(pitches));
            Assert.That(readLines, Is.EqualTo(lines));
        }

        [Test]
        public void WriteCopiesAllPlanes()
        {
            var pitches = new uint[3];
            var lines = new uint[3];
            var expectedPitches = new uint[] { 640, 320, 320 };
            var expectedLines = new uint[] { 480, 240, 240 };

            VideoFormatPlaneHelper.Write(ref pitches[0], ref lines[0], 3, expectedPitches, expectedLines);

            Assert.That(pitches, Is.EqualTo(expectedPitches));
            Assert.That(lines, Is.EqualTo(expectedLines));
        }

        [Test]
        public void ReadValidatesDestinationLength()
        {
            var pitches = new uint[] { 640, 320, 320 };
            var lines = new uint[] { 480, 240, 240 };

            Assert.That(
                () => VideoFormatPlaneHelper.Read(ref pitches[0], ref lines[0], 3, new uint[2], new uint[3]),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void WriteValidatesPlaneCount()
        {
            var pitches = new uint[3];
            var lines = new uint[3];

            Assert.That(
                () => VideoFormatPlaneHelper.Write(ref pitches[0], ref lines[0], 0, new uint[3], new uint[3]),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
