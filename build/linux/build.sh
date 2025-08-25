# Linux/Unix Build

# Save current directory and move two levels up
pushd . > /dev/null
cd "$(dirname "$0")/../.."

# Set build type
BUILD_TYPE="linux"

# Set optimization flags
OPTIMIZATIONS="-Os --no-exception-messages --no-debug-info --no-pie --no-stacktrace-data"

# Initialize global variable sets
source build/globalSets.sh

BFLAT build \
  -r "$SHARED_PATH/System.CommandLine.dll" \
  -o "$OUTPUT_PATH" \
  $OPTIMIZATIONS \
  @"$SOURCE_PATH"

# Restore previous directory
popd > /dev/null
