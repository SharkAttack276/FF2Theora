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

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OggPacket
{
    public OggPacket()
    {

    }
    public readonly OggConstants MagicNumber = OggConstants.MagicNumber;
    public byte StructureVersion;
    public OggFlag Flags;
    public long Granule;
    public int SerialNumber;
    public int PacketNumber;
    public uint Crc32;
    public byte SegmentLength;
    public override string ToString() =>
        $"{nameof(OggPacket)} {PacketNumber}";
}