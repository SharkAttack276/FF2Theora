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
namespace FF2Theora.Ogg.Crc32;

internal static class Crc32
{
    // This is the same CRC function but with some inner changes to match the OGG crc.
    public static uint Compute(ReadOnlySpan<byte> Span, uint Initial = uint.MinValue)
    {
        uint Crc = Initial;
        for (int i = 0; i < Span.Length; i++)
            Crc = Crc << 8 ^ Table.Lookup[Crc >> 24 ^ Span[i]];
        return Crc;
    }
}