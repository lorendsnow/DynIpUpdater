DynIpUpdater is a simple program to periodically check your public IP address, and update specified Cloudflare DNS A records with the new IP upon any changes.

To use it, just get the applicable binary for your system from the [releases](https://github.com/lorendsnow/DynIpUpdater/releases) page, and place a valid configuration file named "config.yaml" in the same folder (see the "ConfigTemplate.yaml" file above for a starting point and documentation regarding configuration). DynIpUpdater will look for and automatically load "config.yaml" if it is in the same directory as the binary.

If you want to name your config file something different, and/or put it in a different location, pass either a relative or full path to the config file as a command-line arg when launching the executeable:
```
$ dynipupdater-linux-x64 /path/to/your/configfile.yaml
```
