The api is pretty obvious, but here's a head start.

If you want to load from a file use the static method TagCompound.ReadFromFile(filename).
If you want to safe to a file use the instance method TagCompound.WriteToFile(filename).

Also for streams use TagCompound.Write and AbstractTag.Read.