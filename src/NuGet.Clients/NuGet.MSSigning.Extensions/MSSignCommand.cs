// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Protocol;

namespace NuGet.MSSigning.Extensions
{
    [Command(typeof(NuGetMSSignCommand), "mssign", "MSSignCommandDescription",
       MinArgs = 1,
       MaxArgs = 1,
       UsageSummaryResourceName = "MSSignCommandUsageSummary",
       UsageExampleResourceName = "MSSignCommandUsageExamples",
       UsageDescriptionResourceName = "MSSignCommandUsageDescription")]
    public class MSSignCommand : MSSignAbstract
    {
        // Default constructor used only for testing, since the Command Default Constructor is protected
        public MSSignCommand() : base()
        {
        }

        [Option(typeof(NuGetMSSignCommand), "MSSignCommandOverwriteDescription")]
        public bool Overwrite { get; set; }

        public override async Task ExecuteCommandAsync()
        {
            var signRequest = GetAuthorSignRequest();
            var packages = GetPackages();
            var signCommandRunner = new SignCommandRunner();
            var result = await signCommandRunner.ExecuteCommandAsync(
                packages, signRequest, Timestamper, Console, OutputDirectory, Overwrite, CancellationToken.None);


            if (result != 0)
            {
                throw new ExitCodeException(exitCode: result);
            }
        }

        private AuthorSignPackageRequest GetAuthorSignRequest()
        {
            ValidatePackagePath();
            WarnIfNoTimestamper(Console);
            ValidateCertificateInputs();
            EnsureOutputDirectory();

            var signingSpec = SigningSpecifications.V1;
            var signatureHashAlgorithm = ValidateAndParseHashAlgorithm(HashAlgorithm, nameof(HashAlgorithm), signingSpec);
            var timestampHashAlgorithm = ValidateAndParseHashAlgorithm(TimestampHashAlgorithm, nameof(TimestampHashAlgorithm), signingSpec);
            var certCollection = GetCertificateCollection();
            var certificate = GetCertificate(certCollection);
            var privateKey = GetPrivateKey(certificate);

            var request = new AuthorSignPackageRequest(
                certificate,
                signatureHashAlgorithm,
                timestampHashAlgorithm)
            {
                PrivateKey = privateKey
            };
            request.AdditionalCertificates.AddRange(certCollection);

            return request;
        }
    }
}