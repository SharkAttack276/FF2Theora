# Project Source Files

The following files are part of the `src` directory of this project. They are organized by functionality for better clarity.

## Root Files
- `src\GlobalUsings.cs` – Global using directives for the project.
- `src\Program.cs` – Main entry point of the application.

## Command Line
Files related to command line processing and parsing:
- `src\CommandLine\ArgumentDefaults.cs` – Default values for command line arguments.
- `src\CommandLine\CommandContext.cs` – Context information for command execution.
- `src\CommandLine\CommandMode.cs` – Modes in which commands can operate.
- `src\CommandLine\CommandParser.cs` – Logic for parsing command line arguments.
- `src\CommandLine\CommandVariants.cs` – Different variants of supported commands.

## Extensions
Custom extension methods and related types:
- `src\Extensions\LoggerExtension.cs` – Extension methods for logging.
- `src\Extensions\LogType.cs` – Enum defining different types of logs.

## Ogg
Files related to Ogg file handling and processing:

### Core Ogg
- `src\Ogg\OggConstants.cs` – Constant values for Ogg processing.
- `src\Ogg\OggFlags.cs` – Flags used in Ogg file handling.
- `src\Ogg\OggPacket.cs` – Represents an Ogg packet.
- `src\Ogg\OggSegment.cs` – Represents an Ogg segment.

### CRC32
- `src\Ogg\Crc32\Crc32.cs` – CRC32 checksum calculations.
- `src\Ogg\Crc32\Table.cs` – Lookup tables for CRC32 calculations.

### FF2
- `src\Ogg\FF2\Ff2Packet.cs` – Handling of FF2 packets within Ogg files.
