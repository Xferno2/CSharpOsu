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
OsuUser[] beatmap = osu.GetUser("user id or username");
var us = beatmap[0]; //0 means first result found.
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
OsuMatch[] match = osu.GetMatch("match id");
var mh = match[0]; //0 means first result found.
Console.WriteLine(mh.match.name);
```
### Replay data([/api/get_replay](https://github.com/ppy/osu-api/wiki#apiget_replay))
`(eg: https://osu.ppy.sh/b/000000 and 0 and https://osu.ppy.sh/u/000000 or https://osu.ppy.sh/u/username)`
```c#
OsuReplay[] replay = osu.GetReplay("beatmap id", "game mode", "user id or username");
var ry = replay[0]; //0 means first result found.
Console.WriteLine(ry.content);
```

## Side note
The functions from osu!api are all covered and all the searching criteria are accepted by functions parameters. I've made the code cleaner but I still got a lot to do with cleaning. I've added comments and summaries to help you figuring out what everything dose.

Everything, except Beatmap and Users functions, haven't been tested so if any function or a parametre of a function returns an error just make an Issues post and I will see where is the problem.

### To do
1. It's still not clear for me how replay encoding works but I will try to make a converter to output a .osr file.
