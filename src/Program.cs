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
namespace FF2Theora;

public static class Program
{
    public static unsafe int Main(string[] Arguments)
    {
        RootCommand Root = (RootCommand)CommandVariants.Create(CommandMode.Default);
        Command Decode = CommandVariants.Create(CommandMode.Decode);
        CommandContext CommandContext = default;
        EnumeratorContext EnumeratorContext = default;
        Stream InStream = null, OutStream = null, PacketBuffer = null;
        Action? StreamActions = null;
        Root.AddCommand(Decode);
        Action<InvocationContext> Setup = ctx =>
        {
            CommandContext = CommandParser.Parse(ctx);
            if (!CommandContext.CopyrightAttend)
                Extension.Log(ArgumentDefaults.CopyrightMessage, LogType.Information);
            // Check whatever the input is, pipe trigger, or either a file.

            InStream = CommandContext.InputFile == "-" ? Console.OpenStandardInput() :
                File.Exists(CommandContext.InputFile) ? File.OpenRead(CommandContext.InputFile) :
                throw new FileNotFoundException("Input file DOESN'T exist.");
            PacketBuffer = new MemoryStream(CommandContext.BufferSize); // preloaded packet buffer stream.
            var Out = CommandContext.OutputFile ?? Path.ChangeExtension(CommandContext.InputFile, CommandContext.Mode switch
            {
                CommandMode.Default => "theora",
                CommandMode.Decode => "ogv",
                _ => throw new NotImplementedException()
            });
            if (Out == CommandContext.InputFile && Out != "-")
                throw new ArgumentException("Output file cannot be the same as input file.");
            OutStream = Out == "-" ? Console.OpenStandardOutput() :
                File.Exists(Out) && !CommandContext.Overwrite ? throw new("Output file already exists. Use --overwrite option. to replace it.") :
                File.Create(Out);
            if (CommandContext.FlushPacket)
                StreamActions += () => OutStream.Flush();
            if (CommandContext.PacketDelay > 0) // Add the action when necessary.
                StreamActions += () => System.Threading.Thread.Sleep(CommandContext.PacketDelay); // Delay the packet by this amount of time (in ms)
            if (CommandContext.Verbose)
                StreamActions += () => Extension.Log($"[Page {EnumeratorContext.CurrentPageNumber}] Size: {EnumeratorContext.CurrentPageSize} bytes.", LogType.Information);
        };

        Root.SetHandler(ctx =>
        {
            try
            {
                Setup(ctx);
                Span<byte> SegmentRecord = stackalloc byte[256];
                Span<byte> Buffer = new byte[CommandContext.BufferSize];
                OggPacket PacketRecord = default;
                Ff2Packet CPacketRecord = default;
                Ff2Packet PCPacketRecord = default; // Previous packet record
                OggSegmentState SegmentState = default;

                int bytesRead, remainingBytes;
                if (CommandContext.AdjustPacket)
                {
                    if (!OutStream.CanSeek)
                        throw new("Output stream is not seekable.");
                    CPacketRecord.StartOfStreamFlag = 1;
                    StreamActions += () => EnumeratorContext.PreviousPacketPosition = OutStream.Position;
                }

                // Process logic starts here
                while (InStream.Read(new Span<byte>(&PacketRecord, sizeof(OggPacket))) == sizeof(OggPacket))
                {
                    // Read the segments data.
                    InStream.Read(SegmentRecord[..PacketRecord.SegmentLength]);
                    uint CrcBackup = PacketRecord.Crc32; // Backup the Crc value
                    PacketRecord.Crc32 = uint.MinValue; // Ogg CRC Initially is zero here.
                    uint Crc = // Validation time
                        Crc32.Compute(new ReadOnlySpan<byte>(&PacketRecord, sizeof(OggPacket)));

                    // Include the segments data in the CRC calculation
                    Crc =
                        Crc32.Compute(SegmentRecord[..PacketRecord.SegmentLength], Crc);
                    SegmentState.TotalSegments = OggSegment.CalculateSegments(PacketRecord.SegmentLength, ref SegmentRecord);

                    for (int SegmentIndex = 0; SegmentIndex < PacketRecord.SegmentLength; SegmentIndex++)
                    {
                        EnumeratorContext.CurrentPageSize += SegmentRecord[SegmentIndex];
                        if (SegmentRecord[SegmentIndex] == byte.MaxValue) continue; // Skip if the segment is 255 bytes (0xFF)
                        // Set the EOS flag if its the last segment.
                        if (SegmentState.CurrentSegment == SegmentState.TotalSegments - 1)
                            CPacketRecord.EndOfStreamFlag = (int)(PacketRecord.Flags & OggFlag.EndOfStream) >> 2;
                        // Calculate Crc by the payload data.
                        remainingBytes = EnumeratorContext.CurrentPageSize; // Start processing
                        while ((bytesRead = InStream.Read(Buffer[..Math.Min(CommandContext.BufferSize, remainingBytes)])) > 0)
                        {
                            remainingBytes -= bytesRead;
                            Crc =
                                Crc32.Compute(Buffer[..bytesRead], Crc); // Calculate the CRC value of the payload data.
                            PacketBuffer.Write(Buffer[..bytesRead]);
                        }
                        EnumeratorContext.CurrentPageSize = (int)PacketBuffer.Length;
                        CPacketRecord.StartOfStreamFlag |= (int)(PacketRecord.Flags & OggFlag.BeginningOfStream) >> 1;
                        CPacketRecord.PacketSize = EnumeratorContext.CurrentPageSize;
                        CPacketRecord.SerialNumber = PacketRecord.SerialNumber;
                        CPacketRecord.PacketNumber = EnumeratorContext.CurrentPageNumber;
                        CPacketRecord.Granule = PacketRecord.Granule;
                        PacketBuffer.Position = 0; // Reset the position of the stream to the beginning.
                        StreamActions?.Invoke();
                        // Start flush the payload data of the packet.
                        OutStream.Write(new ReadOnlySpan<byte>(&CPacketRecord, sizeof(Ff2Packet)));
                        while ((bytesRead = PacketBuffer.Read(Buffer[..CommandContext.BufferSize])) > 0)
                            OutStream.Write(Buffer[..bytesRead]);
                        PacketBuffer.SetLength(0); // Clear the stream for the next packet.
                        PCPacketRecord = CPacketRecord; // Save the current packet record for the next packet.
                        CPacketRecord = default;
                        SegmentState.CurrentSegment++;
                        EnumeratorContext.CurrentPageNumber++;
                        EnumeratorContext.CurrentPageSize = default;
                    }
                    // Those who know
                    remainingBytes = EnumeratorContext.CurrentPageSize;
                    while ((bytesRead = InStream.Read(Buffer[..Math.Min(CommandContext.BufferSize, remainingBytes)])) > 0)
                    {
                        remainingBytes -= bytesRead;
                        Crc =
                            Crc32.Compute(Buffer[..bytesRead], Crc);

                        PacketBuffer.Write(Buffer[..bytesRead]);
                    }
                    EnumeratorContext.CurrentPageSize = default;
                    SegmentState = default; // Reset the segment state for the next segment.
                    if (Crc != CrcBackup) throw new InvalidDataException($"{PacketRecord}: Invalid packet: checksum mismatch."); // Validate packets 
                    if ((OggConstants)PacketRecord.MagicNumber != OggConstants.MagicNumber || (OggConstants)PacketRecord.StructureVersion != OggConstants.StructureVersion) throw new InvalidDataException("Tampered packet detected: header fields are inconsistent with Ogg specification.");
                }
                if (EnumeratorContext.CurrentPageNumber == 0)
                    throw new("No valid packets found in the stream.");
                if (CommandContext.AdjustPacket)
                {
                    OutStream.Position = EnumeratorContext.PreviousPacketPosition; // Set the position of the stream to the previous packet position.
                    PCPacketRecord.EndOfStreamFlag = 1; // Set the end of stream flag to 1.
                    OutStream.Write(new ReadOnlySpan<byte>(&PCPacketRecord, sizeof(Ff2Packet))); // Write the packet record to the stream.
                }
            }
            catch (Exception ex)
            {
                Extension.Log(ex.Message, LogType.Error);
            }
            finally
            {
                InStream?.Dispose();
                OutStream?.Dispose();
                PacketBuffer?.Dispose();
                StreamActions = null;
            }
        });
        Decode.SetHandler(ctx =>
        {
            try
            {
                Setup(ctx);
                Span<byte> SegmentRecord = stackalloc byte[256];
                Span<byte> Buffer = new byte[CommandContext.BufferSize];
                OggPacket PacketRecord = new();
                Ff2Packet CPacketRecord = default;
                while (InStream.Read(new Span<byte>(&CPacketRecord, sizeof(Ff2Packet))) == sizeof(Ff2Packet))
                {
                    bool Continued;
                    int PageNumber = 0;
                    PacketRecord.Flags |= CPacketRecord.StartOfStreamFlag != 0 ? OggFlag.BeginningOfStream : OggFlag.None;
                    PacketRecord.Flags |= CPacketRecord.EndOfStreamFlag != 0 ? OggFlag.EndOfStream : OggFlag.None;
                    while (CPacketRecord.PacketSize > 0)
                    {
                        Continued = PageNumber > 0;
                        EnumeratorContext.CurrentPageSize = Math.Min(CPacketRecord.PacketSize, CommandContext.PacketLimitSize * byte.MaxValue);
                        PacketRecord.SegmentLength = (byte)OggSegment.SingleFormat(EnumeratorContext.CurrentPageSize, ref SegmentRecord);
                        CPacketRecord.PacketSize -= EnumeratorContext.CurrentPageSize;
                        PacketRecord.PacketNumber = EnumeratorContext.CurrentPageNumber++;
                        PacketRecord.SerialNumber = CPacketRecord.SerialNumber;
                        PacketRecord.Granule = CPacketRecord.PacketSize == 0 ? CPacketRecord.Granule : -*(byte*)&Continued; // Set the granule to -1 if the packet is not complete.
                        PacketRecord.Flags |= *(OggFlag*)&Continued; // Set the continued packet flag if the packet is not complete.
                        PacketRecord.Crc32 = Crc32.Compute(new ReadOnlySpan<byte>(&PacketRecord, sizeof(OggPacket)));
                        PacketRecord.Crc32 = Crc32.Compute(SegmentRecord[..PacketRecord.SegmentLength], PacketRecord.Crc32);
                        int bytesRead, remainingBytes = EnumeratorContext.CurrentPageSize;
                        while ((bytesRead = InStream.Read(Buffer[..Math.Min(remainingBytes, CommandContext.BufferSize)])) > 0)
                        {
                            remainingBytes -= bytesRead;
                            PacketRecord.Crc32 = Crc32.Compute(Buffer[..bytesRead], PacketRecord.Crc32); // Calculate the CRC value of the payload data.
                            PacketBuffer.Write(Buffer[..bytesRead]);
                        }
                        PacketBuffer.Position = 0; // Reset the position of the stream.
                        OutStream.Write(new ReadOnlySpan<byte>(&PacketRecord, sizeof(OggPacket)));
                        OutStream.Write(SegmentRecord[..PacketRecord.SegmentLength]);
                        while ((bytesRead = PacketBuffer.Read(Buffer[..CommandContext.BufferSize])) > 0)
                            OutStream.Write(Buffer[..bytesRead]);
                        PacketBuffer.SetLength(0); // Clear the stream for the next packet.
                        PacketRecord = new(); // Clear the packet for the next packet.
                        PageNumber++;
                        StreamActions?.Invoke();
                    }
                }
                if (EnumeratorContext.CurrentPageNumber == 0)
                    throw new("No valid packets found in the stream.");
            }
            catch (Exception ex)
            {
                Extension.Log(ex.Message, LogType.Error);
            }
            finally
            {
                InStream?.Dispose();
                OutStream?.Dispose();
                PacketBuffer?.Dispose();
                StreamActions = null;
            }
        });
        return Root.Invoke(Arguments);
    }
}