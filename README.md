# CSharpOsu
C# library for [osu!api](https://github.com/ppy/osu-api/wiki)
using [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
This library is CLS Compilant , that means that can be used with
any .NET Framework ( C#, C++/CLI, Eiffel, F#, IronPython, IronRuby,
PowerBuilder, Visual Basic, Visual COBOL, and Windows PowerShell).
project.

## Directions for use
To use this library you must add a reference to the DLL file in your project (Project -> Add Reference -> Browse) and search for CSharpOsu.dll (can be found in the Debug folder of this project) and add it to your project.

### Getting started
```c#
OsuClient osu = new OsuClient("api key");
```

### Beatmaps([/api/get_beatmaps](https://github.com/ppy/osu-api/wiki#apiget_beatmaps))
For beatmapset: `(eg: https://osu.ppy.sh/s/000000)`
``` c#
OsuBeatmap[] beatmap = osu.GetBeatmap("beatmapset id", true);
var bm = beatmap[0]; //0 means first result found.
Console.WriteLine(bm.title);
```
For a single beatmap: `(eg: https://osu.ppy.sh/b/000000)`
``` c#
OsuBeatmap[] beatmap = osu.GetBeatmap("beatmap id", false);
var bm = beatmap[0]; //0 means first result found.
Console.WriteLine(bm.title);
```

### Users([/api/get_user](https://github.com/ppy/osu-api/wiki#apiget_user))
`(eg: https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuUser[] user = osu.GetUser("user id or username");
var us = user[0]; //0 means first result found.
Console.WriteLine(us.user_id);
```

### Scores([/api/get_scores](https://github.com/ppy/osu-api/wiki#apiget_scores))
`(eg: https://osu.ppy.sh/b/000000)`
```c#
OsuScore[] score = osu.GetScore("beatmap id");
var sc = score[0]; //0 means first result found.
Console.WriteLine(sc.score);
```
### Best Performance([/api/get_user_best](https://github.com/ppy/osu-api/wiki#apiget_user_best))
`(eg: https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuUserBest[] userbest = osu.GetUserBest("user id or username");
var ub = userbest[0]; //0 means first result found.
Console.WriteLine(ub.rank);
```
### Recently Played([/api/get_user_recent](https://github.com/ppy/osu-api/wiki#apiget_user_recent))
`(eg: https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuUserRecent[] userrecent = osu.GetUserRecent("user id or username");
var ur = userrecent[0]; //0 means first result found.
Console.WriteLine(ur.maxcombo);
```
### Multiplayer([/api/get_match](https://github.com/ppy/osu-api/wiki#apiget_match))
`(eg: https://osu.ppy.sh/mp/00000000)`
```c#
OsuMatch multip = osu.GetMatch("match id");
Console.WriteLine(multip.match.name);
```
### Replay data([/api/get_replay](https://github.com/ppy/osu-api/wiki#apiget_replay))
For getting replay EncodedBase64 content(pretty useless without parsing): `(eg: https://osu.ppy.sh/b/000000 | 0 | https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuReplay replay = osu.GetReplay("beatmap id", "game mode", "user id or username");
Console.WriteLine(replay.content);
```
For getting the .osr file on disk: `(eg: "C:/replay.osr" | 0 | https://osu.ppy.sh/b/000000 | https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuReplay replay = osu.GetReplay("path on disk with name and .osr extenstion", "game mode", "beatmap id", "user id or username");
```

## Side note
The functions from osu!api are all covered and all the searching criteria are accepted by functions parameters. I've made the code cleaner but I still got a lot to do with cleaning.

### To do
1. Add error handlers.
