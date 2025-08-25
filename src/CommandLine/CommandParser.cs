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

public static class CommandParser
{
    public static CommandContext Parse(InvocationContext Context) =>
        new(
            Context.ParseResult.HasOption(CommandVariants.BufferSizeOption) ? Math.Max(Context.ParseResult.GetValueForOption(CommandVariants.BufferSizeOption), ArgumentDefaults.MinimumBufferSize) : ArgumentDefaults.MinimumBufferSize,
            Context.ParseResult.HasOption(CommandVariants.CopyrightAttend) ? Context.ParseResult.GetValueForOption(CommandVariants.CopyrightAttend) : true,
            Context.ParseResult.GetValueForArgument<string>(CommandVariants.InputFile),
            Context.ParseResult.GetValueForOption(CommandVariants.OutputFile),
            Context.ParseResult.HasOption(CommandVariants.OverwriteOption) ? Context.ParseResult.GetValueForOption(CommandVariants.OverwriteOption) : false,
            Context.ParseResult.HasOption(CommandVariants.AdjustPacketOption) ? Context.ParseResult.GetValueForOption(CommandVariants.AdjustPacketOption) : false,
            Context.ParseResult.HasOption(CommandVariants.FlushPacketOption) ? Context.ParseResult.GetValueForOption(CommandVariants.FlushPacketOption) : false,
            Context.ParseResult.HasOption(CommandVariants.PacketLimitSize) ? Math.Clamp(Context.ParseResult.GetValueForOption(CommandVariants.PacketLimitSize), ArgumentDefaults.OggMinSize, ArgumentDefaults.OggLimitSize) : ArgumentDefaults.OggLimitSize,
            Context.ParseResult.HasOption(CommandVariants.PacketDelayOption) ? Context.ParseResult.GetValueForOption(CommandVariants.PacketDelayOption) : 0,
            Context.ParseResult.HasOption(CommandVariants.VerboseOption) ? Context.ParseResult.GetValueForOption(CommandVariants.VerboseOption) : false,
            Enum.TryParse<CommandMode>(Context.ParseResult.CommandResult.Command.Name, true, out var CommandMode) ? CommandMode : CommandMode.Default
        );
}