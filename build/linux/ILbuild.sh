# Linux/Unix Build

# Save current directory and move two levels up
pushd . > /dev/null
cd "$(dirname "$0")/../.."

# Set build type
BUILD_TYPE="linux"

# Set optimization flags
OPTIMIZATIONS="--no-debug-info"

# Initialize global variable sets
source build/globalSets.sh

BFLAT build-il \
  -r "$SHARED_PATH/System.CommandLine.dll" \
  -o "$OUTPUT_PATH" \
  $OPTIMIZATIONS

# Restore previous directory
popd > /dev/null
