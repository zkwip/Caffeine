using Cocona;
using System;
using System.Threading;

namespace Caffeine;

static class Entry
{
    static void Main(string[] args)
    {
        /*
        const bool keyboard = true;
        const bool sound = true;
        const bool visible = false;

        Run(keyboard, sound, visible);*/
        CoconaApp.Run(Run, args);
    }
    private static void Run([Option("k")]bool keyboard = true, [Option("s")] bool sound = true, [Option("v")] bool visible = true)
    {
        var cmgr = new CaffeineManager(keyboard, sound, visible);
        cmgr.Run();
    }
}