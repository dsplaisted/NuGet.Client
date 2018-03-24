// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Contains project metadata needed for project restore operation.
    /// </summary>
    [ComImport]
    [Guid("ab43992d-b977-436d-84c2-e76aeed20de2")]
    public interface IVsProjectRestoreInfo
    {
        /// <summary>
        /// Base intermediate path of the project.
        /// </summary>
        string BaseIntermediatePath { get; }

        //  TODO: Update project-system to add this property, and figure out how to sync the updates
        //  https://github.com/dotnet/project-system/blob/master/src/Microsoft.VisualStudio.ProjectSystem.Managed.VS/ProjectSystem/VS/NuGet/ProjectRestoreInfo.cs
        /// <summary>
        /// MSBuildProjectExtensionsPath of the project
        /// </summary>
        string MSBuildProjectExtensionsPath { get; }

        /// <summary>
        /// Target frameworks metadata.
        /// </summary>
        IVsTargetFrameworks TargetFrameworks { get; }

        /// <summary>
        /// Collection of tool references.
        /// </summary>
        IVsReferenceItems ToolReferences { get; }

        /// <summary>
        /// Original raw value of TargetFrameworks property as set in a project file.
        /// </summary>
        string OriginalTargetFrameworks { get; }
    }
}
