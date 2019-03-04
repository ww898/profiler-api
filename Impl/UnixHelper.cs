﻿using System;
using System.Runtime.InteropServices;
using JetBrains.Profiler.Api.Impl.Unix;

namespace JetBrains.Profiler.Api.Impl
{
  internal static class UnixHelper
  {
    public static readonly bool IsMacOsX = DeduceIsMacOsX();

    private static string GetSysnameFromUname()
    {
      var buf = Marshal.AllocHGlobal(8 * 1024);
      try
      {
        if (LibC.uname(buf) != 0)
          throw new Exception("Failed to get Unix system name");

        // Note: utsname::sysname is the first member of structure, so simple take it!
        return Marshal.PtrToStringAnsi(buf);
      }
      finally
      {
        Marshal.FreeHGlobal(buf);
      }
    }

    private static bool DeduceIsMacOsX()
    {
      var sysname = GetSysnameFromUname();
      switch (sysname)
      {
      case "Darwin":
        return true;
      case "Linux":
        return false;
      default:
        throw new Exception("Unsupported system name: " + sysname);
      }
    }

    public static bool IsAlreadyLoaded(string libraryName)
    {
      var handle = LibDl.dlopen(libraryName, (int) (DlFlags.RTLD_GLOBAL | DlFlags.RTLD_LAZY | DlFlags.RTLD_NOLOAD));
      if (handle == IntPtr.Zero)
        return false;
      LibDl.dlclose(handle);
      return true;
    }
  }
}