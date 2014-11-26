﻿using System;
using System.Collections.Generic;
using System.IO;
using AppGet.Manifests;
using AppGet.Serialization;
using NUnit.Framework;

namespace AppGet.Tests.Manifests
{
    [TestFixture]
    public class PackageManifestFixture
    {
        [Test]
        public void print_sample_manifest()
        {
            var manifest = new PackageManifest
            {
                Id = "linqpad",
                Version = "4.51.03",
                Exe = new[] { "LINQPad.exe" },
                ProductUrl ="http://www.linqpad.net/",
                InstallMethod = InstallMethodType.Zip,
                Installers = new List<Installer>
                {
                    new Installer
                    {
                        Location = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureType.Any
                    },
                      new Installer
                    {
                        Location = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureType.Any,
                        MinWindowsVersion = WindowsVersion.VistaSp2,
                        MaxWindowsVersion = WindowsVersion.Eight
                    }
                }
            };


            Console.WriteLine(Yaml.Serialize(manifest));
        }


        [Test]
        public void read_sample_manifest()
        {
            var text = File.ReadAllText("Manifests\\SampleManifests\\mongodb.yaml");

            Yaml.Deserialize<PackageManifest>(text);
        }
    }
}