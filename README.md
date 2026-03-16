# PointerScanBizhawk

This is a small tool designed to imitate Cheat Engine's pointer scan for [BizHawk](https://github.com/TASEmulators/BizHawk).

## Known Issues

- As the script loads in the entire memory during the first scan, the emulator will lag for a while afterwards.
- When sorting the columns in the table, the Emulator will also lag due to calculating several columns for easier view of pointers
- As I designed this with a game on the genesis, other consoles may not be entirely supported, feel free to open an issue to make me aware
- Currently the tool is limited to what BizHawk considers the "MainMemory" of the core.

# Credits

I referenced [Pokebot](https://github.com/Kakumi/Pokebot) to learn how to make a basic Bizhawk tool.
