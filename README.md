# FF2Theora

## Whats this?

FF2Theora is a specialized remuxer for OggTheora streams, built specifically for Feeding Frenzy 2. This game stores its sprite animations in **OggTheora streams wrapped in a custom container**, rather than standard Ogg files. FF2Theora allows you to convert between standard OggTheora and the game’s proprietary format, making it easy to create, modify, or extract sprite animations for modding, restoration, or content creation.

The name **FF2Theora** comes from the game’s legacy format itself—it’s essentially “FF2-compatible Theora.”


## Key Features
- **Game-compatible remuxing** – Convert standard OggTheora streams into the game’s custom container **and back** to standard OggTheora.  
- **Robust Ogg handling** – Safely handles complex streams, even partially corrupted files.  
- **High performance** – Minimal overhead and fast remuxing.  
- **Pure streaming** – Zero-seeking for efficient, pipeline-friendly workflows.  
- **Pipe-friendly** – Full stdout and stdin support for integration with FFmpeg and other tools.  
- **Repair and debug tools** – Inspect and fix streams to ensure compatibility with the game.

## Usage Examples

### Quick Preview
Stream a sprite animation directly to a player without temporary files:
```bash
ff2theora decode -o - animation.theora | ffplay -
```
### Quick Extraction
Extract frames efficiently for editing or inspection:
```bash
ff2theora decode -o - animation.theora | ffmpeg -i - frame.%d.jpg
```
### Direct Encode
```bash
ffmpeg -i - frame.%d.jpg -f ogg - | ff2theora -o animation.theora -
```
### Flush per packet for large files:
```bash
ff2theora -f animation.ogg
```
### Remux and fix a corrupted Ogg (Requires seeking)
```bash
ff2theora -F animation.ogg
```
## Advanced Options

FF2Theora includes several options that are particularly useful for game sprite workflows:

- `-F` : Repair or sanitize corrupted streams to ensure compatibility with the game.  
- `-d <ms>` / `--delay <ms>` : Add a delay in milliseconds to each packet, useful for timing adjustments or testing playback.  
- `-l` / `--verbose` : Print detailed log information about the packet stats.  
- `-f` : Flush per packet immediately to minimize buffering and memory usage, ideal for large files or streaming pipelines.  
- `-b <size>` / `--buffersize <size>` : Set the internal buffer size in bytes to control memory usage during processing.
## License

FF2Theora is licensed under the **GNU General Public License v3.0 (GPL-3.0)**. See the [LICENSE](LICENSE) file for full details.

