// SPDX-License-Identifier: GPL-3.0-or-later
//
// Project: FF2Theora
// Author: Shark Attack (https://github.com/SharkAttack276)
// License: GPL-3.0-or-later
//
// This file is part of FF2Theora.
//
// You are free to modify and redistribute this code under the terms of
// the GNU General Public License, version 3 or later.
//
// If you modify this file, YOU MUST:
//   1. Retain attribution to the original author.
//   2. Clearly state what changes you made.
//
// See the LICENSE file in the root of this repository for more details.
namespace FF2Theora.Ogg;

public static class OggSegment
{
    public static int SingleFormat(int Size, scoped ref Span<byte> Span)
    {
        int bytesWritten = Math.Min(Size / byte.MaxValue, Span.Length);
        Span[..bytesWritten].Fill(byte.MaxValue);
        if ((Size %= byte.MaxValue) != byte.MinValue)
            Span[bytesWritten++] = (byte)Size;
        return bytesWritten;
    }
    public static bool IsContinuedPacket(int SegmentLength, scoped ref Span<byte> Span) =>
        Span[--SegmentLength] == byte.MaxValue;

    public static int CalculateSegments(int SegmentLength, scoped ref Span<byte> Span) =>
        SegmentLength - Span[..SegmentLength].Count(byte.MaxValue);
    public static int CalculateRawSize(int SegmentLength, scoped ref Span<byte> Span) =>
        byte.MaxValue * --SegmentLength + Span[SegmentLength];
}