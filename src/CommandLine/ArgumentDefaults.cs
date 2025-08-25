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

public static class ArgumentDefaults
{
    public const int
        BufferSize = 4096,
        MinimumBufferSize = 1024;
    public const int OggLimitSize = 255;
    public const int OggMinSize = 1;
    public const string Version = "1.0";
    public const string CopyrightMessage =
    $@"FF2Theora © 2025 release {Version},
    A remuxer tool for remuxing Ogg Theora files to and from the FF2 Theora format.";
}