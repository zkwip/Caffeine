using Cocona;
using System;
using System.Threading;

namespace Caffeine;

static class Entry
{
    static void Main(string[] args)
    {
        //CoconaApp.Run(Run, args);

        const bool keyboard = true;
        const bool sound = true;
        const bool visible = false;

        Run(keyboard, sound, visible);
    }
    private static void Run([Option("k")]bool keyboard = true, [Option("s")] bool sound = true, [Option("v")] bool visible = true)
    {
        var cmgr = new CaffeineManager(keyboard, sound, visible);
        cmgr.Start();
        while(cmgr.Active)
        {
            Thread.Sleep(1000);
        }
    }
}