# Build Scheme

Each OS build has its own respective files:  
- **build**  
- **ILbuild**  
- **globalSets**  

To build, run the following in the terminal from the root path:  
- Windows: `build\win\build`  
- Linux: `build/linux/build`  

Alternatively, you can open it using the interface. **Note:** It must be executed from its respective directory, not the root path.

## Missing bflat?

- **Windows:** Install via terminal:  
  ```bash
  winget install bflat
  ```
- **Linux** download it from [here](https://github.com/bflattened/bflat)
