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

public static class CommandVariants
{
    public static readonly Option<int> BufferSizeOption = new(
        ["-b", "--buffer"],
        () => ArgumentDefaults.BufferSize,
        $"Specify buffer size when reading/writing payload data. (min: {ArgumentDefaults.MinimumBufferSize})")
    {
        ArgumentHelpName = "size"
    };
    public static readonly Option<string> OutputFile = new(
        ["-o", "--output"],
        "Specify whatever output. Either a file or stdout."
    )
    {
        ArgumentHelpName = "file/stdout"
    };
    public static readonly Option<bool> CopyrightAttend = new(
        ["-c", "--copyright"],
        "Disable copyright notice."
    );
    public static readonly Option<bool> OverwriteOption = new(
        ["-y", "--overwrite"],
        "Allow overwriting existing output file."
    );
    public static readonly Option<bool> AdjustPacketOption = new(
        ["-F", "--adjust"],
        "Adjust flags to the packet, (Requires seekable output stream)"
    );
    public static readonly Option<int> PacketLimitSize = new(
        ["-s", "--limit"],
        () => ArgumentDefaults.OggLimitSize,
        "Limit OGG packet size during decoding."
    )
    {
        ArgumentHelpName = "segment size"
    };
    public static readonly Option<int> PacketDelayOption = new(
        ["-d", "--delay"],
        () => 0,
        "Delay the packet by this amount of time"
    )
    {
        ArgumentHelpName = "ms"
    };
    public static readonly Option<bool> VerboseOption = new(
        ["-l", "--verbose"],
        "Log each packet header information."
    );
    public static readonly Option<bool> FlushPacketOption = new(
        ["-f", "--flush"],
        "Flush every packet immediately (Less memory usage, slower depending on DISK I/O speed)"
    );
    public static readonly Argument<string> InputFile = new(
        "input", "theora file/stdin, either way.");
    public static Command Create(CommandMode Mode) => Mode switch
    {
        CommandMode.Default => new RootCommand("Remux Ogg Theora to FF2 format") {
            InputFile,
            BufferSizeOption,
            OutputFile,
            AdjustPacketOption,
            FlushPacketOption,
            PacketDelayOption,
            VerboseOption,
            CopyrightAttend,
            OverwriteOption
        },
        CommandMode.Demux => new Command("demux", "Remux FF2 format back to Ogg Theora") {
            InputFile,
            BufferSizeOption,
            OutputFile,
            PacketLimitSize,
            FlushPacketOption,
            PacketDelayOption,
            VerboseOption,
            CopyrightAttend,
            OverwriteOption
        }
    };
}