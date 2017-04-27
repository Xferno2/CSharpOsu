# CSharpOsu
C# library for [osu!api](https://github.com/ppy/osu-api/wiki)
using [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
project.

## Directions for use
To use this library you must add a reference to the DLL file in your project (Project -> Add Reference -> Browse) and search for CSharpOsu.dll (can be found in the Debug folder of this project) and add it to your project.

### Getting started
```c#
OsuClient osu = new OsuClient("api key");
```

### Beatmaps([/api/get_beatmaps](https://github.com/ppy/osu-api/wiki#apiget_beatmaps))
For map sets: `(eg: https://osu.ppy.sh/s/000000)`
``` c#
OsuBeatmap[] beatmap = osu.GetBeatmap("Beatmap id", true);
var bm = beatmap[0]; //0 means first beatmap found.
Console.WriteLine(beatmap.title);
```
For a single beatmap: `(eg: https://osu.ppy.sh/b/000000)`
``` c#
var bm = new OsuBeatmap();
OsuBeatmap[] beatmap = osu.GetBeatmap("Beatmap id", false);
var bm = beatmap[0]; //0 means first beatmap found.
Console.WriteLine(beatmap.title);
```

### Users([/api/get_user](https://github.com/ppy/osu-api/wiki#apiget_user))
`(eg: https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuUser[] beatmap = osu.GetUser("user id or username");
var us = beatmap[0]; //0 means first user found.
Console.WriteLine(user.user_id);
```

## Side note
This library is incomplete and its code is not the cleanest but it works ,
at least for me. I will add more usage to the library soon.
