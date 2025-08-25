global using System;
global using System.Buffers.Binary;
global using System.IO;
global using System.Runtime.InteropServices;

// System.CommandLine namespaces
global using System.CommandLine;
global using System.CommandLine.Invocation;
global using System.CommandLine.Parsing;

// Project-related namespaces
global using FF2Theora;
global using FF2Theora.CommandLine;
global using FF2Theora.Extensions;
global using FF2Theora.Ogg;
global using FF2Theora.Ogg.Crc32;

// Aliases
global using EnumeratorContext = (int CurrentPageNumber, long PreviousPacketPosition, int CurrentPageSize);
global using OggSegmentState = (int CurrentSegment, int TotalSegments);
