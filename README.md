# CSharpOsu
C# library for [osu!api](https://github.com/ppy/osu-api/wiki)
using [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
project.

## Getting started
```c#
OsuClient osu = new OsuClient("api key");
```

### Beatmaps([/api/get_beatmaps](https://github.com/ppy/osu-api/wiki#apiget_beatmaps))
For map sets: `(eg: https://osu.ppy.sh/s/000000)`
``` c#
var bm = new OsuBeatmap();
bm = Osu.GetBeatmapSet("map set id");
```
For a single beatmap: `(eg: https://osu.ppy.sh/b/000000)`
``` c#
var bm = new OsuBeatmap();
bm = Osu.GetBeatmap("beatmap id");
```

### Users([/api/get_user](https://github.com/ppy/osu-api/wiki#apiget_user))
`(eg: https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
var us = new OsuUser();
us = Osu.GetUser("userid or username");
```

## Side note
This library is incomplete and its code is not the cleanest but it works ,
at least for me. I will add more usage to the library soon.
