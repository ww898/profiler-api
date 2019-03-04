﻿using System;

// ReSharper disable InconsistentNaming

namespace JetBrains.Profiler.Api.Impl.Unix
{
  [Flags]
  internal enum DlFlags
  {
    RTLD_LAZY = 0x00001,
    RTLD_NOLOAD = 0x00004,
    RTLD_GLOBAL = 0x00100
  }
}