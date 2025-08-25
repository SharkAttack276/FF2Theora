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
namespace FF2Theora.CommandLine;

public record class CommandContext(
    int BufferSize,
    bool CopyrightAttend,
    string InputFile,
    string OutputFile,
    bool Overwrite,
    bool AdjustPacket,
    bool FlushPacket,
    int PacketLimitSize,
    int PacketDelay,
    bool Verbose,
    CommandMode Mode
);